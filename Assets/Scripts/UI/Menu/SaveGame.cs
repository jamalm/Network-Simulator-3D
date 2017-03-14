using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    public Text errorMsg;
    //private bool noFiles = false;

    private void Awake()
    {

    }
    public void Overwrite()
    {
        List<string> files = ConfigurationManager.config.LoadAllFiles();
        if (files.Count > 0)
        {
            Debug.Log("Not empty");
            foreach (string a in files)
            {
                if (ConfigurationManager.config.filename.Equals(a))
                {
                    ConfigurationManager.config.Save(a);
                }
                Debug.Log("end of foreach loop");
            }
        }
        else
        {
            Debug.Log("No files");
            StartCoroutine(Wait("No Save Found", 3.0f));
        }
    }

    public void SaveNew(InputField field)
    {
        string filename = "/" + field.text + ".ns";
        ConfigurationManager.config.Save(filename);
    }


    private void ShowMessage(string message)
    {
        errorMsg.text = message;
        errorMsg.enabled = true;
    }
    public void HideMessage()
    {
        errorMsg.text = "";
        errorMsg.enabled = false;
    }

    IEnumerator Wait(string message, float time)
    {
        Time.timeScale = 1.0f;
        Debug.Log("entered coroutine");
        ShowMessage(message);
        Debug.Log("showed message");
        yield return new WaitForSeconds(time);
        Debug.Log("hiding message");
        HideMessage();
        Time.timeScale = 0.0f;
    }
}