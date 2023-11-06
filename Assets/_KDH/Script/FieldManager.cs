using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;
using System;
using Photon.Pun;
using Photon.Realtime;
public class FieldManager : MonoBehaviour
{
    public List<GameObject> fields;
    public static FieldManager Instance;
    public GameObject FieldPrefab;
    public LinkedBattleField battleFields;
    public int enemyCardNum
    {
        get
        {
            return _enemyCardNum;
        }
        set
        {
            _enemyCardNum = value;
        }
    }

    public bool turnEnd
    {
        set
        {
            if (value == true && GameManager.Instance.gamePhase == GamePhase.ActionPhase)
            {
                GameManager.Instance.EndPhase();
            }
            _turnEnd = value;
        }
    }

    private bool _turnEnd = false;

    private int _enemyCardNum;
    private Vector2 instantiatePosition;

    const int FULL_FIELD_COUNT = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < fields.Count; i++)
        {
            battleFields.Add(fields[i]);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.gamePhase == GamePhase.ActionPhase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                instantiatePosition = new Vector3(mousePos.x, 0, 0);
                RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (rayhit.collider != null)
                {
                    if (rayhit.collider.GetComponent<Field>() != null)
                    {
                        GameManager.Instance.photonView.RPC("PlaceCardForPun", RpcTarget.All, Camera.main.ScreenToWorldPoint(Input.mousePosition), HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID));
                        HandManager.Instance.RemoveHand();
                        //PlaceCard(rayhit.collider.GetComponent<Field>());
                    }
                }
            }
        }
    }

    public void AddUnit(GameObject GO, Card cardData, int id = -1)
    {
        battleFields.Find(GO).unitObject.CardChange(cardData);
        if (cardData.cardName != string.Empty)
        {
            battleFields.Find(GO).canBattle = true;
        }
        BattleManager.instance.unitList.Add(battleFields.Find(GO));
        if (id == -1)
        {
            battleFields.Find(GO).unitObject.playername = GameManager.Instance.playerID;
        }
        else
        {
            battleFields.Find(GO).unitObject.playername = id.ToString();
        }
        GameManager.Instance.myTurn = false;
        //Debug.LogError(cardData.attackStartEffects.Count);
        //Debug.LogError(cardData.attackStartEffects[0].ToString());
    }

    public void PlaceCard(Field field)
    {
        Debug.Log("Test");
        if (HandManager.Instance.selectedHand == null) return;
        if (field.isEmpty)
        {
            //GameManager.Instance.photonView.RPC("AddUnit", RpcTarget.All,field.gameObject, HandManager.Instance.selectedHand.card);
            AddUnit(field.gameObject, HandManager.Instance.selectedHand.card);
            field.SetCard(HandManager.Instance.selectedHand.card);
        }
        else
        {
            if (IsFieldFull()) return;
            fields.Add(field.gameObject);
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card);
            //newField.GetComponent<Field>().Prev = field.Prev;
            //newField.GetComponent<Field>().Next = field;
            //field.Prev.Next = newField.GetComponent<Field>();
            //field.Prev = newField.GetComponent<Field>();  
            battleFields.AddBefore(field, newField);
            Field tmpField = battleFields.First;
            fields.Clear();
            while (tmpField != null)
            {
                fields.Add(tmpField.gameObject);
                tmpField = tmpField.Next;
            }
            for (int pos = (fields.Count - 1) * -9, i = 0; ; pos += 18, i++)
            {
                try
                {
                    fields[i].transform.position = new Vector3(pos, 0, 0);
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }
        HandManager.Instance.RemoveHand();
    }

    public void PlaceCard(Field field, Card card, int id)
    {
        Debug.Log("Test");
        if (card == null) return;
        if (field.isEmpty)
        {
            //GameManager.Instance.photonView.RPC("AddUnit", RpcTarget.All,field.gameObject, HandManager.Instance.selectedHand.card);
            AddUnit(field.gameObject, card, id);
            field.SetCard(card, id % 2 == 0 ? false : true);
        }
        else
        {
            if (IsFieldFull()) return;
            fields.Add(field.gameObject);
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            newField.GetComponent<Field>().SetCard(card, id % 2 == 0 ? false : true);
            //newField.GetComponent<Field>().Prev = field.Prev;
            //newField.GetComponent<Field>().Next = field;
            //field.Prev.Next = newField.GetComponent<Field>();
            //field.Prev = newField.GetComponent<Field>();  
            battleFields.AddBefore(field, newField);
            Field tmpField = battleFields.First;
            fields.Clear();
            while (tmpField != null)
            {
                fields.Add(tmpField.gameObject);
                tmpField = tmpField.Next;
            }
            for (int pos = (fields.Count - 1) * -9, i = 0; ; pos += 18, i++)
            {
                try
                {
                    fields[i].transform.position = new Vector3(pos, 0, 0);
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }
    }

    bool IsFieldFull()
    {
        return fields.Count == FULL_FIELD_COUNT;
    }

    public void TurnEnd()
    {
        turnEnd = true;
    }
}
