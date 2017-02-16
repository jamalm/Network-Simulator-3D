using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GraphicManager : MonoBehaviour
{
    public static GraphicManager graphics;
    public GameObject pingPrefab;
    public GameObject arpPrefab;
    public float sendSpeed;
    public Engine engine;

    private bool finished = false; //used to tell the engine when the packet is sent

    private void Awake()
    {
        graphics = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Ping(string from, string to)
    {
        Transform startPos = GetStart(from);
        Vector3 endPos = GetEnd(to);
        GameObject ping = Instantiate(pingPrefab, new Vector3(startPos.position.x, 1.0f, startPos.position.z), Quaternion.identity);

        StartCoroutine(Move(endPos, ping));
        //wait on co routine to finish
        if (finished)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ARP(string from, string to)
    {
        Transform startPos = GetStart(from);
        Vector3 endPos = GetEnd(to);
        GameObject arp = Instantiate(arpPrefab, new Vector3(startPos.position.x, 1.0f, startPos.position.z), Quaternion.identity);

        StartCoroutine(Move(endPos, arp));
        //wait on co routine to finish
        if(finished)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    

    
    private IEnumerator Move(Vector3 pos, GameObject obj)
    {
        finished = false;
        //direction is the magnitude for now 
        Vector3 direction = pos - obj.transform.position;
        direction.y = 0f;   //stop the object from dropping downwards

        float distance = direction.magnitude;   //get the float distance

        direction.Normalize();  //create an arrow for the direction to take

        for (int i=0;i<=sendSpeed;i++)
        {
            obj.transform.Translate(direction*(distance/sendSpeed));
            yield return new WaitForSeconds(0.1f);
        }
        finished = true;
        DestroyObject(obj);
        
    }



    private Transform GetStart(string id)
    {
        if(id.Contains("PC"))
        {
            for(int i=0; i<engine.pcs.Count;i++)
            {
                if(id.Contains((i+1).ToString()))
                {
                    //we have our device!!!
                    return engine.pcs[i].transform;
                }
            }
        }
        else if(id.Contains("Router"))
        {
            for (int i = 0; i < engine.routers.Count; i++)
            {
                if (id.Contains((i + 1).ToString()))
                {
                    //we have our device!!!
                    return engine.routers[i].transform;
                }
            }
        }
        else if(id.Contains("Switch"))
        {
            for (int i = 0; i < engine.switches.Count; i++)
            {
                if (id.Contains((i + 1).ToString()))
                {
                    //we have our device!!!
                    return engine.switches[i].transform;
                }
            }
        }
        Debug.LogAssertion("GRAPHICS: NO TRANSFORM FOUND ");
        return null;
    }

    private Vector3 GetEnd(string id)
    {
        if (id.Contains("PC"))
        {
            for (int i = 0; i < engine.pcs.Count; i++)
            {
                if (id.Contains((i + 1).ToString()))
                {
                    //we have our device!!!
                    return engine.pcs[i].transform.position;
                }
            }
        }
        else if (id.Contains("Router"))
        {
            for (int i = 0; i < engine.routers.Count; i++)
            {
                if (id.Contains((i + 1).ToString()))
                {
                    //we have our device!!!
                    return engine.routers[i].transform.position;
                }
            }
        }
        else if (id.Contains("Switch"))
        {
            for (int i = 0; i < engine.switches.Count; i++)
            {
                if (id.Contains((i + 1).ToString()))
                {
                    //we have our device!!!
                    return engine.switches[i].transform.position;
                }
            }
        }
        Debug.LogAssertion("GRAPHICS: NO TRANSFORM FOUND ");
        return Vector3.zero;
        
    }

    public bool isFinished()
    {
        return finished;
    }
}
