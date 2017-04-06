using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    List<Image> screens;
    int index;

	// Use this for initialization
	void Start () {
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetScreens(List<Image> images)
    {
        screens = images;
    }

    public Image GetNextImage()
    {
        index++;
        if(screens != null && !(index >= screens.Count))
        {
            return screens[index];
        } else
        {
            Debug.LogAssertion("TUTORIAL: FAILED TO LOAD IMAGE");
            return null;
        }
    }
    public Image GetLastImage()
    {
        index--;
        if (screens != null && !(index < 0))
        {
            return screens[index];
        }
        else
        {
            Debug.LogAssertion("TUTORIAL: FAILED TO LOAD IMAGE");
            return null;
        }
    }

}
