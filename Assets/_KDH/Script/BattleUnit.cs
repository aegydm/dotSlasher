using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit
{
    public virtual void AttackStart(LinkedBattleField battleFieldInfo, Field attackUnitInfo)
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

        Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "�� ����");

        Field enemyUnitInfo;
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
            Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "�� ������ �����߽��ϴ�.");
            enemyUnitInfo.Damaged(attackPower);
        }
        else
        {
            Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "�� ������ �����߽��ϴ�.");
        }
        return;
    }

    protected virtual Field FindEnemy(LinkedBattleField battelFieldInfo, Field attackUnitInfo)
    {
        Field tmp = attackUnitInfo;
        if (attackUnitInfo.unitObject.lookingLeft)
        {
            if (tmp.Prev == null || tmp.Prev.unitObject.cardData.cardName == string.Empty)
            {
                return null;
            }
            else if (tmp.Prev.unitObject.playername != string.Empty)
            {
                return tmp.Prev;
            }
            return null;
        }
        else
        {
            if (tmp.Next == null || tmp.Next.unitObject.cardData.cardName == string.Empty)
            {
                return null;
            }
            else if (tmp.Next.unitObject.playername != string.Empty)
            {
                return tmp.Next;
            }
            return null;
        }
    }

    protected virtual void CalculatePower(Field attacker, Field defender, out int attackPower, out int defencePower)
    {
        attackPower = attacker.unitObject.cardData.frontDamage;
        defencePower = attacker.unitObject.lookingLeft != defender.unitObject.lookingLeft ? defender.unitObject.cardData.frontDamage : defender.unitObject.cardData.backDamage;
        Debug.Log(attacker.unitObject.cardData.cardName + "�� ���ݷ� : " + attackPower + "\n" + defender.unitObject.cardData.cardName+"�� ���ݷ� : " + defencePower);
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
