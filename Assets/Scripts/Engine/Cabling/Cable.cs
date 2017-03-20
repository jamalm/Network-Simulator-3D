using UnityEngine;

public class Cable : MonoBehaviour
{
	private string type;
	public Port port1;
	public Port port2;
    public bool faulty = false;
    public bool plugged = false;

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
        port2.plugIn(this, port1);
		this.port1 = port1;
		this.port2 = port2;
        plugged = true;
	}

    public void unplug()
    {
        port1.plugOut();
        port2.plugOut();
        //port1 = null;
        //port2 = null;
        plugged = false;
        //hide physical cable and increase collider along the x axis
        GetComponent<MeshRenderer>().enabled = false;
        faulty = true;
        GetComponent<BoxCollider>().size.Set(5, 1, 1);
    }

    public void replug()
    {
        GetComponent<MeshRenderer>().enabled = true;
        faulty = false;
        GetComponent<BoxCollider>().size.Set(1, 1, 1);
        
        plug(port1, port2);
    }

    


	//checks to see who's port is who's
	public bool send(Packet packet, Port sender){
		Debug.Log("CABLE: received packet ,forwarding..");
        if(!faulty && plugged)
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

