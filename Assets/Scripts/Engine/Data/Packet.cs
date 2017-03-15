using UnityEngine;
using System.Collections;
using System;

public class Packet : MonoBehaviour{

	//TCP/IP layers to be loaded into packet
	public Layer app, trans, internet, netAccess;
	public string type;


    public void CreatePacket(string type)
    {
        switch (type)
        {
            case "PING":
                {
                    app = new AppLayer("PING");
                    trans = new TransLayer("");
                    internet = new InternetLayer("IP");
                    netAccess = new NALayer("Ethernet");
                    this.type = type;
                    break;
                }
            case "DHCP":
                {
                    app = new AppLayer("DHCP");
                    trans = new TransLayer("UDP");
                    internet = new InternetLayer("IP");
                    netAccess = new NALayer("Ethernet");
                    this.type = type;
                    break;
                }
            case "ARP":
                {
                    app = new AppLayer("ARP");
                    trans = new TransLayer("");
                    internet = new InternetLayer("IP");
                    netAccess = new NALayer("Ethernet");
                    this.type = type;
                    break;
                }
            case "TEST":
                {
                    //null
                    break;
                }
        }
    }

}
