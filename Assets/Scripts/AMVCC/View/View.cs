using UnityEngine;

public class View : Entity
{
    //physical view responding to model and controller input
    public PlayerView player;

    private void Start()
    {
        player = GetComponentInChildren<PlayerView>();
    }
    private void Update()
    {
        
    }
}
