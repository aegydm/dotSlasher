using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;
using UnityEngine.UI;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager instance;

    public bool isDrag;

    public HandCardObject dragCardGO;

    public FieldCardObject field;

    public HandCardObject[] handCardObjectArray = new HandCardObject[5];

    public GameObject selectUI;

    private bool skill1Use = false;

    public bool dirtyForInter
    {
        get
        {
            return _dirtyForInter;
        }
        set
        {
            _dirtyForInter = value;
        }
    }
    [SerializeField] private bool _dirtyForInter = false;

    public event Action CancelAction;
    public event Action NewFieldAction;
    public event Action CancelSelect;

    public int handCardCount
    {
        get
        {
            return _handCardCount;
        }
        set
        {
            if (_handCardCount != value)
            {
                _handCardCount = value;
            }
        }
    }


    public int _handCardCount;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            //Debug.LogError("PlayerActionManager is already exist.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isDrag)
            {
                CancelAll();
            }
            else if (CancelSelect != null)
            {
                Debug.Log("CancelSelect111");
                CancelAll();
            }
        }
    }

    public void CancelWithNewField()
    {
        CancelSelect = null;
        NewFieldAction?.Invoke();
        NewFieldAction = null;
        CancelAction = null;
        if (field != null)
            field.CancelFieldSelect();
        dragCardGO.CancelDrag();
    }

    public void CancelAll()
    {
        CancelSelect?.Invoke();
        CancelAction?.Invoke();
        NewFieldAction = null;
        CancelSelect = null;
        CancelAction = null;
        NewFieldAction = null;
        if (field != null)
            field.CancelFieldSelect();
        if (dragCardGO != null)
            dragCardGO.CancelDrag();
        FieldManager.instance.isOpenDirection = false;
    }

    public bool AddHandCard(Card card)
    {
        for (int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (handCardObjectArray[i].isEmpty)
            {
                handCardObjectArray[i].cardData = card;
                SortHandCard();
                return true;
            }
        }
        return false;
    }

    public bool RemoveHandCard(HandCardObject handCardObject)
    {
        if (handCardObject == null)
        {

            return false;
        }
        for (int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (handCardObjectArray[i] == handCardObject)
            {
                handCardObjectArray[i].cardData = null;
                SortHandCard();
                return true;
            }
        }
        return false;
    }

    public bool RemoveHandCard(Card card)
    {
        if (card == null || card == new Card())
        {

            return false;
        }
        for (int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (handCardObjectArray[i].cardData == card)
            {
                handCardObjectArray[i].cardData = null;
                SortHandCard();
                return true;
            }
        }
        return false;
    }

    public void ResetHandCard()
    {
        for (int i = 0; i < handCardObjectArray.Length; i++)
        {
            handCardObjectArray[i].cardData = null;
        }
    }

    private void SortHandCard()
    {
        for (int i = handCardObjectArray.Length - 1; i > 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (handCardObjectArray[i].isEmpty == false && handCardObjectArray[j].isEmpty)
                {
                    handCardObjectArray[j].cardData = handCardObjectArray[i].cardData;
                    handCardObjectArray[i].cardData = null;
                }
            }
        }
        CountHandCard();
        GameManager.instance.photonView.RPC("EnemyHandCardChange", Photon.Pun.RpcTarget.Others, handCardCount);
    }

    private void CountHandCard()
    {
        int count = 0;
        for (int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (handCardObjectArray[i].isEmpty == false)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        handCardCount = count;
        return;
    }

    public void TestSkillActive()
    {
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.cardData != null && temp.playerID.ToString() == GameManager.instance.playerID && temp.cardData.cardCategory == CardCategory.hero && skill1Use == false)
            {
                if (GameManager.instance.gamePhase == GamePhase.ActionPhase && GameManager.instance.canAct)
                {
                    skill1Use = true;
                    ((Hero)temp.cardData).SkillUse(FieldManager.instance.battleField, temp);
                    GameManager.instance.mySkill1GO.GetComponent<Image>().color = Color.gray;
                    GameManager.instance.photonView.RPC("EnemyHeroSkill1", Photon.Pun.RpcTarget.Others);
                }
                return;
            }
            temp = temp.Next;
        }
    }
}
