using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {
    //timer stuff
    public bool timerStarted;
    public bool manualSet;
    Text timer;
    int seconds;

    //Tasks
    public List<Text> tasks = new List<Text>();
    public Text textPrefab;
    public GameObject UITaskList;

    private void Awake()
    {
        //add timer text
        timer= GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        timer.text = "Time: ";
        timerStarted = false;
        manualSet = false;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DisplayTimer();

	}

    /* TIME STUFF 
     */

    public void startTimer()
    {
        timerStarted = true;
        StartCoroutine(Timer());
    }
    public void stopTimer()
    {
        timerStarted = false;
        seconds = 0;
    }
    void DisplayTimer()
    {
        timer.text = "Time: " + seconds;
    }

    private IEnumerator Timer()
    {
        while(timerStarted)
        {
            //if the game is not paused
            if(GameController.gameState.currentState != GameController.state.PAUSEGAME)
                seconds++; 

            yield return new WaitForSecondsRealtime(1);
        }
        
    }



    /*Tasks Stuff for displaying tasks on screen
     * /
     */
    public void AddTask(Task task)
    {
        //create a text element
        Text UITaskElement = Instantiate(textPrefab);
        //set the list as it's parent
        UITaskElement.rectTransform.SetParent(UITaskList.transform);
        UITaskElement.rectTransform.localScale = new Vector3(1,1,1);
        UITaskElement.transform.localPosition = new Vector3(0, 0, 0);
        //add text to element from task
        string name = task.GetName();
        string desc = task.GetDesc();
        string src = task.KeyObjective.GetComponent<PC>().GetID();
        string dest = task.ValueObjective.GetComponent<PC>().GetID();
        //concatenate it
        UITaskElement.text = src + " -> " + dest + ": " + name + ", " + desc;

        //add to list for reference
        tasks.Add(UITaskElement);
    }
    
    public void RemoveTask(int index)
    {
        //get unique id of task and remove it from the list , destroy it also
        Text tempText = tasks[index];
        tasks.RemoveAt(index);
        Destroy(tempText.gameObject);
    } 
}
