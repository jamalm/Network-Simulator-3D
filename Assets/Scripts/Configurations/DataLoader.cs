using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    
    //Saves data to a bin file
    public void Save(string filename, List<PCData> pcs, List<RouterData> routers, List<SwitchData> switches, List<int> brokenCables, List<int> watchers, List<Task> tasks)
    {
        //writer
        BinaryFormatter bf = new BinaryFormatter();
        // creates a file in a persistent location and naming it
        FileStream f = File.Open(Application.persistentDataPath + filename, FileMode.Create);

        //create a data container
        Configuration data = new Configuration(pcs, switches, routers, brokenCables, watchers, tasks);

        //serialise it and save it to file
        bf.Serialize(f, data);
        f.Close();
    }

    public object Load(string filename)
    {
        if (File.Exists(Application.persistentDataPath + filename))
        {
            object conf;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream f = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            conf = bf.Deserialize(f);
            //then set vars to the data's files.. aka load them
            f.Close();
            return conf;
        }
        else
        {
            Debug.Log("No file to load");
            return null;
        }
    }
}