using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RouterConfig : MonoBehaviour {

    Router router;
    public Dropdown list;
    public InputField mac;

    // Use this for initialization
	void Start () {
        //list.ClearOptions();
	}
	
	// Update is called once per frame
	void Update () {
        list.RefreshShownValue();
    }
    public void UpdateRouter(Router router)
    {
        this.router = router;
        //add the ip's to the dropdown list
        list.ClearOptions();
        list.AddOptions(router.RoutingTable.Keys.ToList());
        mac.text = router.getMAC();
    }

    public void AddRoute(InputField input)
    {
        //take input and check if it is an ip
        Regex ipRgx = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
        if (!ipRgx.IsMatch(input.text))
        {
            Debug.LogAssertion("UI: Invalid IP address; Check format");
        }
        else
        {
            //if it is valid 
            //create a temporary space for the port reference
            Port tempPort;
            //get port
            router.RoutingTable.TryGetValue(list.options[list.value].text, out tempPort);
            //remove entry from table
            router.RoutingTable.Remove(list.options[list.value].text);
            //add new entry to table
            router.RoutingTable.Add(input.text, tempPort);
            //change entry in dropdown now aswell
            list.options[list.value] = new Dropdown.OptionData(input.text);
        }
    }
    public void RemoveRoute()
    {

        //create a temporary space for the port reference
        Port tempPort;
        //get port
        router.RoutingTable.TryGetValue(list.options[list.value].ToString(), out tempPort);
        //remove entry from table
        router.RoutingTable.Remove(list.options[list.value].ToString());
        //add new entry to table
        router.RoutingTable.Add("0.0.0.0", tempPort);
    }
}
