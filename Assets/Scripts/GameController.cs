using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public List<Image> tutImgs;
    public TutorialManager tutMan;

    public static GameController gameState; //static reference
    public Scene CurrentScene;              //Currently loaded scene

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
    /*
    void OnLevelWasLoaded(int level)
    {
        if (CurrentScene.name.Equals("Problem1"))
        {
            //tutMan = gameObject.AddComponent<TutorialManager>();
            //tutMan.SetScreens(tutImgs);

        }
    }*/

    public void UpdateScene(Scene scene)
    {
        //update the current scene and change game state
        CurrentScene = scene;
        if (!CurrentScene.name.Equals("Menu"))
        {
            currentState = state.STARTGAME;
            //set up the tutorial if its the first level

        }
            
        if(CurrentScene != null)
        {
            ConfigurationManager.config.filename = "/" + CurrentScene.name + ".ns";

        }
        
    }
}
