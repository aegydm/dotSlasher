using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStartEffect", menuName = "Effect/BaseEffect/AttackStartEffect")]
public class AttackStartEffect : CardEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        targets.Clear();
        if (battleFieldInfo == null)
        {
            Debug.Log("필드가 비어있습니다.");
            return;
        }
        if (caster == null)
        {
            Debug.Log("빈 오브젝트");
            return;
        }
        if (caster.canBattle == false)
        {
            Debug.Log("공격할 수 없는 상태입니다.");
            caster.canBattle = false;
            return;
        }

        Debug.Log(caster.unitObject.cardData.cardName + "의 공격");
    }
}
