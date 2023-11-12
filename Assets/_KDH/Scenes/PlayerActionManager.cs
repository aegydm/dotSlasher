using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager instance;
    [Header("드래그 중인지 체크용 변수")]
    public bool isDrag;
    [Header("드래그 중인 오브젝트")]
    public HandCardObject dragCardGO;
    [Header("현재 마우스를 올려둔 필드의 정보")]
    public FieldCardObjectTest field;
    [Header("손패 오브젝트를 5개 넣어주세요")]
    public HandCardObject[] handCardObjectArray = new HandCardObject[5];

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
    private bool _dirtyForInter = false;

    public event Action CancelAction;
    public int handCardCount
    {
        get
        {
            return _handCardCount;
        }
        set
        {
            if(_handCardCount != value )
            {
                _handCardCount = value;
            }
        }
    }
    [Header("현재 손패에 있는 카드의 수")]
    public int _handCardCount;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogError("PlayerActionManager is already exist.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isDrag)
        {
            if (Input.GetMouseButtonDown(1))
            {
                CancelAll();
            }
        }
    }

    public void CancelAll()
    {
        CancelAction?.Invoke();
        if(field != null)
            field.CancelFieldSelect();
        dragCardGO.CancelDrag();
    }

    public bool AddHandCard(Card card)
    {
        for(int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (handCardObjectArray[i].isEmpty)
            {
                handCardObjectArray[i].cardData = card;
                Debug.Log(card.cardName);
                SortHandCard();
                return true;
            }
        }
        return false;
    }

    public bool RemoveHandCard(HandCardObject handCardObject)
    {
        if(handCardObject == null)
        {
            Debug.LogError("RemoveHandCard에는 null을 넣을 수 없습니다.");
            return false;
        }
        for(int i = 0; i < handCardObjectArray.Length; i++)
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
            Debug.LogError("RemoveHandCard에는 null을 넣을 수 없습니다.");
            return false;
        }
        for(int i = 0; i < handCardObjectArray.Length; i++)
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
        for(int i = 0; i < handCardObjectArray.Length; i++)
        {
            handCardObjectArray[i].cardData = null;
        }
    }

    private void SortHandCard()
    {
        for(int i = handCardObjectArray.Length-1; i > 0; i--)
        {
            for(int j = i-1; j >= 0; j--)
            {
                if (handCardObjectArray[i].isEmpty == false && handCardObjectArray[j].isEmpty)
                {
                    handCardObjectArray[j].cardData = handCardObjectArray[i].cardData;
                    handCardObjectArray[i].cardData = null;
                }
            }
        }
    }

    private void CountHandCard()
    {
        int count = 0;
        for(int i = 0; i < handCardObjectArray.Length; i++)
        {
            if (!handCardObjectArray[i].isEmpty == false) 
            {
                count++;
            }
            else
            {
                handCardCount = count;
                return;
            }
        }
        return;
    }
}
