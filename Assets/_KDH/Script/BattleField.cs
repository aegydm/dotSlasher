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
    /// 공격할 필드를 지정해서 공격을 실행한 후 적을 처치했을 경우 true를, 실패했을 경우 false를 반환
    /// </summary>
    /// <param name="battleField"> 공격할 필드를 지정해서 공격</param>
    /// <param name="enemyFront"> 적이 나를 보고 있는지 여부</param>
    /// <returns></returns>
    public virtual bool Attack(BattleField battleField, bool enemyFront)
    {
        if (canBattle && battleField != null && battleField.gameObject.GetComponent<Unit>().cardData.name != string.Empty && gameObject.GetComponent<Unit>().cardData.name != string.Empty)
        {
            Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "이/가 " + battleField.gameObject.GetComponent<Unit>().cardData.name + "을/를 공격!");
            int attackPower = gameObject.GetComponent<Unit>().cardData.frontDamage;
            int defensePower = enemyFront ? battleField.gameObject.GetComponent<Unit>().cardData.frontDamage : battleField.gameObject.GetComponent<Unit>().cardData.backDamage;
            canBattle = false;
            Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "의 공격력 : " + attackPower + " / " + battleField.gameObject.GetComponent<Unit>().cardData.name + "의 공격력 : " + defensePower);
            if (attackPower > defensePower)
            {
                Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "의 승리!");
                battleField.canBattle = false;
                battleField.Die();
                return true;
            }
        }
        Debug.Log(gameObject.GetComponent<Unit>().cardData.name + "의 공격이 실패했습니다.");
        return false;
    }

    private void Die()
    {
        gameObject.GetComponent<Unit>().CardChange(new Card());
    }
}
