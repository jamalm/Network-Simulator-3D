using System.Collections.Generic;
using System;

[Serializable]
public class RouterData
{
    public List<string> routingTable;
    public List<PortData> ports;
    public string MAC;
}