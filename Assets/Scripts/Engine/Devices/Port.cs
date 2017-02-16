using UnityEngine;
using System.Collections;
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
	public bool connected;		//bool for isConnected()
	public Cable cable;		//cable attached to port
	public string device;		//device port belongs to 
    public string ip;
    public string mac;

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

    //initialise the ARP Table at the beginning of the application start up 
    void Awake(){
		arpTable = new Dictionary<string, string> ();
	}
	void Start(){
		
	}

	void Update(){

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
       // if (!arpTable.ContainsKey(IP))
       // {
            arpTable.Add(IP, MAC);
       // }
        
    }


	

	// checks if the packet's destination ip has a mac address in the ARP table
	public virtual bool isListed(Packet packet){
		if (arpTable.ContainsKey (packet.internet.getIP ("dest"))) {
			//checks if ip address is in arptable.
			return true;
		} else {
			return false;
		}
	}














	//Send the specified packet.
	public virtual void send(Packet packet){
		Debug.Log ("PORT: Sending packet through cable");

        Animate(packet.type);
        cable.send(packet, this);
        
	}

	//handle incoming packets
	public virtual void receive(Packet packet){
		Debug.Log ("PORT: Receiving packet from cable");
        
		switch (device) {
		case "pc":
			{
				pc.handlePacket (packet);
				break;
			}
		case "switch":
			{
				swit.handlePacket (packet, this);
				break;
			}
		case "router":
			{
				router.handlePacket (packet, this);
				break;
			}
		}
	}

	//plugs in the cable to the port
	public virtual void plugIn(Cable cable, Port endPort){
		
		Debug.Log ("PORT: plugging in cable");
		setCable(cable);
		endPort.setCable(cable);
		endPort.setConnection(true);
        setConnection(true);
        
		//if the port belongs to a switch
		if (device.Equals ("switch")) {
			switch (endPort.device) {
			case "pc" :
				{
					//bind mac address of pc and store in switch port
					setMAC(endPort.getPC().getMAC());
					break;
				}
			case "router": 
				{
					//bind mac address of router and store in switch port
					setMAC(endPort.getRouter().getMAC());
					break;
				}
			}
		}
	}
		
	//not tested
	public void plugOut(Cable cable,Port endPort){
		cable.unplug ();
		this.cable = null;
		this.connected = false;
		endPort.cable = null;
		endPort.connected = false;
		setMAC(null);
		endPort.setMAC (null);
	}


    private void Animate(string type)
    {
        string to = "";
        string from = "";
        if (cable.port1 == this)
        {
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

        if(type.Contains("PING"))
        {

            GraphicManager.graphics.Ping(from, to);
        }
        else if(type.Contains("ARP"))
        {
            GraphicManager.graphics.ARP(from, to);
        }
        
        
    }
}

