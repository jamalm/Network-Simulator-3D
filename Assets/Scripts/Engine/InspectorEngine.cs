using UnityEngine;

public class InspectorEngine : MonoBehaviour {

    private string deviceType;

    public PC PCPrefab;
    public Switch SwitchPrefab;
    public Router RouterPrefab;
    public Cable CablePrefab;
    public Port PortPrefab;
    public Engine engine;
    public bool pcs, routers, switches;



    // Use this for initialization
    void Start () {

        engine = gameObject.AddComponent<Engine>();
        engine.PCPrefab = PCPrefab;
        engine.RouterPrefab = RouterPrefab;
        engine.SwitchPrefab = SwitchPrefab;
        engine.CablePrefab = CablePrefab;
        engine.PortPrefab = PortPrefab;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
