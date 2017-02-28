using UnityEngine;

public class ICMP : MonoBehaviour {

    //internet layer handling

    private string type;
    
    /*
    public Packet CreatePacket(string IP, PC pc)
    {
        Packet packet = new Packet("PING " + type);
        packet.internet.setIP(pc.getIP(), "src");
        packet.internet.setIP(IP, "dest");
        return packet;
    }*/

    public void CreateICMP(string type)
    {
        this.type = type;
    }
}


