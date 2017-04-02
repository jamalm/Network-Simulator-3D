using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditScreen : MonoBehaviour {

    public GameObject PcConfigPrefab;
    public GameObject RouterConfigPrefab;
    public GameObject SwitchConfigPrefab;

    GameObject dev;
    InputField[] inputs;

    // Use this for initialization
    void Start () {
        //mainInputField.onEndEdit.AddListener(delegate { LockInput(mainInputField); });
        if(inputs != null)
        {
            for(int i=0;i<inputs.Length;i++)
            {
                inputs[i].onEndEdit.AddListener(delegate { ConfigureDevice(); });
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.B))
        {
            //close screen
            CloseScreen();
        }
	}

    void ConfigureDevice()
    {
        if(dev != null)
        {
            if(dev.GetComponent<PC>())
            {
                PC pc = dev.GetComponent<PC>();
                pc.subnet.EditScreenConfig(pc.IP);
                ConfigurePC(dev.GetComponent<PC>());
            }
        }
    }

    public void OpenScreen(string device, GameObject obj)
    {
        dev = obj;
        GetComponentInParent<SelectObject>().enabled = false;
        GetComponentInParent<LookAtMouse>().enabled = false;
        GetComponentInParent<Movement>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        switch (device)
        {
            case "pc":
                {
                    PcConfigPrefab.SetActive(true);
                    ConfigurePC(obj.GetComponent<PC>());
                    break;
                }
            case "router":
                {
                    RouterConfigPrefab.SetActive(true);
                    break;
                }
            case "switch":
                {
                    SwitchConfigPrefab.SetActive(true);
                    break;
                }
        }
    }


    private void CloseScreen()
    {
        GetComponentInParent<SelectObject>().enabled = true;
        GetComponentInParent<LookAtMouse>().enabled = true;
        GetComponentInParent<Movement>().enabled = true;
        PcConfigPrefab.SetActive(false);
        RouterConfigPrefab.SetActive(false);
        SwitchConfigPrefab.SetActive(false);
        GameObject.FindGameObjectWithTag("EditWindow").SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        dev = null;
        inputs = null;
    }

    private void ConfigurePC(PC pc)
    {
        
        
        //for data: 
        /*
         * 1. Subnet(subnet mask, gateway)
         * 2. PC(ip ,mac, dhcpenabled)
         * 3. DHCPClient(server address)
         * */
        List<string> data = new List<string>();
        //string ip, mac, mask, gateway, serverAddress;
        bool dhcpenabled = pc.dhcpEnabled;
        //init values
        /*
        ip = pc.IP;
        mac = pc.MAC;
        mask = pc.GetComponent<Subnet>().mask;
        gateway = pc.GetComponent<Subnet>().defaultGateway;
        serverAddress = pc.GetComponent<DHCPClient>().dhcpserver;
        */
        data.Add(pc.IP);
        data.Add(pc.MAC);
        data.Add(pc.GetComponent<Subnet>().mask);
        data.Add(pc.GetComponent<Subnet>().defaultGateway);
        data.Add(pc.GetComponent<DHCPClient>().dhcpserver);
        inputs = GetComponentsInChildren<InputField>();
        

        for(int i=0;i<data.Count;i++)
        {
            inputs[i].text = data[i];
            if(dhcpenabled)
            {
                inputs[i].interactable = false;
            }

        }
        Toggle dhcp = GetComponentInChildren<Toggle>();
        dhcp.isOn = dhcpenabled;

        

        //for applications
        //get ping
        Ping ping = pc.ping;
    }

    public void ToggleDHCP()
    {
        //get selected pc
        PC pc = dev.GetComponent<PC>();

        //get toggle involved
        Toggle dhcp = GetComponentInChildren<Toggle>();
        //switch logic on pc 
        pc.dhcpEnabled = dhcp.isOn;

        //toggle input fields
        for (int i = 0; i < 4; i++)
        {
            if (dhcp.isOn)
            {
                inputs[i].interactable = false;
            }
            else
            {
                inputs[i].interactable = true;
            }
            
        }
        ConfigurePC(pc);
    }
    
}
