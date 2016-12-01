
public class ICMP {

    //internet layer handling

    private string type;

    public ICMP(string type)
    {
        this.type = type;
    }

    public Packet CreatePacket(string IP, PC pc)
    {
        Packet packet = new Packet("PING " + type);
        packet.internet.setIP(pc.getIP(), "src");
        packet.internet.setIP(IP, "dest");
        return packet;
    }

    public Packet CreatePacket(string IP, Router router)
    {
        return null; //for now
    }
}


