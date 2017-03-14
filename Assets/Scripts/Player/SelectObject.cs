using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour {

    private float distance;
    private float crosshairScale;
    //private bool hitting;
    private GameObject obj;


    public GameObject editWindow;
    public EditScreen editScreen;

    public Texture2D crosshairTexture;

    //initialisation
    private void Start()
    {
        crosshairScale = 1;
        distance = 5;
        //hitting = false;
        obj = null;
        editScreen = GetComponentInChildren<EditScreen>();
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray grabRay = new Ray(transform.position, transform.forward);

	    if(Physics.Raycast(grabRay, out hit, distance))
        {
            Transform objTrans = hit.transform;
            obj = objTrans.gameObject;
            //Debug.Log("ray is casting to: " + obj);
            Debug.DrawRay(transform.position, transform.forward*10, Color.red);
            //hitting = true;
        } else
        {
            //hitting = false;
            obj = null;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked!");
            handleClick(obj);
        }
	}

    private void OnGUI()
    {
       //if there is a texture present
        if(crosshairTexture != null)
        {
            GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
        }
        else
        {
            Debug.Log("No texture for crosshair!");
        }
    }

    //add obj into next scene! 
    public void handleClick(GameObject obj)
    {
        
        if (obj != null)
        {
            
            //LoadScene s = obj.GetComponent<LoadScene>();
            if (obj.CompareTag("PC"))
            {
                editWindow.SetActive(true);
                editScreen = gameObject.GetComponentInChildren<EditScreen>();
                //pass in pc
                //ConfigurationManager.config.RunPC(obj.GetComponent<PC>());
                editScreen.OpenScreen("pc");
            }
            else if (obj.CompareTag("Switch"))
            {
                editWindow.SetActive(true);
                editScreen = gameObject.GetComponentInChildren<EditScreen>();
                //pass in switch as inspected object
                //ConfigurationManager.config.RunSwitch(obj.GetComponent<Switch>());
                editScreen.OpenScreen("switch");
            }
            else if (obj.CompareTag("Router"))
            {
                editWindow.SetActive(true);
                editScreen = gameObject.GetComponentInChildren<EditScreen>();
                //pass in router
                //ConfigurationManager.config.RunRouter(obj.GetComponent<Router>());
                editScreen.OpenScreen("router");
            }
            //s.LoadSceneByIndex(4);
            
        }
        else
        {
            Debug.Log("Object null");
        }
        
    }
}