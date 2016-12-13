using UnityEngine;

public class SwitchPortController : Entity
{
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        //add notifications here
        switch(event_path)
        {
            case PortNotify.SwitchReceived:
                {
                    break;
                }
            case PortNotify.SwitchSent:
                {
                    break;
                }
        }
    }
}