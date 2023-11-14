using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "AttackStartEffect", menuName = "Effect/BaseEffect/AttackStartEffect")]
public class AttackStartEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        targets.Clear();
        if (battleFieldInfo == null)
        {
            Debug.Log("필드가 비어있습니다.");
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }
        if (caster == null)
        {
            Debug.Log("빈 오브젝트입니다");
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }
        if (caster.attackChance == false)
        {
            Debug.Log("공격권이 없습니다.");
            caster.canBattle = false;
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }

        if (caster.canBattle == false)
        {
            Debug.Log("공격할 수 없는 필드입니다.");
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }

        Debug.Log(caster.cardData.cardName + "의 공격");
        await Task.Delay((int)(Time.deltaTime * 1000));
        return;
    }
}
