using System;
using System.Collections.Generic;

[Serializable]
public class SwitchData
{
    public int numFEPorts;
    public int numGPorts;
    public List<int> vlanmaps;
}