using System.Collections.Generic;

public class PCPortModel : Entity
{
    //pc port info 
    private bool connected;
    private Cable cable;
    private string ip;
    private string mac;
    private Dictionary<string, string> arptable;

    private void Start()
    {
        connected = false;
        cable = null;
        ip = null;
        mac = null;
        arptable = new Dictionary<string, string>();
    }

    public bool isConnected()
    {
        return connected;
    }
    public void setConnected(bool connected)
    {
        this.connected = connected;
    }

    public Cable getCable()
    {
        return cable;
    }
    public void setCable(Cable cable)
    {
        this.cable = cable;
    }

    public string getIP()
    {
        return ip;
    }
    public void setIP(string ip)
    {
        this.ip = ip;
    }

    public string getMAC()
    {
        return mac;
    }
    public void setMAC(string mac)
    {
        if (this.mac == null)
        {
            this.mac = mac;
        }
    }

    public bool getARP(string ip)
    {
        if(arptable.ContainsKey(ip))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void setARP(string ip, string mac)
    {
        arptable.Add(ip, mac);
    }
}