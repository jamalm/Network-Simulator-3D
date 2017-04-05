using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SwitchConfig : MonoBehaviour {

    Switch swit;
    public Dropdown list;
    public InputField mac;
    public InputField vlan;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        list.RefreshShownValue();
	}

    //for updating current device selected
    public void UpdateSwitch(Switch newswit)
    {
        swit = newswit;
        //add ports to the dropdown list
        list.ClearOptions();
        List<string> portIDs = new List<string>();
        //init a string list to put into the dropdown
        for(int i = 0; i < swit.ports.Count; i++)
            portIDs.Add(swit.ports[i].getType());

        list.AddOptions(portIDs);
        //the mac address of the port is the end user's mac address
        SetMAC();
        SetVLAN();
    }

    public void SetMAC()
    {
        mac.text = swit.ports[list.value].mac;
    }
    public void SetVLAN()
    {
        if (!swit.ports[list.value].isConnected())
            vlan.text = "";
        else 
            vlan.text = swit.ports[list.value].link.vlan.ToString();
        
    }

    public void EditVLAN(InputField input)
    {
        //take input and check if it is a number
        //take input and check if it is an ip
        Regex ipRgx = new Regex(@"^[0-9]+$");
        if (!ipRgx.IsMatch(input.text))
        {
            Debug.LogAssertion("UI: Invalid VLAN; Check format");
        }
        else
        {
            //if valid VLAN 
            //get Port associated
            Port port = swit.ports[list.value];
            //update VLAN of port and endport aswell
            port.link.vlan = int.Parse(input.text);
            port.GetEnd().link.vlan = int.Parse(input.text);

            
        }
    }
}
