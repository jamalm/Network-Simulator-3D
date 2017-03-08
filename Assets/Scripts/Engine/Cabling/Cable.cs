using UnityEngine;
using System.Collections;

public class Cable : MonoBehaviour
{
	private string type;
	public Port port1;
	public Port port2;
    public bool faulty = false;

	// Use this for initialization
	void Start ()
	{
		type = "Copper Straight";
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public string getType(){
		return type;
	}

	public void plug(Port port1, Port port2){
		Debug.Log("CABLE: Plugging in ports to cable");
		port1.plugIn (this, port2);
		this.port1 = port1;
		this.port2 = port2;
	}

    public void unplug()
    {
        
        port1 = null;
        port2 = null;
        Destroy(gameObject);
    }

    


	//checks to see who's port is who's
	public bool send(Packet packet, Port sender){
		Debug.Log("CABLE: received packet ,forwarding..");
        if(!faulty)
        {
            //if port1's device is the same as the sender..
            if (sender.getDevice().Equals(port1.getDevice()))
            {
                return port2.receive(packet);//send to port 2

            }
            else
            {
                return port1.receive(packet);//send to port 1

            }
        } else 
        {
            return false;
        }

	}
}

