using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ConfigurationManager : MonoBehaviour
{
    public static ConfigurationManager config;
    public DataLoader loader;
    

    public List<PCData> pcs;
    public List<RouterData> routers;
    public List<SwitchData> switches;

    public int numPCs;
    public int numRouters;
    public int numSwitches;

    public PCData inspectorPC;
    public SwitchData inspectorSwitch;
    public RouterData inspectorRouter;
    public string currentInspectable;

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
    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void Save(string filename)
    {
        loader.Save(filename, pcs, routers, switches);
    }
    public List<string> LoadAllFiles()
    {
        List<string> saved = new List<string>();
        foreach(string a in Directory.GetFiles(Application.persistentDataPath, ".ns"))
        {
            saved.Add(a);
        }


        return saved;
    }
    public void Load(string filename)
    {
        Configuration data = (Configuration)loader.Load(filename);
        if(data != null)
        {
            pcs = data.pcs;
            routers = data.routers;
            switches = data.switches;
        }
    }


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


    
}

//container class
[Serializable]
class Configuration
{
    public List<PCData> pcs;
    public List<RouterData> routers;
    public List<SwitchData> switches;

    //data kept here
    public Configuration(List<PCData> pcs, List<SwitchData> switches, List<RouterData> routers)
    {
        //constructor
        this.pcs = pcs;
        this.routers = routers;
        this.switches = switches;
    }
}