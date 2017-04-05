using System.Collections.Generic;
using UnityEngine;

/*************************************************
 * 
 * Author: Jamal Mahmoud
 * Last Updated: 10/11/16
 * 
 * Switch works on the second layer to forward packets based on the dest MAC address inside the local area network
 * switch ports hold the mac address of the device on the other end
 * 
 * 
 * ***********************************************/

public class Switch : MonoBehaviour {

	//private List<PC> pcs;           //list of pc's connected
	//private Router router;          //router connected
	public List<Port> ports;        //list of ports available
    public int numFEPorts = 8;            //number of ports to be assigned
    public int numGPorts = 1;              
	public List <string> macTable; //the mac table for forwarding packets
    private string id;
    GameObject engine;
    float timeElapsed = 0.0f;

    public void Load(SwitchData data)
    {
        /* (int i = 0; i < ports.Count; i++)
        {
            ports[i].Load(data.ports[i]);
        }
        macTable = data.mactable;*/
        numFEPorts = data.numFEPorts;
        numGPorts = data.numGPorts;
    }
    public SwitchData Save()
    {
        SwitchData data = new SwitchData();
        /*data.mactable = macTable;
        for(int i=0;i<ports.Count;i++)
        {
            data.ports[i] = ports[i].Save();
        }*/
        data.numGPorts = numGPorts;
        data.numFEPorts = numFEPorts;
        return data;
    }

	void Awake(){

        /* for(int i = 0; i < ports.Count; i++)
         {
             macTable.Add("");
         }*/
        macTable = new List<string>();
        ports = new List<Port>();
    }

