using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit : Card
{
    public Unit()
    {
        this.cardName = string.Empty;
        this.skill = string.Empty;
        this.skillContents = string.Empty;
        this.cost = 0;
        this.cardColor = CardType.Neutral;
    }

    public Unit(string name, int frontDamage, int backDamage, CardType cardType, int cost = 0, bool left = false)
    {
        this.cardName = name;
        this.frontDamage = frontDamage;
        this.backDamage = backDamage;
        this.cardColor = cardType;
    }

    public Unit(Card card)
    {
        cardName = card.cardName;
        skill = card.skill;
        skillContents = card.skillContents;
        cost = card.cost;
        cardColor = card.cardColor;
        cardSprite = card.cardSprite;
        cardCategory = card.cardCategory;
        frontDamage = card.frontDamage;
        backDamage = card.backDamage;
    }
}
