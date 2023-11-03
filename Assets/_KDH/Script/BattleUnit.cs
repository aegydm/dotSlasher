using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit
{
    public virtual void AttackStart(LinkedBattleField battleFieldInfo, BattleField attackUnitInfo)
    {
        if(battleFieldInfo == null)
        {
            Debug.Log("�ʵ尡 ����ֽ��ϴ�.");
            return;
        }
        if (attackUnitInfo == null)
        {
            Debug.Log("�� ������Ʈ");
            return;
        }
        if(attackUnitInfo.canBattle == false)
        {
            Debug.Log("������ �� ���� �����Դϴ�.");
            return;
        } 

        Debug.Log(attackUnitInfo.unit.cardData.cardName + "�� ����");

        BattleField enemyUnitInfo;
        enemyUnitInfo = FindEnemy(battleFieldInfo, attackUnitInfo);
        if (enemyUnitInfo == null)
        {
            Debug.Log("���� ����� �����ϴ�. ������ �����մϴ�");
            return;
        }
        int attackPower;
        int defencePower;
        CalculatePower(attackUnitInfo, enemyUnitInfo, out attackPower, out defencePower);

        if(Attack(attackPower, defencePower))
        {
            Debug.Log(attackUnitInfo.unit.cardData.cardName + "�� ������ �����߽��ϴ�.");
            enemyUnitInfo.Damaged(attackPower);
        }
        else
        {
            Debug.Log(attackUnitInfo.unit.cardData.cardName + "�� ������ �����߽��ϴ�.");
        }
        return;
    }

    protected virtual BattleField FindEnemy(LinkedBattleField battelFieldInfo, BattleField attackUnitInfo)
    {
        BattleField tmp = attackUnitInfo;
        if (attackUnitInfo.unit.lookingLeft)
        {
            if (tmp.Prev == null || tmp.Prev.unit.cardData.cardName == string.Empty)
            {
                return null;
            }
            else if (tmp.Prev.unit.playername != string.Empty)
            {
                return tmp.Prev;
            }
            return null;
        }
        else
        {
            if (tmp.Next == null || tmp.Next.unit.cardData.cardName == string.Empty)
            {
                return null;
            }
            else if (tmp.Next.unit.playername != string.Empty)
            {
                return tmp.Next;
            }
            return null;
        }
    }

    protected virtual void CalculatePower(BattleField attacker, BattleField defender, out int attackPower, out int defencePower)
    {
        attackPower = attacker.unit.cardData.frontDamage;
        defencePower = attacker.unit.lookingLeft != defender.unit.lookingLeft ? defender.unit.cardData.frontDamage : defender.unit.cardData.backDamage;
        Debug.Log(attacker.unit.cardData.cardName + "�� ���ݷ� : " + attackPower + "\n" + defender.unit.cardData.cardName+"�� ���ݷ� : " + defencePower);
    }

    protected virtual bool Attack(int attackPower, int defensePower)
    {
        if(attackPower > defensePower)
        {
            return true;
        }
        return false;
    }
}
