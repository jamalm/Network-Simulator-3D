//container class
using System;
using System.Collections.Generic;

[Serializable]
public class Configuration
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