using UnityEngine;

public class PCPortController : Entity
{
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        //add notifications here
        switch(event_path)
        {
            case PortNotify.PcReceived:
                {
                    //receive message
                    break;
                }
            case PortNotify.PcSent:
                {
                    //send message
                    break;
                }
        }
    }
}