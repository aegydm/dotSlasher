using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FindEnemyEffect", menuName = "Effect/BaseEffect/FindEnemyEffect")]
public class FindEnemyEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObjectTest caster, List<FieldCardObjectTest> targets)
    {
        if (caster.canBattle)
        {
            FieldCardObjectTest tmp = caster;
            if (caster.lookingLeft)
            {
                if (tmp.Prev == null || tmp.Prev.isEmpty)
                {
                    Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (tmp.Prev.playerID != caster.playerID)
                {
                    Debug.Log("공격 대상 : " + tmp.Prev.cardData.cardName);
                    targets.Add(tmp.Prev);
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
                if (tmp.Next == null || tmp.Next.isEmpty)
                {
                    Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (tmp.Next.playerID != caster.playerID)
                {
                    Debug.Log("공격 대상 : " + tmp.Next.cardData.cardName);
                    targets.Add(tmp.Next);
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
