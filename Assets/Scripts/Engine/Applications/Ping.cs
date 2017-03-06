using UnityEngine;
using System.Collections;

public class Ping : MonoBehaviour
{
    public GameObject packetprefab;
    PC pc;

    void Start()
    {
        pc = GetComponent<PC>();
    }

    private void Update()
    {

    }
    
    public Packet Echo(string IP)
    {
        GameObject pingPacket = Instantiate(packetprefab);
        Packet packet = pingPacket.GetComponent<Packet>();
        packet.CreatePacket("PING");
        packet.internet.setIP(IP, "dest");
        packet.internet.setIP(pc.IP, "src");
        packet.gameObject.AddComponent<ICMP>();
        ICMP icmp = packet.GetComponent<ICMP>();
        icmp.CreateICMP("ECHO");

        return packet;
    }

    public Packet Reply(string IP)
    {
        GameObject pingPacket = Instantiate(packetprefab);
        Packet packet = pingPacket.GetComponent<Packet>();
        packet.CreatePacket("PING");
        packet.internet.setIP(IP, "dest");
        packet.internet.setIP(pc.IP, "src");
        packet.gameObject.AddComponent<ICMP>();
        ICMP icmp = packet.GetComponent<ICMP>();
        icmp.CreateICMP("REPLY");

        return packet;
    }
}
