public class PortModel : Entity
{
    public PCPortModel pport;
    public RouterPortModel rport;
    public SwitchPortModel sport;

    private void Start()
    {
        pport = GetComponentInChildren<PCPortModel>();
        rport = GetComponentInChildren<RouterPortModel>();
        sport = GetComponentInChildren<SwitchPortModel>();
    }
    private void Update()
    {
        
    }
}