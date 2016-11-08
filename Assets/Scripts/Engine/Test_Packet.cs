using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Test_Packet : MonoBehaviour
{

	/*
	//physical and logical address of test device
	private string defGate;
	private string physAdd;
	private string logAdd;
	private string recipient;
	*/
	private Packet packet;
	public Router router;
	public Switch swit;
	public PC pc1;
	public PC pc2;
	public List<Cable> cables;

	void Start(){
		Debug.Log ("TEST_PACKET: Physically connecting..");
		//pc1 = GetComponent<PC> ();
		//pc2 = GetComponent<PC> ();
		//swit = GetComponent<Switch> ();
		//router = GetComponent<Router> ();

		/*cables = new List<Cable> ();*/

		pc1.TEST (1);
		pc2.TEST (2);
		//cables.Add (GetComponent<Cable>());
		Debug.Log ("TEST PACKET: BINDING SWITCH TO PC1");
		cables [0].plug (swit.getFEPort (true, ""), pc1.getNewPort ());

		//cables.Add (GetComponent<Cable>());
		Debug.Log ("TEST PACKET: BINDING SWITCH TO PC2");
		cables [1].plug (swit.getFEPort (true, ""), pc2.getNewPort ());

		//cables.Add (GetComponent<Cable>());
		Debug.Log ("TEST PACKET: BINDING SWITCH TO ROUTER");
		cables [2].plug (swit.getGPort(true), router.getNewGPort());

									//send message
	}

	void Update(){
		if(Input.GetKey(KeyCode.Space)){
			Debug.LogAssertion ("TEST_PACKET: SENDING PING...");
			pc1.pingEcho("192.168.1.3");
		}
	}
}
