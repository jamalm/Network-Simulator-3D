public class PortView : Entity
{
    public PCPortView pport;
    public RouterPortView rport;
    public SwitchportView sport;

    private void Start()
    {
        pport = GetComponentInChildren<PCPortView>();
        rport = GetComponentInChildren<RouterPortView>();
        sport = GetComponentInChildren<SwitchportView>();
    }
}