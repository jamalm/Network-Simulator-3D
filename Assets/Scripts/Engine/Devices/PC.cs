using UnityEngine;
using System.Collections;

/*************************************************
 * 
 * Author: Jamal Mahmoud
 * Last Updated: 04/11/16
 * 
 * PC is used to run scripts for the PC GameObject
 * It can create and send packets out to the network 
 * It can connect a cable to the port it sends packets out through
 * 
 * ***********************************************/

public class PC : MonoBehaviour {

	public Packet packet;
	public Port port;

	private string defaultGate;
	public string MAC;			//TODO quick fix
	private string IP;

	void Awake(){
		//port = GetComponent<Port> ();
		port.pcInit("fe0/0", this, MAC);
	}

	//init
	void Start (){
		/*port = GetComponent<Port> ();*/

	}

	//
	void Update (){
		/*
		if (Input.GetKey(KeyCode.Mouse0)) {
			createPING ();
		}*/
	}

	public void setIP(string ip){
		IP = ip;
	}

	public void setMAC(string mac){
		MAC = mac;
	}

	public void setGate(string gate){
		defaultGate = gate;
	}

	public void pingEcho(string destIP){
		packet = new Packet ("PING ECHO");
		packet.netAccess.setMAC (MAC, "src");
		packet.netAccess.setMAC ("5678", "dest");	//TODO this is just a test, better way to do this!
		packet.internet.setIP (IP, "src");
		packet.internet.setIP (destIP, "dest");
		sendPacket (packet);
	}

	public void pingReply(string destIP, string destMAC){
		packet = new Packet ("PING REPLY");
		packet.netAccess.setMAC (MAC, "src");
		packet.netAccess.setMAC (destMAC, "dest");	//TODO this is just a test, better way to do this!
		packet.internet.setIP (IP, "src");
		packet.internet.setIP (destIP, "dest");
		sendPacket (packet);
	}

	public bool sendPacket(Packet packet){
		if(port.isConnected()){
			port.send (packet);
			Debug.Log("PC: PACKET SENT");
			return true;
		} else {
			Debug.Log("PC: NOT Connected");
			return false;
		}
	}

	public void handlePacket(Packet packet){
		Debug.LogAssertion ("PC: Packet Received! -> " + packet.type);
		if (packet.type.Equals ("PING ECHO")) {
			pingReply (packet.internet.getIP ("src"), packet.netAccess.getMAC ("src"));
		}
	}

	public Port getNewPort(){
		Debug.Log ("PC: finding new port to bind");
		if (!port.isConnected ()) {
			return port;
		} else {
			return null;
		}
	}

	public string getIP(){
		return IP;
	}

	/////////////////////////////////////////////
	/// 
	/// TESING
	/// ///////////////////////////////////////

	public void TEST(int select){
		if (select == 1) {
			setIP ("192.168.1.2");
			setMAC ("1234");
		} else {
			setIP ("192.168.1.3");
			setMAC ("5678");
		}
	}
}
