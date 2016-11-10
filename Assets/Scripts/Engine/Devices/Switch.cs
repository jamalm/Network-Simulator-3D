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

	private List<PC> pcs;           //list of pc's connected
	private Router router;          //router connected
	public List<Port> ports;        //list of ports available
	private List <string> macTable; //the mac table for forwarding packets

	void Awake(){
       
       /* for(int i = 0; i < ports.Count; i++)
        {
            macTable.Add("");
        }*/
	}

	// Use this for initialization
	void Start () {
        //initialising the ports
        ports[0].switchInit("fe0/0", this);
        ports[1].switchInit("fe0/1", this);
        ports[2].switchInit("fe0/2", this);
        ports[3].switchInit("fe0/3", this);
        ports[4].switchInit("g0/0", this);
        //init the mactable
        macTable = new List<string>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMacTable();
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
        for(int i = 0; i < ports.Count; i++)
        {
            if (ports[i].isConnected())
            {
                if (macTable.Count == 0)
                {
                    update = true;
                } else
                {
                    for (int j = 0; j < macTable.Count; j++)
                    {
                        if (macTable[j].Equals(ports[i].getMAC()))
                        {
                            update = false;
                            break;
                        }
                        else
                        {
                            update = true;
                        }
                    }
                }
                
                if (update)
                {
                    macTable.Add(ports[i].getMAC());
                    Debug.Log("SWITCH: updating MAC table"
                + "\nSWITCH: Port is " + ports[i].getType()
                + "\nSWITCH: MAC is " + ports[i].getMAC());
                }

            }
        }
        
    }

    //fetch new port to be attached to external device
    public Port getNewPort(string type) 
    {
        Debug.Log("SWITCH: Finding new port to bind..");
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
	private void broadcast(Packet packet, Port incomingPort) {
		for (int i = 0; i < ports.Count; i++) {
			if (ports [i].isConnected () && ports[i] != incomingPort) {
				ports [i].send (packet);
			}
		}
	}


    //forwards incoming packets
	public void handlePacket(Packet packet, Port incomingPort){
		Debug.Log ("Switch: Receiving packet");

        //if mac is addressed to a broadcast, do so
		if (packet.netAccess.getMAC ("dest").Equals ("FFFFFFFF")) {
            Debug.Log("SWITCH: BroadCast MAC ADDRESS!");
            broadcast (packet, incomingPort);
		}
        //else just unicast to MAC address user
        else
        {
			Debug.Log("SWITCH: Sending packet to specific MAC: " + packet.netAccess.getMAC("dest"));
            //iterate through mactable
			for (int i = 0; i < macTable.Count; i++) {
                //if dest mac matches one on table, forward
				if(packet.netAccess.getMAC("dest").Equals(macTable[i])){
					getPort (macTable[i]).send (packet);
				}
			}
            
        }
	}


    public void plugPC(Cable cable ,Port PCPort, Port SwitchPort)
    {
        cable.plug(SwitchPort, PCPort);
    }

    public void plugRouter(Cable cable, Port RPort, Port SwitchPort)
    {
        cable.plug(SwitchPort, RPort);
    }
}
