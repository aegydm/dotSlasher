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
            Debug.Log("�ʵ尡 ����ֽ��ϴ�.");
            return;
        }
        if (caster == null)
        {
            Debug.Log("�� ������Ʈ");
            return;
        }
        if (caster.canBattle == false)
        {
            Debug.Log("������ �� ���� �����Դϴ�.");
            caster.canBattle = false;
            return;
        }

        Debug.Log(caster.unitObject.cardData.cardName + "�� ����");
    }
}
