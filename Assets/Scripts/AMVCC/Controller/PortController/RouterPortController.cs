using UnityEngine;

public class RouterPortController : Entity
{
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        //add notifications here
        switch(event_path)
        {
            case PortNotify.RouterSent:
                {
                    break;
                }
            case PortNotify.RouterReceived:
                {
                    break;
                }
        }
    }
}