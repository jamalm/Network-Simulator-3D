using UnityEngine;

public class PortStatus : MonoBehaviour {

    public Material on, off;
    Renderer rend;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    // Use this for initialization
    void Start () {
        
        rend.material = off;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOff()
    {
        rend.material = off;
    }

    public void TurnOn()
    {
        rend.material = on;
    }
}
