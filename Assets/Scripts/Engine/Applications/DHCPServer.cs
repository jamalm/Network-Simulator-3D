using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DHCPServer : MonoBehaviour {

    public string netmask;
    public string gateway;
    public Dictionary<string, bool> IPList = new Dictionary<string, bool>();
    private Subnet subnet;
    public Port port;

    // Use this for initialization
    void Start () {
        /*netmask = GetComponent<Subnet>().mask;
        gateway = GetComponent<Subnet>().defaultGateway;
        GenerateLeases();*/
	}

    public void Setup(Subnet subnet_)
    {
        subnet = subnet_;
        port = subnet.GetComponent<Port>();
        netmask = subnet.mask;
        gateway = subnet.defaultGateway;
        GenerateLeases();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //handles incoming packets
    public void handle(Packet packet)
    {
        switch (packet.GetComponent<DHCP>().type)
        {
            case "DHCPDISCOVER":
                {

                    Debug.Log(GetComponent<Router>().GetID() + ": RECEIVED DHCP DISCOVER");
                    Offer(packet);
                    break;
                }
            case "DHCPREQUEST":
                {
                    Debug.Log(GetComponent<Router>().GetID() + ": RECEIVED DHCP REQUEST");
                    Ack(packet);
                    break;
                }
            case "DHCPRELEASE":
                {
                    break;
                }
            default:
                {
                    Debug.Log(GetComponent<Router>().GetID() + ": SERVER DROPPING DHCP PACKET " + packet.GetComponent<DHCP>().type);
                    break;
                }
        }
    }

    private void Offer(Packet packet)
    {
        //fill in lease addr, mask, gateway, cliMac, 
        DHCP dhcp = packet.GetComponent<DHCP>();
        dhcp.type = "DHCPOFFER";
        dhcp.cliMac = packet.netAccess.getMAC("src");
        dhcp.mask = netmask;
        dhcp.gateway = gateway;
        if((dhcp.leaseAddr = GetLease()) == null)
        {
            //handle no leases left
            Debug.LogAssertion("SERVERDHCP: NO LEASES LEFT");
        } else
        {
            packet.netAccess.setMAC(GetComponent<Router>().getMAC(), "src");
            packet.netAccess.setMAC(dhcp.cliMac, "dest");
            packet.internet.setIP(gateway, "src");
            packet.internet.setIP("255.255.255.255", "dest");

            port.send(packet);
        }
        
    }

    private void Ack(Packet packet)
    {
        IPList[packet.gameObject.GetComponent<DHCP>().cliAddr] = true;
        packet.GetComponent<DHCP>().type = "DHCPACK";
        packet.internet.setIP(packet.internet.getIP("src"), "dest");
        packet.internet.setIP(gateway, "src");
        
        packet.netAccess.setMAC(GetComponent<Router>().getMAC(), "src");
        packet.netAccess.setMAC(packet.GetComponent<DHCP>().cliMac, "dest");
        //GetComponent<PC>().sendPacket(packet);
        port.updateARPTable(packet.GetComponent<DHCP>().leaseAddr, packet.GetComponent<DHCP>().cliMac);
        port.send(packet);
    }

    private string GetLease()
    {

        if (IPList != null)
        {
            var ip = IPList.FirstOrDefault(x => x.Value == false).Key;
            if (ip != null)
            {
                IPList[ip] = true;
                return ip;
            }
            else
            {
                return null;
            }
        } else
        {
            return null;
        }

    }

    private void GenerateLeases()
    {
        int CIDR = subnet.CIDR;
        //calculate number of clients
        int hostBits = 32 - CIDR;
        //2^n-2 where n is hostbits
        int numclients = (int)Mathf.Pow(2, hostBits) - 2;

        string network = subnet.network;
        string[] netBitString = network.Split('.');
        
        if (CIDR >= 24)
        {
            
            //8 bits class C
            //only modify last octet i.e [3]
            for (int i = 0; i < numclients; i++)
            {
                string lease = "";
                //for each octet to be copied over
                for (int j=0;j<4;j++)
                {
                    //if last octet, add 1
                    if(j==3)
                    {
                        lease += (int.Parse(netBitString[j]) + (i + 1));
                    } else
                    {
                        lease += netBitString[j] + ".";
                    }
                    
                }

                //now add ip to list of leases
                if(i==0)
                {
                    //set gateway to be taken
                    IPList.Add(lease, true);
                } else
                {
                    //other hosts
                    //add ip + (i+1)
                    IPList.Add(lease, false);
                }
                
            }
        }
        else if (CIDR >= 16)
        {
            //16 bits class B
        }
        else if (CIDR > 8)
        {
            //24 bits class A
        }
    }
}
