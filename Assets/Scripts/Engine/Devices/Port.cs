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
	private bool connected;		//bool for isConnected()
	private Cable cable;		//cable attached to port
	private string device;		//device port belongs to 

	private string endPortMAC;			//MAC address of port on other end of cable (for switches)

	private Dictionary<string, string> arpTable;	//Address Resolution Protocol table, storing all the available MAC addresses to this port

	// devices that this port could be connected to 
	private PC pc = null;
	private Switch swit = null;
	private Router router = null;

	//if port belongs to PC 
	public void pcInit(string _type, PC pc){
		this.pc = pc;
		type = _type;
		device = "pc";
		connected = false;
		cable = null;
		//arpTable.Add ("192.168.1.1", "1");
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

	// sets the port to be the endports Device MAC address
	public void setMAC(string mac){
		endPortMAC = mac;
	}
	public string getMAC(){
		return endPortMAC;
	}

	public string getType(){
		return type;
	}


	//returns the device this port is a child of 
	private PC getPC(){
		return pc;
	}
	private Router getRouter(){
		return router;
	}

	//returns the ID of the device this port is a child of 
	public string getDevice(){
		return device;
	}
		

	//returns the cable that this port has plugged in
	public Cable getCable(){
		return cable;
	}


	/// <summary>
	/// Gets the MAC address associated with the destination IP .
	/// </summary>
	/// <returns>The MAC Address.</returns>
	/// <param name="packet">Packet for sending.</param>
	public string getDestMAC(Packet packet){
		string MAC = arpTable[packet.internet.getIP ("dest")];
		return MAC;
	}

    public void updateARPTable(string IP, string MAC)
    {

        Debug.Log("PORT: Updated ARPTable!");
       // if (!arpTable.ContainsKey(IP))
       // {
            arpTable.Add(IP, MAC);
       // }
        
    }


	// Checks if port is connected
	public bool isConnected(){
		//Debug.Log ("PORT: port connected is " + this.connected);
		return this.connected;
	}

	// checks if the packet's destination ip has a mac address in the ARP table
	public bool isListed(Packet packet){
		if (arpTable.ContainsKey (packet.internet.getIP ("dest"))) {
			//checks if ip address is in arptable.
			return true;
		} else {
			return false;
		}
	}

	//Send the specified packet.
	public void send(Packet packet){
		Debug.Log ("PORT: Sending packet through cable");
		this.cable.send (packet, this);
	}

	//handle incoming packets
	public void receive(Packet packet){
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
	public void plugIn(Cable cable, Port endPort){
		
		Debug.Log ("PORT: plugging in cable");
		this.cable = cable;
		endPort.cable = cable;
		endPort.connected = true;
		this.connected = true;

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
}

