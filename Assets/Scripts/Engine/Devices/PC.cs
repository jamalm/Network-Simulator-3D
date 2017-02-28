using UnityEngine;
using System.Collections;
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
	public Port port;
    public Ping ping;

	public string MAC;
	public string IP;
    private string id;

    //for dhcp settings
    public bool dhcpEnabled = false;
    

    

    public void Load(PCData data)
    {
        ping = data.ping;
        port.Load(data.port);
        MAC = data.MAC;
        IP = data.IP;
    }
    public PCData Save()
    {
        PCData data = new PCData();
        data.ping = ping;
        data.port = port.Save();
        data.MAC = MAC;
        data.IP = IP;

        return data;
    }

    void Awake(){
        //port = GetComponent<Port> ();
        
    }

	//init
	void Start (){
        port.pcInit("fe0/0", this);
        ping = gameObject.GetComponent<Ping>();
        
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
    private void requestARP(Packet arpReq){
        Debug.Log(id + ": Creating an ARP request!");
		if(port.isConnected()){
            port.send(arpReq);
		} 
	}
	private void replyARP(Packet arpRep){
        Debug.Log("Replying to ARP!");

        if (port.isConnected ()) {
			port.send (arpRep);
		}
	}


    //handles the data link layer, assigning mac addresses and forwarding through port

	public bool sendPacket(Packet packet){
        //check if the port is connected
        if (port.isConnected()) { 
            Debug.Log(id + ": port is connected!");

            //check if the network mask is valid
            if (GetComponent<Subnet>().validMask)
            {
                Debug.Log(id + ": Valid Network mask");

                //check if dest network is local 
                if (GetComponent<Subnet>().CheckNetwork(packet.internet.getIP("dest")))
                {
                    Debug.Log(id + ": destination is on local network!");
                    if (port.isListed(packet))
                    {
                        Debug.Log(id + ": MAC ADDRESS IS LISTED");
                        packet.netAccess.setMAC(MAC, "src");
                        packet.netAccess.setMAC(port.getDestMAC(packet), "dest");   //set destination mac address
                        port.send(packet);
                        return true;
                    }
                    else
                    {
                        Debug.LogAssertion(id + ": ARP TABLE OUT OF DATE, REQUESTING>>>");
                        //send ARP Request!
                        Packet arpReq = new ARP("ARP REQUEST");
                        arpReq.internet.setIP(getIP(), "src");                          //set the src ip
                        arpReq.internet.setIP(packet.internet.getIP("dest"), "dest");   //set the dest ip
                        arpReq.netAccess.setMAC(MAC, "src");
                        requestARP(arpReq);

                        return false;
                    }
                }
                else
                {
                    Debug.LogAssertion(id + ": Not on same subnet!");
                    return false;
                }
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
		if (packet.type.Equals ("PING ECHO"))
        {
            //pingReply (packet.internet.getIP ("src"), packet.netAccess.getMAC ("src"));
            if (!sendPacket(ping.Reply(packet.internet.getIP("src"))))
            {
                //if packet doesn't send...
                //TODO do something when the packet doesn't send
                Debug.LogAssertion(id + ": PACKET FAILED TO SEND");
            }
        }
        //if it is an arp request
		if (packet.type.Equals ("ARP REQUEST"))
        {
            if (packet.internet.getIP("dest").Equals(IP))
            {
                Packet arpRep = new ARP("ARP REPLY");
                arpRep.internet.setIP(packet.internet.getIP("src"), "dest");    //set the src ip as our dest ip 
                arpRep.internet.setIP(IP, "src");
                arpRep.netAccess.setMAC(MAC, "src");
                arpRep.netAccess.setMAC(packet.netAccess.getMAC("src"), "dest");
                replyARP(arpRep);
                
            } 
            else
            {
                Debug.Log(id + ": dropping ARP request , not my ip!");
                
            }
		}
        //if it is an ARP Reply
        if(packet.type.Equals("ARP REPLY"))
        {
            Debug.Log(id + ": Processing ARP reply..");
            port.updateARPTable(packet.internet.getIP("src"), packet.netAccess.getMAC("src"));
            
        }
			
	}

	public Port getNewPort(){
		Debug.Log (id + ": finding new port to bind");
		if (!port.isConnected ()) {
			return port;
		} else {
			return null;
		}
	}
    public Port getPort()
    {
        return port;
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
                setMAC("1234");
                break;
            }
            case 2:
            {
                setIP("192.168.1.3");
                setMAC("5678");
                break;
            }
            case 3:
            {
                setIP("192.168.1.4");
                setMAC("9012");
                break;
            }
            case 4:
            {
                setIP("192.168.1.129");
                setMAC("3456");
                break;
            }
        }
	}
}
