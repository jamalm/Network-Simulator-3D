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
    public Subnet subnet;
    GameObject engine;

	public string MAC;
	public string IP = "0.0.0.0";
    private string id;

    //for dhcp settings
    public bool dhcpEnabled;
    

    

    public void Load(PCData data)
    {
        ping = data.ping;
        ports[0].Load(data.port);
        MAC = data.MAC;
        IP = data.IP;
    }
    public PCData Save()
    {
        PCData data = new PCData();
        data.ping = ping;
        data.port = ports[0].Save();
        data.MAC = MAC;
        data.IP = IP;

        return data;
    }

    void Awake(){
        //port = GetComponent<Port> ();
        dhcpEnabled = false;
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
            //instantiate port and set parent as this transform
            ports.Add(Instantiate(engine.GetComponent<Engine>().PortPrefab, transform.position, transform.rotation));
            ports[i].transform.parent = transform;

            //set up type
            ports[i].Init("fe0/" + i);
        }
        
        //other applications
        ping = gameObject.GetComponent<Ping>();
        subnet = gameObject.GetComponent<Subnet>();
        
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

    public void Ping(string IP)
    {
        //regular expression to validate an ip 
        Regex ipRgx = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
        int count = 0;
        int success = 0;
        int failure = 0;
        if(!ipRgx.IsMatch(IP))
        {
            Debug.LogAssertion(id + ": Invalid IP address; Check format");
            return;
        }
        while(count < 4)
        {
            if (!sendPacket(ping.Echo(IP)))
            {
                failure++;
                //if there is an active task on this, notify of failure
                if(GetComponent<TaskWatcher>().isActive())
                {
                    GetComponent<TaskWatcher>().PINGFailure();
                }
            } 
            else
            {
                success++;
                if(GetComponent<TaskWatcher>().isActive())
                {
                    GetComponent<TaskWatcher>().PINGSuccess();
                }
            }
            count++;
        }
        Debug.LogAssertion("PING: Successful: " + success);
        Debug.LogAssertion("PING: failed: " + failure);
        
    }





    /// protocol list:
    /// PING 
    /// ARP 
    /// etc...


    /*
    //ping
	public void pingEcho(string destIP){
		packet = new Packet ("PING ECHO");
		packet.netAccess.setMAC (MAC, "src");
		//packet.netAccess.setMAC ("5678", "dest");	//TODO this is just a test, better way to do this!
		packet.internet.setIP (IP, "src");
		packet.internet.setIP (destIP, "dest");
        Debug.Log("PC: sending packet");
		sendPacket (packet);
	}
	private void pingReply(string destIP, string destMAC){
		packet = new Packet ("PING REPLY");
		packet.netAccess.setMAC (MAC, "src");
		//packet.netAccess.setMAC (destMAC, "dest");	//TODO this is just a test, better way to do this!
		packet.internet.setIP (IP, "src");
		packet.internet.setIP (destIP, "dest");
		sendPacket (packet);
	}*/

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

            //check if the network mask is valid
            if (GetComponent<Subnet>().validMask)
            {
                Debug.Log(id + ": Valid Network mask");

                //check if dest network is local 
                if (GetComponent<Subnet>().CheckNetwork(packet.internet.getIP("dest")))
                {
                    Debug.Log(id + ": destination is on local network!");
                    if (ports[0].isListed(packet))
                    {
                        Debug.Log(id + ": MAC ADDRESS IS LISTED");
                        packet.netAccess.setMAC(MAC, "src");
                        packet.netAccess.setMAC(ports[0].getDestMAC(packet), "dest");   //set destination mac address
                        return ports[0].send(packet);
                        
                    }
                    else
                    {
                        Debug.LogAssertion(id + ": ARP TABLE OUT OF DATE, REQUESTING>>>");
                        //send ARP Request!
                        Packet arpReq = Instantiate(packet);
                        arpReq.CreatePacket("ARP");
                        arpReq.internet.setIP(getIP(), "src");                          //set the src ip
                        arpReq.internet.setIP(packet.internet.getIP("dest"), "dest");   //set the dest ip
                        arpReq.netAccess.setMAC(MAC, "src");
                        arpReq.netAccess.setMAC("FF:FF:FF:FF:FF:FF", "dest");
                        arpReq.gameObject.AddComponent<ARP>();
                        ARP arp = arpReq.GetComponent<ARP>();
                        arp.CreateARP("REQUEST");
                        requestARP(arpReq);

                        return false;
                    }
                }
                else if(packet.GetComponent<DHCP>())
                {
                    //if the packet has a dhcp component, forward
                    return ports[0].send(packet);
                    
                }
                else
                {
                    Debug.LogAssertion(id + ": Not on same subnet!");
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
            if(packet.GetComponent<ICMP>().GetType().Equals("REQUEST"))
            {
                //pingReply (packet.internet.getIP ("src"), packet.netAccess.getMAC ("src"));
                if (!sendPacket(ping.Reply(packet.internet.getIP("src"))))
                {
                    //if packet doesn't send...
                    //TODO do something when the packet doesn't send
                    Debug.LogAssertion(id + ": PACKET FAILED TO SEND");
                }
            }
        }
        //if it is an arp request
		if (packet.type.Equals ("ARP"))
        {
            if(packet.GetComponent<ARP>().type.Equals("REQUEST"))
            {
                if (packet.internet.getIP("dest").Equals(IP))
                {
                    Packet arpRep = Instantiate(packet);
                    arpRep.CreatePacket("ARP");
                    arpRep.internet.setIP(packet.internet.getIP("src"), "dest");    //set the src ip as our dest ip 
                    arpRep.internet.setIP(IP, "src");
                    arpRep.netAccess.setMAC(MAC, "src");
                    arpRep.netAccess.setMAC(packet.netAccess.getMAC("src"), "dest");
                    ARP arp = arpRep.gameObject.GetComponent<ARP>();
                    arp.CreateARP("REPLY");
                    replyARP(arpRep);
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
