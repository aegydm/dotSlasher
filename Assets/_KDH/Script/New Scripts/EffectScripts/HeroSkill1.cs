using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroSkill1", menuName = "Effect/HeroSkills/HeroSkill1")]
public class HeroSkill1 : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleField, FieldCardObject caster, List<FieldCardObject> targets)
    {
        caster.cardData.frontDamage += 5;
        caster.cardData.backDamage += 5;
        ((Hero)caster.cardData).canAttack = true;
        caster.RenderCard();
        await Task.Delay(1);
    }
}
