using UnityEngine;

public class Movement : MonoBehaviour {

    public float moveSpeed = 10f;
    public Camera player;
    //private Rigidbody rb;
    public Vector3 force = Vector3.zero;

    // Use this for initialization
    private void Start () {
        player = GetComponent<Camera>();
        //rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	private void Update () {
        //force = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
			if (Input.GetKey (KeyCode.LeftShift)) {
                player.transform.Translate (Vector3.up * moveSpeed * Time.deltaTime);
                //force += (Vector3.up * moveSpeed * Time.deltaTime);
			} else {
				player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                //force += (Vector3.forward * moveSpeed * Time.deltaTime);
            }
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            //force += ((-Vector3.right) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
			if (Input.GetKey (KeyCode.LeftShift)) {
                player.transform.Translate (Vector3.down * moveSpeed * Time.deltaTime);
                //force += ((-Vector3.up) * moveSpeed * Time.deltaTime);
			} else {
                player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                //force += ((-Vector3.forward) * moveSpeed * Time.deltaTime);
			}
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            //force += (UnityEngine.Vector3.right * moveSpeed * Time.deltaTime);
        }
        /*if(!Input.anyKey)
        {
            rb.velocity = Vector3.zero;
        }*/

        //rb.AddRelativeForce(-rb.velocity);
    }
}
