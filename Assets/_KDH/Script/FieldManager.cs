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
    bool canPlace = true;
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
    private Vector2 mousePos;

    const int FULL_FIELD_COUNT = 10;

    Field tmpField;
    [SerializeField] GameObject directionCanvas;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.gamePhase == GamePhase.ActionPhase)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                instantiatePosition = new Vector3(mousePos.x, 0, 0);
                RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (canPlace)
                {
                    //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //instantiatePosition = new Vector3(mousePos.x, 0, 0);
                    //RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                    if (rayhit.collider != null)
                    {
                        if (rayhit.collider.GetComponent<Field>() != null)
                        {
                            tmpField = rayhit.collider.GetComponent<Field>();
                            SelectDirection(tmpField);
                        }
                    }
                }
                else
                {
                    //UnitObject unitData = rayhit.collider.GetComponent<Field>().getFieldUnit();
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
    void SelectDirection(Field field)
    {
        directionCanvas.transform.position = field.transform.position;
        directionCanvas.SetActive(true);
        canPlace = false;
    }

    public void PlaceCard(Field field, Card card, int id, bool lookLeft)
    {
        Debug.Log("Test2");
        if (card == null) return;
        if (field.isEmpty)
        {
            AddUnit(field.gameObject, card, id);
            field.SetCard(card, lookLeft);
        }
        else
        {
            if (IsFieldFull()) return;
            fields.Add(field.gameObject);
            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);

            battleFields.AddBefore(field, newField);
            AddUnit(newField.gameObject, card);
            newField.GetComponent<Field>().SetCard(card, lookLeft);

            Field tmpField = battleFields.First;
            fields.Clear();
            while (tmpField != null)
            {
                fields.Add(tmpField.gameObject);
                tmpField = tmpField.Next;
            }
            for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
            {
                try
                {
                    fields[i].transform.position = new Vector3(pos, 0, 0);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    break;
                }
            }
        }
        directionCanvas.SetActive(false);
        canPlace = true;
    }

    bool IsFieldFull()
    {
        return fields.Count == FULL_FIELD_COUNT;
    }

    public void ResetAllField()
    {
        int i = 0;
        Field tmpField = battleFields.First;

        for (; i < 5; i++)
        {
            tmpField = tmpField.Next;
        }
        while (tmpField != null)
        {
            battleFields.Remove(tmpField);
            fields.Remove(tmpField.gameObject);
            Destroy(tmpField.gameObject);
            tmpField = tmpField.Next;
        }
    }

    /// <summary>
    /// OnClick용도의 함수
    /// </summary>
    /// <param name="lookingLeft"></param>
    public void SelectDirection(bool lookingLeft)
    {
        //PlaceCard(tmpField, lookingLeft);
        //GameManager.Instance.PlaceCardForPun(mousePos, HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID), lookingLeft);
        GameManager.Instance.photonView.RPC("PlaceCardForPun", RpcTarget.All, mousePos, HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID), lookingLeft);
        HandManager.Instance.RemoveHand();
    }

    public void TurnEnd()
    {
        turnEnd = true;
    }

    //public void PlaceCard(Field field)
    //{
    //    Debug.Log("Test1");
    //    if (HandManager.Instance.selectedHand == null) return;
    //    if (field.isEmpty)
    //    {
    //        AddUnit(field.gameObject, HandManager.Instance.selectedHand.card);
    //        field.SetCard(HandManager.Instance.selectedHand.card);
    //    }
    //    else
    //    {
    //        if (IsFieldFull()) return;
    //        fields.Add(field.gameObject);
    //        GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
    //        newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card);
    //        battleFields.AddBefore(field, newField);
    //        Field tmpField = battleFields.First;
    //        fields.Clear();
    //        while (tmpField != null)
    //        {
    //            fields.Add(tmpField.gameObject);
    //            tmpField = tmpField.Next;
    //        }
    //        for (int pos = (fields.Count - 1) * -9, i = 0; ; pos += 18, i++)
    //        {
    //            try
    //            {
    //                fields[i].transform.position = new Vector3(pos, 0, 0);
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.LogError(e.Message);
    //                break;
    //            }
    //        }
    //    }
    //    HandManager.Instance.RemoveHand();
    //}

    //void PlaceCard(Field field, bool lookingLeft)
    //{
    //    Debug.Log("Test33");
    //    if (HandManager.Instance.selectedHand == null) return;
    //    if (field.isEmpty)
    //    {
    //        AddUnit(field.gameObject, HandManager.Instance.selectedHand.card);
    //        field.SetCard(HandManager.Instance.selectedHand.card, lookingLeft);
    //    }
    //    else
    //    {
    //        if (IsFieldFull()) return;

    //        fields.Add(field.gameObject);
    //        GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);

    //        battleFields.AddBefore(field, newField);
    //        AddUnit(newField.gameObject, HandManager.Instance.selectedHand.card);
    //        newField.GetComponent<Field>().SetCard(HandManager.Instance.selectedHand.card, lookingLeft);

    //        Field tmpField = battleFields.First;
    //        fields.Clear();
    //        while (tmpField != null)
    //        {
    //            fields.Add(tmpField.gameObject);
    //            tmpField = tmpField.Next;
    //        }
    //        for (int pos = (fields.Count - 1) * -9, i = 0; ; pos += 18, i++)
    //        {
    //            try
    //            {
    //                fields[i].transform.position = new Vector3(pos, 0, 0);
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.LogError(e.Message);
    //                break;
    //            }
    //        }
    //    }
    //    directionCanvas.SetActive(false);
    //    canPlace = true;
    //    HandManager.Instance.RemoveHand();
    //}

}
