using UnityEngine;

public class SceneController: MonoBehaviour {
    GameObject devices;
    public SpriteRenderer[] pcSprites;
    public GameObject[] pcControllers;
    public GameObject player;
    bool dimension;
    // Use this for initialization


    void Start()
    {
        dimension = true;
        player = GameObject.FindGameObjectWithTag("Player");
        devices = GameObject.FindGameObjectWithTag("Device");
        pcSprites = devices.GetComponentsInChildren<SpriteRenderer>();
        pcControllers = GameObject.FindGameObjectsWithTag("PC");
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowScene()
    {
        player.GetComponent<Camera>().orthographic = true;
        player.GetComponent<Select2D>().enabled = true;
        player.GetComponent<PauseGame>().flatSceneEnabled = true;

        player.transform.position = new Vector3(0, 1, 30);
        player.transform.rotation = Quaternion.identity;

        player.GetComponent<Movement>().enabled = false;
        player.GetComponent<SelectObject>().enabled = false;
        player.GetComponent<LookAtMouse>().enabled = false;
        

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        dimension = false;
    }

    public void HideScene()
    {
        player.GetComponent<Camera>().orthographic = false;
        player.GetComponent<Select2D>().enabled = false;
        player.GetComponent<PauseGame>().flatSceneEnabled = false;

        if (!dimension)
        {
            player.transform.position = new Vector3(0, 10, -20);
        }

        dimension = true;
    }

    public PC GetPC(GameObject sprite)
    {
        for(int i=0;i<pcSprites.Length;i++)
        {
            if (sprite.GetComponent<SpriteRenderer>().Equals(pcSprites[i]))
            {
                return pcControllers[i].GetComponent<PC>();
            }
        }
        return null;
    }
}
