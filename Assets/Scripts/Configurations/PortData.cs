using System.Collections.Generic;
using System;

[Serializable]
public class PortData
{
    public string type;
    public bool connected;
    [NonSerialized]
    public Cable cable;
    public string device;
    public string ip;
    public string mac;
    public string endPortMAC;
    //public PCData pc;
    //public SwitchData swit;
    //public RouterData router;
    public Dictionary<string, string> arpTable;
}