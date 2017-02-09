using System.Collections.Generic;
using System;

[Serializable]
public class RouterData
{
    public List<string> routingTable = new List<string>();
    public List<PortData> ports = new List<PortData>();
    public string MAC;
}