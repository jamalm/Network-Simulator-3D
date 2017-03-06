using UnityEngine;
using System.Collections.Generic;

public class Engine : MonoBehaviour {

    public GraphicManager graphics;

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
        

    }

	// Use this for initialization
	void Start () {
        numPCs = ConfigurationManager.config.numPCs;
        numRouters = ConfigurationManager.config.numRouters;
        numSwitches = ConfigurationManager.config.numSwitches;
        numCables = (numPCs + numRouters + numSwitches) - 1;
        cableCount = 0;
        connected = false;
        pc_gap = mapSize / numPCs + 1;    //gap = size of the map / number of pcs
        Debug.Log("ENGINE: STARTED");
        LoadScene();
    }

    public void LoadScene()
    {
        //load PCs
        
        for (int i = 0; i < numPCs; i++)
        {
            pcs.Add(Instantiate(PCPrefab, new Vector3(-(mapSize / 2) + (pc_gap * i), transform.position.y, (mapSize / 2)), transform.rotation * Quaternion.AngleAxis(-90, Vector3.right)));
            pcs[i].SetID("PC" + (i + 1));
            if(i==0)
            {
                //if first pc, set as dhcp server
                pcs[i].gameObject.AddComponent<DHCPServer>();
            } else
            {
                //otherwise its a client
                pcs[i].gameObject.AddComponent<DHCPClient>();
            }
        }

        //load Routers
        for (int i = 0; i < numRouters; i++)
        {
            routers.Add(Instantiate(RouterPrefab, new Vector3(5 * i, 0, -10), Quaternion.Euler(-90, -90, 0)));
            routers[i].SetID("Router" + (i + 1));
        }



        //load Switches
        for (int i = 0; i < numSwitches; i++)
        {
            switches.Add((Switch)Instantiate(SwitchPrefab, new Vector3(5 * i, -0.5f, 0), transform.rotation));
            switches[i].SetID("Switch" + (i + 1));
        }


        loadCables();
        //SaveConfig();
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

    public PC CreatePC(Transform transform)
    {
        PC pc = (PC)Instantiate(PCPrefab, transform.position, transform.rotation);
        return pc;
    }
    public PC LoadPC(Transform transform, PCData load)
    {
        PC pc = (PC)Instantiate(PCPrefab, transform.position, transform.rotation);
        pc.Load(load);

        return pc;
    }

    public Router CreateRouter(Transform transform, int numPorts)
    {
        Router router = (Router)Instantiate(RouterPrefab, transform.position, transform.rotation);
        for(int i = 0; i < numPorts; i++)
        {
            router.ports.Add((Port)Instantiate(PortPrefab, router.transform.position, transform.rotation));
        }
        

        return router;
    }
    public Router LoadRouter(Transform transform, RouterData load, int numPorts)
    {
        Router router = (Router)Instantiate(RouterPrefab, transform.position, transform.rotation);
        for (int i = 0; i < numPorts; i++)
        {
            router.ports.Add((Port)Instantiate(PortPrefab, router.transform.position, transform.rotation));
        }
        router.Load(load);
        return router;
    }

    public Switch CreateSwitch(Transform transform, int numPorts)
    {
        Switch swit = (Switch)Instantiate(SwitchPrefab, transform.position, transform.rotation);


        return swit;
    }

    private void connect()
    {
        connected = true;
        //connect devices to switch
        for (int i = 0; i < switches.Count; i++)
        {//for each switch

            for (int j = 0; j < pcs.Count; j++)
            {//for every pc
                Port port1, port2;
                port1 = pcs[j].getNewPort();
                port2 = switches[i].getNewPort("fe");
                if(port1 !=null && port2 != null)
                {
                    switches[i].plug(cables[cableCount], port1, port2);
                    cableCount++;
                    Debug.Log("ENGINE: PC " + (j + 1) + " connected to switch " + (i + 1) + "!");
                }
                else
                {
                    //port is missing?
                    Debug.LogAssertion("missing port? pc&switch connect", pcs[j]);
                }
                
            }
            for (int j = 0; j < routers.Count; j++)
            {
                Port port1, port2;
                port1 = routers[j].getNewPort("g");
                port2 = switches[i].getNewPort("g");
                if(port1 !=null && port2 !=null)
                {
                    switches[i].plug(cables[cableCount], port1, port2);
                }
                else
                {
                    //port is missing?
                    Debug.LogAssertion("missing port? router&switch connect", this);
                }
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



    private void SaveConfig()
    {
        for(int i=0;i<pcs.Count;i++)
        {
            ConfigurationManager.config.pcs.Add(pcs[i].Save());
        }
        for(int i=0;i<routers.Count;i++)
        {
            ConfigurationManager.config.routers.Add(routers[i].Save());
        }
        for(int i = 0; i < switches.Count; i++)
        {
            ConfigurationManager.config.switches.Add(switches[i].Save());
        }
    }
}
