using UnityEngine;

public class PortController : Entity
{
    PCPortController pport;
    SwitchPortController sport;
    RouterPortController rport;

    private void Start()
    {
        pport = GetComponentInChildren<PCPortController>();
        rport = GetComponentInChildren<RouterPortController>();
        sport = GetComponentInChildren<SwitchPortController>();
    }

    public void OnNotification(string event_path, Object o, params object[] data)
    {
        string trimmed = cutEvent(event_path, "port.");
        if(trimmed.StartsWith("pc"))
        {
            pport.OnNotification(trimmed, o, data);
        }
        else if(trimmed.StartsWith("router"))
        {
            rport.OnNotification(trimmed, o, data);
        }
        else if(trimmed.StartsWith("switch"))
        {
            sport.OnNotification(trimmed, o, data);
        }
    }

    private string cutEvent(string event_path, string e)
    {
        //cuts off the head of the event_path as it travels
        event_path.Remove(event_path.IndexOf(e), e.Length);
        return event_path;
    }
}