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

        Debug.Log(attackUnitInfo.unit.cardData.cardName + "의 공격");

        BattleField enemyUnitInfo;
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
            Debug.Log(attackUnitInfo.unit.cardData.cardName + "의 공격이 성공했습니다.");
            enemyUnitInfo.Damaged(attackPower);
        }
        else
        {
            Debug.Log(attackUnitInfo.unit.cardData.cardName + "의 공격이 실패했습니다.");
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
        Debug.Log(attacker.unit.cardData.cardName + "의 공격력 : " + attackPower + "\n" + defender.unit.cardData.cardName+"의 공격력 : " + defencePower);
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
