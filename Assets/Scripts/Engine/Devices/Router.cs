using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Router : MonoBehaviour {

	private List<string> routingTable = new List<string>();
	public List<Port> ports = new List<Port> ();
	private Packet packet;
	//private bool validIp;	//check to see if packet destIP is located in routingTable

	void Awake(){
		routingTable.Add ("192.168.1.1");
		routingTable.Add ("192.168.1.2");
		routingTable.Add ("192.168.1.3");

		//ports [0] = GetComponent<Port> ();
		ports [0].routerInit ("g0/0" ,this);
		//ports [1] = GetComponent<Port> ();
		ports [1].routerInit ("g0/1" ,this);
		//ports [2] = GetComponent<Port> ();
		ports [2].routerInit ("fe0/0" ,this);
	}

	// Use this for initialization
	void Start () {
		
		//validIp = false;
		/*ports.Add (GetComponent<Port>());
		ports.Add(GetComponent<Port>());
		ports.Add(GetComponent<Port>());*/

	}

	// Update is called once per frame
	void Update () {
		//checkForNewComputers ();
	}



	//return all ports
	public List<Port> getPorts(){
		return ports;
	}


	//connect a new port
	public Port getNewGPort(){
		Debug.Log ("ROUTER: finding new port to bind");
		for (int i = 0; i < ports.Count; i++) {
			if (ports [i].getType ().Contains ("g") && !ports[i].isConnected()) {
				return ports [i];
			}
		}
		return null; //TODO fix this
	}

	//get specific port
	public Port getGPort(string type){
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
		Debug.LogAssertion ("ROUTER: RECEIVED PACKET");
		if (packet.type.Contains ("PING")) {
			for (int i = 0; i < routingTable.Count; i++) {
				if (packet.internet.getIP ("dest").Equals (routingTable [i])) {
					push (packet, incomingPort.getType ());
				}
			}
		}


		/*
		bool validIP = false;

		//check if message is a ping echo
		if (packet.type.Equals ("PING")) {
			for (int i = 0; i < routingTable.Count; i++) {
				if (packet.internet.getIP ("dest").Equals (routingTable [i])) {
					response (packet, routingTable [i], packet.type);	//unicast response
					validIp = true;
				}
			}
		} else {
			for (int i = 0; i < routingTable.Count; i++) {
				if (packet.internet.getIP ("dest").Equals (routingTable [i])) {
					push (packet, routingTable [i]);	//push on message
					validIp = true;
				}
			}
		}
		if (!validIP) {
			//IP is not on routing table
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
		Debug.Log ("ROUTER: SENDING PACKET BACK");
		getGPort (portName).send (packet);
	}



	public void response(string dest){
		//TODO respond back to host
		packet = new Packet("PING");
		packet.internet.setIP (routingTable[0], "src");
		packet.internet.setIP (dest, "dest");


	}
}
