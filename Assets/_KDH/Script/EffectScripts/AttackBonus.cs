using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBonus", menuName = "Effect/CDEffects/AttackBonusEffect")]
public class AttackBonus : CalculateDamageEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.card.frontDamage += 10;
        base.ExecuteEffect(battleFieldInfo, caster, targets);
        caster.card.frontDamage -= 10;
    }
}
