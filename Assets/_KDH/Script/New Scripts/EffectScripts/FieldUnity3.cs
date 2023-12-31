using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "FieldUnity3", menuName = "Effect/Effects/FieldUnity3")]
public class FieldUnity3 : FieldChangeEffect
{
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        int sameCount = 0;
        bool straight = false;
        FieldCardObject temp = caster.Prev;
        while (temp != null)
        {
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill == "시너지3")
            {
                if (sameCount > 2)
                {
                    break;
                }
                sameCount++;
            }
            else
            {
                break;
            }
            temp = temp.Prev;
        }
        temp = caster.Next;
        while (temp != null)
        {
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill == "시너지3")
            {
                if(sameCount > 2)
                {
                    break;
                }
                sameCount++;
            }
            else
            {
                break;
            }
            temp = temp.Next;
        }
        if (sameCount > 0)
        {
            sameCount++;
        }

        temp = caster.Prev;
        if (temp != null && temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill == "시너지2")
        {
            temp = temp.Prev;
            if (temp != null && temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill == "시너지1")
            {
                temp = caster.Next;
                if (temp != null && temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill == "시너지4")
                {
                    straight = true;
                }
            }
        }


        Debug.Log("Attack Add " + sameCount + " + Straight is " + straight);
        if (caster.cardData != null)
        {
            caster.cardData.frontDamage = straight? (CardDB.instance.FindCardFromID(caster.cardData.cardID).frontDamage + sameCount)*2 : CardDB.instance.FindCardFromID(caster.cardData.cardID).frontDamage + sameCount;
            caster.cardData.backDamage = straight? (CardDB.instance.FindCardFromID(caster.cardData.cardID).backDamage + sameCount) * 2 : CardDB.instance.FindCardFromID(caster.cardData.cardID).backDamage + sameCount;
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
