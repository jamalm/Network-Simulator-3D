using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public enum state
    {
        
        MENU,           //Before game start
        STARTGAME,      //load up engine etc
        CHALLENGE,      //break the network
        PAUSEGAME,      //Game is Paused
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

    public static GameController gameState; //static reference
    public Scene CurrentScene;              //Currently loaded scene
    public GameObject engine;               //game engine
    public state currentState;              //game state
    public NetworkState netState;           //network state


    //singleton pattern
    private void Awake()
    {
        if(gameState == null)
        {
            DontDestroyOnLoad(gameObject);
            gameState = this;
            currentState = state.MENU;
            
        }
        else if(gameState != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		switch(currentState)
        {
            case state.MENU:
                {
                    netState = NetworkState.INACTIVE;
                    break;
                }
            case state.STARTGAME:
                {
                    
                    break;
                }
            case state.CHALLENGE:
                {
                    break;
                }
            case state.PAUSEGAME:
                {

                    break;
                }
            case state.ENDGAME:
                {
                    netState = NetworkState.INACTIVE;
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

    public void UpdateScene(Scene scene)
    {
        //update the current scene and change game state
        CurrentScene = scene;
        if (!CurrentScene.name.Equals("Menu"))
            currentState = state.STARTGAME;
    }
}
