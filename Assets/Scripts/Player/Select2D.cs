using System.Collections;
using UnityEngine;

public class Select2D : MonoBehaviour {

    bool selected;
    public PC startPC, endPC;
    SceneController sceneController;
    
    // Use this for initialization
    void Start () {
        enabled = false;
        sceneController = FindObjectOfType<SceneController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleClick(GameObject obj)
    {
        if(obj != null)
        {
            Selectable2D clicked = obj.GetComponent<Selectable2D>();
            //if no object selected previously, highlight it 
            if(!selected)
            {
                //make it green
                clicked.Success();
                startPC = sceneController.GetPC(clicked.gameObject);
                selected = true;
            }
            else if(selected)
            {
                if(sceneController.GetPC(clicked.gameObject) == startPC)
                {
                    clicked.GetComponent<SpriteRenderer>().color = Color.white;
                    startPC = null;
                    endPC = null;
                    selected = false;
                } else
                {
                    endPC = sceneController.GetPC(clicked.gameObject);

                    startPC.Ping(endPC.IP);
                    if (startPC.ping.success > startPC.ping.failure)
                    {
                        StartCoroutine(ChangeColor(clicked, true));
                    }
                    else
                    {
                        StartCoroutine(ChangeColor(clicked, false));
                    }
                }

            }
        }
        
        
    }

    public bool hasSelected()
    {
        return selected;
    }

    private IEnumerator ChangeColor(Selectable2D pc, bool success)
    {
        if (sceneController.GetPC(pc.gameObject) != startPC)
        {
            if (success)
            {
                pc.Success();
                //remove a task if there is one
                if (startPC.gameObject.GetComponent<TaskWatcher>().isActive())
                {
                    startPC.gameObject.GetComponent<TaskWatcher>().PINGSuccess(startPC.gameObject, endPC.gameObject);
                }
            }
            else
            {
                pc.Failure();
                //add a new task if it is active on this member
                if (startPC.gameObject.GetComponent<TaskWatcher>().isActive())
                    startPC.gameObject.GetComponent<TaskWatcher>().PINGFailure(startPC.gameObject, endPC.gameObject);
            }
            yield return new WaitForSeconds(2.0f);
            if (sceneController.GetPC(pc.gameObject) != startPC)
            {

            }
            pc.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }
}
