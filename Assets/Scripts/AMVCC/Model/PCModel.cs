using UnityEngine;

public class PCModel : Entity
{
    //data for PC
    private string MAC;
    private PCPort port;      //TODO if port only will ever have one interface! 
    private Ping ping;

    private void Start()
    {
        Random.InitState(42);
        MAC = ((int)Random.Range(10000.0f, 99999.0f)).ToString();
        port = gameObject.AddComponent<PCPort>();
        ping = gameObject.AddComponent<Ping>();
    }

    public string getMAC()
    {
        return MAC;
    }

    public PCPort getPort()
    {
        if(port != null)
        {
            return port;
        }
        else
        {
            Debug.Log("This computer has no interfaces!");
            return null;
        }
    }
}