using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {
    //describes the link associated with the port
    public string type;
    public int vlan = 1;
    public List<int> forbiddenVlans;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
