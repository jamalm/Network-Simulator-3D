using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


/************************************
 * 
 * Taken from: https://docs.unity3d.com/Manual/class-Transition.html
 * Unity3d tutorial on screen transitions
 * Copyright © 2016 Unity Technologies. Publication 5.4-X
 * 
 * *********************************/

public class ScreenManager : MonoBehaviour {

    //Screen to open automatically at the start of the scene
    public Animator initOpen;

    //Currently Open Screen
    private Animator CurrOpen;

    //Hash of the parameter we use to control the transitions
    private int openID;

    //The last GameObject selected before current screen
    //used when closing a Screen to go back to previous screen
    private GameObject prevSelected;

    //Animator State and Transition names we need to check against
    const string openTransitionName = "Open";
    const string closedStateName = "Closed";

    public void OnEnable()
    {
        //cache the hash to the OPEN parameter to feed to Animator.SetBool
        openID = Animator.StringToHash(openTransitionName);

        //if set, open the initial screen now.
        if (initOpen == null)
        {
            return;
        }
        OpenPanel(initOpen);
    }

    //Closes the currently open panel and opens the provided one.
    //also takes care of handling navigation, setting the new Selected Element
    public void OpenPanel(Animator anim)
    {
        if(CurrOpen == anim)
        {
            return;
        }

        //Activate the new Screen hierarchy so we can animate it.
        anim.gameObject.SetActive(true);
        //Save the currently selected button that was used to open current screen
        var newPrevSelected = EventSystem.current.currentSelectedGameObject;
        //Move the Screen to Front.
        anim.transform.SetAsLastSibling();

        CloseCurrent();

        prevSelected = newPrevSelected;

        //Set the new screen as the open one.
        CurrOpen = anim;
        //start the open animation
        CurrOpen.SetBool(openID, true);

        //Set an element in the new screen as the new Selected one.
        GameObject selectable = FindFirstEnabledSelectable(anim.gameObject);
        SetSelected(selectable);
    }

    //Find the first Selectable element in the provided hierarchy.
    static GameObject FindFirstEnabledSelectable(GameObject  _gameObj)
    {
        GameObject gameObj = null;
        var selectables = _gameObj.GetComponentsInChildren < Selectable > (true);

        foreach(var selectable in selectables)
        {
            if(selectable.IsActive() && selectable.IsInteractable())
            {
                gameObj = selectable.gameObject;
                break;
            }
        }
        return gameObj;
    }

    public void CloseCurrent()
    {
        if(CurrOpen == null)
        {
            return;
        }

        //start the close animation
        CurrOpen.SetBool(openID, false);

        //Reverting selection to the Selectable used before opening the current screen
        SetSelected(prevSelected);
        //Start Coroutine to disable the hierarchy when closing animation finishes
        StartCoroutine(DisablePanelDelayed(CurrOpen));
        //no screen open.
        CurrOpen = null;
    }

    //Coroutine that will detect when the closing animation is finished and it will deactivate the hierarchy
    IEnumerator DisablePanelDelayed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;

        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
            {
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(closedStateName);
            }

            wantToClose = !anim.GetBool(openID);

            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
        {
            anim.gameObject.SetActive(false);
        }
    }


    //Make the provided GameObject selected
    //When using the mouse/touch we actually want to set it as the previously selected and
    //set nothing as selected for now.
    private void SetSelected(GameObject go)
    {
        //Select the GameObject
        EventSystem.current.SetSelectedGameObject(go);

        //If we are using the keyboard right now, that's all we need to do.
        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null && standaloneInputModule.inputMode == StandaloneInputModule.InputMode.Buttons)
            return;

        //Since we are using a pointer device, we don't want anything selected. 
        //But if the user switches to the keyboard, we want to start the navigation from the provided game object.
        //So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
        EventSystem.current.SetSelectedGameObject(null);
    }
}
