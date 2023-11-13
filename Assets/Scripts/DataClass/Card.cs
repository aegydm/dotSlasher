using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        minion,
        hero,
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
        List<FieldCardObjectTest> enemyUnitInfo = new List<FieldCardObjectTest>();
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
            this.cardID = 0;
            this.cardName = string.Empty;
            this.cardSprite = null;
            this.animator = string.Empty;
            this.skill = string.Empty;
            this.skillContents = string.Empty;
            this.cost = 0;
            this.cardColor = CardType.f7;
            this.cardCategory = CardCategory.minion;
            this.frontDamage = -1;
            this.backDamage = -1;
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

        private async Task ActiveEffect(List<CardEffect> cardEffects, LinkedBattleField battleFieldInfo, FieldCardObjectTest casterInfo, List<FieldCardObjectTest> targetInfos)
        {
            for (int i = 0; i < cardEffects.Count; i++)
            {
                Task effectTask = cardEffects[i].ExecuteEffect(battleFieldInfo, casterInfo, targetInfos);
                await effectTask;
            }
        }

        public async void Summon(LinkedBattleField battleFieldInfo, FieldCardObjectTest casterInfo)
        {
            await ActiveEffect(summonEffects, battleFieldInfo, casterInfo, enemyUnitInfo);
        }

        public async void AttackStart(LinkedBattleField battleFieldInfo, FieldCardObjectTest casterInfo)
        {
            await ActiveEffect(attackStartEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            await ActiveEffect(findEnemyEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            await ActiveEffect(calculateDamageEffects, battleFieldInfo, casterInfo, enemyUnitInfo);

            await ActiveEffect(attackProcessEffects, battleFieldInfo, casterInfo, enemyUnitInfo);
            return;
        }

        public async void GetDamage(FieldCardObjectTest thisCard, int damageVal)
        {
            if (thisCard.cardData.cardCategory == CardCategory.hero && thisCard.playerID.ToString() == GameManager.Instance.playerID)
            {
                BattleManager.instance.damageSum += damageVal;
            }
            await ActiveEffect(getDamageEffects, null, thisCard, null);
        }
        #endregion

        public virtual Card Copy()
        {
            Card tmp = new Card();
            tmp.cardID = cardID;
            tmp.cardName = cardName;
            tmp.cardSprite = cardSprite;
            tmp.animator = animator;
            tmp.skill = skill;
            tmp.skillContents = skillContents;
            tmp.cost = cost;
            tmp.cardColor = cardColor;
            tmp.cardCategory = cardCategory;
            tmp.frontDamage = frontDamage;
            tmp.backDamage = backDamage;

            tmp.summonEffects = summonEffects;
            tmp.attackStartEffects = attackStartEffects;
            tmp.findEnemyEffects = findEnemyEffects;
            tmp.calculateDamageEffects = calculateDamageEffects;
            tmp.attackProcessEffects = attackProcessEffects;
            tmp.getDamageEffects = getDamageEffects;
            return tmp;
        }
    }
}

