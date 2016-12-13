using UnityEngine;

public class View : Entity
{
    //physical view responding to model and controller input
    public PlayerView player;
    public PCView pc;
    public RouterView router;

    private void Start()
    {
        player = GetComponentInChildren<PlayerView>();
        pc = GetComponentInChildren<PCView>();
        router = GetComponentInChildren<RouterView>();
    }
    private void Update()
    {
        
    }
}
