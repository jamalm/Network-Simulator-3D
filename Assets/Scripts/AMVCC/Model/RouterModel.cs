using System.Collections.Generic;
using UnityEngine;

public class RouterModel : Entity
{
    //data for router
    private List<Port> ports;
    private string MAC;

    private void Start()
    {
        Random.InitState(42);
        MAC = ((int)Random.Range(10000, 99999)).ToString();
    }

    private void Update()
    {
        //check for new computers here and incoming data packets
    }

    public string getMAC()
    {
        return MAC;
    }
    public Port getPort(int index)
    {
        return ports[index];
    }
}