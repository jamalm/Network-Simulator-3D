using UnityEngine;
using System.Collections;

public class InspectorEngine : MonoBehaviour {

    private string deviceType;

    public PC PCPrefab;
    public Switch SwitchPrefab;
    public Router RouterPrefab;
    public Port PortPrefab;

    // Use this for initialization
    void Start () {
        
        
        
    deviceType = ConfigurationManager.config.currentInspectable;
        switch(deviceType)
        {
            case "PC":
                {
                    PC pc = (PC)Instantiate(PCPrefab, Vector3.zero, transform.rotation);
                    pc.Load(ConfigurationManager.config.inspectorPC);
                    //device = pc.GetComponent<GameObject>();
                    break;
                }
                /*
            case "Switch":
                {
                    Switch swit = GetComponent<Switch>();
                    swit.Load(ConfigurationManager.config.inspectorSwitch);
                    device = swit.GetComponent<GameObject>();
                    break;
                }
            case "Router":
                {
                    Router router = GetComponent<Router>();
                    router.Load(ConfigurationManager.config.inspectorRouter);
                    device = router.GetComponent<GameObject>();
                    break;
                }*/
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
