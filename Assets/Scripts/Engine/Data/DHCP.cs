using UnityEngine;

public class DHCP : MonoBehaviour{
    /*******************************************************************************/
    /* Component to be attached to a packet being sent as a DHCP datagram
    /* Allows devices to distinguish packet as a DHCP packet.
     * Contains information for the client and the server to communicate also
    /***********************************************************/

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