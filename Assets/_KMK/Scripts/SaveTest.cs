using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;

public class SaveTest : MonoBehaviour
{
    public string path = Path.Combine(Application.dataPath, "ID.data");
    public BinaryFormatter binaryFormatter = new BinaryFormatter();
    public BildManager bildManager;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        DataFrame dataFrame = new DataFrame();
        dataFrame.ID = bildManager.myDeck;

        OnSave(dataFrame.ID);
    }

    private void OnSave(List<int> ID)
    {
        try
        {
            using (Stream ws = new FileStream(path, FileMode.Create))
            {
                binaryFormatter.Serialize(ws, ID);
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
}
