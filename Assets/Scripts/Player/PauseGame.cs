using UnityEngine;

public class PauseGame : MonoBehaviour
{

    private bool paused;
    public GameObject pausePanel;
    public LookAtMouse LookAround;
    public GameObject HUD;
    private SelectObject selectable;
    public bool flatSceneEnabled;

    // Use this for initialization
    void Start()
    {
        flatSceneEnabled = false;
        paused = false;
        LookAround = GetComponent<LookAtMouse>();
        selectable = GetComponent<SelectObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UnPause();
        }

    }

    private void Pause()
    {
        Debug.Log("Game Paused: " + paused);
        if (paused == true)
        {
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
            GameController.gameState.currentState = GameController.state.STARTGAME;
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
}