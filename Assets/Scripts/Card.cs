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
        public CardType? cardColor;
        public Animator spriteAnimator;


        public Card(string name, int frontDamage, int backDamage, bool left)
        {
            this.name = name;
            this.frontDamage = frontDamage;
            this.backDamage = backDamage;
            this.lookingLeft = left;
        }
    }
}

