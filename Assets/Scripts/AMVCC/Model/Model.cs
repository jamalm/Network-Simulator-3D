public class Model : Entity
{
    //contains all the data models of the devices
    public PlayerModel player;
    //device models
    public PCModel pc;
    public RouterModel router;
    public SwitchModel swit;

    //interface model
    public PortModel port;

    public void Start()
    {
        player = GetComponentInChildren<PlayerModel>();
    }
}