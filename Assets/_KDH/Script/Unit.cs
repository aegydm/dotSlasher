using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit : Card
{
    public int frontDamage;
    public int backDamage;

    public Unit()
    {
        this.cardName = string.Empty;
        this.frontDamage = 0;
        this.backDamage = 0;
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

    public virtual void GetDamage(int damageVal)
    {
        Debug.Log("GETDAMAGE");
    }
}
