using System;

[Serializable]
public class PCData
{
    public PortData port;
    [NonSerialized]
    public Ping ping;
    public string MAC;
    public string IP;
}