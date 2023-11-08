using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageEffect", menuName = "Effect/BaseEffect/GetDamageEffect")]
public class GetDamageEffect : CardEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        caster.animator.Play("Death");
        while ((caster.animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && caster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) == false)
        {

        }
    }
}
