using UnityEngine;
using System.Collections;

public class Port : MonoBehaviour {

	private string type;
	private bool connected;
	private Cable cable;
	private string device;

	private string MAC;

	private PC pc = null;
	private Switch swit = null;
	private Router router = null;

	public void pcInit(string _type, PC pc, string mac){
		this.pc = pc;
		type = _type;
		device = "pc";
		connected = false;
		cable = null;
		MAC = mac;
		Debug.Log ("PC PORT: Created!");
	}

	public void switchInit(string _type, Switch swit){
		this.swit = swit;
		type = _type;
		device = "switch";
		connected = false;
		cable = null;
		Debug.Log ("SWITCH PORT: Created!");
	}

	public void routerInit (string _type, Router router){
		this.router = router;
		type = _type;
		device = "router";
		connected = false;
		cable = null;
		MAC = "1";
		Debug.Log ("ROUTER PORT: Created!");
	}
	void Awake(){
		Debug.Log ("AWAKE!!!!");
	}
	void Start(){
		Debug.Log ("STARTED!!!!");
	}

	void Update(){

	}



	public string getType(){
		return type;
	}
	public string getDevice(){
		return device;
	}

	public string getMAC(){
		return MAC;
	}

	public Cable getCable(){
		return cable;
	}

	public bool isConnected(){
		Debug.Log ("PORT: port connected is " + this.connected);
		return this.connected;
	}

	public void send(Packet packet){
		Debug.Log ("PORT: Sending packet through cable");
		this.cable.send (packet, this);

	}

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

	public void plugIn(Cable cable, Port endPort){
		Debug.Log ("PORT: plugging in cable");
		this.cable = cable;
		endPort.cable = cable;
		endPort.connected = true;
		this.connected = true;
		this.MAC = endPort.getMAC ();

	}

	public void plugOut(Cable cable,Port endPort){
		cable.unplug ();
		this.connected = false;
		endPort.connected = false;
	}
}

