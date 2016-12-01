using UnityEngine;

public class PlayerView : Entity
{
    private Vector3 lastPos;
    private void Start()
    {

    }
    //view for the player!
    private void Update()
    {

        //notify controller of input
        //get updated position from model
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
            transform.Translate(app.model.player.getPosition());
        }
        if (Input.GetKey(KeyCode.A))
        {
            app.Notify(WatchGuard.PlayerMovedLeft, this);
            transform.Translate(app.model.player.getPosition());
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
            transform.Translate(app.model.player.getPosition());

        }
        if (Input.GetKey(KeyCode.D))
        {
            app.Notify(WatchGuard.PlayerMovedRight, this);  
            transform.Translate(app.model.player.getPosition());
        }
        
        
        

    }

}