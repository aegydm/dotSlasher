using CCGCard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<Field> unitList;

    //Test Code
    //public List<Unit> units;
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
            FieldManager.Instance.battleFields.Add(gameObjects[i]);
        }

        //for (int i = 0; i < gameObjects.Count; i++)
        //{
        //    AddUnit(gameObjects[i], units[i]);
        //    //AddUnit(gameObjects[i], new Card("테스트 유닛" + i, Random.Range(0, 10), Random.Range(0, 10), CardType.Neutral, 0, Random.Range(0, 100) < 50));
        //}
        //Test End
    }

    //public void AddUnit(GameObject GO, Unit cardData)
    //{
    //    FieldManager.Instance.battleFields.Find(GO).unit.CardChange(cardData);
    //    if (cardData.cardName != string.Empty)
    //    {
    //        FieldManager.Instance.battleFields.Find(GO).canBattle = true;
    //    }
    //    unitList.Add(FieldManager.Instance.battleFields.Find(GO));
    //}

    public void AttackPhase()
    {
        StartCoroutine(AttackProcess());
    }

    private IEnumerator AttackProcess()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            Field tmp;
            if (unitList[i].unitObject.cardData.cardName != string.Empty)
            {
                if (unitList[i].unitObject.lookingLeft)
                {
                    tmp = unitList[i].Prev;
                }
                else
                {
                    tmp = unitList[i].Next;
                }
                //unitList[i].Attack(FieldManager.Instance.battleFields);
                unitList[i].unitObject.cardData.AttackStart(FieldManager.Instance.battleFields, unitList[i]);
                yield return new WaitForSeconds(3);
            }
        }
        Debug.Log("*** 배틀 종료 ***");
    }
}
