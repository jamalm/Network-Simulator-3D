using UnityEngine;

public class ARP : MonoBehaviour
{
    public string type;
    public string ip;
    
    public void CreateARP(string type, string ip)
    {
        this.type = type;
        this.ip = ip;
    }
}
