using CCGCard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<Field> unitList;
    public List<Field> battleList;
    public bool isBattlePhase = false;
    //Test Code
    //public List<Unit> units;
    //public List<GameObject> gameObjects;
    //Test End

    public bool dirtySet = false;
    public bool clickDirty = false;

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

    private void FixedUpdate()
    {
        if (clickDirty == false && isBattlePhase && GameManager.Instance.gamePhase == GamePhase.BattlePhase && GameManager.Instance.canAct && (UIManager.Instance.isPopUI == false))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
                List<float> distanceList = new();
                float leastDis = float.MaxValue;
                Field clickField = null;
                colliders.Reverse();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.layer == 7 && collider.gameObject.GetComponent<Field>().unitObject.playerID == GameManager.Instance.playerID)
                    {
                        distanceList.Add(Mathf.Abs(collider.transform.position.x - mousePos.x));
                        if (leastDis >= Mathf.Abs(collider.transform.position.x - mousePos.x))
                        {
                            clickField = collider.transform.GetComponent<Field>();
                        }
                    }
                }
                if (clickField != null)
                {
                    if (clickField.unitObject.cardData.cardName != string.Empty)
                    {
                        clickDirty = true;
                        GameManager.Instance.photonView.RPC("AttackStartForPun", RpcTarget.All, clickField.transform.position);
                    }
                }
            }
        }
    }
    public IEnumerator AttackProcess(Field field)
    {
        field.orderText.color = Color.red;
        field.orderIMG.color = Color.blue;
        yield return new WaitForSeconds(5);
        battleList.Remove(field);
        if (battleList.Count > 0)
        {
            GameManager.Instance.canAct = false;
            clickDirty = false;
        }
        else
        {
            dirtySet = false;
            isBattlePhase = false;
            Debug.LogError("*** ��Ʋ ���� ***");
            GameManager.Instance.EndPhase();
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
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].card.cardCategory == CardCategory.hero)
                {
                    unitList[i].canBattle = ((Hero)unitList[i].card).canAttack;
                }

                if (unitList[i].canBattle)
                {
                    battleList.Add(unitList[i]);
                    unitList[i].animator.Play("Idle");
                }
                else
                {
                    unitList[i].orderIMG.color = Color.blue;
                }
            }
            isBattlePhase = true;
        }
    }


}
