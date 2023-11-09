using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

[System.Serializable]
public class Hero : Card
{
    public bool canAttack = false;
    public CardEffect[] heroSkillEffect = new CardEffect[3];
    public Card equipCard = null;

}
