using UnityEngine;

public class NewPc : MonoBehaviour {

    public string mac;
    public NewPort port;
    
    
	// Use this for initialization
	void Start () {
        mac = "1";
        port = GetComponent<NewPort>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Send()
    {

    }

    public void Receive()
    {

    }
}
