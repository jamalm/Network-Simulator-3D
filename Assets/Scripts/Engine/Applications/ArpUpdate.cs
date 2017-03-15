using System;
using System.Collections.Generic;
using UnityEngine;

public class ArpUpdate : MonoBehaviour
{
    public GameObject packetprefab;


    private void Start()
    {

    }

    public Packet Request(string ipsrc, string ipdest)
    { 
        GameObject reqPack = Instantiate(packetprefab);
        Packet packet = reqPack.GetComponent<Packet>();
        packet.CreatePacket("ARP");
        packet.internet.setIP(ipdest, "dest");
        packet.internet.setIP(ipsrc, "src");
        if (GetComponent<PC>())
        {
            packet.netAccess.setMAC(GetComponent<PC>().MAC, "src");
        } else
        {
            packet.netAccess.setMAC(GetComponent<Router>().getMAC(), "src");
        }
        packet.netAccess.setMAC("FF:FF:FF:FF:FF:FF", "dest");
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
        packet.netAccess.setMAC(mac, "dest");
        ARP arp = repPack.AddComponent<ARP>();
        arp.CreateARP("REPLY", ipdest);
        return packet;
    }


}

