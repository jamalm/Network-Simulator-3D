using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

    private bool paused;
    public bool tutorial;
    private bool init;
    public GameObject pausePanel;
    public LookAtMouse LookAround;
    public GameObject HUD;
    public GameObject TutorialPanel;
    private SelectObject selectable;
    public bool flatSceneEnabled;
    public GameController.state prevState;

    // Use this for initialization
    void Start()
    {
        flatSceneEnabled = false;
        paused = false;
        tutorial = false;
        init = true;
        LookAround = GetComponent<LookAtMouse>();
        selectable = GetComponent<SelectObject>();


    }

    // Update is called once per frame
    void Update()
    {
        if(init)
        {
            //if this is the first level
            if (SceneManager.GetActiveScene().name.Equals("Problem1"))
            {
                TutorialPanel.SetActive(true);
                EnterTutorialScreen();
            }
            init = false;
        }

        if (!tutorial)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                UnPause();
            }
            if(!paused)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    //break the network
                    GameController.gameState.currentState = GameController.state.CHALLENGE;
                    GameObject.FindGameObjectWithTag("challenge").SetActive(false);
                }
            }

        }

    }

    private void Pause()
    {
        Debug.Log("Game Paused: " + paused);
        if (paused == true)
        {
            prevState = GameController.gameState.currentState;
            GameController.gameState.currentState = GameController.state.PAUSEGAME;
            GetComponent<Movement>().enabled = false;
            HUD.SetActive(false);
            selectable.enabled = false;
            pausePanel.SetActive(true);
            LookAround.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            GameObject.FindGameObjectWithTag("EditWindow").SetActive(false);
            Time.timeScale = 0.0f;
        }
        else
        {
            GameController.gameState.currentState = prevState;
            if (!flatSceneEnabled)
            {
                GetComponent<Movement>().enabled = true;
                HUD.SetActive(true);
                selectable.enabled = true;
                pausePanel.SetActive(false);
                LookAround.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1.0f;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void UnPause()
    {
        paused = !paused;
        Pause();
    }

    public void Exit()
    {
        Cursor.visible = true;
        Time.timeScale = 1.0f;
    }

    public void ExitTutorialScreen()
    {
        paused = false;
        
        GameController.gameState.currentState = prevState;
        tutorial = false;
        GetComponent<Movement>().enabled = true;
        HUD.SetActive(true);
        selectable.enabled = true;
        LookAround.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void EnterTutorialScreen()
    {
        tutorial = true;
        prevState = GameController.gameState.currentState;
        GameController.gameState.currentState = GameController.state.PAUSEGAME;
        GetComponent<Movement>().enabled = false;
        HUD.SetActive(false);
        selectable.enabled = false;
        LookAround.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.0f;
    }
}