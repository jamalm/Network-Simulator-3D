using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/*************************************************
 * 
 * Author: Jamal Mahmoud
 * Last Updated: 10/11/16
 * 
 * Router is attached to the router GameObject
 * It take packets and routes them to the correct destination
 * it holds a routing table 
 * it contains more than one port
 * handles data that goes outside the local area network
 * 
 * ***********************************************/

public class Router : MonoBehaviour {

	//private List<string> routingTable;
	public List<Port> ports;


    public Dictionary<string, Port> RoutingTable = new Dictionary<string, Port>();
    public int numFEPorts = 1;
    public int numGPorts = 4;
    private string id;
	private string MAC;

    GameObject engine;
    public DHCPServer[] servers;
    public ArpUpdate arp;


    
    public void Load(RouterData data)
    {
        //routingTable = data.routingTable;
        numFEPorts = data.numFEPorts;
        numGPorts = data.numGPorts;
    }
    public RouterData Save()
    {
        RouterData data = new RouterData();
        data.numGPorts = numGPorts;
        data.numFEPorts = numFEPorts;
        return data;
    }

	void Awake(){
        //routingTable = new List<string>();
        ports = new List<Port>();
	}

	// Use this for initialization
	void Start () {
        arp = GetComponent<ArpUpdate>();
        int numPorts = numFEPorts + numGPorts;
        for (int i = 0; i < 6; i++)
        {
            if (i != 5)
            {
                MAC += Random.Range(0, 99).ToString() + ":";

            }
            else
            {
                MAC += Random.Range(0, 99).ToString();
            }
        }

        engine = GameObject.FindGameObjectWithTag("Engine");
        //set up ports
        for(int i=0;i<numPorts;i++)
        {
            Vector3 position = new Vector3(-21 ,-15+(2*i), 0);
            ports.Add(Instantiate(engine.GetComponent<Engine>().PortPrefab,transform.Find("PivotPoint").position, Quaternion.identity));
            ports[i].transform.parent = transform;
            ports[i].transform.localPosition = position;

            if(i >= numFEPorts)
            {
                ports[i].Init("g0/" + (i-numFEPorts));
                ports[i].gameObject.AddComponent<Subnet>();
                ports[i].GetComponent<Subnet>().CreateRouteConfiguration("192.168." + ((i-numFEPorts) + 1) + ".0", "255.255.255.0", "192.168." + ((i - numFEPorts) + 1) + ".1");
                gameObject.AddComponent<DHCPServer>();

            } else
            {
                ports[i].Init("fe0/" + i);
                ports[i].gameObject.AddComponent<Subnet>();
            }
        }
        servers = GetComponents<DHCPServer>();

        int serverNum = 0;
        for(int i=0;i<numPorts;i++)
        {
            if(ports[i].getType().Contains("g"))
            {
                servers[serverNum].Setup(ports[i].GetComponent<Subnet>());
                RoutingTable.Add(ports[i].GetComponent<Subnet>().network, ports[i]);
                serverNum++;
            }
        }

    }


    // Update is called once per frame
    

	// Update is called once per frame
	void Update () {
		//checkForNewComputers ();
	}

    public Port GetRoutePort(string ip)
    {
        Port port;
        RoutingTable.TryGetValue(ip, out port);
        return port;
    }
    public string GetRouteIP(Port port)
    {
        //return the ip that is attached to that port;
        return RoutingTable.FirstOrDefault(route => route.Value == port).Key;
    }


    public void SetID(string id)
    {
        this.id = id;
    }
    public string GetID()
    {
        return id;
    }

    public string getMAC(){
		return MAC;
	}


    //ARP
    private void requestARP(Packet arpReq, Port port)
    {
        Debug.Log("PC: Creating an ARP request!");
        if (port.isConnected())
        {
            port.send(arpReq);
        }
    }
    private bool replyARP(Packet arpRep, Port port)
    {
        Debug.Log("Replying to ARP!");

        if (port.isConnected())
        {
            return port.send(arpRep);
        } else
        {
            return false;
        }
    }


    //return all ports
    public List<Port> getPorts(){
		return ports;
	}


