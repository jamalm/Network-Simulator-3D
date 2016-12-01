using UnityEngine;

public class PlayerModel : Entity
{
    //contains data about the player
    private float speed;
    private Vector3 position;
    

    public PlayerModel()
    {
        this.speed = 10;
        position.Set(0 ,0 , 0);
    }

    public Vector3 getPosition()
    {
        return position;
    }
    public float getSpeed()
    {
        return speed;
    }
    public void setPosition(Vector3 position)
    {
        this.position = position;
    }
}