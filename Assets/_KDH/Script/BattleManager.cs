using CCGCard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public LinkedBattleField battleFields;
    [HideInInspector] public List<BattleField> unitList;

    //Test Code
    public List<Unit> units;
    public List<GameObject> gameObjects;
    //Test End
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //Test Code
        for (int i = 0; i < gameObjects.Count; i++)
        {
            battleFields.Add(gameObjects[i]);
        }

        for (int i = 0; i < gameObjects.Count; i++)
        {
            AddUnit(gameObjects[i], units[i]);
            //AddUnit(gameObjects[i], new Card("테스트 유닛" + i, Random.Range(0, 10), Random.Range(0, 10), CardType.Neutral, 0, Random.Range(0, 100) < 50));
        }
        //Test End
    }

    public void AddUnit(GameObject GO, Unit cardData)
    {
        battleFields.Find(GO).unit.CardChange(cardData);
        if (cardData.cardName != string.Empty)
        {
            battleFields.Find(GO).canBattle = true;
        }
        unitList.Add(battleFields.Find(GO));
    }

    public void AttackPhase()
    {
        StartCoroutine(AttackProcess());
    }

    private IEnumerator AttackProcess()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            BattleField tmp;
            if (unitList[i].unit.cardData.cardName != string.Empty)
            {
                if (unitList[i].unit.lookingLeft)
                {
                    tmp = unitList[i].Prev;
                }
                else
                {
                    tmp = unitList[i].Next;
                }
                unitList[i].Attack(battleFields);
                yield return new WaitForSeconds(3);
            }
        }
        Debug.Log("*** 배틀 종료 ***");
    }
}
