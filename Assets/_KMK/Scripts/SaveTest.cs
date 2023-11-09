using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;

public class SaveTest : MonoBehaviour
{
    public string path;
    public BinaryFormatter binaryFormatter;
    public BildManager bildManager;
    public DataFrame loadDeck;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.dataPath, "ID.data");
        binaryFormatter = new BinaryFormatter();

        if(bildManager == null)
        {
            print("빌드 매니저 초기화");
            return;
        }

        DataFrame dataFrame = new DataFrame();
        dataFrame.ID = bildManager.myDeck;

        OnSave(dataFrame);

        LoadData(path);
    }

    private void OnSave(DataFrame dataFrame)
    {
        try
        {
            using (Stream ws = new FileStream(path, FileMode.Create))
            {
                binaryFormatter.Serialize(ws, dataFrame);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DataFrame LoadData(string path)
    {
        DataFrame loadDeck = null;

        try
        {
            using (Stream rs = new FileStream(path, FileMode.Open))
            {
                loadDeck = (DataFrame)binaryFormatter.Deserialize(rs);
            }

            Debug.Log(loadDeck.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Loaded data:" + string.Join(",", loadDeck.ID));
        }

        print(loadDeck.ID.Count);
        return loadDeck;
    }
}
