using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

[System.Serializable]
public class Hero : Card
{
    public bool canAttack
    {
        get
        {
            return _canAttack;
        }
        set
        {
            _canAttack = value;
        }
    }

    private bool _canAttack = false;
    public CardEffect[] heroSkillEffect = new CardEffect[3];
    public Card equipCard = null;

    public override Card Copy()
    {
        Card tmp = base.Copy();
        if(base.Copy() is Hero)
        {
            ((Hero)base.Copy()).canAttack = canAttack;
            ((Hero)base.Copy()).heroSkillEffect = heroSkillEffect;
            ((Hero)base.Copy()).equipCard = equipCard;
        }
        return base.Copy();
    }

    public static Card ChangeToHero(Card card)
    {
        Hero tmp = new Hero();
        tmp.cardID = card.cardID;
        tmp.cardName = card.cardName;
        tmp.cardSprite = card.cardSprite;
        tmp.animator = card.animator;
        tmp.skill = card.skill;
        tmp.skillContents = card.skillContents;
        tmp.cost = card.cost;
        tmp.cardColor = card.cardColor;
        tmp.cardCategory = card.cardCategory;
        tmp.frontDamage = card.frontDamage;
        tmp.backDamage = card.backDamage;

        tmp.summonEffects = card.summonEffects;
        tmp.attackStartEffects = card.attackStartEffects;
        tmp.findEnemyEffects = card.findEnemyEffects;
        tmp.calculateDamageEffects = card.calculateDamageEffects;
        tmp.attackProcessEffects = card.attackProcessEffects;
        tmp.getDamageEffects = card.getDamageEffects;

        tmp.heroSkillEffect[0] = Resources.Load<CardEffect>("ScriptableObject/HeroSkills/HeroSkill1");

        return tmp;
    }

    public async void SkillUse(LinkedBattleField battleFieldInfo, FieldCardObject casterInfo)
    {
        List<CardEffect> skill1 = new List<CardEffect>();
        skill1.Add(heroSkillEffect[0]);
        await ActiveEffect(skill1, battleFieldInfo, casterInfo, enemyUnitInfo);
    }
}
