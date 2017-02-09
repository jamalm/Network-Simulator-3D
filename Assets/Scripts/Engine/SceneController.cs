using System;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Engine engine;
    string filename = ConfigurationManager.config.filename;

    void Start()
    {
        //load configuration file from disk
        engine.LoadConfig(filename);

        //load devices
        LoadDevices();
        
    }

    void Update()
    {

    }

    private void LoadDevices()
    {
        //create devices
        //PCS first
        for (int i = 0; i < engine.numPCs; i++)
        {
            engine.LoadPC(transform, ConfigurationManager.config.pcs[i]);
        }
        //routers next
        for (int i = 0; i < engine.numRouters; i++)
        {
            engine.LoadRouter(transform, ConfigurationManager.config.routers[i], 3);
        }
        for (int i = 0; i < engine.numSwitches; i++)
        {
            engine.LoadSwitch(transform, ConfigurationManager.config.switches[i], engine.numPCs + 1);
        }
    }
}