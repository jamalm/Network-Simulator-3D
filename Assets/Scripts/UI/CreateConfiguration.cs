using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateConfiguration : MonoBehaviour {

    public void AddElement(Dropdown selection)
    {
        int index = selection.value;
        switch(index)
        {
            case 0:
                {
                    //pc
                    ConfigurationManager.config.numPCs++;
                    break;
                }
            case 1:
                {
                    //routers
                    ConfigurationManager.config.numRouters++;
                    break;
                }
            case 2:
                {
                    //switches
                    ConfigurationManager.config.numSwitches++;
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
                    }
                    else
                    {
                        ConfigurationManager.config.numSwitches = 0;
                    }
                    break;
                }
        }
    }
}
