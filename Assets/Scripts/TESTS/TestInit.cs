using UnityEngine;
using System.Collections;

public class TestInit : MonoBehaviour
{
	Test_Packet packet;
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("TEST INIT: Creating test object..");
		packet = new Test_Packet ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

