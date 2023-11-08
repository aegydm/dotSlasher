using CCGCard;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

/// <summary>
/// �� ��ũ��Ʈ ������ Json���Ͽ� �� ������ �� �����ϰ� �����ϰų� ������ �����͸� ���� �޾� ���������� �����Ͽ��� �Ѵ�.
/// </summary>
[System.Serializable]
public class Json : MonoBehaviour
{
    public JsonSaveData data = new JsonSaveData();
    public string path;
    public List<int> IDList;
    public List<int> myDeck;

    void Start()
    {
        
    }

    public void JsonSave()
    {
        List<int> myDeck = BildManager.instance.myDeck;
        IDList = myDeck;
        IDList.Sort();
        string jsonData = JsonUtility.ToJson(IDList);
        path = Path.Combine(Application.dataPath, "data.json");
        Debug.Log(path);
        File.WriteAllText(path + "PlayerDeckData", jsonData);
        Debug.Log(myDeck);

        string readData = File.ReadAllText(path);
        List<int> unSerializedData = JsonUtility.FromJson<List<int>>(readData);
        data.myDeck = new List<int>(unSerializedData);
        Debug.Log(myDeck);

    }    
}
