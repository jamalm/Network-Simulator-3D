using UnityEngine;
using System.Collections;


public class InternetLayer : Layer {

	private string srcIP,destIP;

	public InternetLayer (string protocol) {
		setProtocol(protocol);
		setup ();
	}

	public override string getIP(string type){
		if (type.Equals ("src")) {
			return srcIP;
		} else if (type.Equals ("dest")) {
			return destIP;
		} else {
			return "";
		}
	}

	public override void setIP(string IP, string type){
		if (type.Equals ("src")) {
			srcIP = IP;
		} else if (type.Equals ("dest")) {
			destIP = IP;
		}
	}


	private void setup(){
		if (getProtocol().Equals ("IP")) {
			//add something to packet
			setActive(true);
		} else {
			//TODO for other protocols
			setActive(false);
		}
	}


}