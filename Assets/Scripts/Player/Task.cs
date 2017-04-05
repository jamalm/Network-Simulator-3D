using UnityEngine;

public class Task : MonoBehaviour
{

    bool completed;
    public string taskName;
    public string desc;
    public GameObject KeyObjective;
    public GameObject ValueObjective;
    private void Start()
    {
        completed = false;
    }

    public string GetName()
    {
        return taskName;
    }

    public string GetDesc()
    {
        return desc;
    }

    public void SetDesc(string n, string d)
    {
        taskName = n;
        desc = d;
    }

    public bool hasCompleted()
    {
        return completed;
    }

    public void TaskCompleted()
    {
        completed = true;
    }
}
