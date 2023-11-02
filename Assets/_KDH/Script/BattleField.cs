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
    public bool canBattle = true;

    public BattleField(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    /// <summary>
    /// ������ �ʵ带 �����ؼ� ������ ������ �� ���� óġ���� ��� true��, �������� ��� false�� ��ȯ
    /// </summary>
    /// <param name="battleField"> ������ �ʵ带 �����ؼ� ����</param>
    /// <param name="enemyFront"> ���� ���� ���� �ִ��� ����</param>
    /// <returns></returns>
    public virtual bool Attack(BattleField battleField, bool enemyFront)
    {
        if (canBattle && battleField != null && battleField.gameObject.GetComponent<Unit>().cardData.name != string.Empty && gameObject.GetComponent<Unit>().cardData.name != string.Empty)
        {
            Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "��/�� " + battleField.gameObject.GetComponent<Unit>().cardData.name + "��/�� ����!");
            int attackPower = gameObject.GetComponent<Unit>().cardData.frontDamage;
            int defensePower = enemyFront ? battleField.gameObject.GetComponent<Unit>().cardData.frontDamage : battleField.gameObject.GetComponent<Unit>().cardData.backDamage;
            canBattle = false;
            Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "�� ���ݷ� : " + attackPower + " / " + battleField.gameObject.GetComponent<Unit>().cardData.name + "�� ���ݷ� : " + defensePower);
            if (attackPower > defensePower)
            {
                Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "�� �¸�!");
                battleField.canBattle = false;
                battleField.Die();
                return true;
            }
        }
        Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "�� ������ �����߽��ϴ�.");
        return false;
    }

    private void Die()
    {
        gameObject.GetComponent<Unit>().CardChange(new Card());
    }
}
