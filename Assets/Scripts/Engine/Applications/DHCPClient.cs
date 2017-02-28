using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHCPClient : MonoBehaviour {

    enum STATE
    {
        INIT,
        SELECT,
        REQUEST,
        BOUND
    }
    STATE dhcpState;
    public GameObject packetPrefab;
    PC pc;

	// Use this for initialization
	void Start () {
        pc = GetComponent<PC>();
        if(pc.dhcpEnabled)
        {
            Discover();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Discover()
    {
        //initialisation stage
        dhcpState = STATE.INIT;
        GameObject dhcpPacket = Instantiate(packetPrefab);
        
        //setup packet
        Packet packet = dhcpPacket.GetComponent<Packet>();
        packet.CreatePacket("DHCPDISCOVER");
        packet.internet.setIP("0.0.0.0", "src");
        packet.internet.setIP("255.255.255.255", "dest");
        
        //attach dhcp component
        packet.gameObject.AddComponent<DHCP>();
        DHCP dhcp = packet.GetComponent<DHCP>();
        dhcp.CreateDHCP("DISCOVER");
        
        //populate dhcp with data
        dhcp.servAddr = packet.internet.getIP("dest");
        dhcp.cliAddr = packet.internet.getIP("src");

        //send packet out to network
        pc.sendPacket(packet);
        dhcpState = STATE.SELECT;
    }

    //TODO: send this client to the server to store
    private void Request(Packet packet)
    {
        //request stage
        dhcpState = STATE.REQUEST;
        
    }

    public void Release()
    {

    }

    private void UseConfig(Packet packet)
    {

    }

    public void handle(Packet packet)
    {
        switch (packet.type)
        {
            case "DHCPOFFER":
                {
                    Request(packet);
                    break;
                }
            case "DHCPACK":
                {
                    UseConfig(packet);
                    break;
                }
        }
    }
}
