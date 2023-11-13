using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManagerTest : MonoBehaviour
{
    public static FieldManagerTest instance;
    public LinkedBattleField battleField;
    [Header("Field의 부모 오브젝트")]
    public GameObject parentField;
    [Header("초기 5개 타일을 넣어주세요")]
    public List<FieldCardObjectTest> startFieldList;
    [Header("추가로 생성될 타일을 넣어주세요")]
    public List<FieldCardObjectTest> additionalFieldList = new();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("FieldManager is already exist.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for(int i = 0; i < startFieldList.Count; i++)
        {
            battleField.Add(startFieldList[i].gameObject);
        }
        CheckInterAll();
    }

    public FieldCardObjectTest GetAdditionalField()
    {
        for(int i = 0; i < additionalFieldList.Count; i++)
        {
            if (additionalFieldList[i].gameObject.activeSelf == false)
            {
                return additionalFieldList[i];
            }
        }
        return null;
    }

    public void AddFieldAfter(int index, int cardID)
    {

    }

    public void CheckInterAll()
    {
        FieldCardObjectTest tmp = battleField.First;
        while(tmp != null) 
        {
            tmp.CheckInter();
            tmp = tmp.Next;
        }
    }
}