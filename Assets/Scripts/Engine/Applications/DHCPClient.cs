using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHCPClient : MonoBehaviour {

    enum STATE
    {
        INIT,
        SELECT,
        REQUEST,
        BOUND, 
        RENEW
    }

    STATE dhcpState;
    public GameObject packetPrefab;
    PC pc;
    

	// Use this for initialization
	void Start () {
        pc = GetComponent<PC>();
        packetPrefab = GetComponent<Ping>().packetprefab;

	}
	
	// Update is called once per frame
	void Update () {

        if (pc.dhcpEnabled && pc.IP.Equals("0.0.0.0") && GameController.gameState.netState == GameController.NetworkState.ACTIVE)
        {
            StartCoroutine(Discovery());
            if(pc.IP.Equals("0.0.0.0"))
                GetComponent<Subnet>().SetDefaultConfiguration();
        }
        
    }

    private void Discover()
    {
        //initialisation stage
        dhcpState = STATE.INIT;
        GameObject dhcpPacket = Instantiate(packetPrefab);
        
        //setup packet
        Packet packet = dhcpPacket.GetComponent<Packet>();
        packet.CreatePacket("DHCP");
        packet.internet.setIP("0.0.0.0", "src");
        packet.internet.setIP("255.255.255.255", "dest");
        packet.netAccess.setMAC("FF:FF:FF:FF:FF:FF", "dest");
        packet.netAccess.setMAC(pc.MAC, "src");
        
        //attach dhcp component
        packet.gameObject.AddComponent<DHCP>();
        DHCP dhcp = packet.GetComponent<DHCP>();
        dhcp.CreateDHCP("DHCPDISCOVER");
        
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
        /*
        Packet arpReq = new ARP("ARP REQUEST");
        arpReq.internet.setIP(GetComponent<PC>().getIP(), "src");                          //set the src ip
        arpReq.internet.setIP(packet.internet.getIP("dest"), "dest");   //set the dest ip
        arpReq.netAccess.setMAC(GetComponent<PC>().MAC, "src");
        GetComponent<PC>().requestARP(arpReq);*/
        
        DHCP dhcp = packet.gameObject.GetComponent<DHCP>();
        dhcp.type = "DHCPREQUEST";
        dhcp.servAddr = packet.internet.getIP("src");
        packet.internet.setIP("0.0.0.0", "src");
        packet.internet.setIP(dhcp.servAddr, "dest");

        packet.netAccess.setMAC(packet.netAccess.getMAC("src"), "dest");
        packet.netAccess.setMAC(dhcp.cliMac, "src");
        pc.sendPacket(packet);
    }

    public void Release()
    {

    }

    private void UseConfig(Packet packet)
    {
        dhcpState = STATE.BOUND;
        DHCP dhcp = packet.gameObject.GetComponent<DHCP>();
        pc.GetComponent<Subnet>().SetConfiguration(dhcp);
    }

    public void handle(Packet packet)
    {
        
        if(packet.netAccess.getMAC("dest").Equals(pc.MAC))
        {
            switch (packet.GetComponent<DHCP>().type)
            {
                case "DHCPOFFER":
                    {
                        Debug.Log(GetComponent<PC>().GetID() + ": RECEIVED DHCP OFFER: " + packet.GetComponent<DHCP>().type);
                        Request(packet);
                        break;
                    }
                case "DHCPACK":
                    {
                        Debug.Log(GetComponent<PC>().GetID() + ": RECEIVED DHCP ACK: " + packet.GetComponent<DHCP>().type);
                        UseConfig(packet);
                        break;
                    }
                default:
                    {
                        Debug.Log(GetComponent<PC>().GetID() + ": CLIENT DROPPING DHCP PACKET");
                        break;
                    }
            }
        }
       
    }

    private IEnumerator Discovery()
    {
        Discover();
        yield return null;

    }
}
