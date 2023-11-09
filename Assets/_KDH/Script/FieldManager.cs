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

    public bool canPlace
    {
        get
        {
            return _canPlace;
        }
        set
        {
            _canPlace = value;
        }
    }

    private bool _canPlace;

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
    public Vector2 mousePos;

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
        canPlace = true;
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
    void SelectDirection(Field field)
    {
        if (HandManager.Instance.selectedHand == null) return;
        directionCanvas.transform.position = field.transform.position;
        directionCanvas.SetActive(true);
        canPlace = false;
    }

    public bool SelectField(Field field, bool isLeft)
    {
        if (field.isEmpty)
        {
            tmpField = field;
            SelectDirection(field);
            return true;
        }
        else
        {
            if (isLeft)
            {
                if (field.Prev != null && field.Prev.isEmpty)
                {
                    return false;
                }
                else
                {
                    if (IsFieldFull())
                    {
                        return false;
                    }
                    mousePos = field.transform.position;
                    Debug.LogError("謝難" + (isLeft ? "謝難" : "辦難"));
                    GameManager.Instance.photonView.RPC("SelectFieldForPun", RpcTarget.Others, mousePos, instantiatePosition, isLeft);
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
                    mousePos = this.tmpField.transform.position;
                    return true;
                }
            }
            else
            {
                if (field.Next != null && field.Next.isEmpty)
                {
                    return false;
                }
                else
                {
                    if (IsFieldFull())
                    {
                        return false;
                    }
                    Debug.LogError("辦難" + (isLeft ? "謝難" : "辦難"));
                    GameManager.Instance.photonView.RPC("SelectFieldForPun", RpcTarget.Others, mousePos, instantiatePosition, isLeft);
                    GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
                    battleFields.AddAfter(field, newField);
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
                    mousePos = this.tmpField.transform.position;
                    return true;
                }
            }
            //if (field.Prev != null && field.Prev.isEmpty)
            //{
            //    return false;
            //}
            //else
            //{
            //    if (IsFieldFull())
            //    {
            //        return false;
            //    }
            //    GameManager.Instance.photonView.RPC("SelectFieldForPun", RpcTarget.Others, mousePos, instantiatePosition);
            //    GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
            //    battleFields.AddBefore(field, newField);
            //    this.tmpField = battleFields.Find(newField);

            //    // Draw field in screen
            //    Field tmpField = battleFields.First;
            //    fields.Clear();
            //    while (tmpField != null)
            //    {
            //        fields.Add(tmpField.gameObject);
            //        tmpField = tmpField.Next;
            //    }
            //    for (int pos = (fields.Count - 2) * -9, i = 0; i < fields.Count; pos += 18, i++)
            //    {
            //        try
            //        {
            //            fields[i].transform.position = new Vector3(pos, 0, 0);
            //        }
            //        catch (Exception e)
            //        {
            //            Debug.LogError(e.Message);
            //            break;
            //        }
            //    }
            //    SelectDirection(this.tmpField);
            //    mousePos = this.tmpField.transform.position;
            //    return true;
            //}
        }
    }


    //public bool SelectField(Field field, Card card, int id, bool lookLeft)
    //{
    //    if (field.isEmpty)
    //    {
    //        tmpField = field;
    //        SelectDirection(field);
    //        return true;
    //    }
    //    else
    //    {
    //        if (field.Prev.isEmpty)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            if (IsFieldFull())
    //            {
    //                return false;
    //            }
    //            GameObject newField = Instantiate(FieldPrefab, instantiatePosition, Quaternion.identity);
    //            battleFields.AddBefore(field, newField);
    //            this.tmpField = battleFields.Find(newField);
    //            // Draw field in screen
    //            Field tmpField = battleFields.First;
    //            fields.Clear();
    //            while (tmpField != null)
    //            {
    //                fields.Add(tmpField.gameObject);
    //                tmpField = tmpField.Next;
    //            }
    //            for (int pos = (fields.Count - 1) * -9, i = 0; i < fields.Count; pos += 18, i++)
    //            {
    //                try
    //                {
    //                    fields[i].transform.position = new Vector3(pos, 0, 0);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.LogError(e.Message);
    //                    break;
    //                }
    //            }
    //            SelectDirection(this.tmpField);
    //            return true;
    //        }
    //    }
    //}

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

    public bool IsFieldFull()
    {
        return fields.Count >= FULL_FIELD_COUNT;
    }

    public void ResetAllField()
    {

        Field tmpField = battleFields.First;

        for (int i = 0; i < 5; i++)
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

    /// <summary>
    /// OnClick辨紫曖 л熱
    /// </summary>
    /// <param name="lookingLeft"></param>
    public void SelectDirection(bool lookingLeft)
    {
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
}
