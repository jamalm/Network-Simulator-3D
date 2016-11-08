using UnityEngine;
using System.Collections;


public class NALayer : Layer {

	//for ethernet protocol
	public string destMAC;
	public string srcMAC;

	public NALayer (string interface_)
	{
		setProtocol(interface_);
		setup ();
	}



	public override string getMAC(string type){
		if (type.Equals ("src")) {
			return srcMAC;
		} else if (type.Equals ("dest")) {
			return destMAC;
		} else {
			return "";
		}
	}


	public override void setMAC(string MAC, string type) {
		if (type.Equals ("src")) {
			srcMAC = MAC;
		} else {
			destMAC = MAC;
		}
	}

	private void setup(){
		if (getProtocol().Equals("Ethernet")) {
			//add something to packet
			setActive(true);
		} else {
			//TODO other interfaces
			setActive(false);
		}
	}
}