using UnityEngine;
using System.Collections.Generic;


/*************************************************
 * 
 * Author: Jamal Mahmoud
 * Last Updated: 10/11/16
 * 
 * Port is used to lookup the ARP table
 * It forwards packets through the cable to other devices
 * for the switch it stores information about the ports on the other end of the cable
 * 
 * ***********************************************/

public class Port : MonoBehaviour {

	private string type;		//type of port
	public bool connected = false;		//bool for isConnected()
	public Cable cable = null;		//cable attached to port
	public string device;		//device port belongs to 
    public string ip;
    public string mac;
    public Link link;
    private Port endPort;

	private string endPortMAC;			//MAC address of port on other end of cable (for switches)

	private Dictionary<string, string> arpTable;	//Address Resolution Protocol table, storing all the available MAC addresses to this port
 
	// devices that this port could be connected to 
	public PC pc = null;
	public Switch swit = null;
	public Router router = null;

    public void Load(PortData data)
    {
        type = data.type;
        connected = data.connected;
        cable = data.cable;
        device = data.device;
        ip = data.ip;
        mac = data.mac;
        endPortMAC = data.endPortMAC;
        arpTable = data.arpTable;
        //pc.Load(data.pc);
        //swit.Load(data.swit);
        //router.Load(data.router);
    }
    public PortData Save()
    {
        PortData data = new PortData();

        data.type = type;
        data.connected = connected;
        data.cable = cable;
        data.device = device;
        data.ip = ip;
        data.mac = mac;
        data.endPortMAC = endPortMAC;
        data.arpTable = arpTable;
        //data.pc = pc.Save();
        //data.swit = swit.Save();
        //data.router = router.Save();

        return data;
    }

    /*

	//if port belongs to PC 
	public void pcInit(string _type, PC pc){
		this.pc = pc;
		type = _type;
		device = "pc";
		connected = false;
		cable = null;
        //arpTable.Add ("192.168.1.1", "1");
        mac = pc.getMAC();
        ip = pc.getIP();
		Debug.Log ("PC PORT: Created!");
	}

	//if port belongs to a switch
	public void switchInit(string _type, Switch swit){
		this.swit = swit;
		type = _type;
		device = "switch";
		connected = false;
		cable = null;
		Debug.Log ("SWITCH PORT: Created!");
	}

	//if port belongs to a router
	public void routerInit (string _type, Router router){
		this.router = router;
		type = _type;
		device = "router";
		connected = false;
		cable = null;
		Debug.Log ("ROUTER PORT: Created!");
	}
    */
    public void Init(string type)
    {
        this.type = type;

        if(GetComponentInParent<PC>())
        {
            //if device is pc
            device = "pc";
            mac = GetComponentInParent<PC>().MAC;
            ip = GetComponentInParent<PC>().IP;
            pc = GetComponentInParent<PC>();
            Debug.Log("PC PORT: Created!");

        }
        else if(GetComponentInParent<Switch>())
        {
            device = "switch";
            swit = GetComponentInParent<Switch>();
            Debug.Log("Switch PORT: Created!");
        }
        else if(GetComponentInParent<Router>())
        {
            device = "router";
            router = GetComponentInParent<Router>();
            Debug.Log("Router PORT: Created!");
        }
    }
    //initialise the ARP Table at the beginning of the application start up 
    void Awake(){
		arpTable = new Dictionary<string, string> ();
        link = GetComponent<Link>();
    }
	void Start(){
        
        
    }

	void Update(){
        if(cable != null)
        {
            if(cable.faulty)
            {
                connected = false;
            } else
            {
                connected = true;
            }
        }
        if(connected)
        {
            GetComponent<PortStatus>().TurnOn();
        } else
        {
            GetComponent<PortStatus>().TurnOff();
        }
	}

    public void setIP(string ip)
    {
        this.ip = ip;
    }
    public string getIP()
    {
        return ip;
    }
	// sets the port to be the endports Device MAC address
	public void setMAC(string mac) { this.mac = mac;}
    
	public string getEndMAC(){
		return endPortMAC;
	}

    public string getMAC() { return mac; }

	public string getType() { return type; }

    public void setType(string _type) { type = _type; }

    
	//returns the device this port is a child of 
	private PC getPC(){
		return pc;
	}
	private Router getRouter(){
		return router;
	}
    private Switch getSwitch()
    {
        return swit;
    }

	//returns the ID of the device this port is a child of 
	public string getDevice(){
		return device;
	}
		
    public void setCable(Cable cable)
    {
        this.cable = cable;
    }
	//returns the cable that this port has plugged in
	public Cable getCable(){
		return cable;
	}

    public void setConnection(bool connected)
    {
        this.connected = connected;
    }
    // Checks if port is connected
    public bool isConnected()
    {
        //Debug.Log ("PORT: port connected is " + this.connected);
        return this.connected;
    }

    
    // Gets the MAC address associated with the destination IP .
    public virtual string getDestMAC(Packet packet){
		string MAC = arpTable[packet.internet.getIP ("dest")];
		return MAC;
	}

