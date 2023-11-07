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
    //public List<GameObject> gameObjects;
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

    public void AttackButton()
    {
        if (PhotonNetwork.InRoom)
        {
            if(GameManager.Instance.gamePhase == GamePhase.BattlePhase)
            {
                GameManager.Instance.photonView.RPC("AttackPhaseNetwork", RpcTarget.All);
            }
            else
            {
                Debug.LogWarning("��Ʋ ����� �ƴմϴ�.");
            }
        }
        else
        {
            if (GameManager.Instance.gamePhase == GamePhase.BattlePhase && dirtySet == false)
            {
                AttackPhase();
            }
            else
            {
                Debug.LogWarning("��Ʋ ����� �ƴմϴ�. -single");
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
        Debug.LogError("~~~ ��Ʋ ����~~~");
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
        Debug.LogError("*** ��Ʋ ���� ***");
        GameManager.Instance.EndPhase();
    }
}
