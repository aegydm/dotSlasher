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
            Debug.LogError("Unit ������Ʈ�� ������ ���� ������ �߰��Ϸ��� �߽��ϴ�.");
        }
    }

    /// <summary>
    /// ������ �ʵ带 �����ؼ� ������ ������ �� ���� óġ���� ��� true��, �������� ��� false�� ��ȯ
    /// </summary>
    /// <param name="battleField"> ������ �ʵ带 �����ؼ� ����</param>
    /// <param name="enemyFront"> ���� ���� ���� �ִ��� ����</param>
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
            Debug.LogError("�������� ������ �� �����ϴ�");
        }
    }
}
