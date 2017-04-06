using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class ConfigurationManager : MonoBehaviour
{

    /*
     * Config manager tells Engine what to load
     * Number of devices: PCs, Switches, Routers
     * Device Configurations: 
     * PC: IP, MAC, mask, default gateway
     * Router: 
     * Switch: 
     */
    public static ConfigurationManager config;
    public DataLoader loader;
    public string filename;

    /// <summary>
    /// Configuration data for loading level
    /// </summary>
    public List<PCData> pcs = new List<PCData>();
    public List<RouterData> routers = new List<RouterData>();
    public List<SwitchData> switches = new List<SwitchData>();
    public List<int> brokenCableList = new List<int>();
    public int numPCs;
    public int numRouters;
    public int numSwitches;
    public List<Task> tasks = new List<Task>();
    public List<int> watchers = new List<int>();

    //redundant
    public PCData inspectorPC;
    public SwitchData inspectorSwitch;
    public RouterData inspectorRouter;
    public string currentInspectable;

    //singleton pattern
    void Awake()
    {
        if(config == null)
        {
            DontDestroyOnLoad(gameObject);
            config = this; 
        }
        else if(config != this)
        {
            Destroy(gameObject);
        }
        //Load(filename);
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    /*
     * Methods to get data from file for each device
     */
    public PCData GetPCData(int index)
    {
        return pcs[index];
    }
    public SwitchData GetSwitchData(int index)
    {
        return switches[index];
    }
    public RouterData GetRouterData(int index)
    {
        return routers[index];
    }
    public List<Task> GetTasks()
    {
        return tasks;
    }





    //save game here
    public void Save(string filename)
    {
        loader.Save(filename, pcs, routers, switches, brokenCableList, watchers,tasks);
    }
    //might be used in the future
    public List<string> LoadAllFiles()
    {
        List<string> saved = new List<string>();
        foreach(string a in Directory.GetFiles(Application.persistentDataPath, ".ns"))
        {
            saved.Add(a);
        }


        return saved;
    }
    //load files for new level
    public void Load(string filename)
    {
        Configuration data = (Configuration)loader.Load(filename);
        if(data != null)
        {
            pcs = data.GetPCs();
            routers = data.GetRouters();
            switches = data.GetSwitches();
            numPCs = pcs.Count;
            numSwitches = switches.Count;
            numRouters = routers.Count;
            brokenCableList = data.GetBrokenCables();
            watchers = data.GetWatchers();
            tasks = data.GetTasks();
        }
    }

    /*
    public void RunPC(PC pc)
    {
        inspectorPC = pc.Save();
        currentInspectable = "PC";
    }
    public void RunSwitch(Switch swit)
    {
        inspectorSwitch = swit.Save();
        currentInspectable = "Switch";
    }
    public void RunRouter(Router router)
    {
        inspectorRouter = router.Save();
        currentInspectable = "Router";
    }
    */

    
}

//container class
[Serializable]
class Configuration
{
    //device info
    List<PCData> pcs;
    List<RouterData> routers;
    List<SwitchData> switches;

    //misc data
    List<Task> tasks;
    List<int> brokenCables;
    List<int> watchers;

    //constructor with tasks 
    public Configuration(List<PCData> pcs, List<SwitchData> switches, List<RouterData> routers, List<int> broken, List<int> watched,  List<Task> tasks)
    {
        //constructor
        this.pcs = pcs;

        this.routers = routers;

        this.switches = switches;

        this.tasks = tasks;

        brokenCables = broken;
    }

    //constructor without tasks
    public Configuration(List<PCData> pcs, List<SwitchData> switches, List<RouterData> routers, List<int> broken, List<int> watchers)
    {
        //constructor
        this.pcs = pcs;

        this.routers = routers;

        this.switches = switches;

        this.watchers = watchers;

        tasks = null;
        brokenCables = broken;    
    }

    public List<PCData> GetPCs()
    {
        return pcs;
    }
    public List<RouterData> GetRouters()
    {
        return routers;
    }
    public List<SwitchData> GetSwitches()
    {
        return switches;
    }
    public List<int> GetBrokenCables()
    {
        return brokenCables;
    }
    public List<Task> GetTasks()
    {
        return tasks;
    }
    public List<int> GetWatchers()
    {
        return watchers;
    }
}