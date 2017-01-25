using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ConfigurationManager : MonoBehaviour
{
    public static ConfigurationManager config;
    private DataLoader loader;
    public string filename;

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
        filename = "\newfile.ns";
        if(config == null)
        {
            DontDestroyOnLoad(gameObject);
            config = this;
            pcs = new List<PCData>();
            routers = new List<RouterData>();
            switches = new List<SwitchData>();
            loader = GetComponent<DataLoader>();
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
        Configuration config = new Configuration(pcs, switches, routers);
        loader.Save(filename, config);
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
        } else
        {
            Debug.LogAssertion("ConfigurationManager: Unable to load data, file empty/missing");
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

