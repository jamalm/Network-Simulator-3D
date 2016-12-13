using UnityEngine;

public class PCView : Entity
{
    //view for PC
    private void Start()
    {

    }

    private void Update()
    {

        //notify controller of input
        if (Input.GetKey(KeyCode.Return))
        {
            if (app.model.pc.getPort() != null)
            app.Notify(PCNotify.PcActivatedPing, this);
        }

    }
}