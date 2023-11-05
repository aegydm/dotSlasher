using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;

[RequireComponent(typeof(UnitObject))]
public class Field : MonoBehaviour
{
    public bool isEmpty = true;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Field Prev;
    public Field Next;
    public bool canBattle
    {
        get
        {
            return _canBattle;
        }
        set
        {
            _canBattle = value;
        }
    }
    public UnitObject unitObject;

    private bool _canBattle = false;

    public TMP_Text frontDamageText;
    public TMP_Text backDamageText;

    private void Awake()
    {
        unitObject = GetComponent<UnitObject>();
    }

    private void Start()
    {
        unitObject.spriteRenderer = spriteRenderer;
    }

    //public void Attack(LinkedBattleField battleField)
    //{
    //    unitObject.cardData.AttackStart(battleField, this);
    //}

    //public void Damaged(int damageVal = 0)
    //{
    //    if (unitObject.cardData.cardCategory == CardCategory.hero)
    //    {
    //        unitObject.cardData.GetDamage(ref damageVal);
    //    }
    //    else if (unitObject.cardData.cardCategory == CardCategory.minion)
    //    {
    //        unitObject.CardChange(new Card());
    //        ResetField();
    //    }
    //    else
    //    {
    //        Debug.LogError("아이템을 공격할 수 없습니다");
    //    }
    //}

    public void ResetField()
    {
        isEmpty = true;
        card = new Card();
        spriteRenderer.sprite = null;
        canBattle = false;
        frontDamageText.text = string.Empty;
        backDamageText.text = string.Empty;
    }

    public void SetCard(Card newCard)
    {
        if (isEmpty)
        {
            card = newCard;
            isEmpty = false;
            unitObject.lookingLeft = Random.Range(0, 100) < 50 ? true : false;
            unitObject.Setting(newCard);
            spriteRenderer.sprite = card.cardSprite;
            frontDamageText.text = card.frontDamage.ToString();
            backDamageText.text = card.backDamage.ToString();
        }
    }
}
