using CCGCard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<Field> unitList;

    //Test Code
    //public List<Unit> units;
    public List<GameObject> gameObjects;
    //Test End

    public bool dirtySet = false;

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
        //for (int i = 0; i < gameObjects.Count; i++)
        //{
        //    FieldManager.Instance.battleFields.Add(gameObjects[i]);
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

    public void AttackButton()
    {
        if (PhotonNetwork.InRoom)
        {
            GameManager.Instance.photonView.RPC("AttackPhaseNetwork", RpcTarget.All);
        }
        else
        {
            if (GameManager.Instance.gamePhase == GamePhase.BattlePhase && dirtySet == false)
            {
                AttackPhase();
            }
        }
    }

    public void AttackPhase()
    {
        if (GameManager.Instance.gamePhase == GamePhase.BattlePhase && dirtySet == false)
        {
            dirtySet = true;
            StartCoroutine(AttackProcess());
        }
    }

    private IEnumerator AttackProcess()
    {
        Debug.LogError("~~~ 배틀 시작~~~");
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
        dirtySet = false;
        Debug.LogError("*** 배틀 종료 ***");
        GameManager.Instance.EndPhase();
    }
}
