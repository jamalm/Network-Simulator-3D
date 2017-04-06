using UnityEngine;

public class TaskWatcher : MonoBehaviour {

    public bool active;
    private GameObject device;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PINGCheck()
    {
        GetComponent<PC>();
    }

    public bool isActive()
    {
        return active;
    }

    //when a ping fails, call this
    public void PINGFailure(GameObject key, GameObject value)
    {
        //set network to faulty if not already
        if(GameController.gameState.netState != (GameController.NetworkState.FAULTY))
            GameController.gameState.netState = GameController.NetworkState.FAULTY;
        
        //add new task 
        if(!GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().hasTaskAlready(key, value))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().AddTask(key, value, "PING FAILED", "FIX THE PING");
        }
    }

    //when a ping on a device that is being watched passees, call this here
    public void PINGSuccess(GameObject key, GameObject value)
    {
        if(GameController.gameState.netState != (GameController.NetworkState.ACTIVE))
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().RemoveTask(key, value);

            /*
            List<Task> tasks = GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().tasks;
            for (int i=0;i<tasks.Count;i++)
            {
                if(tasks[i].GetName() == "PING FAILURE")
                {
                    tasks[i].TaskCompleted();
                }
            }*/
            
        }
    }
}
