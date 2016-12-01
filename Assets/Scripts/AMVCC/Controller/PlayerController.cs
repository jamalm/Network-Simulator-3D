using UnityEngine;

public class PlayerController : Entity
{
    //controls for player
    public void OnNotification(string event_path, Object o, params object[] data)
    {
        Debug.Log("Inside player's controller notification: " + event_path);
        switch(event_path)
        {
            case WatchGuard.PlayerMovedForward:
                {
                    app.model.player.setPosition(Vector3.forward * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerMovedBackward:
                {
                    app.model.player.setPosition(Vector3.back * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerMovedUp:
                {
                    app.model.player.setPosition(Vector3.up * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerMovedDown:
                {
                    app.model.player.setPosition(Vector3.down * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerMovedLeft:
                {
                    app.model.player.setPosition(Vector3.left * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerMovedRight:
                {
                    app.model.player.setPosition(Vector3.right * app.model.player.getSpeed() * Time.deltaTime);
                    break;
                }
            case WatchGuard.PlayerStopped:
                {
                    app.model.player.setPosition(app.model.player.getPosition());
                    break;
                }
        }
    }
}