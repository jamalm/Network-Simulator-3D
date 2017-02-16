using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour {

    public List<Task> tasks = new List<Task>();
    public Task currTask;
    public Task taskPrefab;

    //testcase
    public string tname;
    public string desc;

    private Task SetTask(string tname, string desc)
    {
        currTask = Instantiate(taskPrefab);
        currTask.SetDesc(tname, desc);
        return currTask;
    }
        

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //CheckTasks();
	}
    
    //add tasks here 
    public void AddTask(GameObject taskObj, string tname, string desc)
    {
        Task task = SetTask(tname, desc);
        task.KeyObjective = taskObj;
        tasks.Add(task);
    }

    public void RemoveTask(GameObject taskObj, string tname)
    {

        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].GetName() == tname)
            {
                Task finishedTask = tasks[i];
                tasks.RemoveAt(i);
                Destroy(finishedTask.gameObject);
            }
        }
    }
    
    //check if any tasks complete, if so , remove them
    private void CheckTasks()
    {
        if(tasks.Count != 0)
        {
            for (int i = 0;i<tasks.Count;i++)
            {
                if(tasks[i].hasCompleted())
                {
                    Task finishedTask = tasks[i];
                    tasks.RemoveAt(i);
                    Destroy(finishedTask);
                }
            }
        }
        
    }
}
