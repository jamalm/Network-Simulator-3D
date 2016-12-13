public class CableModel : Entity
{
    private string port1;
    private string port2;
    private string type;

    private void Start()
    {
        type = null;
        port1 = null;
        port2 = null;
    }
    
    public string getType()
    {
        return type;
    }

    public void setType(string type)
    {
        this.type = type;
    }

    public void plug(string port1, string port2)
    {
        this.port1 = port1;
        this.port2 = port2;
    }
}