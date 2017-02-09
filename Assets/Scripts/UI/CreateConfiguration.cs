using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateConfiguration : MonoBehaviour {

    public GameObject content;

    public List<PCData> pcs = new List<PCData>();
    public List<RouterData> routers = new List<RouterData>();
    public List<SwitchData> switches = new List<SwitchData>();
    
    public void AddElement(Dropdown selection)
    {
        int index = selection.value;
        switch(index)
        {
            case 0:
                {
                    //pc
                    ConfigurationManager.config.numPCs++;
                    pcs.Add(new PCData());
                    break;
                }
            case 1:
                {
                    //routers
                    ConfigurationManager.config.numRouters++;
                    routers.Add(new RouterData());
                    break;
                }
            case 2:
                {
                    //switches
                    ConfigurationManager.config.numSwitches++;
                    switches.Add(new SwitchData());
                    break;
                }
        }
    }

    public void RemoveElement(Dropdown selection)
    {
        int index = selection.value;
        switch (index)
        {
            case 0:
                {
                    //pc
                    if (ConfigurationManager.config.numPCs > 0)
                    {
                        ConfigurationManager.config.numPCs--;
                        pcs.RemoveAt(ConfigurationManager.config.numPCs);
                    }
                    else
                    {
                        ConfigurationManager.config.numPCs = 0;
                    }
                    break;
                }
            case 1:
                {
                    //routers
                    if (ConfigurationManager.config.numRouters > 0)
                    {
                        ConfigurationManager.config.numRouters--;
                        routers.RemoveAt(ConfigurationManager.config.numRouters);
                    }
                    else
                    {
                        ConfigurationManager.config.numRouters = 0;
                    }
                    break;
                }
            case 2:
                {
                    //switches
                    if (ConfigurationManager.config.numSwitches > 0)
                    {
                        ConfigurationManager.config.numSwitches--;
                        switches.RemoveAt(ConfigurationManager.config.numSwitches);
                    }
                    else
                    {
                        ConfigurationManager.config.numSwitches = 0;
                    }
                    break;
                }
        }
    }

    //load elements into the config before starting
    public void CommitElements()
    {
        ConfigurationManager.config.pcs = pcs;
        ConfigurationManager.config.routers = routers;
        ConfigurationManager.config.switches = switches;
    }
}
