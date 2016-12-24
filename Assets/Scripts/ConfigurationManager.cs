using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ConfigurationManager : MonoBehaviour
{
    public static ConfigurationManager config;

    public List<PC> pcs;
    public List<Router> routers;
    public List<Switch> switches;
    public List<Cable> cables;

    public int numPCs;
    public int numRouters;
    public int numSwitches;

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


    //Saves data to a bin file
    public void Save()
    {
        //writer
        BinaryFormatter bf = new BinaryFormatter();
        // creates a file in a persistent location and naming it
        FileStream f = File.Open(Application.persistentDataPath + "/config.dat", FileMode.Create);

        //create a data container
        Configuration data = new Configuration(pcs, switches, routers);

        //serialise it and save it to file
        bf.Serialize(f, data);
        f.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/config.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream f = File.Open(Application.persistentDataPath + "/config.dat", FileMode.Open);
            Configuration data = (Configuration)bf.Deserialize(f);
            //then set vars to the data's files.. aka load them
        }
        else
        {
            Debug.Log("No file to load");
        }
    }
}

//container class
[Serializable]
class Configuration
{
    public List<PC> pcs;
    public List<Router> routers;
    public List<Switch> switches;
    public List<Cable> cables;

    //data kept here
    public Configuration(List<PC> pcs, List<Switch> switches, List<Router> routers)
    {
        //constructor
        this.pcs = pcs;
        this.routers = routers;
        this.switches = switches;
    }
}