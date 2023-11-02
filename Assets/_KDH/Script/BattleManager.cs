using CCGCard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public LinkedBattleField battleFields;
    public List<BattleField> unitList;

    //Test Code
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
            Debug.Log(battleFields.Last.gameObject.name);
        }

        Debug.Log(gameObjects.Count);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            Debug.Log(gameObjects[i].name);
            AddUnit(gameObjects[i], new Card("Å×½ºÆ® À¯´Ö" + i, Random.Range(0, 10), Random.Range(0, 10), CardType.Neutral, 0, Random.Range(0, 100) < 50));
        }
        //Test End
    }

    public void AddUnit(GameObject GO, Card cardData)
    {
        battleFields.Find(GO).gameObject.GetComponent<Unit>().CardChange(cardData);
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
            bool lookFront;
            if (unitList[i].gameObject.GetComponent<Unit>().cardData.lookingLeft)
            {
                tmp = unitList[i].Prev;
            }
            else
            {
                tmp = unitList[i].Next;
            }
            if (tmp.gameObject.GetComponent<Unit>().cardData.lookingLeft != unitList[i].gameObject.GetComponent<Unit>().cardData.lookingLeft)
            {
                lookFront = true;
            }
            else
            {
                lookFront = false;
            }
            unitList[i].Attack(tmp, lookFront);
            yield return new WaitForSeconds(3);
        }
    }
}
