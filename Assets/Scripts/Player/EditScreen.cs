using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditScreen : MonoBehaviour {

    public GameObject PcConfigPrefab;
    public GameObject RouterConfigPrefab;
    public GameObject SwitchConfigPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenScreen(string device)
    {
        switch(device)
        {
            case "pc":
                {
                    PcConfigPrefab.SetActive(true);
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
}