    public virtual void updateARPTable(string IP, string MAC)
    {
        Debug.Log("PORT: Updated ARPTable!");
        arpTable.Add(IP, MAC);
    }

	// checks if the packet's destination ip has a mac address in the ARP table
	public virtual bool isListed(string ip){
        return arpTable.ContainsKey(ip);
	}

	//Send the specified packet.
	public virtual bool send(Packet packet){
        string mac;
        
        //if the arp table has record of the dest mac
        if (arpTable.TryGetValue(packet.internet.getIP("dest"), out mac))
        {
            Debug.Log("PORT: Sending packet through cable");
            packet.netAccess.setMAC(mac, "dest");
        }
        
        //else if it is a broadcast mac or if it is a dhcp packet
        else if (packet.netAccess.getMAC("dest").Equals("FF:FF:FF:FF:FF:FF") || packet.GetComponent<DHCP>())
        {
            Debug.Log("PORT: Sending packet through cable");
        }
        //else if the device is a switch and it doesnt have an arp table, just check the mac address only
        else if(device.Equals("switch") && packet.netAccess.getMAC("dest").Equals(this.mac))
        {
            Debug.Log("PORT: Sending packet through cable");
        }
        else
        {
            Debug.LogAssertion("PORT: ERROR FINDING MAC ADDRESS BINDING");
            return false;
        }


        /*while(Animate(packet))
        {

        }*/
        //tag packet with vlan
        if(link.type.Equals("trunk"))
        {
            if(packet.gameObject.GetComponent<Link>())
            {
                packet.gameObject.GetComponent<Link>().type = link.type;
                packet.gameObject.GetComponent<Link>().vlan = link.vlan;
            }
            else
            {
                packet.gameObject.AddComponent<Link>();
                packet.gameObject.GetComponent<Link>().type = link.type;
                packet.gameObject.GetComponent<Link>().vlan = link.vlan;
            }
            

        }
        if(connected)
        {
            return cable.send(packet, this);
        } else
        {
            return false;
        }
        
        
	}

	//handle incoming packets
	public virtual bool receive(Packet packet){
		Debug.Log ("PORT: Receiving packet from cable");
        if(connected)
        {
            switch (device)
            {
                case "pc":
                    {
                        pc.handlePacket(packet);
                        return true;
                    }
                case "switch":
                    {
                        return swit.handlePacket(packet, this);
                    }
                case "router":
                    {
                        router.handlePacket(packet, this);
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        } else
        {
            return false;
        }

	}

	//plugs in the cable to the port
	public virtual void plugIn(Cable cable, Port endPort){
		Debug.Log ("PORT: plugging in cable");
		setCable(cable);
        setConnection(true);
        this.endPort = endPort;
        
		//if the port belongs to a switch
		if (device.Equals ("switch")) {
            switch (endPort.device)
            {
                case "pc":
                    {
                        //bind mac address of pc and store in switch port
                        setMAC(endPort.getPC().getMAC());
                        link.type = "access";
                        link.vlan = endPort.link.vlan;
                        break;
                    }
                case "router":
                    {
                        //bind mac address of router and store in switch port
                        setMAC(endPort.getRouter().getMAC());
                        link.type = "trunk";
                        endPort.link.type = link.type;
                        link.vlan = 0;
                        endPort.link.vlan = 0;
                        break;
                    }
            }
		}
	}
		
	//not tested
	public void plugOut(){
		cable = null;
		connected = false;
        endPort = null;
        //TODO need to remove subnet details from routing table
        //TODO same here

		setMAC(null);
        link.vlan = 0;
        link.type = null;
        if(device.Equals("switch"))
        {
            endPortMAC = null;
        } 
	}


    private bool Animate(Packet packet)
    {
        //animate packet being sent across the wire
        string to = "";
        string from = "";

        //find out who is receiver
        if (cable.port1 == this)
        {
            //find port2's device
            switch (cable.port2.device)
            {
                case "pc":
                    {
                        to = cable.port2.pc.GetID();
                        break;
                    }
                case "router":
                    {
                        to = cable.port2.router.GetID();
                        break;
                    }
                case "switch":
                    {
                        to = cable.port2.swit.GetID();
                        break;
                    }
            }
        }
        else
        {
            //else find port1's device
            switch (cable.port1.device)
            {
                case "pc":
                    {
                        to = cable.port1.pc.GetID();
                        break;
                    }
                case "router":
                    {
                        to = cable.port1.router.GetID();
                        break;
                    }
                case "switch":
                    {
                        to = cable.port1.swit.GetID();
                        break;
                    }
            }
        }

        //find out who it's from
        switch(device)
        {
            case "pc":
                {
                    from = pc.GetID();
                    break;
                }
            case "router":
                {
                    from = router.GetID();
                    break;
                }
            case "switch":
                {
                    from = swit.GetID();
                    break;
                }                                
        }

        //types of messages
        if(type.Contains("PING"))
        {
            GraphicManager.graphics.Ping(from, to, packet);
        }
        else if(type.Contains("ARP"))
        {
            GraphicManager.graphics.ARP(from, to, packet);
        }

        return true;
    }
}

