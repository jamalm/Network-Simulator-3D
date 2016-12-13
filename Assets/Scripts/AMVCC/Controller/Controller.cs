using UnityEngine;

public class Controller : Entity
{
    public PlayerController player;
    public PCController pc;
    public RouterController router;
    public PortController port;
   

    private void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        pc = GetComponentInChildren<PCController>();
        router = GetComponentInChildren<RouterController>();
        port = GetComponentInChildren<PortController>();
    }

    public void OnNotification(string event_path, Object o, params object[] data)
    {
        if (event_path.Contains("port"))
        {
            port.OnNotification(event_path, o, data);
        }
        else if (event_path.Contains("pc"))
        {
            pc.OnNotification(event_path, o, data);
        }
        else if (event_path.Contains("router"))
        {
            router.OnNotification(event_path, o, data);
        }
        else if (event_path.Contains("player"))
        {
            player.OnNotification(event_path, o, data);
        }
    }
    
}