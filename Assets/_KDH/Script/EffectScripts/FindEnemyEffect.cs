using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FindEnemyEffect", menuName = "Effect/BaseEffect/FindEnemyEffect")]
public class FindEnemyEffect : CardEffect
{
    public override void ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
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
                    return;
                }
                else if (tmp.Prev.unitObject.playerID != caster.unitObject.playerID)
                {
                    Debug.Log("���� ��� : " + tmp.Prev.unitObject.cardData.cardName);
                    targets.Add(tmp.Prev);
                    return;
                }
                Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                caster.canBattle = false;
                return;
            }
            else
            {
                if (tmp.Next == null || tmp.Next.isEmpty)
                {
                    Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                    caster.canBattle = false;
                    return;
                }
                else if (tmp.Next.unitObject.playerID != caster.unitObject.playerID)
                {
                    Debug.Log("���� ��� : " + tmp.Next.unitObject.cardData.cardName);
                    targets.Add(tmp.Next);
                    return;
                }
                Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
                caster.canBattle = false;
                return;
            }
        }
    }
}