	//connect a new port
	public Port getNewPort(string type){
		Debug.Log ("ROUTER: finding new port to bind");
		for (int i = 0; i < ports.Count; i++) {
			if (ports [i].getType ().Contains (type) && !ports[i].isConnected()) {
				return ports [i];
			}
		}
		return null; //TODO fix this
	}

	//get specific port
	public Port getPort(string type){
		for (int i = 0; i < ports.Count; i++) {
			if (ports [i].getType ().Equals (type) && ports[i].isConnected()) {
				return ports [i];
			}
		}
		return null; //TODO fix this
	}


	/**********************************
	 * Receives and Handles incoming packets
	 * 
	 */
	public void handlePacket(Packet packet, Port incomingPort) {
		Debug.Log ("ROUTER: RECEIVED PACKET");
        /*
        //add incoming device's MAC to port
        if(!incomingPort.isListed(packet.internet.getIP("src")))
        {
            incomingPort.updateARPTable(packet.internet.getIP("src"), packet.netAccess.getMAC("src"));
        }*/

        //for dhcp
        if (packet.type.Equals("DHCP"))
        {
            for(int i=0;i<servers.Length;i++)
            {
                if(servers[i].port.Equals(incomingPort.GetComponent<Port>()))
                {
                    servers[i].handle(packet);
                }
            }
        }

        //for ping
        if(packet.type.Equals("PING"))
        {
            
            //get destination ip    for ICMP currently
            string destIp = packet.GetComponent<ICMP>().ip;
            //get network of dest ip
            string destNetwork = incomingPort.GetComponent<Subnet>().GetNetworkFromIP(destIp);
            //fetches outgoing port based on route returned from routing table
            Port outPort = GetRoutePort(destNetwork);
            //if the port does not have the mac address, request it then send it after
            if(!outPort.isListed(destIp))
            {
                requestARP(arp.Request(outPort.GetComponent<Subnet>().defaultGateway, destIp), outPort);
            }
            if(outPort != null)
            {
                //set the new destination ip
                packet.internet.setIP(destIp, "dest");
                outPort.send(packet);
            } else
            {
                Debug.LogAssertion(id + ": Dropping Ping, no routes found!");
            }
        }

        //for arp
        //if incoming packet is an ARP request addressed to this IP , return a reply
        if(packet.type.Equals("ARP") && packet.internet.getIP("dest").Equals(incomingPort.GetComponent<Subnet>().defaultGateway))
        {
            //if this is a reply to this ip, add it to the incoming port's list
            if(packet.GetComponent<ARP>().type.Equals("REPLY"))
            {
                incomingPort.updateARPTable(packet.internet.getIP("src"), packet.netAccess.getMAC("src"));
            }
            else if(!replyARP(arp.Reply(incomingPort.GetComponent<Subnet>().defaultGateway, packet.internet.getIP("src"), packet.netAccess.getMAC("src")) ,incomingPort))
            {
                if(!incomingPort.isConnected())
                {
                    Debug.Log(id + ": Port is not connected, cant reply to ARP");
                } else
                {
                    requestARP(arp.Request(incomingPort.GetComponent<Subnet>().defaultGateway, packet.internet.getIP("src")), incomingPort);
                }
            }
        }
        /*
        for(int i = 0; i < routingTable.Count; i++)
        {
            
            if(packet.type.Equals("ARP"))
            {
                Debug.Log("ROUTER: Dropping ARP request");
                break;
            }
            //if dest IP is on routing table, forward to the switch 
            //TODO foreign networks , different ip ranges ,
            //this should only be for local area network.
            
            else if (packet.internet.getIP("dest").Equals(routingTable[i]))
            {
                push(packet, incomingPort.getType());
            }
        }*/
	}



	/*
	 * 
	 * 
	 * DO these later! 
	 * 
	 * 
	 */
	public void checkForNewComputers(){
		//TODO build routing table 
	}

	public void push(Packet packet,string portName){
		Debug.Log ("ROUTER: PUSHING PACKET BACK");
		getPort (portName).send (packet);
	}



	public void response(string dest){
		//TODO respond back to host
        /*
		packet = new Packet("PING");
		packet.internet.setIP (routingTable[0], "src");
		packet.internet.setIP (dest, "dest");
        */
        
	}
}
