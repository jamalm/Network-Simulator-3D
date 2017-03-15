using UnityEngine;

public class ICMP : MonoBehaviour {

    //internet layer handling

    public string type;
    public string ip;

    /*
    public Packet CreatePacket(string IP, PC pc)
    {
        Packet packet = new Packet("PING " + type);
        packet.internet.setIP(pc.getIP(), "src");
        packet.internet.setIP(IP, "dest");
        return packet;
    }*/

    public void CreateICMP(string type, string ip)
    {
        this.type = type;
        this.ip = ip;
    }
}


