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

    public List<FieldCardObjectTest> unitList;
    public List<FieldCardObjectTest> battleList;
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
                Debug.LogError("데미지는 0보다 커야 합니다.");
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
        if (clickDirty == false && isBattlePhase && GameManager.Instance.gamePhase == GamePhaseOld.BattlePhase && GameManager.Instance.canAct && (UIManager.Instance.isPopUI == false))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
                List<float> distanceList = new();
                float leastDis = float.MaxValue;
                FieldCardObjectTest clickField = null;
                colliders.Reverse();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.layer == 7 && collider.gameObject.GetComponent<FieldCardObjectTest>().playerID.ToString() == GameManager.Instance.playerID)
                    {
                        distanceList.Add(Mathf.Abs(collider.transform.position.x - mousePos.x));
                        if (leastDis >= Mathf.Abs(collider.transform.position.x - mousePos.x))
                        {
                            clickField = collider.transform.GetComponent<FieldCardObjectTest>();
                        }
                    }
                }
                if (clickField != null)
                {
                    if (clickField.cardData.cardName != string.Empty)
                    {
                        clickDirty = true;
                        GameManager.Instance.photonView.RPC("AttackStartForPun", RpcTarget.All, clickField.transform.position);
                    }
                }
            }
        }
    }
    public IEnumerator AttackProcess(FieldCardObjectTest field)
    {
        //field.orderText.color = Color.red;
        //field.orderIMG.color = Color.blue;
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
            Debug.LogError("*** 배틀 종료 ***");
            GameManager.Instance.EndPhase();
        }
    }

    public void AttackButton()
    {
        if (PhotonNetwork.InRoom)
        {
            if (GameManager.Instance.gamePhase == GamePhaseOld.BattlePhase)
            {
                GameManager.Instance.photonView.RPC("AttackPhaseNetwork", RpcTarget.All);
            }
            else
            {
                Debug.LogWarning("배틀 페이즈가 아닙니다.");
            }
        }
        else
        {
            if (GameManager.Instance.gamePhase == GamePhaseOld.BattlePhase && dirtySet == false)
            {
                AttackPhase();
            }
            else
            {
                Debug.LogWarning("배틀 페이즈가 아닙니다. -single");
            }
        }
    }

    public void AttackPhase()
    {
        if (GameManager.Instance.gamePhase == GamePhaseOld.BattlePhase && dirtySet == false)
        {
            dirtySet = true;
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].cardData.cardCategory == CardCategory.hero)
                {
                    unitList[i].canBattle = ((Hero)unitList[i].cardData).canAttack;
                }

                if (unitList[i].canBattle)
                {
                    battleList.Add(unitList[i]);
                    unitList[i].animator.Play("Idle");
                }
                else
                {
                    //unitList[i].orderIMG.color = Color.blue;
                }
            }
            isBattlePhase = true;
        }
    }


}
