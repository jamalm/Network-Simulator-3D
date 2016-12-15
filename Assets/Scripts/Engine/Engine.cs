using UnityEngine;
using System.Collections.Generic;

public class Engine : MonoBehaviour {

    public List<PC> pcs = new List<PC>();
    public List<Switch> switches = new List<Switch>();
    public List<Router> routers = new List<Router>();
	public List<Cable> cables = new List<Cable> ();

    public PC PCPrefab;
    public Switch SwitchPrefab;
    public Router RouterPrefab;
	public Cable CablePrefab;
	public Port PortPrefab;

    public int numPCs;
    public int numRouters;
    public int numSwitches;
	private int numCables;
    private int cableCount;
    private bool connected;

    public string ping;     //this is for testing

    //formatting device placement
    private float mapSize = 28;
    private float pc_gap;

    void Awake()
    {
        //numPCs = 4;
        //numSwitches = 1;
        //numRouters = 1;
		numCables = (numPCs + numRouters + numSwitches) - 1;
        cableCount = 0;
        connected = false;
        pc_gap = mapSize / numPCs+1;    //gap = size of the map / number of pcs
    }

	// Use this for initialization
	void Start () {

        //load PCs
        //PC requires 1 port and a mac address
        for (int i = 0; i < numPCs; i++)
        {
            pcs.Add((PC)Instantiate(PCPrefab, new Vector3(-(mapSize/2) + (pc_gap*i), transform.position.y, (mapSize/2)) , transform.rotation * Quaternion.AngleAxis(-90, Vector3.right)));
            pcs[i].port = (Port)Instantiate(PortPrefab, pcs[i].transform.position, pcs[i].transform.rotation);
        }

        //init PCS config
        for (int i = 0; i < pcs.Count; i++)
        {
            pcs[i].TEST(i + 1);
        }

        //load Routers
        //router requires 3 ports
        for (int i = 0; i < numRouters; i++)
        {
            routers.Add((Router)Instantiate(RouterPrefab, new Vector3(5*i, 0, -10), Quaternion.Euler(-90, -90, 0)));
            routers[i].ports.Add((Port)Instantiate(PortPrefab, routers[i].transform.position, transform.rotation));
            routers[i].ports.Add((Port)Instantiate(PortPrefab, routers[i].transform.position, transform.rotation));
            routers[i].ports.Add((Port)Instantiate(PortPrefab, routers[i].transform.position, transform.rotation));
        }

        

        //load Switches
        //switch requires 1 port + x ports where x is the number of PCs' 
        for (int i = 0; i < numSwitches; i++)
        {
            switches.Add((Switch)Instantiate(SwitchPrefab, new Vector3(5 * i, -0.5f, 0), transform.rotation));
            switches[i].transform.localScale -= new Vector3(0.9F, 0.9F, 0.9F);
            switches[i].ports.Add((Port)Instantiate(PortPrefab, switches[i].transform.position, switches[i].transform.rotation));
            for (int j = 0; j < pcs.Count; j++)
            {
                switches[i].ports.Add((Port)Instantiate(PortPrefab, switches[i].transform.position, switches[i].transform.rotation));
            }
        }
        loadCables();
    }
	
	// Update is called once per frame
	void Update () {
        if (!connected)
        {
            connect();
        }
		//TODO press 1 to activate ping! 
		if (Input.GetKeyUp (KeyCode.Keypad1)) {
            pcs[0].Ping(ping);
        }
    }

    private void connect()
    {
        connected = true;
        //connect devices to switch
        for (int i = 0; i < switches.Count; i++)
        {//for each switch

            for (int j = 0; j < pcs.Count; j++)
            {//for every pc
                switches[i].plug(cables[cableCount], pcs[j].getNewPort(), switches[i].getNewPort("fe"));
                cableCount++;
                Debug.Log("ENGINE: PC " + (j + 1) + " connected to switch " + (i + 1) + "!");
            }
            for (int j = 0; j < routers.Count; j++)
            {
                switches[i].plug(cables[cableCount], routers[j].getNewPort("g"), switches[i].getNewPort("g"));
            }
        }
    }

    private Vector3 CablesPosition(Vector3 pos)
    {
        
        Vector3 cablePos = pos * 0.5f;
        return cablePos;
    }
    private void loadCables()
    {

        //calculate pc cable positions
        Vector3[] pcCables = new Vector3[numPCs];
        for (int i = 0; i < numPCs; i++)
        {
            pcCables[i] = pcs[i].transform.position * 0.5f;
        }
        //calculate router cables positions
        Vector3[] routerCables = new Vector3[numRouters];
        for (int i = 0; i < numRouters; i++)
        {
            Transform pivot = routers[i].transform.Find("PivotPoint");
            routerCables[i] = pivot.position*0.5f;
        }
        //display vectors
        for (int i = 0; i < pcCables.Length; i++)
        {
            Debug.Log("Cable pc vector " + i + ": " + pcCables[i]);
        }
        Debug.Log("Cable router vector 0: " + routerCables[0]);

        //Load Cables
        for (int i = 0; i < numCables; i++)
        {
            if (i < (numPCs))
            { 
                //for pcs to switch
                cables.Add((Cable)Instantiate(CablePrefab, pcCables[i], transform.rotation));
                cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcs[i].transform.position, Vector3.zero));
            }
            if (i >= (numPCs))
            {
                Transform pivot = routers[i - numPCs].transform;
                //for routers to switch
                cables.Add((Cable)Instantiate(CablePrefab, routerCables[i - numPCs], transform.rotation));
                cables[i].transform.localScale = new Vector3(0.1f, 0.1f, (Vector3.Distance(pivot.position, Vector3.zero)));
            }
            cables[i].transform.LookAt(Vector3.zero);

        }
    }
}
