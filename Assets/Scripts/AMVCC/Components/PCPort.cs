using UnityEngine;
using System.Collections.Generic;

public class PCPort : MonoBehaviour
{
    private bool connected;
    private Cable cable;
    private string ip;
    private string mac;
    private Dictionary<string, string> arptable;

    private void Awake()
    {
        arptable = new Dictionary<string, string>();
    }

    private void Start()
    {
        
        cable = null;
        ip = null;
        mac = null;
        connected = false;
    }

    private void Update()
    {

    }

    public Cable getCable()
    {
        return cable;
    }
    public bool plugCable(Cable _cable)
    {
        if (cable == null)
        {
            cable = _cable;
            connected = true;
        }
        else
        {
            return false;
        }
        return connected;
    }
    public bool unPlugCable(Cable _cable)
    {
        if(_cable == cable)
        {
            cable = null;
            connected = false;
        }
        else
        {
            return false;
        }
        return !connected;
    }

    public bool isConnected()
    {
        return connected;
    }

    public string getIP()
    {
        return ip;
    }
    public void setIP(string _ip)
    {
        ip = _ip;
    }

    public string getMAC()
    {
        return mac;
    }
    public void setMAC(string _mac)
    {
        mac = _mac;
    }
    public void updateARPTable(string ip, string mac)
    {
        arptable.Add(ip, mac);
    }


    public bool checkARP(Packet packet)
    {
        if (arptable.ContainsValue(packet.internet.getMAC("dest")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void send(Packet packet)
    {
        
    }

}