using System.Collections.Generic;
public class PortData
{
    public string type;
    public bool connected;
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