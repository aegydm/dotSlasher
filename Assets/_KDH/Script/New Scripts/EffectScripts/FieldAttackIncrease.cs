using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "FieldAttackIncrease", menuName = "Effect/Effects/FieldAttackIncrease")]
public class FieldAttackIncrease : FieldChangeEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        int count = 0;
        FieldCardObject temp = battleFieldInfo.First;
        while (temp != null)
        {
            if (temp.cardData != null && temp.cardData.cardID != 0)
            {
                count++;
            }
            temp = temp.Next;
        }

        Debug.Log("Attack Add " + count);
        if (caster.cardData != null)
        {
            caster.cardData.frontDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).frontDamage + count;
            caster.cardData.backDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).backDamage + count;
            caster.RenderCard();
        }
        else
        {
            caster.ResetField();
        }
        await Task.Delay(1);
        return;
    }
}
