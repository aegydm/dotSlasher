using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

namespace tester
{
    //[System.Serializable]
    //public class Field
    //{
    //    public Field Prev;
    //    public Field Next;
    //    public GameObject gameObject;
    //    public bool canBattle = false;
    //    [HideInInspector] public UnitObject unit;

    //    public Field(GameObject gameObject)
    //    {
    //        this.gameObject = gameObject;
    //        if (!gameObject.TryGetComponent<UnitObject>(out unit))
    //        {
    //            Debug.LogError("Unit 컴포넌트를 가지지 않은 유닛을 추가하려고 했습니다.");
    //        }
    //    }

    //    //public void Attack(LinkedBattleField battleField)
    //    //{
    //    //    unit.cardData.AttackStart(battleField, this);
    //    //}

    //    public void Damaged(int damageVal = 0)
    //    {
    //        if (unit.cardData.cardCategory == CardCategory.Hero)
    //        {
    //            unit.cardData.GetDamage(damageVal);
    //        }
    //        else if (unit.cardData.cardCategory == CardCategory.Follower)
    //        {
    //            unit.CardChange(new Unit());
    //        }
    //        else
    //        {
    //            Debug.LogError("아이템을 공격할 수 없습니다");
    //        }
    //    }
    //}
}
