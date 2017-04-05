using System.Collections;
using UnityEngine;

public class Selectable2D : MonoBehaviour {

    public Material green, red;
    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        player.GetComponent<Select2D>().HandleClick(gameObject);
        Select();
        
    }

    private IEnumerator ScaleUp()
    {
        Vector3 scalar = new Vector3(.001f, .001f, 0);
        for (int i = 0; i < 10; i++)
        {
            transform.localScale += scalar;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private IEnumerator ScaleDown()
    {
        Vector3 scalar = new Vector3(.001f, .001f, 0);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
        {
            transform.localScale -= scalar;
            
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Select()
    {
        StartCoroutine(ScaleUp());
        StartCoroutine(ScaleDown());
    }

    public void Success()
    {
        GetComponent<SpriteRenderer>().color = green.color;
    }
    public void Failure()
    {
        GetComponent<SpriteRenderer>().color = red.color;
    }

    
}
