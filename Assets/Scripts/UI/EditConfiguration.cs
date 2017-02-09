using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditConfiguration : MonoBehaviour {

    public Dropdown list;
    public CreateConfiguration devices;
    public GameObject PCPanel, RouterPanel, SwitchPanel;

    

    //select panel based on device selected in dropdown
    public void PanelSelect()
    {
        if(list.options.Count > 0)
        {
            if (list.options[list.value].text.Contains("PC"))
            {
                PCPanel.SetActive(true);
                RouterPanel.SetActive(false);
                SwitchPanel.SetActive(false);
            }
            else if (list.options[list.value].text.Contains("Router"))
            {
                PCPanel.SetActive(false);
                RouterPanel.SetActive(true);
                SwitchPanel.SetActive(false);
            }
            else if (list.options[list.value].text.Contains("Switch"))
            {
                PCPanel.SetActive(false);
                RouterPanel.SetActive(false);
                SwitchPanel.SetActive(true);
            }
        }
        else
        {
            PCPanel.SetActive(false);
            RouterPanel.SetActive(false);
            SwitchPanel.SetActive(false);
        }
       
    }

    private void UpdatePCData(string value, string type)
    {
        int deviceIndex = list.value;
        switch (type)
        {
            case "IP":
                {
                    devices.pcs[deviceIndex].IP = value;
                    break;
                }
            case "MAC":
                {
                    devices.pcs[deviceIndex].MAC = value;
                    break;
                }
        }
    }
    private void UpdateRouterData(string value, string type)
    {
        //routers is in devices behind pcs
        int deviceIndex = list.value - (ConfigurationManager.config.numPCs);
        switch(type)
        {
            case "MAC":
                {
                    devices.routers[deviceIndex].MAC = value;
                    break;
                }
            case "table":
                {
                    devices.routers[deviceIndex].routingTable.Add(value);
                    break;
                }
        }
    }

    private void UpdateSwitchData(string value, string type)
    {
        int deviceIndex = list.value - ((ConfigurationManager.config.numRouters) + (ConfigurationManager.config.numPCs));
        switch(type)
        {
            case "mactable":
                {
                    devices.switches[deviceIndex].mactable.Add(value);
                    break;
                }
        }
    }
        
        /*
        if ( deviceIndex <= (ConfigurationManager.config.numPCs - 1))
        {
            //we have the device data file
            devices.pcs[deviceIndex].IP = value;
        }
        deviceIndex -= (ConfigurationManager.config.numPCs - 1);
        if (deviceIndex <= (ConfigurationManager.config.numRouters-1))
        {
            devices.routers[deviceIndex].IP = value;
        }
        deviceIndex -= (ConfigurationManager.config.numRouters - 1);
        if(deviceIndex <= (ConfigurationManager.config.numSwitches -1))
        {
            devices.switches[deviceIndex]. = value;
        }*/
    

    public void UpdatePCIP(InputField field)
    {
        
        UpdatePCData(field.text, "IP");
    }

    public void UpdatePCMAC(InputField field)
    {
        UpdatePCData(field.text, "MAC");
    }

    public void UpdateRouterMAC(InputField field)
    {
        UpdateRouterData(field.text, "MAC");
    }
    public void UpdateRouterTable(InputField field)
    {
        UpdateRouterData(field.text, "table");
    }
    public void UpdateSwitchMactable(InputField field)
    {
        UpdateSwitchData(field.text, "mactable");
    }

    

    //more methods can be added to accomodate more fields
}
