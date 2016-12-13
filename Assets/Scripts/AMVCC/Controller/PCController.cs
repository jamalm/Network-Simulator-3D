using UnityEngine;

public class PCController : Entity
{
    //controls for pc
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        Debug.Log("Inside pc's controller notification: " + event_path);
        switch(event_path)
        {
            case PCNotify.PcActivatedPing:
                {
                    //TODO HERE 
                    
                    break;
                }
            case PCNotify.PcSentPacket:
                {
                    break;
                }
            case PCNotify.PcReceivedPacket:
                {
                    break;
                }
        }
    }
}