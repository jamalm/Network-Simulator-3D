using UnityEngine;

public class PlayerView : Entity
{
   
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
                app.Notify(PlayerNotify.PlayerMovedUp, this);
            }
            else
            {
                app.Notify(PlayerNotify.PlayerMovedForward, this);
            }
            transform.Translate(app.model.player.getPosition());
        }
        if (Input.GetKey(KeyCode.A))
        {
            app.Notify(PlayerNotify.PlayerMovedLeft, this);
            transform.Translate(app.model.player.getPosition());
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                app.Notify(PlayerNotify.PlayerMovedDown, this);
            }
            else
            {
                app.Notify(PlayerNotify.PlayerMovedBackward, this);
            }
            transform.Translate(app.model.player.getPosition());

        }
        if (Input.GetKey(KeyCode.D))
        {
            app.Notify(PlayerNotify.PlayerMovedRight, this);  
            transform.Translate(app.model.player.getPosition());
        }
    }

}