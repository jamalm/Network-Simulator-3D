using System;

public class ARP : Packet
{
	public ARP (string _type)
	{
        
		type = _type;
		netAccess = new NALayer ("Ethernet");
		internet = new InternetLayer ("IP");

		if (type.Equals ("ARP REQUEST")) {
			netAccess.setMAC ("FFFFFFFF", "dest");
		} 
	}
}
