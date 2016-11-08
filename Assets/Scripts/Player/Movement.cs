using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float moveSpeed;
    public Camera player;

    // Use this for initialization
    private void Start () {
        player = GetComponent<Camera>();
        moveSpeed = 10f;
	}
	
	// Update is called once per frame
	private void Update () {
        if (Input.GetKey(KeyCode.W))
        {
			if (Input.GetKey (KeyCode.LeftShift)) {
				player.transform.Translate (Vector3.up * moveSpeed * Time.deltaTime);
			} else {
				player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			}
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
			if (Input.GetKey (KeyCode.LeftShift)) {
				player.transform.Translate (Vector3.down * moveSpeed * Time.deltaTime);
			} else {
				player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
			}
           
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }
}
