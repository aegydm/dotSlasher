using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "AttackStartEffect", menuName = "Effect/BaseEffect/AttackStartEffect")]
public class AttackStartEffect : CardEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, Field caster, List<Field> targets)
    {
        targets.Clear();
        if (battleFieldInfo == null)
        {
            Debug.Log("�ʵ尡 ����ֽ��ϴ�.");
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }
        if (caster == null)
        {
            Debug.Log("�� ������Ʈ");
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }
        if (caster.canBattle == false)
        {
            Debug.Log("������ �� ���� �����Դϴ�.");
            caster.canBattle = false;
            await Task.Delay((int)(Time.deltaTime * 1000));
            return;
        }

        Debug.Log(caster.unitObject.cardData.cardName + "�� ����");
        await Task.Delay((int)(Time.deltaTime * 1000));
        return;
    }
}
