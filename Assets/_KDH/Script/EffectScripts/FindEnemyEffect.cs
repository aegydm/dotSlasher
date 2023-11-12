using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FindEnemyEffect", menuName = "Effect/BaseEffect/FindEnemyEffect")]
public class FindEnemyEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        if (caster.canBattle)
        {
            Field tmp = caster;
            if (caster.unitObject.lookingLeft)
            {
                if (tmp.Prev == null || tmp.Prev.isEmpty)
                {
                    Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (tmp.Prev.unitObject.playerID != caster.unitObject.playerID)
                {
                    Debug.Log("���� ��� : " + tmp.Prev.unitObject.cardData.cardName);
                    targets.Add(tmp.Prev);
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                caster.canBattle = false;
                await Task.Delay((int)(Time.deltaTime * 1000));
                return;
            }
            else
            {
                if (tmp.Next == null || tmp.Next.isEmpty)
                {
                    Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                    caster.canBattle = false;
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                else if (tmp.Next.unitObject.playerID != caster.unitObject.playerID)
                {
                    Debug.Log("���� ��� : " + tmp.Next.unitObject.cardData.cardName);
                    targets.Add(tmp.Next);
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    return;
                }
                Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                caster.canBattle = false;
                await Task.Delay((int)(Time.deltaTime * 1000));
                return;
            }
        }
    }
}
