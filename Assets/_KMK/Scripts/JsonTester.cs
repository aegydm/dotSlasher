using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class JsonObject
{
    [SerializeField]
    public int index;
    [SerializeField]
    public int value;
}

public class JsonTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // save
        //JsonObject obj = new JsonObject();
        //obj.index = 1;
        //obj.value = 10;
        //JsonHelper.SaveJsonFile(Application.streamingAssetsPath + "/" + "out.json", obj);

        //load
        JsonObject[] obj;
        if (File.Exists(Application.streamingAssetsPath + "/" + "out.json"))
        {
            StreamReader reader = new StreamReader(File.OpenRead(Application.streamingAssetsPath + "/" + "out.json"));
            string readall = reader.ReadToEnd();
            obj = JsonHelper.FromJson<JsonObject>(readall);
            Debug.Log($"index({obj[0].index}), value({obj[0].value})");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
