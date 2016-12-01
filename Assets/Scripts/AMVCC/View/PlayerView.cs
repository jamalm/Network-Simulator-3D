using UnityEngine;

public class PlayerView : Entity
{

    private void Start()
    {

    }
    //view for the player!
    private void Update()
    {
        

        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                app.Notify(WatchGuard.PlayerMovedUp, this);
            }
            else
            {
                app.Notify(WatchGuard.PlayerMovedForward, this);
            }

        }
        if (Input.GetKey(KeyCode.A))
        {
            app.Notify(WatchGuard.PlayerMovedLeft, this);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                app.Notify(WatchGuard.PlayerMovedDown, this);
            }
            else
            {
                app.Notify(WatchGuard.PlayerMovedBackward, this);
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            app.Notify(WatchGuard.PlayerMovedRight, this);
        }
        if (!Input.anyKeyDown)
        {
            app.Notify(WatchGuard.PlayerStopped, this);
        }
        

    }

}