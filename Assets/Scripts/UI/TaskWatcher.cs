using UnityEngine;

public class TaskWatcher : MonoBehaviour {

    public bool active;
    private GameObject device;
	// Use this for initialization
	void Start () {
        active = false;
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

    public void PINGFailure()
    {
        if(GameController.gameState.netState != (GameController.NetworkState.FAULTY))
        {
            GameController.gameState.netState = (GameController.NetworkState)2;
            GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().AddTask(gameObject, "PING FAILED", "FIX THE PING");
        }
    }

    public void PINGSuccess()
    {
        if(GameController.gameState.netState != (GameController.NetworkState.ACTIVE))
        {
            GameController.gameState.netState = (GameController.NetworkState)1;
            GameObject.FindGameObjectWithTag("Player").GetComponent<TaskManager>().RemoveTask(gameObject, "PING FAILED");

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
