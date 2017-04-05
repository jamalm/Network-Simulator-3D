using UnityEngine;

public class ArpUpdate : MonoBehaviour
{
    public GameObject packetprefab;


    private void Start()
    {

    }

    /*******************************************************************************/
    /* Methods Implemented to Create ARP requests and Replies using the ARP protocol
    /* header and to set up the required information for the header before returning back to the PC
    /***********************************************************/
    public Packet Request(string ipsrc, string ipdest)
    {
        //Request packet instantiated in the game 
        GameObject reqPack = Instantiate(packetprefab);

        //get ref to packet component
        Packet packet = reqPack.GetComponent<Packet>();

        //create the packet with the ARP ID tag
        packet.CreatePacket("ARP");
        //set info for packet
        packet.internet.setIP(ipdest, "dest");
        packet.internet.setIP(ipsrc, "src");

        //if the device is a PC, get the pc component and return it's MAC address, else get router's MAC
        if (GetComponent<PC>())
        {
            packet.netAccess.setMAC(GetComponent<PC>().MAC, "src");
        } else
        {
            packet.netAccess.setMAC(GetComponent<Router>().getMAC(), "src");
        }

        //set destination MAC address to be a broadcast message
        packet.netAccess.setMAC("FF:FF:FF:FF:FF:FF", "dest");

        //Add and fill ARP header with data
        ARP arp = reqPack.AddComponent<ARP>();
        arp.CreateARP("REQUEST", ipdest);

        return packet;
    }

    public Packet Reply(string ipsrc ,string ipdest, string mac)
    {
        GameObject repPack = Instantiate(packetprefab);

        Packet packet = repPack.GetComponent<Packet>();

        packet.CreatePacket("ARP");

        packet.internet.setIP(ipdest, "dest");
        packet.internet.setIP(ipsrc, "src");

        if (GetComponent<PC>())
        {
            packet.netAccess.setMAC(GetComponent<PC>().MAC, "src");
        }
        else
        {
            packet.netAccess.setMAC(GetComponent<Router>().getMAC(), "src");
        }
        //set destination MAC address to be the packet's source MAC provided as a parameter to this method
        packet.netAccess.setMAC(mac, "dest");

        ARP arp = repPack.AddComponent<ARP>();
        arp.CreateARP("REPLY", ipdest);

        return packet;
    }
}

