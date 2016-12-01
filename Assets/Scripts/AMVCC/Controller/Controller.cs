using UnityEngine;

public class Controller : Entity
{
    public PlayerController player;
    private object[] myData;
    private void Start()
    {
        player = GetComponentInChildren<PlayerController>();
    }

    public void OnNotification(string event_path, Object o, params object[] data)
    {
        if(event_path.Contains("player"))
        {
            player.OnNotification(event_path, o, data);
        }
    }
}