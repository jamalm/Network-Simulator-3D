
public class Model : Entity
{
    //contains all the data models of the devices
    public PlayerModel player;

    //device models
    public PCModel[] pcs;
    public RouterModel[] routers;
    public SwitchModel[] switches;

    //interface model
    public PortModel[] ports;

    public void Start()
    {
        player = GetComponentInChildren<PlayerModel>();
        pcs = GetComponentsInChildren<PCModel>();
        routers = GetComponentsInChildren<RouterModel>();
        switches = GetComponentsInChildren<SwitchModel>();
        ports = GetComponentsInChildren<PortModel>();
    }
}