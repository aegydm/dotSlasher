using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCGCard
{
    public enum CardType
    {
        Knight,
        Ninza,
        Egypt,
        Ghost,
        Monster,
        Ice,
        Neutral,
    }

    [System.Serializable]
    public class Card
    {
        public string name;
        public int frontDamage;
        public int backDamage;
        public bool lookingLeft;
        public Sprite cardSprite;
        public string skill;
        public string skillContents;
        public int cost;
        public CardType cardColor;

        public Card()
        {
            this.name = string.Empty;
            this.frontDamage = 0;
            this.backDamage = 0;
            this.lookingLeft = false;
            this.skill = string.Empty;
            this.skillContents = string.Empty;
            this.cost = 0;
            this.cardColor = CardType.Neutral;
        }

        public Card(string name, int frontDamage, int backDamage, CardType cardType, int cost = 0, bool left = false)
        {
            this.name = name;
            this.frontDamage = frontDamage;
            this.backDamage = backDamage;
            this.lookingLeft = left;
            this.cardColor = cardType;
        }
    }
}

