using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public List<Task> tasks = new List<Task>();
    public Task currTask;
    public Task taskPrefab;
    HeadsUpDisplay hud;

    //testcase
    public string tname;
    public string desc;

    //sets the current task to the last one added.
    Task SetTask(string tname, string desc)
    {
        currTask = Instantiate(taskPrefab);
        currTask.SetDesc(tname, desc);
        return currTask;
    }
    private void Awake()
    {

    }


    // Use this for initialization
    void Start () {
        hud = GetComponent<HeadsUpDisplay>();
        //if tasks are preloaded, set last task added as the current task
        if (tasks.Count != 0)
        {
            currTask = tasks[tasks.Count-1];
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(hud.timerStarted)
            CheckTasks();
	}
    
    //add tasks here 
    public void AddTask(GameObject key, GameObject value, string tname, string desc)
    {
        Task task = SetTask(tname, desc);
        task.KeyObjective = key;
        task.ValueObjective = value;
        tasks.Add(task);
        hud.AddTask(task);
        if(!hud.timerStarted)
            hud.startTimer();
    }

    public void RemoveTask(GameObject key, GameObject value)
    {
        //if the key is equal to the task key and the value aswell to the task value
        for (int i = 0; i < tasks.Count; i++)
        {
            //if the key's id is the same as the passed key's id
            if (tasks[i].KeyObjective.GetComponent<PC>().GetID().Equals(key.GetComponent<PC>().GetID()) && tasks[i].ValueObjective.GetComponent<PC>().GetID().Equals(value.GetComponent<PC>().GetID()))
            {
                Task finishedTask = tasks[i];
                tasks.RemoveAt(i);
                hud.RemoveTask(i);
                Destroy(finishedTask.gameObject);
                break;
            }
        }
    }

    public bool hasTaskAlready(GameObject key, GameObject value)
    {
        if(tasks.Count != 0)
        {
            for(int i=0; i<tasks.Count;i++)
            {
                //if the key matches and the value matches, duplicate, return true
                if (key.GetComponent<PC>().GetID().Equals(tasks[i].KeyObjective.GetComponent<PC>().GetID()) && value.GetComponent<PC>().GetID().Equals(tasks[i].ValueObjective.GetComponent<PC>().GetID()))
                    return true;
            }
            return false;
        }
        return false;
        
        
    }
    

    
    //check if no tasks remain
    private void CheckTasks()
    {
        if(tasks.Count == 0)
        {
            GameController.gameState.netState = GameController.NetworkState.ACTIVE;
            hud.stopTimer();
        }
        
    }
}
