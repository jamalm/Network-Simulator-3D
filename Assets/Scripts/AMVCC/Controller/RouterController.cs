using UnityEngine;

public class RouterController : Entity
{
    //controlled events for router
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        switch(event_path)
        {
            case RouterNotify.RouterReceivedPacket:
                {
                    break;
                }
            case RouterNotify.RouterSentPacket:
                {
                    break;
                }
            case RouterNotify.RouterDroppedPacket:
                {
                    break;
                }
        }
    }
}