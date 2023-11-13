using Photon.Pun;
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
    [Header("방향키 선택 화면 여부")]
    public bool isOpenDirection = false;

    public PhotonView photonView;

    public bool make = false;
    public bool makeLeft = false;

    private void Awake()
    {
        if (instance == null)
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
        for (int i = 0; i < startFieldList.Count; i++)
        {
            battleField.Add(startFieldList[i].gameObject);
        }
        CheckInterAll();
    }

    public FieldCardObjectTest GetAdditionalField()
    {
        for (int i = 0; i < additionalFieldList.Count; i++)
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
        Debug.Log("CHECKINTER");
        FieldCardObjectTest tmp = battleField.First;
        while (tmp != null)
        {
            tmp.CheckInter();
            tmp = tmp.Next;
        }
    }

    public void SelectDirection(bool lookingLeft)
    {
        Debug.Log(lookingLeft);
        if (isOpenDirection)
        {
            PlayerActionManager.instance.field.cardData = PlayerActionManager.instance.dragCardGO.cardData;
            PlayerActionManager.instance.field.lookingLeft = lookingLeft;
            PlayerActionManager.instance.selectUI.SetActive(false);
            if (make)
            {
                if (makeLeft)
                {
                    battleField.AddFirst(PlayerActionManager.instance.field.gameObject);
                }
                else
                {
                    battleField.AddAfter(PlayerActionManager.instance.field.Prev, PlayerActionManager.instance.field.gameObject);
                }
            }
            PlayerActionManager.instance.RemoveHandCard(PlayerActionManager.instance.dragCardGO.GetComponent<HandCardObject>());
            PlayerActionManager.instance.CancelWithNewField();
            PlayerActionManager.instance.dirtyForInter = false;
            isOpenDirection = false;
            CheckInterAll();
            make = false;
            makeLeft = false;
            TestManager.instance.canAct = false;
        }
        
    }
    
    public void CancelSelect()
    {
        make = false;
        makeLeft = false;
        Debug.Log("CancelSelect");

        //PlayerActionManager.instance.dirtyForInter = false;
        PlayerActionManager.instance.selectUI.SetActive(false);
    }

    [PunRPC]
    public void SetCardToFieldForPun(int index, bool make, bool makeLeft, int cardID, bool lookingLeft)
    {
        if (make)
        {
            if (makeLeft)
            {
                FieldCardObjectTest temp = GetAdditionalField();
                battleField.AddBefore(battleField[index], temp.gameObject);
                temp.cardData = CardDB.instance.FindCardFromID(cardID);
                temp.lookingLeft = lookingLeft;
            }
            else
            {
                FieldCardObjectTest temp = GetAdditionalField();
                battleField.AddAfter(battleField[index], temp.gameObject);
                temp.cardData = CardDB.instance.FindCardFromID(cardID);
                temp.lookingLeft = lookingLeft;
            }
        }
        else
        {
            battleField[index].cardData = CardDB.instance.FindCardFromID(cardID);
            battleField[index].lookingLeft = lookingLeft;
        }
        isOpenDirection = false;
        return;
    }
}