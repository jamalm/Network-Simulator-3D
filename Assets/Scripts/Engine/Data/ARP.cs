using UnityEngine;

public class ARP : MonoBehaviour
{
    /*******************************************************************************/
    /* Component to be attached to a packet being sent as a ARP Request or Reply
    /* Allows devices to distinguish packet as an ARP packet.
    /***********************************************************/

    public string type;
    //ip that is being binded or replied to
    public string ip;
    
    public void CreateARP(string type, string ip)
    {
        this.type = type;
        this.ip = ip;
    }
}
