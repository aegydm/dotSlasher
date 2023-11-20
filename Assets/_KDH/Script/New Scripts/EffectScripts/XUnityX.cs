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

        #region gemAdd
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
        #endregion

        #region rankAdd
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
        #endregion

        #region Gem Straight
        gemSmallStraight = false;
        gemLargeStraight = false;
        temp = caster;
        for (int i = 0; i < 5; i++)
        {
            int gemTemp = gem;
            int gemStraightCount = 0;
            List<int> gemList = new List<int>();
            gemList.Add(gem);
            temp = caster;
            for (int j = i; j < 4; j++)
            {
                temp = temp.Next;
                if (temp != null && temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && !gemList.Contains(int.Parse(temp.cardData.skill[0].ToString())))
                {
                    gemStraightCount++;
                    gemList.Add(int.Parse(temp.cardData.skill[0].ToString()));
                    //Debug.Log("Caster : " + caster + "\nGem Count: " + gemStraightCount+"\nAdd Field : "+temp+"\nAddNumber: " + int.Parse(temp.cardData.skill[0].ToString()));
                }
                else
                {
                    break;
                }
            }
            temp = caster;
            for (int k = 4 - i; k < 4; k++)
            {
                temp = temp.Prev;
                if (temp != null && temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && !gemList.Contains(int.Parse(temp.cardData.skill[0].ToString())))
                {
                    gemStraightCount++;
                    gemList.Add(int.Parse(temp.cardData.skill[0].ToString()));
                    //Debug.Log("Caster : " + caster + "\nGem Count: " + gemStraightCount + "\nAdd Field : " + temp + "\nAddNumber: " + int.Parse(temp.cardData.skill[0].ToString()));
                }
                else
                {
                    break;
                }
            }
            //Debug.Log("Count is : " + gemStraightCount);
            if (gemStraightCount >= 4)
            {
                gemLargeStraight = true;
                break;
            }
            else if (gemStraightCount >= 2)
            {
                gemLargeStraight = false;
                gemSmallStraight = true;
            }
            else
            {
            }
        }

        #endregion

        #region Rank Straight
        rankSmallStraight = false;
        rankLargeStraight = false;
        temp = caster;
        int rankTemp = rank;
        int rankStraightCount = 0;
        temp = caster.Prev;
        for (int i = rankTemp - 1; i >= 1; i--)
        {
            if (temp == null)
            {
                break;
            }
            else if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == i)
            {
                rankStraightCount++;
            }
            else
            {
                break;
            }
            temp = temp.Prev;
        }
        temp = caster.Next;
        for (int i = rankTemp + 1; i <= 5; i++)
        {
            if (temp == null)
            {
                break;
            }
            else if (temp.cardData != null && temp.cardData.cardID != 0 && temp.cardData.skill != string.Empty && int.Parse(temp.cardData.skill[1].ToString()) == i)
            {
                rankStraightCount++;
            }
            else
            {
                break;
            }
            temp = temp.Next;
        }
        if (rankStraightCount >= 4)
        {
            rankLargeStraight = true;
        }
        else if (rankStraightCount >= 2)
        {
            rankLargeStraight = false;
            rankSmallStraight = true;
        }
        else
        {
        }

        #endregion


        if (caster.cardData != null)
        {
            caster.cardData.frontDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).frontDamage + gemCount + rankCount;
            caster.cardData.backDamage = CardDB.instance.FindCardFromID(caster.cardData.cardID).backDamage + gemCount + rankCount;
            if (gemCount > 0)
            {
                //Debug.Log(gemCount);
                caster.gemAddGO.SetActive(true);
                caster.gemAddText.text = "+" + gemCount;
            }
            else
            {
                caster.gemAddGO.SetActive(false);
                caster.gemAddText.text = string.Empty;

            }
            if (rankCount > 0)
            {
                //Debug.Log(rankCount);
                caster.rankAddGO.SetActive(true);
                caster.rankAddText.text = "+" + rankCount;
            }
            else
            {
                caster.rankAddGO.SetActive(false);
                caster.rankAddText.text = string.Empty;
            }
            if (gemLargeStraight)
            {
                caster.cardData.frontDamage *= 3;
                caster.cardData.backDamage *= 3;
                caster.gemMultiGO.SetActive(true);
                caster.gemMultiText.text = "X3";
            }
            else if (gemSmallStraight)
            {
                caster.cardData.frontDamage *= 2;
                caster.cardData.backDamage *= 2;
                caster.gemMultiGO.SetActive(true);
                caster.gemMultiText.text = "X2";
            }
            else
            {
                caster.gemMultiGO.SetActive(false);
                caster.gemMultiText.text = string.Empty;
            }
            if (rankLargeStraight)
            {
                caster.cardData.frontDamage *= 3;
                caster.cardData.backDamage *= 3;
                //Debug.Log("On1");
                caster.rankMultiGO.SetActive(true);
                caster.rankMultiText.text = "X3";
            }
            else if (rankSmallStraight)
            {
                caster.cardData.frontDamage *= 2;
                caster.cardData.backDamage *= 2;
                //Debug.Log("On2");
                caster.rankMultiGO.SetActive(true);
                caster.rankMultiText.text = "X2";
            }
            else
            {
                //Debug.Log(1);
                caster.rankMultiGO.SetActive(false);
                caster.rankMultiText.text = string.Empty;
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
