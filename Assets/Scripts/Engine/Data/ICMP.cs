using UnityEngine;

public class ICMP : MonoBehaviour {
    /*******************************************************************************/
    /* Component to be attached to a packet being sent as a Ping Echo or Reply
    /* Allows devices to distinguish packet as an ICMP packet.
    /***********************************************************/

    //type of ICMP message
    public string type;
    //IP address being pinged
    public string ip;

    public void CreateICMP(string type, string ip)
    {
        this.type = type;
        this.ip = ip;
    }
}


