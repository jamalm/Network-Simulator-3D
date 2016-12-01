using UnityEngine;

public class Entity : MonoBehaviour
{
    public Application app { get { return FindObjectOfType<Application>(); } }
}

public class Application : MonoBehaviour
{
    public Model model;
    public View view;
    public Controller controller;
    

    void Start()
    {
        model = GetComponentInChildren<Model>();
        view = GetComponentInChildren<View>();
        controller = GetComponentInChildren<Controller>();
    }

    public void Notify(string event_path, Object o, params object[] data)
    {
        Controller[] controller_list = GetAllControllers();
        foreach(Controller c in controller_list)
        {
            c.OnNotification(event_path, o, data);
        }
    }

    private Controller[] GetAllControllers()
    {
        return GetComponentsInChildren<Controller>();
    }
}
