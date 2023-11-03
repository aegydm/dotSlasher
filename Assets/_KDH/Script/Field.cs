using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;

[System.Serializable]
[RequireComponent(typeof(UnitObject))]
public class Field : MonoBehaviour
{
    public bool isEmpty = true;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Field Prev;
    public Field Next;
    public bool canBattle = false;
    public UnitObject unitObject;

    public TMP_Text frontDamageText;
    public TMP_Text backDamageText;

    private void Awake()
    {
        unitObject = GetComponent<UnitObject>();
    }

    public void Attack(LinkedBattleField battleField)
    {
        unitObject.cardData.AttackStart(battleField, this);
    }

    public void Damaged(int damageVal = 0)
    {
        if (unitObject.cardData.cardCategory == CardCategory.Hero)
        {
            unitObject.cardData.GetDamage(damageVal);
        }
        else if (unitObject.cardData.cardCategory == CardCategory.Follower)
        {
            unitObject.CardChange(new Unit());
            ResetField();
        }
        else
        {
            Debug.LogError("아이템을 공격할 수 없습니다");
        }
    }

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
            spriteRenderer.sprite = card.cardSprite;
            frontDamageText.text = card.frontDamage.ToString();
            backDamageText.text = card.backDamage.ToString();
        }
    }
}
