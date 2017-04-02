using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*************************************************
 * 
 * Author: Jamal Mahmoud
 * Last Updated: 10/11/16
 * 
 * PC is used to run scripts for the PC GameObject
 * It can create and send packets out to the network 
 * It can use it's port to send packets to other devices
 * 
 * ***********************************************/

public class PC : MonoBehaviour {
	public List<Port> ports = new List<Port>();
    public int numPorts = 1;
    public Ping ping;
    public ArpUpdate arp;
    public Subnet subnet;
    GameObject engine;

	public string MAC;
	public string IP = "0.0.0.0";
    private string id;

    //for dhcp settings
    public bool dhcpEnabled;
    

    

    public void Load(PCData data)
    {
        IP = data.IP;
        subnet.LoadFreshConfig(data.IP, data.mask, data.gate);
    }
    public PCData Save()
    {
        PCData data = new PCData();

        data.IP = IP;
        data.gate = subnet.defaultGateway;
        data.mask = subnet.mask;

        return data;
    }


	//init
	void Start (){
        
        IP = "0.0.0.0";
        //setup mac address
        for (int i=0;i<6;i++)
        {
            if(i!= 5)
            {
                MAC += Random.Range(0, 99).ToString() + ":";

            }
            else
            {
                MAC += Random.Range(0, 99).ToString();
            }
        }
        engine = GameObject.FindGameObjectWithTag("Engine");
        //setup ports
        for (int i=0;i<numPorts;i++)
        {
            Vector3 position = new Vector3(0, 0.5f, 0);
            //instantiate port and set parent as this transform
            ports.Add(Instantiate(engine.GetComponent<Engine>().PortPrefab, transform.position, Quaternion.identity));
            ports[i].transform.parent = transform;
            ports[i].transform.localPosition = position;

            //set up type
            ports[i].Init("fe0/" + i);
        }
        
        //other applications
        ping = gameObject.GetComponent<Ping>();
        subnet = gameObject.GetComponent<Subnet>();
        arp = gameObject.GetComponent<ArpUpdate>();
    }

	//
	void Update (){
	}

    public void SetID(string id)
    {
        this.id = id;
    }
    public string GetID()
    {
        return id;
    }

	public void setIP(string ip)	{IP = ip;}
	public string getIP()	{return IP;}
	public void setMAC(string mac)	{MAC = mac;}
	public string getMAC()	{ return MAC;}

    public string[] Ping(string IP)
    {
        ping.count = 0;
        ping.success = 0;
        ping.failure = 0;
        //regular expression to validate an ip 
        Regex ipRgx = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
        if(!ipRgx.IsMatch(IP))
        {
            Debug.LogAssertion(id + ": Invalid IP address; Check format");
            return null;
        }
        while(ping.count < 4)
        {
            if (!sendPacket(ping.Echo(IP)) )
            {
                ping.count++;
                //if there is an active task on this, notify of failure
                if(GetComponent<TaskWatcher>().isActive())
                {
                    GetComponent<TaskWatcher>().PINGFailure();
                }
            } else
            {
                ping.count++;
            }
            /*else
            {
                
                if(GetComponent<TaskWatcher>().isActive())
                {
                    GetComponent<TaskWatcher>().PINGSuccess();
                }
            }*/
            
            
        }
        ping.failure = ping.count - ping.success;
        Debug.LogAssertion("PING: Successful: " + ping.success);
        Debug.LogAssertion("PING: failed: " + ping.failure);

        string[] results = new string[3];
        results[0] = ping.count.ToString();
        results[1] = ping.success.ToString();
        results[2] = ping.failure.ToString();

        
        return results;
    }





    /// protocol list:
    /// PING 
    /// ARP 
    /// etc...

    //ARP
    public void requestARP(Packet arpReq){
        Debug.Log(id + ": Creating an ARP request!");
		if(ports[0].isConnected()){
            ports[0].send(arpReq);
		} 
	}
	private void replyARP(Packet arpRep){
        Debug.Log("Replying to ARP!");

        if (ports[0].isConnected ()) {
			ports[0].send (arpRep);
		}
	}


    //handles the data link layer, assigning mac addresses and forwarding through port

