using JetBrains.Annotations;
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

    public enum CardCategory
    {
        Hero,
        Follower,
        Equip,
    }

    [System.Serializable]
    public class Card : BattleUnit
    {
        public string cardName;
        public Sprite cardSprite;
        public string skill;
        public string skillContents;
        public int cost;
        public CardType cardColor;
        public CardCategory cardCategory;
        public int frontDamage;
        public int backDamage;

        public Card()
        {
            this.cardName = string.Empty;
            this.skill = string.Empty;
            this.skillContents = string.Empty;
            this.cost = 0;
            this.cardColor = CardType.Neutral;
        }

        public Card(string name, CardType cardType, int cost = 0, bool left = false)
        {
            this.cardName = name;
            this.cardColor = cardType;
        }
    }
}

