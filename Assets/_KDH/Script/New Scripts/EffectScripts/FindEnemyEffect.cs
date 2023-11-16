using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FindEnemyEffect", menuName = "Effect/BaseEffect/FindEnemyEffect")]
public class FindEnemyEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        if (caster.attackChance)
        {
            if (caster.lookingLeft)
            {
                if (caster.Prev == null || caster.Prev.isEmpty)
                {
                    Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (caster.Prev.playerID != caster.playerID)
                {
                    Debug.Log("공격 대상 : " + caster.Prev.cardData.cardName);
                    targets.Add(caster.Prev);
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                caster.canBattle = false;
                await Task.Delay((int)(Time.deltaTime * 1000));
                return;
            }
            else
            {
                if (caster.Next == null || caster.Next.isEmpty)
                {
                    Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (caster.Next.playerID != caster.playerID)
                {
                    Debug.Log("공격 대상 : " + caster.Next.cardData.cardName);
                    targets.Add(caster.Next);
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                caster.canBattle = false;
                await Task.Delay((int)(Time.deltaTime * 1000));
                return;
            }
        }
    }
}
