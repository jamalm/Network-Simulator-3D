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

}
