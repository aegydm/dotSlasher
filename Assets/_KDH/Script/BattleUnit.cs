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
            Debug.Log("필드가 비어있습니다.");
            return;
        }
        if (attackUnitInfo == null)
        {
            Debug.Log("빈 오브젝트");
            return;
        }
        if(attackUnitInfo.canBattle == false)
        {
            Debug.Log("공격할 수 없는 상태입니다.");
            return;
        } 

        Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "의 공격");

        Field enemyUnitInfo;
        enemyUnitInfo = FindEnemy(battleFieldInfo, attackUnitInfo);
        if (enemyUnitInfo == null)
        {
            Debug.Log("공격 대상이 없습니다. 공격을 종료합니다");
            return;
        }
        int attackPower;
        int defencePower;
        CalculatePower(attackUnitInfo, enemyUnitInfo, out attackPower, out defencePower);

        if(Attack(attackPower, defencePower))
        {
            Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "의 공격이 성공했습니다.");
            enemyUnitInfo.Damaged(attackPower);
        }
        else
        {
            Debug.Log(attackUnitInfo.unitObject.cardData.cardName + "의 공격이 실패했습니다.");
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
        Debug.Log(attacker.unitObject.cardData.cardName + "의 공격력 : " + attackPower + "\n" + defender.unitObject.cardData.cardName+"의 공격력 : " + defencePower);
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
