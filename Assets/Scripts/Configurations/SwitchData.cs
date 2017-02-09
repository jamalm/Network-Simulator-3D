using System.Collections.Generic;
using System;

[Serializable]
public class SwitchData
{
    public List<PortData> ports = new List<PortData>();
    public List<string> mactable = new List<string>();
}