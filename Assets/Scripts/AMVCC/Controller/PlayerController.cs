using UnityEngine;

public class PlayerController : Entity
{
    //controls for player
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        Debug.Log("Inside player's controller notification: " + event_path);
        switch(event_path)
        {
            case PlayerNotify.PlayerMovedForward:
                {
                    app.model.player.setPosition(Vector3.forward * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerMovedBackward:
                {
                    app.model.player.setPosition(Vector3.back * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerMovedUp:
                {
                    app.model.player.setPosition(Vector3.up * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerMovedDown:
                {
                    app.model.player.setPosition(Vector3.down * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerMovedLeft:
                {
                    app.model.player.setPosition(Vector3.left * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerMovedRight:
                {
                    app.model.player.setPosition(Vector3.right * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case PlayerNotify.PlayerStopped:
                {
                    app.model.player.setPosition(app.model.player.getPosition());
                    break;
                }
        }
    }
}