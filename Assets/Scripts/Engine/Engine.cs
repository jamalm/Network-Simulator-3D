using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Engine : MonoBehaviour {

    private List<PC> pcs = new List<PC>();
    private List<Switch> switches = new List<Switch>();
    private List<Router> routers = new List<Router>();
	private List<Cable> cables = new List<Cable> ();

    public PC PCPrefab;
    public Switch SwitchPrefab;
    public Router RouterPrefab;
	public Cable CablePrefab;
	public Port PortPrefab;

    public int numPCs;
    public int numRouters;
    public int numSwitches;
	private int numCables;

    void Awake()
    {
        numPCs = 4;
        numSwitches = 1;
        numRouters = 1;
		numCables = (numPCs + numRouters + numSwitches) - 1;

		//load PCs
		//PC requires 1 port and a mac address
		for (int i = 0; i < numPCs; i++)
		{
			pcs.Add((PC)Instantiate(PCPrefab, new Vector3(5 * i, 1, 10), transform.rotation * Quaternion.AngleAxis(-90, Vector3.right)));
			pcs [i].port = (Port)Instantiate(PortPrefab ,pcs[i].transform.position, pcs[i].transform.rotation);
		}
		//load Switches
		//switch requires 1 port + x ports where x is the number of PCs' 
		for(int i = 0; i < numSwitches; i++)
		{
			switches.Add((Switch)Instantiate(SwitchPrefab, new Vector3(5 * i, 1, 0), transform.rotation));
			switches[i].transform.localScale -= new Vector3(0.9F, 0.9F, 0.9F);
			switches [i].ports.Add((Port)Instantiate(PortPrefab ,switches[i].transform.position, switches[i].transform.rotation));
			for(int j=0;j<pcs.Count;j++){
				switches [i].ports.Add((Port)Instantiate(PortPrefab ,switches[i].transform.position, switches[i].transform.rotation));
			}
		}
		//load Routers
		//router requires 2 port
		for(int i = 0; i < numRouters; i++)
		{
			routers.Add((Router)Instantiate(RouterPrefab, new Vector3(5 * i, 5, -10), transform.rotation));
			routers[i].transform.localScale -= new Vector3(0.9F, 0.9F, 0.9F);
			routers [i].ports.Add ((Port)Instantiate(PortPrefab ,routers[i].transform.position, transform.rotation));
			routers [i].ports.Add ((Port)Instantiate(PortPrefab ,routers[i].transform.position, transform.rotation));
		}

		//Load Cables
		for (int i = 0; i < numCables; i++) {
			cables.Add ((Cable)Instantiate(CablePrefab, new Vector3(0,10+i,0), transform.rotation));
		}

    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		//TODO press 1 to activate connection (TEST ONLY)
		if (Input.GetKey (KeyCode.Keypad1)) {
			connect ();
		}
	}

	//connect devices together
	private void connect(){

	}
}
