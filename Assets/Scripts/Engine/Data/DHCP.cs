using UnityEngine;

public class DHCP : MonoBehaviour{

    public string type; //type of dhcp message
    public string cliMac;   //clients mac address
    public string mask; //netmask
    public string gateway;  //default gateway
    public string cliAddr; //clients current ip
    public string leaseAddr;    //clients "to-be" address
    public string servAddr; //server address


    public void CreateDHCP(string type)
    {
        this.type = type;
    }
}