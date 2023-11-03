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
        Ninza,
        Egypt,
        Ghost,
        Monster,
        Ice,
        Neutral,
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
        List<Field> enemyUnitInfo;
        public string cardName;
        public Sprite cardSprite;
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
            this.cardColor = CardType.Neutral;
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

        public void GetDamage(ref int damageVal)
        {
            ActiveEffect(getDamageEffects, null, null, null);
        }
        #endregion
    }
}

