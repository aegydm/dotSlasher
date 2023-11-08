using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCGCard;
using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
            OnEnemyHandChanged?.Invoke(enemyCardNum);
        }
    }

    private int _enemyCardNum;
    public Vector2 instantiatePosition;
    private Vector2 mousePos;

    public event Action<int> OnEnemyHandChanged;

    const int FULL_FIELD_COUNT = 10;

    Field tmpField;
    [SerializeField] GameObject directionCanvas;
    [SerializeField] TMP_Text enemyHandCount;
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
        OnEnemyHandChanged += EnemyHandCountRender;
    }

    void EnemyHandCountRender(int count)
    {
        enemyHandCount.text = $"EnemyHand : {count}";
    }

    private void Start()
    {
        for (int i = 0; i < fields.Count; i++)
        {
            battleFields.Add(fields[i]);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        if (GameManager.Instance.gamePhase == GamePhase.ActionPhase && GameManager.Instance.canAct)
    //        {
    //            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
    //            foreach (Collider2D collider in colliders)
    //            {
    //                if (collider.gameObject.layer == 7)
    //                {
    //                    //tmpField = collider.gameObject.GetComponent<Field>();
    //                }
    //            }
    //        }
    //    }
    //}

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
            battleFields.Find(GO).unitObject.playerName = GameManager.Instance.playerID;
        }
        else
        {
            battleFields.Find(GO).unitObject.playerName = id.ToString();
        }
    }

    public bool SelectField(Field field)
    {
        if (field.isEmpty)
        {
            tmpField = field;
            SelectDirection(field);
            return true;
        }
        else
        {
            if (field.Prev.isEmpty)
            {
                return false;
            }
            else
            {
                if (IsFieldFull())
                {
                    return false;
                }

                GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
                battleFields.AddBefore(field, newField);
                this.tmpField = battleFields.Find(newField);
                // Draw field in screen
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
                SelectDirection(this.tmpField);
                return true;
            }
        }
    }

    void SelectDirection(Field field)
    {
        if (HandManager.Instance.selectedHand == null) return;
        directionCanvas.transform.position = field.transform.position;
        directionCanvas.SetActive(true);
        canPlace = false;
    }

    public bool SelectField(Field field, Card card, int id, bool lookLeft)
    {
        if (field.isEmpty)
        {
            tmpField = field;
            SelectDirection(field);
            return true;
        }
        else
        {
            if (field.Prev.isEmpty)
            {
                return false;
            }
            else
            {
                if (IsFieldFull())
                {
                    return false;
                }
                GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
                battleFields.AddBefore(field, newField);
                this.tmpField = battleFields.Find(newField);
                // Draw field in screen
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
                SelectDirection(this.tmpField);
                return true;
            }
        }
    }

    public void PlaceCard(Field field, Card card, int id, bool lookLeft)
    {
        if (card == null) return;
        if (field.isEmpty)
        {
            AddUnit(field.gameObject, card, id);
            field.SetCard(card, lookLeft);
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
        GameManager.Instance.PlaceCardForPun(tmpField.transform.position, HandManager.Instance.selectedHand.card.cardID, 0, lookingLeft);
        //PlaceCard(tmpField, HandManager.Instance.selectedHand.card, 0, lookingLeft);
        GameManager.Instance.canAct = false;
        GameManager.Instance.photonView.RPC("PlaceCardForPun", RpcTarget.All, mousePos, HandManager.Instance.selectedHand.card.cardID, int.Parse(GameManager.Instance.playerID), lookingLeft);
        HandManager.Instance.RemoveHand();
    }

    public void TurnEnd()
    {
        if (GameManager.Instance.canAct)
        {
            GameManager.Instance.canAct = false;
        }
        GameManager.Instance.playerEnd = true;
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
