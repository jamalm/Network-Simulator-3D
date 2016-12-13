using UnityEngine;


public class Ping : MonoBehaviour{

    private ICMP icmp;
    private PC pc;

    public Ping(PC pc)
    {
        this.pc = pc;
    }

    public Packet Echo(string IP)
    {
        icmp = new ICMP("ECHO");
        return icmp.CreatePacket(IP, pc);
    }

    public Packet Reply(string IP)
    {
        icmp = new ICMP("REPLY");
        return icmp.CreatePacket(IP, pc);
    }
}
