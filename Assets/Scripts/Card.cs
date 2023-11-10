using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CCGCard
{
    public enum CardType
    {
        f1,
        f2,
        f3,
        f4,
        f5,
        f6,
        f7,
    }

    public enum CardCategory
    {
        hero,
        minion,
        equipment,
    }

    [System.Serializable]
    public class Card
    {
        #region 멤버 
        #region 효과 모음
        public List<CardEffect> summonEffects = new();
        public List<CardEffect> attackStartEffects = new();
        public List<CardEffect> findEnemyEffects = new();
        public List<CardEffect> calculateDamageEffects = new();
        public List<CardEffect> attackProcessEffects = new();
        public List<CardEffect> getDamageEffects = new();
        #endregion
        List<Field> enemyUnitInfo = new List<Field>();
        public int cardID;
        public string cardName;
        public Sprite cardSprite;
        public string animator;
        public string skill;
        public string skillContents;
        public int cost;
        public CardType cardColor;
        public CardCategory cardCategory;
        public int frontDamage;
        public int backDamage;
        #endregion

        #region 생성자
        public Card()
        {
            this.cardName = string.Empty;
            this.skill = string.Empty;
            this.skillContents = string.Empty;
            this.cost = 0;
            this.cardColor = CardType.f7;
        }

        public Card(string name, CardType cardType, int cost = 0)
        {
            this.cardName = name;
            this.cardColor = cardType;
            this.cost = cost;
            //summonEffects.Add(new SummonEffect());
            //attackStartEffects.Add(new AttackStartEffect());
            //findEnemyEffects.Add(new FindEnemyEffect());
            //calculateDamageEffects.Add(new CalculateDamageEffect());
            //attackProcessEffects.Add(new AttackProcessEffect());
            //getDamageEffects.Add(new GetDamageEffect());
        }

        #endregion

        #region 순차적 처리

        private void ActiveEffect(List<CardEffect> cardEffects, LinkedBattleField battleFieldInfo, Field casterInfo, List<Field> targetInfos)
        {
            for (int i = 0; i < cardEffects.Count; i++)
            {
                cardEffects[i].ExecuteEffect(battleFieldInfo, casterInfo, targetInfos);
            }
        }

        public void Summon(LinkedBattleField battleFieldInfo, Field casterInfo)
        {
            ActiveEffect(summonEffects, battleFieldInfo, casterInfo, enemyUnitInfo);
        }

        public void AttackStart(LinkedBattleField battleFieldInfo, Field casterInfo)
        {
            ActiveEffect(attackStartEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            ActiveEffect(findEnemyEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            ActiveEffect(calculateDamageEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            ActiveEffect(attackProcessEffects, battleFieldInfo, casterInfo, enemyUnitInfo);
            return;
        }

        public void GetDamage(Field thisCard, ref int damageVal)
        {
            if(thisCard.card.cardCategory == CardCategory.hero && thisCard.unitObject.playerID == GameManager.Instance.playerID)
            {
                BattleManager.instance.damageSum += damageVal;
            }
            ActiveEffect(getDamageEffects, null, thisCard, null);
        }
        #endregion

        public virtual Card Copy()
        {
            Card tmp = new Card();
            tmp.cardID = cardID;
            tmp.cardName = cardName;
            tmp.cardColor = cardColor;
            tmp.cardCategory = cardCategory;
            tmp.frontDamage = frontDamage;
            tmp.backDamage = backDamage;
            tmp.animator = animator;
            tmp.cardSprite = cardSprite;
            tmp.summonEffects = summonEffects;
            tmp.attackStartEffects = attackStartEffects;
            tmp.findEnemyEffects = findEnemyEffects;
            tmp.calculateDamageEffects = calculateDamageEffects;
            tmp.attackProcessEffects = attackProcessEffects;
            return tmp;
        }
    }
}

