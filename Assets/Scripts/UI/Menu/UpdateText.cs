using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour {

    private Text[] updateTexts;
	// Use this for initialization
	void Start () {
        updateTexts = GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        Refresh();
	}

    //updates the display of devices selected in game
    private void Refresh()
    {
        //order of text is :
        //Routers:0
        //PCs:1
        //Switches:2

        string[] newTexts = new string[3];

        newTexts[0] ="Routers: " +  ConfigurationManager.config.numRouters.ToString();
        newTexts[1] = "PCs: " + ConfigurationManager.config.numPCs.ToString();
        newTexts[2] = "Switches: " + ConfigurationManager.config.numSwitches.ToString();

        
        for(int i=0;i<newTexts.Length;i++)
        {
            //fill updated text with new information
            updateTexts[i+1].text = newTexts[i];
        }
    }
}
