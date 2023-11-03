using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

[System.Serializable]
public class BattleField
{
    public BattleField Prev;
    public BattleField Next;
    public GameObject gameObject;
    public bool canBattle = false;
    [HideInInspector] public UnitObject unit;

    public BattleField(GameObject gameObject)
    {
        this.gameObject = gameObject;
        if (!gameObject.TryGetComponent<UnitObject>(out unit))
        {
            Debug.LogError("Unit 컴포넌트를 가지지 않은 유닛을 추가하려고 했습니다.");
        }
    }

    /// <summary>
    /// 공격할 필드를 지정해서 공격을 실행한 후 적을 처치했을 경우 true를, 실패했을 경우 false를 반환
    /// </summary>
    /// <param name="battleField"> 공격할 필드를 지정해서 공격</param>
    /// <param name="enemyFront"> 적이 나를 보고 있는지 여부</param>
    /// <returns></returns>
    public void Attack(LinkedBattleField battleField)
    {
        unit.cardData.AttackStart(battleField, this);
    }

    public void Damaged(int damageVal = 0)
    {
        if(unit.cardData.cardCategory == CardCategory.Hero)
        {
            unit.cardData.GetDamage(damageVal);
        }
        else if(unit.cardData.cardCategory == CardCategory.Follower)
        {
            unit.CardChange(new Unit());
        }
        else
        {
            Debug.LogError("아이템을 공격할 수 없습니다");
        }
    }
}
