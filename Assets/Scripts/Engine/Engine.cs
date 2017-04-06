using UnityEngine;
using System.Collections.Generic;

public class Engine : MonoBehaviour {

    //public static Engine engine;
    public GraphicManager graphics;
    public bool editing = false;
    bool challengeStarted = false;

    //setup info
    public List<PC> pcs = new List<PC>();
    public List<Switch> switches = new List<Switch>();
    public List<Router> routers = new List<Router>();
	public List<Cable> cables = new List<Cable> ();
    public List<int> damagedCables = new List<int>();

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
    public bool dhcp;

    //formatting device placement
    public float mapSize = 28;
    public float pc_gap;
    bool loaded;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        Debug.Log("Engine: Loading File.." + ConfigurationManager.config.filename);
        if(!editing)
            ConfigurationManager.config.Load(ConfigurationManager.config.filename);
        numPCs = ConfigurationManager.config.numPCs;
        numRouters = ConfigurationManager.config.numRouters;
        numSwitches = ConfigurationManager.config.numSwitches;
        damagedCables = ConfigurationManager.config.brokenCableList;

        numCables = (numPCs + numRouters + numSwitches) - 1;
        cableCount = 0;
        connected = false;
        pc_gap = mapSize / numPCs + 1;    //gap = size of the map / number of pcs
        Debug.Log("ENGINE: STARTED");
        LoadScene();
        //GetComponent<Layout_Converter>().Scene();
        
    }

    public void LoadScene()
    {
        //load devices
        LoadPCs();
        LoadRouters();
        LoadSwitches();
        loadCables();
    }

    // Update is called once per frame
    void Update () {
        
        float startTime, endtime;
        if (!connected)
        {

            connect();
            if (!editing)
            {
                ConfigSetup();
            }
            //SaveConfig();
        }


        /*
		//TODO press 1 to activate ping! 
		if (Input.GetKeyUp (KeyCode.Keypad1)) {
            startTime = Time.realtimeSinceStartup;
            //pcs[0].Ping(ping);
            StartCoroutine(PING(pcs[0]));
            endtime = Time.realtimeSinceStartup;
            Debug.LogWarning("Time Taken to Complete Ping: " + (endtime - startTime));
        }*/
        if(GameController.gameState.currentState.Equals(GameController.state.CHALLENGE))
            StartChallenge();
    }

    //test case
    /*
    private IEnumerator PING(PC pc)
    {
        pc.Ping(ping);
        yield return null;
    }*/
    /*

    public PC LoadPC(Transform transform, PCData load)
    {
        PC pc = (PC)Instantiate(PCPrefab, transform.position, transform.rotation);
        pc.Load(load);

        return pc;
    }

    public Router LoadRouter(Transform transform, RouterData load, int numPorts)
    {
        Router router = (Router)Instantiate(RouterPrefab, transform.position, transform.rotation);

        return router;
    }

    public Switch LoadSwitch(Transform transform, SwitchData load, int numPorts)
    {

    }*/

    void ConfigSetup()
    {
        for(int i=0;i<pcs.Count;i++)
        {
            //add config here from config manager
            pcs[i].Load(ConfigurationManager.config.GetPCData(i));
        }
        for (int i = 0; i < routers.Count; i++)
        {
            //add config here from config manager
            routers[i].Load(ConfigurationManager.config.GetRouterData(i));
        }
        for (int i = 0; i < switches.Count; i++)
        {
            //add config here from config manager
            switches[i].Load(ConfigurationManager.config.GetSwitchData(i));
        }
    }

    /* For Each Connection Created, there should be a physical cable counter
     * For now it's a manual set up 
     * 1 Router
     * 2 Switches
     * 4 PCs
     */
    void connect()
    {
        connected = true;
        int s = -1;  //for pc/switch dividing 

        if(numRouters > 0)
        {
            for (int i = 0; i < numRouters; i++)
            {

                //for each switch
                for (int j = 0; j < switches.Count; j++)
                {
                    //connect the ports between the switch and router
                    cables[cableCount].plug(switches[j].getNewPort("g"), routers[i].getNewPort("g"));
                    cableCount++;

                }
                for (int p = 0; p < pcs.Count; p++)
                {

                    //if theres more than one switch, divde the pcs up into fair sections for the switches
                    if (switches.Count > 1)
                    {
                        //if switch has gotten it's share of pcs, move onto the next switch
                        if (p % (pcs.Count/switches.Count) == 0)
                        {
                            
                            if (s != switches.Count - 1)
                            {
                                s++;
                            }
                        }
                        cables[cableCount].plug(switches[s].getNewPort("fe"), pcs[p].getNewPort());
                        cableCount++;
                    }
                    else
                    {
                        cables[cableCount].plug(switches[s + 1].getNewPort("fe"), pcs[p].getNewPort());
                        cableCount++;
                    }
                }
            }
        } else
        {
            for (int p = 0; p < pcs.Count; p++)
            {

                //if theres more than one switch, divde the pcs up into fair sections for the switches
                if (switches.Count > 1)
                {
                    //if switch has gotten it's share of pcs, move onto the next switch
                    if (p % switches.Count == 0)
                    {
                        if (s != switches.Count - 1)
                        {
                            s++;
                        }
                    }
                    cables[cableCount].plug(switches[s].getNewPort("fe"), pcs[p].getNewPort());
                    cableCount++;
                }
                else
                {
                    cables[cableCount].plug(switches[s + 1].getNewPort("fe"), pcs[p].getNewPort());
                    cableCount++;
                }
            }
        }
       
        /*
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
        }*/
    }


    //this method breaks the connections that are prescribed to be broken
    void Break()
    {
        //for faulty cables
        for (int i = 0; i < damagedCables.Count; i++)
        {
            //for each damaged cable index, set to faulty
            cables[damagedCables[i]].faulty = true;
        }
    }

    public void LoadPCs()
    {
        //Load PC's
        for (int i = 0; i < numPCs; i++)
        {
            pcs.Add(Instantiate(PCPrefab, new Vector3(-(mapSize / 2) + (pc_gap * i), transform.position.y, (mapSize / 2)), transform.rotation * Quaternion.AngleAxis(-90, Vector3.right)));
            pcs[i].SetID("PC" + (i + 1));
            pcs[i].gameObject.AddComponent<DHCPClient>();
            pcs[i].dhcpEnabled = dhcp;
        }
    }
    public void LoadRouters()
    {
        //load Routers
        for (int i = 0; i < numRouters; i++)
        {
            routers.Add(Instantiate(RouterPrefab, new Vector3(5 * i, 0, -10), Quaternion.Euler(-90, -90, 0)));
            routers[i].SetID("Router" + (i + 1));
        }
    }
    public void LoadSwitches()
    {
        //load Switches
        for (int i = 0; i < numSwitches; i++)
        {
            switches.Add((Switch)Instantiate(SwitchPrefab, new Vector3(5 * i, -0.5f, 0), transform.rotation));
            switches[i].SetID("Switch" + (i + 1));
        }
    }

    private Vector3 CablesPosition(Vector3 pos)
    {
        
        Vector3 cablePos = pos * 0.5f;
        return cablePos;
    }
    private void loadCables()
    {
        int r = -1;
        int s = -1;
        float percentage = 0.0f;
        if(numRouters > 0)
        {
            percentage = numSwitches / numRouters;
            for (int i = 0; i < numSwitches; i++)
            {

                if (numRouters > 1)
                {

                    if (i % percentage == 0)
                    {
                        if (r != numRouters - 1)
                        {
                            r++;
                        }
                    }
                    //for routers with switches
                    Vector3 routerPos = routers[r].transform.Find("PivotPoint").position;
                    Vector3 switPos = new Vector3(switches[i].transform.position.x, 0.0f, switches[i].transform.position.z);
                    Vector3 distance = (switPos - routerPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, routerPos + distance, transform.rotation));
                    cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(switPos, routerPos));
                    cables[i].transform.LookAt(routerPos);


                }
                else
                {
                    Vector3 routerPos = routers[r + 1].transform.Find("PivotPoint").position;
                    Vector3 switPos = new Vector3(switches[i].transform.position.x, 0.0f, switches[i].transform.position.z);
                    Vector3 distance = (switPos - routerPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, routerPos + distance, transform.rotation));
                    cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(switPos, routerPos));
                    cables[i].transform.LookAt(routerPos);
                }
            }
            percentage = numPCs / numSwitches;
            for (int i = 0; i < numPCs; i++)
            {

                if (numSwitches > 1)
                {
                    if (i % percentage == 0)
                    {
                        if (s != numSwitches - 1)
                        {
                            //this is to prevent out of bounds, just assign all remaining pc's to end computer
                            s++;
                        }

                    }
                    //for switches with pcs
                    Vector3 switPos = new Vector3(switches[s].transform.position.x, 0.0f, switches[s].transform.position.z);
                    Vector3 pcPos = pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    cables[i + numSwitches].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    cables[i + numSwitches].transform.LookAt(switPos);
                }
                else
                {
                    //for switches with pcs
                    Vector3 switPos = new Vector3(switches[s + 1].transform.position.x, 0.0f, switches[s + 1].transform.position.z);
                    Vector3 pcPos = pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    cables[i + numSwitches].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    cables[i + numSwitches].transform.LookAt(switPos);
                }

            }
        }
        else
        {
            percentage = numPCs / numSwitches;
            for (int i = 0; i < numPCs; i++)
            {

                if (numSwitches > 1)
                {
                    if (i % percentage == 0)
                    {
                        if (s != numSwitches - 1)
                        {
                            //this is to prevent out of bounds, just assign all remaining pc's to end computer
                            s++;
                        }

                    }
                    //for switches with pcs
                    Vector3 switPos = new Vector3(switches[s].transform.position.x, 0.0f, switches[s].transform.position.z);
                    Vector3 pcPos = pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    cables[i].transform.LookAt(switPos);
                }
                else
                {
                    //for switches with pcs
                    Vector3 switPos = new Vector3(switches[s + 1].transform.position.x, 0.0f, switches[s + 1].transform.position.z);
                    Vector3 pcPos = pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    cables[i].transform.LookAt(switPos);
                }

            }
        }
        


        /*
        //calculate pc cable positions
        Vector3[] pcCables = new Vector3[numPCs];
        Vector3[] routerCables = new Vector3[numRouters];
        Vector3[] switchCables = new Vector3[numSwitches];
        for (int i = 0; i < numPCs; i++)
        {
            pcCables[i] = pcs[i].transform.position * 0.5f;
        }
        if(numRouters > 0)
        {
            //calculate router cables positions

            for (int i = 0; i < numRouters; i++)
            {
                Transform pivot = routers[i].transform.Find("PivotPoint");
                routerCables[i] = pivot.position * 0.5f;
            }
        }
        if(numSwitches > 0)
        {
            for(int i=0;i<numSwitches; i++)
            {
                
            }
        }

        //display vectors
        for (int i = 0; i < pcCables.Length; i++)
        {
            Debug.Log("Cable pc vector " + i + ": " + pcCables[i]);
        }
        //Debug.Log("Cable router vector 0: " + routerCables[0]);

        //Load Cables
        for (int i = 0; i < numCables; i++)
        {
            if (i < (numPCs))
            { 
                //for pcs to switch
                cables.Add(Instantiate(CablePrefab, pcCables[i], transform.rotation));
                cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcs[i].transform.position, Vector3.zero));
            }
            if (i >= (numPCs) && numRouters > 0)
            {
                Transform pivot = routers[i - numPCs].transform;
                //for routers to switch
                cables.Add((Cable)Instantiate(CablePrefab, routerCables[i - numPCs], transform.rotation));
                cables[i].transform.localScale = new Vector3(0.1f, 0.1f, (Vector3.Distance(pivot.position, Vector3.zero)));
            }
            cables[i].transform.LookAt(Vector3.zero);

        }*/




    }

    

    public void SaveConfig()
    {
        ConfigurationManager.config.pcs = new List<PCData>();
        ConfigurationManager.config.routers = new List<RouterData>();
        ConfigurationManager.config.switches = new List<SwitchData>();
        for (int i=0;i<pcs.Count;i++)
        {
            
            ConfigurationManager.config.pcs.Add(pcs[i].Save());
            ConfigurationManager.config.numPCs = numPCs;
        }
        for(int i=0;i<routers.Count;i++)
        {
            ConfigurationManager.config.routers.Add(routers[i].Save());
            ConfigurationManager.config.numRouters = numRouters;
        }
        for(int i = 0; i < switches.Count; i++)
        {
            ConfigurationManager.config.switches.Add(switches[i].Save());
            ConfigurationManager.config.numSwitches = numSwitches;
        }
        ConfigurationManager.config.brokenCableList = damagedCables;
    }

    public void StartChallenge()
    {
        if(!challengeStarted)
        {
            challengeStarted = true;
            Break();
        }
    }
}
