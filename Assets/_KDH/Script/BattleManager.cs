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

    public int damageSum
    {
        get
        {
            return _damageSum;
        }
        set
        {
            _damageSum = value;
            if (_damageSum < 0)
            {
                Debug.LogError("�������� 0���� Ŀ�� �մϴ�.");
            }
        }
    }

    [SerializeField] private int _damageSum;

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
            if (GameManager.Instance.gamePhase == GamePhase.BattlePhase)
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
        for(int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].card.cardCategory == CardCategory.hero)
            {
                unitList[i].canBattle = ((Hero)unitList[i].card).canAttack;
            }
        }

        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].canBattle)
            {
                unitList[i].animator.Play("Idle");
            }
        }

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
                unitList[i].unitObject.cardData.AttackStart(FieldManager.Instance.battleFields, unitList[i]);
                yield return new WaitForSeconds(5);
                unitList[i].orderText.color = Color.red;
            }

        }
        dirtySet = false;
        Debug.LogError("*** ��Ʋ ���� ***");
        GameManager.Instance.EndPhase();
    }
}
