using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public enum state
    {
        
        MENU,           //Before game start
        STARTGAME,      //load up engine etc
        PAUSEGAME,      //Game is Paused
        CONTINUEGAME,   //Game is Resumed
        ENDGAME,        //Game Objective achieved
        DEFAULT        //should never happen
    };

    public enum NetworkState
    {
        UNKNOWN,         //Network is in an unknown state (initial)
        ACTIVE,         //network is fully functioning
        FAULTY,         //error found in network
        INACTIVE       //network is down
    }

    public static GameController gameState;
    public GameObject engine;
    public state currentState;
    public NetworkState netState;

    private bool paused;

    //private bool load;          //for loading instructions per game state
    

    //singleton pattern
    private void Awake()
    {
        if(gameState == null)
        {
            DontDestroyOnLoad(gameObject);
            gameState = this;
        }
        else if(gameState != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        paused = false;
        //load = false;
        
    }
	
	// Update is called once per frame
	void Update () {
		switch(currentState)
        {
            case state.MENU:
                {
                    break;
                }
            case state.STARTGAME:
                {
                    break;
                }
            case state.PAUSEGAME:
                {
                    paused = !paused;
                    break;
                }
            case state.CONTINUEGAME:
                {
                    break;
                }
            case state.ENDGAME:
                {
                    break;
                }
            case state.DEFAULT:
                {
                    break;
                }
        }

        switch(netState)
        {
            case NetworkState.ACTIVE:
                {
                    break;
                }
            case NetworkState.FAULTY:
                {
                    
                    break;
                }
            case NetworkState.INACTIVE:
                {
                    break;
                }
            case NetworkState.UNKNOWN:
                {
                    break;
                }
        }
	}

    public void SwitchState(int state)
    {
        currentState = (state)state;
    }
}