	// Use this for initialization
	void Start () {

        int numPorts = numFEPorts + numGPorts;

        engine = GameObject.FindGameObjectWithTag("Engine");
        //initialising the ports
        for (int i=0;i<numPorts;i++)
        {
            Vector3 position = new Vector3(-6+(1.5f*(i)), 6, -7);
            //instantiate port and set this to parent transform
            ports.Add(Instantiate(engine.GetComponent<Engine>().PortPrefab, transform.position, transform.rotation));
            ports[i].transform.parent = transform;
            ports[i].transform.localPosition = position;
            //initialise the port types
            if(i >= numFEPorts)
            {
                ports[i].Init("g0/" + (i-numFEPorts));
            } else
            {
                ports[i].Init("fe0/" + i);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMacTable();
        if (timeElapsed > 3.0f && timeElapsed < 4.0f)
        {
            GameController.gameState.netState = GameController.NetworkState.ACTIVE;
        }
        timeElapsed += Time.deltaTime;
	}

    public void SetID(string id)
    {
        this.id = id;
    }
    public string GetID()
    {
        return id;
    }


    /*
	public Port getFEPort(bool newPort, string type){
		if (newPort) {
			Debug.Log ("SWITCH: finding new port to bind");
			for (int i = 0; i < ports.Count; i++) {
				//if port is not used and not the gigport
				if (!ports [i].isConnected () && !ports [i].getType ().Contains ("g")) {
					Debug.Log ("SWITCH: found port, looping?: " + i);
					return ports [i];
				}
			}
			return null; //TODO Make sure to write a fail case for this!!!
		} else {
			for (int i = 0; i < ports.Count; i++) {
				//if port is connected and is the same as specified
				if (ports [i].isConnected () && ports [i].getType ().Equals (type)) {
					return ports [i];
				}
			}
			return null; //TODO and here too!!!
		}

	}
	public Port getGPort(bool newPort){
		//if attaching a new port
		if (newPort) {
			for (int i = 0; i < ports.Count; i++) {
				//if port is gigabit port and not connected
				if (ports [i].getType ().Contains ("g") && !ports [i].isConnected ()) {
					return ports [i];
				}
			}
			return null;//TODO here too
		} // if searching for a used port 
		else {
			for (int i = 0; i < ports.Count; i++) {
				//if port is gigabit port and is connected
				if (ports [i].getType ().Contains ("g") && ports [i].isConnected ()) {
					return ports [i];
				}
			}
			return null; //TODO and here
		}

	}*/

    private void UpdateMacTable()
    {
        bool update = false;
        //perform update checks
        if(ports != null)
        {
            for (int i = 0; i < ports.Count; i++)                                    //for each port
            {
                if (ports[i].isConnected())                                         //if port is connected
                {
                    if (macTable.Count == 0)                                        //if there is no entries..
                    {
                        update = true;                                              //update required
                    }
                    else
                    {
                        for (int j = 0; j < macTable.Count; j++)                    //for each entry
                        {
                            if (macTable[j].Equals(ports[i].getMAC()))              //if the mactable has the port's mac address
                            {
                                update = false;                                     //no update needed
                                break;
                            }
                            else
                            {
                                update = true;                                      //else update is required
                            }
                        }
                    }

                    if (update)                                                     //if update is required
                    {
                        macTable.Add(ports[i].getMAC());                            //add port's mac address
                        Debug.Log(id + ": updating MAC table"
                    + "\nSWITCH: Port is " + ports[i].getType()
                    + "\nSWITCH: MAC is " + ports[i].getMAC());
                    }

                }
            }
        }
    }

    public void RemoveEntry(Port port)
    {
        macTable.Remove(port.getMAC());
    } 

    //fetch new port to be attached to external device
    public Port getNewPort(string type) 
    {
        Debug.Log(id + ": Finding new port to bind..");
        for(int i = 0; i < ports.Count; i++)
        {
            //if port is free and is compatible with cable
            if (!ports[i].isConnected() && ports[i].getType().Contains(type))
            {
                return ports[i];
            }
        }
        return null;
    }


    //ports are now id'd by the mac address of the device on the other end
	private Port getPort(string MAC){
		for (int i = 0; i < ports.Count; i++) {
			if(MAC.Equals(ports [i].getMAC ())){
				return ports[i];
			}
		}
		return null;
	}


    //send packet to all devices on LAN
	private bool broadcast(Packet packet, Port incomingPort) {
        
		for (int i = 0; i < ports.Count; i++) {
            if (ports[i].isConnected())
            {
                //if port is on the same vlan as the incoming one; VLANS YEAH
                if (ports[i].link.vlan == incomingPort.link.vlan || ports[i].link.type.Equals("trunk"))
                {
                    if (ports[i] != incomingPort)
                    {
                        ports[i].send(packet);
                    }
                }
                //if the packet has come from a trunk link
                else if (packet.gameObject.GetComponent<Link>())
                {
                    Link link = packet.GetComponent<Link>();
                    if (link.vlan == 0)
                    {
                        if (ports[i] != incomingPort)
                        {
                            ports[i].send(packet);
                        }
                    }
                }
            }
		}
        return true;
	}


    //forwards incoming packets
    //Transparent Bridge Algorithm
    /*
     * 1. if dest mac is in mactable, send to port
     * 2. if outport is the same as in port, ignore packet
     * 3. if dest mac is not in the mactable, flood vlan that port came from
     * 4. if dest mac is broadcast, flood vlan that port came from
     * 
     */
	public bool handlePacket(Packet packet, Port incomingPort){
		Debug.Log (id + ": Receiving packet");
        
        //dhcp check
        if (packet.type.Contains("DHCP")) 
        {
            if(packet.GetComponent<DHCP>().type.Equals("DHCPREQUEST"))
            {
                DHCP dhcp = packet.GetComponent<DHCP>();
                Debug.LogAssertion(id + ": Receiving DHCP PACKET of type: " + dhcp.type);
            }

        }
        bool IsInMacTable = false; //check if mac is recognised
        for (int i = 0; i < macTable.Count; i++)
        {
            //check if dest mac is in mactable
            if (packet.netAccess.getMAC("dest").Equals(macTable[i]))
            {
                IsInMacTable = true;
            }
        }
        //if it is....
        if(IsInMacTable)
        {
            //if outgoing port is the same as incoming OR if the vlans are not the same
            if (getPort(packet.netAccess.getMAC("dest")).Equals(incomingPort) || (getPort(packet.netAccess.getMAC("dest")).link.vlan != incomingPort.link.vlan))
            {
                //if tagged
                if(packet.gameObject.GetComponent<Link>())
                {
                    //it came from a trunk and is either going through another trunk, an access link, or it's faulty
                    if(incomingPort.link.type.Equals("trunk"))
                    {
                        if(getPort(packet.netAccess.getMAC("dest")).link.type.Equals("trunk"))
                        {
                            return getPort(packet.netAccess.getMAC("dest")).send(packet);
                        } else if(getPort(packet.netAccess.getMAC("dest")).link.type.Equals("access"))
                        {
                            //if its going out an access link, make sure to remove the vlan tag
                            Destroy(packet.gameObject.GetComponent<Link>());
                            return getPort(packet.netAccess.getMAC("dest")).send(packet);
                        }
                    }
                    //send it through trunked ports
                    if(getPort(packet.netAccess.getMAC("dest")).link.type.Equals("trunk"))
                    {
                        
                        return getPort(packet.netAccess.getMAC("dest")).send(packet);
                    }
                    //if the vlan tag matches the port
                    if(packet.gameObject.GetComponent<Link>().vlan.Equals(getPort(packet.netAccess.getMAC("dest")).link.vlan))
                    {
                        return getPort(packet.netAccess.getMAC("dest")).send(packet);
                    } else
                    {
                        return false;
                    }
                } else
                {
                    //if its not tagged, it's come from an access link and either going to a trunk or a faulty packet
                    if(incomingPort.link.type.Equals("access") && getPort(packet.netAccess.getMAC("dest")).link.type.Equals("trunk"))
                    {
                        //tag the packet with vlan info
                        Link vlanTag = packet.gameObject.AddComponent<Link>();
                        vlanTag.vlan = incomingPort.link.vlan;
                        //send packet through trunk
                        return getPort(packet.netAccess.getMAC("dest")).send(packet);
                    }
                    else
                    {
                        return false;
                    }
                }
            } //else if the mac address is broadcast, do it
            else if (packet.netAccess.getMAC("dest").Equals("FF:FF:FF:FF:FF:FF"))
            {
                
                return broadcast(packet, incomingPort); 
            }
            else
            {
                //else forward packet to port with same vlan
                return getPort(packet.netAccess.getMAC("dest")).send(packet);
                
            }
        }
        //if it isnt
        else
        {
            //just broadcast on vlan anyway , flood 
            return broadcast(packet, incomingPort);
        }
        /*
        //if mac is addressed to a broadcast, do so
		if (packet.netAccess.getMAC ("dest").Equals ("FF:FF:FF:FF:FF:FF")) {
            Debug.Log(id + ": BroadCast MAC ADDRESS!");
            broadcast (packet, incomingPort);
		}
        //else just unicast to MAC address user
        else
        {
			Debug.Log(id + ": Sending packet to specific MAC: " + packet.netAccess.getMAC("dest"));
            //iterate through mactable
			for (int i = 0; i < macTable.Count; i++) {
                //if dest mac matches one on table, forward
				if(packet.netAccess.getMAC("dest").Equals(macTable[i])){
					getPort (macTable[i]).send (packet);
				}
			}

            
        }*/

    }
    
    public void plug(Cable cable ,Port endPort, Port startPort)
    {
        cable.plug(startPort, endPort);
    }
}
