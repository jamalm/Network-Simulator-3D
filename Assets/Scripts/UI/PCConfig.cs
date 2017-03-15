using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCConfig : MonoBehaviour {

    PC pc;
    string[] pingResults;
    public List<Text> pingreport = new List<Text>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdatePC(PC pc)
    {
        this.pc = pc;
    }

    public void IP(string ip)
    {
        pc.IP = ip;
    }

    public void MAC(string mac)
    {
        pc.MAC = mac;
    }

    public void Mask(string mask)
    {
        pc.GetComponent<Subnet>().mask = mask;
        pc.GetComponent<Subnet>().validMask = pc.GetComponent<Subnet>().ValidateMask(mask);
    }

    public void Gate(string gate)
    {
        pc.GetComponent<Subnet>().defaultGateway = gate;
    }
    public void Ping(InputField input)
    {
        pingResults = pc.Ping(input.text);
        //set the ping ip to input ip
        pingreport[0].text = pc.GetID() + ":> Ping " + input.text;
        pingreport[3].text = "Ping Statistics for " + input.text + "\n" +
                             "Sent: " + pingResults[0]+ "\n" +
                             "Received: " + pingResults[1] + "\n" +
                             "Lost: " + pingResults[2] ;
    }

    
}
