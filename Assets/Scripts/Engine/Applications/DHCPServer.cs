using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHCPServer : MonoBehaviour {

    List<DHCPClient> clients = new List<DHCPClient>();
    public string netmask;
    public string gateway;
    public Dictionary<string, bool> IPList = new Dictionary<string, bool>();

    // Use this for initialization
    void Start () {
        netmask = GetComponent<Subnet>().mask;
        gateway = GetComponent<Subnet>().defaultGateway;
        GenerateLeases();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //handles incoming packets
    public void handle(Packet packet)
    {
        switch(packet.type)
        {
            case "DHCPDISCOVER":
                {
                    Offer(packet);
                    break;
                }
            case "DHCPREQUEST":
                {
                    Ack(packet);
                    break;
                }
        }
    }

    private void Offer(Packet packet)
    {
        //fill in lease addr, mask, gateway, cliMac, 
        DHCP dhcp = packet.GetComponent<DHCP>();
        dhcp.cliMac = packet.netAccess.getMAC("src");
        dhcp.mask = netmask;
        dhcp.gateway = gateway;
    }

    private void Ack(Packet packet)
    {

    }

    private void GenerateLeases()
    {
        int CIDR = GetComponent<Subnet>().CIDR;
        //calculate number of clients
        int hostBits = 32 - CIDR;
        //2^n-2 where n is hostbits
        int numclients = (int)Mathf.Pow(2, hostBits) - 2;

        string network = GetComponent<Subnet>().network;
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
