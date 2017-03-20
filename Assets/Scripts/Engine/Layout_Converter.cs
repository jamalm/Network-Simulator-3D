using UnityEngine;
using UnityEngine.SceneManagement;

public class Layout_Converter : MonoBehaviour {
    Engine engine;  //this is the engine that is active
    [HideInInspector]
    public bool inspector;    //this flag determines the layout of the scene
	// Use this for initialization
	void Start () {
        engine = GetComponent<Engine>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Scene()
    {
        if (inspector)
        {
            PlaceScene();
        }
        else
        {
            //else return to top down
            HideRouters();
            HideSwitches();
            HideCables();
            PlacePCs();
        }
    }

    private void HideRouters()
    {
        for (int i = 0; i < engine.routers.Count; i++)
        {
            engine.routers[i].gameObject.SetActive(false);
        }
    }
    private void HideSwitches()
    {
        for (int i = 0; i < engine.switches.Count; i++)
        {
            engine.switches[i].gameObject.SetActive(false);
        }
    }
    private void HideCables()
    {
        for(int i=0;i< engine.cables.Count;i++)
        {
            engine.cables[i].gameObject.SetActive(false);
        }
    }
    private void PlacePCs()
    {
        for(int i=0;i<engine.pcs.Count;i++)
        {
            engine.pcs[i].transform.position = new Vector3(-(engine.mapSize / 2) + (engine.pc_gap * i), transform.position.y, (engine.mapSize / 2));
            //engine.pcs[i].transform.rotation *= Quaternion.Euler(-90,0,0);
        }
    }
    private void PlaceScene()
    {
        //place pcs
        for(int i=0;i<engine.pcs.Count;i++)
        {
            engine.pcs[i].gameObject.SetActive(true);
            engine.pcs[i].transform.position = new Vector3(-(engine.mapSize / 2) + (engine.pc_gap * i), transform.position.y, (engine.mapSize / 2));
            //engine.pcs[i].transform.rotation *= Quaternion.Euler(-90, 0, 0);
        }
        //place Routers
        for(int i=0; i<engine.routers.Count;i++)
        {
            engine.routers[i].gameObject.SetActive(true);
            engine.routers[i].transform.position = new Vector3(5 * i, 0, -10);
            engine.routers[i].transform.rotation = Quaternion.Euler(-90, -90, 0);
        }
        //place switches
        for(int i=0;i<engine.switches.Count;i++)
        {
            engine.switches[i].gameObject.SetActive(true);
            engine.switches[i].transform.position = new Vector3(5 * i, -0.5f, 0);
        }
        //place cables
        float percentage = 0.0f;
        int s = -1;
        int r = -1;
        if (engine.numRouters > 0)
        {
            percentage = engine.numSwitches / engine.numRouters;
            for (int i = 0; i < engine.numSwitches; i++)
            {

                if (engine.numRouters > 1)
                {

                    if (i % percentage == 0)
                    {
                        if (r != engine.numRouters - 1)
                        {
                            r++;
                        }
                    }
                    //for routers with switches
                    Vector3 routerPos = engine.routers[r].transform.Find("PivotPoint").position;
                    Vector3 switPos = new Vector3(engine.switches[i].transform.position.x, 0.0f, engine.switches[i].transform.position.z);
                    Vector3 distance = (switPos - routerPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, routerPos + distance, transform.rotation));
                    engine.cables[i].transform.position = routerPos + distance;
                    engine.cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(switPos, routerPos));
                    engine.cables[i].transform.LookAt(routerPos);


                }
                else
                {
                    Vector3 routerPos = engine.routers[r + 1].transform.Find("PivotPoint").position;
                    Vector3 switPos = new Vector3(engine.switches[i].transform.position.x, 0.0f, engine.switches[i].transform.position.z);
                    Vector3 distance = (switPos - routerPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, routerPos + distance, transform.rotation));
                    engine.cables[i].transform.position = routerPos + distance;
                    engine.cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(switPos, routerPos));
                    engine.cables[i].transform.LookAt(routerPos);
                }
            }
            percentage = engine.numPCs / engine.numSwitches;
            for (int i = 0; i < engine.numPCs; i++)
            {

                if (engine.numSwitches > 1)
                {
                    if (i % percentage == 0)
                    {
                        if (s != engine.numSwitches - 1)
                        {
                            //this is to prevent out of bounds, just assign all remaining pc's to end computer
                            s++;
                        }

                    }
                    //for switches with pcs
                    Vector3 switPos = new Vector3(engine.switches[s].transform.position.x, 0.0f, engine.switches[s].transform.position.z);
                    Vector3 pcPos = engine.pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    engine.cables[i].transform.position = switPos + distance;
                    engine.cables[i + engine.numSwitches].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    engine.cables[i + engine.numSwitches].transform.LookAt(switPos);
                }
                else
                {
                    //for switches with pcs
                    Vector3 switPos = new Vector3(engine.switches[s + 1].transform.position.x, 0.0f, engine.switches[s + 1].transform.position.z);
                    Vector3 pcPos = engine.pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    engine.cables[i].transform.position = switPos + distance;
                    engine.cables[i + engine.numSwitches].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    engine.cables[i + engine.numSwitches].transform.LookAt(switPos);
                }

            }
        }
        else
        {
            percentage = engine.numPCs / engine.numSwitches;
            for (int i = 0; i < engine.numPCs; i++)
            {

                if (engine.numSwitches > 1)
                {
                    if (i % percentage == 0)
                    {
                        if (s != engine.numSwitches - 1)
                        {
                            //this is to prevent out of bounds, just assign all remaining pc's to end computer
                            s++;
                        }

                    }
                    //for switches with pcs
                    Vector3 switPos = new Vector3(engine.switches[s].transform.position.x, 0.0f, engine.switches[s].transform.position.z);
                    Vector3 pcPos = engine.pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    engine.cables[i].transform.position = switPos + distance;
                    engine.cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    engine.cables[i].transform.LookAt(switPos);
                }
                else
                {
                    //for switches with pcs
                    Vector3 switPos = new Vector3(engine.switches[s + 1].transform.position.x, 0.0f, engine.switches[s + 1].transform.position.z);
                    Vector3 pcPos = engine.pcs[i].transform.position;
                    Vector3 distance = (pcPos - switPos) * 0.5f;
                    //cables.Add(Instantiate(CablePrefab, switPos + distance, transform.rotation));
                    engine.cables[i].transform.position = switPos + distance;
                    engine.cables[i].transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(pcPos, switPos));
                    engine.cables[i].transform.LookAt(switPos);
                }

            }
        }
    }


}
