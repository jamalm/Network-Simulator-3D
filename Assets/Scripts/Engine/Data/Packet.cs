using UnityEngine;
using System.Collections;
using System;

public class Packet{

	public int layers;
	//TCP/IP layers to be loaded into packet
	public Layer app, trans, internet, netAccess;
	public string type;

	public Packet(){
		//test case : DEFAULT
		netAccess = new NALayer ("Ethernet");
		internet = new InternetLayer ("IP");
		trans = new TransLayer ("TCP");
		app = new AppLayer ("");
	}

	public Packet(string type){
		switch (type) {
		case "PING ECHO":
			{
				app = null;
				trans = new TransLayer ("TCP");
				internet = new InternetLayer ("IP");
				netAccess = new NALayer ("Ethernet");
				this.type = type;
				break;
			}
		case "PING REPLY":
			{
				app = null;
				trans = new TransLayer ("TCP");
				internet = new InternetLayer ("IP");
				netAccess = new NALayer ("Ethernet");
				this.type = type;
				break;
			}
			case "TEST" :
			{
				//null
				break;
			}
		}
	}

}
