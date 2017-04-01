using UnityEngine;
using UnityEngine.UI;

public class AddItem : MonoBehaviour
{
    //adds a dropdown option for each device selected.
    public Dropdown list;

    public void Add()
    {

        for (int i = 0; i < ConfigurationManager.config.numPCs; i++)
        {
            list.options.Add(new Dropdown.OptionData("PC" + (i + 1)));
        }
        for (int i = 0; i < ConfigurationManager.config.numRouters; i++)
        {
            list.options.Add(new Dropdown.OptionData("Router" + (i + 1)));
        }
        for (int i = 0; i < ConfigurationManager.config.numSwitches; i++)
        {
            list.options.Add(new Dropdown.OptionData("Switch" + (i + 1)));
        }

    }
    public void Remove()
    {
        list.ClearOptions();
    }

    private void Update()
    {
        list.RefreshShownValue();
    }

}