	public bool sendPacket(Packet packet){
        //check if the port is connected
        if (ports[0].isConnected()) { 
            Debug.Log(id + ": port is connected!");
            packet.netAccess.setMAC(MAC, "src");
            //check if the network mask is valid
            if (GetComponent<Subnet>().validMask)
            {
                Debug.Log(id + ": Valid Network mask");

                //check if dest network is local 
                if (GetComponent<Subnet>().CheckNetwork(packet.internet.getIP("dest")))
                {
                    Debug.Log(id + ": destination is on local network!");
                    if (ports[0].isListed(packet.internet.getIP("dest")))
                    {
                        Debug.Log(id + ": MAC ADDRESS IS LISTED");
                        
                        packet.netAccess.setMAC(ports[0].getDestMAC(packet), "dest");   //set destination mac address
                        return ports[0].send(packet);
                        
                    }
                    else
                    {
                        Debug.LogAssertion(id + ": ARP TABLE OUT OF DATE, REQUESTING>>>");
                        //send ARP Request!
                        ports[0].send(arp.Request(IP, packet.internet.getIP("dest")));

                        return false;
                    }
                }
                else if(packet.GetComponent<DHCP>())
                {
                    //TODO
                    //if the packet has a dhcp component, forward

                    return ports[0].send(packet);
                    
                }
                else if(subnet.defaultGateway != "")
                {
                    Debug.LogAssertion(id + ": Not on same subnet!forwarding to default gateway");
                    packet.internet.setIP(subnet.defaultGateway, "dest");
                    return sendPacket(packet);
                }
                else
                {
                    Debug.LogAssertion("ERROR FOUND IN PC !!! no gateway assigned");
                    return false;
                }
            }
            else if (packet.GetComponent<DHCP>())
            {
                //if the packet contains a dhcp component, forward it on
                return ports[0].send(packet);
                
            }
            else
            {
                Debug.LogAssertion(id + ": Not a valid Network!");
                return false;
            }
		} else {
			Debug.LogAssertion(id + ": NOT Connected");
			return false;
		}
	}

	public void handlePacket(Packet packet){
		Debug.LogAssertion (id + ": Packet Received! -> " + packet.type);

        //if it is a ping 
		if (packet.type.Equals ("PING"))
        {
            if(packet.GetComponent<ICMP>().type.Equals("ECHO") && packet.GetComponent<ICMP>().ip.Equals(IP))
            {
                //pingReply (packet.internet.getIP ("src"), packet.netAccess.getMAC ("src"));
                if (!sendPacket(ping.Reply(packet.internet.getIP("src"))))
                {
                    //if packet doesn't send...
                    //TODO do something when the packet doesn't send
                    Debug.LogAssertion(id + ": PACKET FAILED TO SEND");
                }
            }
            else if(packet.GetComponent<ICMP>().type.Equals("REPLY"))
            {
                ping.success++;
            }
        }
        //if it is an arp request
		if (packet.type.Equals ("ARP"))
        {
            //if its a request
            if(packet.GetComponent<ARP>().type.Equals("REQUEST"))
            {
                //if the request is addressed to me
                if (packet.internet.getIP("dest").Equals(IP))
                {
                    //check if i have that computer in my arp, if not, update and reply, else just reply
                    if(!ports[0].isListed(packet.internet.getIP("src")))
                    {
                        ports[0].updateARPTable(packet.internet.getIP("src"), packet.netAccess.getMAC("src"));
                    }
                    ports[0].send(arp.Reply(IP, packet.internet.getIP("src"), packet.netAccess.getMAC("src")));
                }
            }
            else if(packet.GetComponent<ARP>().type.Equals("REPLY"))
            {
                Debug.Log(id + ": Processing ARP reply..");
                ports[0].updateARPTable(packet.internet.getIP("src"), packet.netAccess.getMAC("src"));

            }
            else
            {
                Debug.Log(id + ": dropping ARP request , not my ip!");
                
            }
		}

        if(packet.type.Contains("DHCP"))
        {
            //if this device is a DHCP server/client..
            if (GetComponent<DHCPServer>())
            {

                GetComponent<DHCPServer>().handle(packet);
            }
            else if (GetComponent<DHCPClient>())
            {

                GetComponent<DHCPClient>().handle(packet);
            }
        }

			
	}

	public Port getNewPort(){
		Debug.Log (id + ": finding new port to bind");
		if (!ports[0].isConnected ()) {
			return ports[0];
		} else {
			return null;
		}
	}
    public Port getPort()
    {
        return ports[0];
    }

	/////////////////////////////////////////////
	/// 
	/// TESING
	/// ///////////////////////////////////////

	public void TEST(int select){
        
        switch(select)
        {
            case 1:
            {
                setIP("192.168.1.2");
                break;
            }
            case 2:
            {
                setIP("192.168.1.3");
                break;
            }
            case 3:
            {
                setIP("192.168.1.4");
                break;
            }
            case 4:
            {
                setIP("192.168.1.129");
                break;
            }
        }
	}
}
