using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Switch : MonoBehaviour {

	private List<PC> pcs;
	private Router router;
	public List<Port> ports;
	private List<string> macTable;

	void Awake(){
		/*
		ports [0] = GetComponent<Port> ();
		ports [1] = GetComponent<Port> ();
		ports [2] = GetComponent<Port> ();
		ports [3] = GetComponent<Port> ();
		ports [4] = GetComponent<Port> ();*/
		ports [0].switchInit ("fe0/0", this);
		ports [1].switchInit ("fe0/1", this);
		ports [2].switchInit ("fe0/2", this);
		ports [3].switchInit ("fe0/3", this);
		ports [4].switchInit ("g0/0", this);

		macTable = new List<string> ();

		macTable.Add ("1234");
		macTable.Add ("5678");
	}

	// Use this for initialization
	void Start () {
		//ports = new List<Port> ();
		/*ports.Add (GetComponent<Port>());			//4 PC ports
		ports.Add (GetComponent<Port>());
		ports.Add (GetComponent<Port>());
		ports.Add (GetComponent<Port>());
		ports.Add (GetComponent<Port>());			//1 gigabit port for the Router*/



	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Port getFEPort(bool newPort, string type){
		if (newPort) {
			Debug.Log ("SWITCH: finding new port to bind");
			for (int i = 0; i < ports.Count; i++) {
				//if port is not used and not the gigport
				if (!ports [i].isConnected () && !ports [i].getType ().Contains ("g")) {
					Debug.Log ("SWITCH: found port, looping?: " + i);
					return ports [i];
				}
			}
			return null; //TODO Make sure to write a fail case for this!!!
		} else {
			for (int i = 0; i < ports.Count; i++) {
				//if port is connected and is the same as specified
				if (ports [i].isConnected () && ports [i].getType ().Equals (type)) {
					return ports [i];
				}
			}
			return null; //TODO and here too!!!
		}

	}
	public Port getGPort(bool newPort){
		//if attaching a new port
		if (newPort) {
			for (int i = 0; i < ports.Count; i++) {
				//if port is gigabit port and not connected
				if (ports [i].getType ().Contains ("g") && !ports [i].isConnected ()) {
					return ports [i];
				}
			}
			return null;//TODO here too
		} // if searching for a used port 
		else {
			for (int i = 0; i < ports.Count; i++) {
				//if port is gigabit port and is connected
				if (ports [i].getType ().Contains ("g") && ports [i].isConnected ()) {
					return ports [i];
				}
			}
			return null; //TODO and here
		}

	}




	public void handlePacket(Packet packet, Port incomingPort){
		Debug.LogAssertion ("Switch: Receiving packet");
		if (incomingPort.getType ().Contains ("fe")) {
			Debug.Log("SWITCH: Sending packket to router");
			getGPort (false).send (packet);
		}
		if (incomingPort.getType ().Contains ("g")) {
			for (int i = 0; i < ports.Count; i++) {
				if (packet.netAccess.getMAC ("dest").Equals (ports [i].getMAC ())) {	//TODO not very efficient here, fix this
					Debug.Log ("SWITCH: sending packet to pc: " + ports [i].getMAC ());
					ports [i].send (packet);
				}
			}
		}
	}



}
