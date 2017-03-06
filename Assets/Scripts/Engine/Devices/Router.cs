using UnityEngine;
using System.Collections;
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

	private List<string> routingTable;
	public List<Port> ports;
    public int numFEPorts = 1;
    public int numGPorts = 2;
	private Packet packet;
    private string id;

    private string ip;
	private string MAC;

    GameObject engine;

    public void Load(RouterData data)
    {
        routingTable = data.routingTable;
        for(int i=0;i<ports.Count;i++)
        {
            ports[i].Load(data.ports[i]);
        }
        MAC = data.MAC;
    }
    public RouterData Save()
    {
        RouterData data = new RouterData();
        data.MAC = MAC;
        data.routingTable = routingTable;
        for(int i=0;i<ports.Count;i++)
        {
            data.ports[i] = ports[i].Save();
        }
        return data;
    }

	void Awake(){
        routingTable = new List<string>();
        ports = new List<Port>();
	}

	// Use this for initialization
	void Start () {
        int numPorts = numFEPorts + numGPorts;
        MAC = "1";
        routingTable.Add("192.168.1.1");
        routingTable.Add("192.168.1.2");
        routingTable.Add("192.168.1.3");
        routingTable.Add("192.168.1.4");
        routingTable.Add("192.168.1.5");
        ip = routingTable[4];

        engine = GameObject.FindGameObjectWithTag("Engine");
        //set up ports
        for(int i=0;i<numPorts;i++)
        {
            ports.Add(Instantiate(engine.GetComponent<Engine>().PortPrefab, new Vector3(5 * i, 0, -10), Quaternion.Euler(-90, -90, 0)));
            ports[i].transform.parent = transform;

            if(i >= numFEPorts)
            {
                ports[i].Init("g0/" + (i-numFEPorts));
            } else
            {
                ports[i].Init("fe0/" + i);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		//checkForNewComputers ();
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
    private void replyARP(Packet arpRep, Port port)
    {
        Debug.Log("Replying to ARP!");

        if (port.isConnected())
        {
            port.send(arpRep);
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

        //if incoming packet is an ARP request addressed to this IP , return a reply
        if(packet.type.Equals("ARP") && packet.internet.getIP("dest").Equals(ip))
        {
            Packet arpRep = Instantiate(packet);
            arpRep.CreatePacket("ARP");
            arpRep.internet.setIP(packet.internet.getIP("src"), "dest");    //set the src ip as our dest ip 
            arpRep.internet.setIP(routingTable[0], "src");
            arpRep.netAccess.setMAC(MAC, "src");
            arpRep.netAccess.setMAC(packet.netAccess.getMAC("src"), "dest");
            ARP arp = arpRep.gameObject.GetComponent<ARP>();
            arp.CreateARP("REPLY");
            replyARP(arpRep ,incomingPort);
            //TODO Separate local area and wide area requests... 

            /*Packet arpRep = Instantiate(packet);
            arpRep.internet.setIP(packet.internet.getIP("src"), "dest");    //set the src ip as our dest ip 
            arpRep.internet.setIP(routingTable[0], "src");
            arpRep.netAccess.setMAC(MAC, "src");
            arpRep.netAccess.setMAC(packet.netAccess.getMAC("src"), "dest");
            replyARP(arpRep, incomingPort);*/
        }

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
        }
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
