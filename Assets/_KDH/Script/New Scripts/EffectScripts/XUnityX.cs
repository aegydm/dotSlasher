using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "XUnityX", menuName = "Effect/Effects/XUnityX")]
public class XUnityX : FieldChangeEffect
{
    public int gem;
    public int rank;
    public override async Task ExecuteEffect(LinkedBattleField battleFieldInfo, FieldCardObject caster, List<FieldCardObject> targets)
    {
        int gemCount = 0;
        int rankCount = 0;
        bool gemSmallStraight = false;
        bool gemLargeStraight = false;
        bool rankSmallStraight = false;
        bool rankLargeStraight = false;

        FieldCardObject temp = caster.Prev;
        while (temp != null)
        {
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[0].ToString()) == gem)
            {
                if (gemCount > 3)
                {
                    break;
                }
                gemCount++;
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
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[0].ToString()) == gem)
            {
                if (gemCount > 3)
                {
                    break;
                }
                gemCount++;
            }
            else
            {
                break;
            }
            temp = temp.Next;
        }

        temp = caster.Prev;
        while (temp != null)
        {
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == rank)
            {
                if (rankCount > 3)
                {
                    break;
                }
                rankCount++;
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
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == rank)
            {
                if (rankCount > 3)
                {
                    break;
                }
                rankCount++;
            }
            else
            {
                break;
            }
            temp = temp.Next;
        }

        #region Gem Straight
        int gemTemp = gem;
        int gemStraightCount = 0;
        gemSmallStraight = false;
        gemLargeStraight = false;
        temp = caster;
        while (gemTemp > 1)
        {
            temp = temp.Prev;
            gemTemp--;
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[0].ToString()) == gemTemp)
            {
                gemStraightCount++;
            }
            else
            {
                break;
            }
        }
        temp = caster;
        gemTemp = gem;
        while (gemTemp < 5)
        {
            temp = temp.Next;
            gemTemp++;
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[0].ToString()) == gemTemp)
            {
                gemStraightCount++;
            }
            else
            {
                break;
            }
        }

        if (gemStraightCount > 4)
        {
            gemLargeStraight = true;
        }
        else if (gemStraightCount > 2)
        {
            gemLargeStraight = false;
            gemSmallStraight = true;
        }
        else
        {
            gemLargeStraight = false;
            gemSmallStraight = false;
        }
        #endregion

        #region Rank Straight
        int rankTemp = gem;
        int rankStraightCount = 0;
        rankSmallStraight = false;
        rankLargeStraight = false;
        temp = caster;
        while (rankTemp > 1)
        {
            temp = temp.Prev;
            rankTemp--;
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == rankTemp)
            {
                rankStraightCount++;
            }
            else
            {
                break;
            }
        }
        temp = caster;
        rankTemp = gem;
        while (rankTemp < 5)
        {
            temp = temp.Next;
            rankTemp++;
            if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == rankTemp)
            {
                rankStraightCount++;
            }
            else
            {
                break;
            }
        }

        if (rankStraightCount > 4)
        {
            rankLargeStraight = true;
        }
        else if (rankStraightCount > 2)
        {
            rankLargeStraight = false;
            rankSmallStraight = true;
        }
        else
        {
            rankLargeStraight = false;
            rankSmallStraight = false;
        }
        #endregion

        if (caster.cardData != null)
        {
            caster.cardData.frontDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).frontDamage + gemCount + rankCount;
            caster.cardData.backDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).backDamage + gemCount + rankCount;
            if(gemLargeStraight)
            {
                caster.cardData.frontDamage *= 3;
                caster.cardData.backDamage *= 3;
            }
            else if(gemSmallStraight)
            {
                caster.cardData.frontDamage *= 2;
                caster.cardData.backDamage *= 2;
            }
            if (rankLargeStraight)
            {
                caster.cardData.frontDamage *= 3;
                caster.cardData.backDamage *= 3;
            }
            else if (rankSmallStraight)
            {
                caster.cardData.frontDamage *= 2;
                caster.cardData.backDamage *= 2;
            }
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
