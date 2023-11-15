using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;
    public LinkedBattleField battleField;
    [Header("Field의 부모를 넣어주세요")]
    public GameObject parentField;
    [Header("초기 필드 5개를 넣어주세요")]
    public List<FieldCardObject> startFieldList;
    [Header("추가 생성될 필드를 넣어주세요.")]
    public List<FieldCardObject> additionalFieldList = new();
    [Header("방향 설정 캔버스의 작동 유무체크용")]
    public bool isOpenDirection = false;

    public PhotonView photonView;

    public bool make = false;
    public bool makeLeft = false;
    public Action CallSummonUnit;
    public Action CallFieldCardRemoved;
    public Action CallFieldCardChange;

    private readonly Vector3 CARDDISTANCE = new Vector3(1.65f, 0, 0);

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
        CallFieldCardChange += FieldChangeEffects;
        CallSummonUnit += CallFieldCardChange;
        CallFieldCardRemoved += CallFieldCardChange;
        CheckInterAll();
    }

    private void FieldChangeEffects()
    {
        Debug.Log("CallFieldChange");
        FieldCardObject temp = battleField.First;
        while(temp != null)
        {
            Debug.Log("Test1");
            if(temp.cardData != null && temp.cardData.cardID != 0)
            {
                Debug.Log("Test2");
                temp.cardData.FieldChange(battleField, temp);
            }
            temp = temp.Next;
        }
    }

    public void ResetGameField()
    {
        for (int i = 0; i < additionalFieldList.Count; i++)
        {
            if (additionalFieldList[i].gameObject.activeSelf)
            {
                battleField.Remove(additionalFieldList[i]);
                additionalFieldList[i].ResetField();
                additionalFieldList[i].gameObject.SetActive(false);
                additionalFieldList[i].GetComponent<BoxCollider2D>().enabled = false;
                additionalFieldList[i].Prev = null;
                additionalFieldList[i].Next = null;
            }
        }
        for (int i = 0; i < startFieldList.Count; i++)
        {
            startFieldList[i].gameObject.transform.position = (i - ((startFieldList.Count - 1) / 2)) * CARDDISTANCE + new Vector3(0, 2.12f, 0);
            startFieldList[i].ResetField();
        }
        CallSummonUnit = null;
    }

    public FieldCardObject GetAdditionalField()
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

    public bool FieldIsFull()
    {
        if (battleField.Count() == (startFieldList.Count + additionalFieldList.Count))
        {
            FieldCardObject temp = battleField.First;
            while (temp != null)
            {
                if (temp.isEmpty)
                {
                    return false;
                }
                temp = temp.Next;
            }
            return true;
        }
        return false;
    }

    public void CheckInterAll()
    {
        Debug.Log("CHECKINTER");
        FieldCardObject tmp = battleField.First;
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
            int index = -1;
            if (make)
            {
                if (makeLeft)
                {
                    battleField.AddFirst(PlayerActionManager.instance.field.gameObject);
                    index = battleField.FindIndex(PlayerActionManager.instance.field);
                }
                else
                {
                    battleField.AddAfter(PlayerActionManager.instance.field.Prev, PlayerActionManager.instance.field.gameObject);
                    index = battleField.FindIndex(PlayerActionManager.instance.field.Prev);
                }
            }
            else
            {
                index = battleField.FindIndex(PlayerActionManager.instance.field);
            }

            int cardID = PlayerActionManager.instance.dragCardGO.cardData.cardID;
            PlayerActionManager.instance.RemoveHandCard(PlayerActionManager.instance.dragCardGO.GetComponent<HandCardObject>());
            PlayerActionManager.instance.CancelWithNewField();
            PlayerActionManager.instance.dirtyForInter = false;
            battleField[index].cardData.Summon(battleField, battleField[index]);
            CallSummonUnit?.Invoke();
            isOpenDirection = false;
            CheckInterAll();
            Debug.LogError("SummonUnit");
            Debug.LogError($"index is {index}, make is {make}, makeLeft is {makeLeft}, cardID is {cardID}, lookingLeft is {lookingLeft}, playerID is {int.Parse(GameManager.instance.playerID)}");
            GameManager.instance.photonView.RPC("CallSummonUnit", RpcTarget.Others, index, make, makeLeft, cardID, lookingLeft, int.Parse(GameManager.instance.playerID));
            make = false;
            makeLeft = false;
            GameManager.instance.useCard = true;
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

    public void SetCardToFieldForPun(int index, bool make, bool makeLeft, int cardID, bool lookingLeft, int playerID)
    {
        Debug.LogError("CheckSummonUnit");
        Debug.LogError($"index is {index}, make is {make}, makeLeft is {makeLeft}, cardID is {cardID}, lookingLeft is {lookingLeft}, playerID is {playerID}");
        if (make)
        {
            if (makeLeft)
            {
                FieldCardObject temp = GetAdditionalField();
                temp.gameObject.SetActive(true);
                FieldCardObject temp1 = FieldManager.instance.battleField.First;
                while (temp1 != null)
                {
                    temp1.gameObject.transform.position += CARDDISTANCE / 2;
                    temp1 = temp1.Next;
                }
                temp.transform.position = battleField.First.transform.position - CARDDISTANCE;

                battleField.AddBefore(battleField[index], temp.gameObject);
                temp.cardData = CardDB.instance.FindCardFromID(cardID);
                temp.playerID = playerID;
                temp.lookingLeft = lookingLeft;
            }
            else
            {
                FieldCardObject temp = GetAdditionalField();
                temp.gameObject.SetActive(true);

                FieldCardObject temp1 = FieldManager.instance.battleField.First;
                for (int i = 0; i <= index; i++)
                {
                    temp1.gameObject.transform.position -= CARDDISTANCE / 2;
                    temp1 = temp1.Next;
                }
                while (temp1 != null)
                {
                    temp1.gameObject.transform.position += CARDDISTANCE / 2;
                    temp1 = temp1.Next;
                }


                battleField.AddAfter(battleField[index], temp.gameObject);
                temp.transform.position = (battleField[index].transform.position) + CARDDISTANCE;
                temp.cardData = CardDB.instance.FindCardFromID(cardID);
                temp.playerID = playerID;
                temp.lookingLeft = lookingLeft;
            }
        }
        else
        {
            battleField[index].cardData = CardDB.instance.FindCardFromID(cardID);
            battleField[index].lookingLeft = lookingLeft;
            battleField[index].playerID = playerID;
            Debug.Log($"Original Field : {battleField[index]},{index} + Card is : {battleField[index].cardData.cardName}");
        }
        battleField[index].cardData.Summon(battleField, battleField[index]);
        CallSummonUnit?.Invoke();
        isOpenDirection = false;
        CheckInterAll();
        return;
    }
}