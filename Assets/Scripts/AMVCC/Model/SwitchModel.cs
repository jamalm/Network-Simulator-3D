using System.Collections.Generic;

public class SwitchModel : Entity
{
    //data for switch
    private List<Port> ports;
    private List<string> macTable;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public Port getPort(int index)
    {
        return ports[index];
    }
    public string getMAC(int index)
    {
        return macTable[index];
    }
}