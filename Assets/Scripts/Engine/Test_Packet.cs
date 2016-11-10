using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test_Packet : MonoBehaviour
{
	public Router router;
	public Switch swit;
	public PC pc1;
	public PC pc2;
	public List<Cable> cables;


	void Start(){
		Debug.Log ("TEST_PACKET: Physically connecting..");

		pc1.TEST (1);
		pc2.TEST (2);

		Debug.Log ("TEST PACKET: BINDING SWITCH TO PC1");
		cables [0].plug (swit.getNewPort ("fe"), pc1.getNewPort ());
        
		Debug.Log ("TEST PACKET: BINDING SWITCH TO PC2");
		cables [1].plug (swit.getNewPort ("fe"), pc2.getNewPort ());
        
		Debug.Log ("TEST PACKET: BINDING SWITCH TO ROUTER");
		cables [2].plug (swit.getNewPort("g"), router.getNewPort("g"));

        

    }

	void Update(){
		if(Input.GetKeyUp(KeyCode.Space)){
			Debug.Log ("TEST_PACKET: SENDING PING...");
            pc1.pingEcho("192.168.1.3");
        }
	}
}
