using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using Photon.Pun;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UIElements;

public class HandManager : MonoBehaviour
{
    [SerializeField] public List<HandCard> hands = new List<HandCard>();
    public HandCard selectedHand;
    public GameObject HandPrefab;
    public bool isDraggingOnField = false;
    public static HandManager Instance;
    bool isDragging = false;
    GameObject draggingObject;

    Vector3 startPos;
    bool usingSelectedCard = true;
    [SerializeField] HandCard tmpHand;

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



    // Update is called once per frame
    void Update()
    {
        if (selectedHand == null)
        {
            usingSelectedCard = false;
        }
        if (GameManager.Instance.gamePhase == GamePhase.ActionPhase && GameManager.Instance.canAct && UIManager.Instance.isPopUI == false)
        {
            if (usingSelectedCard == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                    if (rayhit.collider != null)
                    {
                        if (rayhit.collider.gameObject.layer == 6)
                        {
                            startPos = rayhit.transform.position;
                            draggingObject = rayhit.collider.gameObject;
                            ToggleCardSelection(rayhit.collider.gameObject);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    FieldManager.Instance.mousePos = mousePos;
                    Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
                    colliders.Reverse();
                    if (draggingObject != null)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.gameObject.layer == 7)
                            {
                                usingSelectedCard = FieldManager.Instance.SelectField(collider.GetComponent<Field>(), mousePos.x <= collider.transform.position.x);
                                FieldManager.Instance.mousePos = mousePos;
                                //GameManager.Instance.photonView.RPC("SelectFieldForPun", RpcTarget.Others, mousePos, mousePos.x <= collider.transform.position.x);
                                break;
                            }
                        }
                        draggingObject.transform.position = startPos;
                        if (usingSelectedCard == false)
                            ToggleCardSelection(selectedHand);
                    }
                    isDragging = false;
                    isDraggingOnField = false;
                    draggingObject = null;
                }

                if (isDragging)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (draggingObject != null)
                    {
                        draggingObject.transform.position = mousePos;
                        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
                        foreach(Collider2D collider in colliders)
                        {
                            if(collider.gameObject.layer == 7)
                            {
                                isDraggingOnField = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public void RemoveHand()
    {
        selectedHand.RemoveCard();
        ToggleCardSelection(selectedHand.gameObject);
        hands.Remove(selectedHand);
        selectedHand = null;
        SortHand();
        GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, GetHandCardNum());
    }

    public void RemoveHand(HandCard hand)
    {
        hand.RemoveCard();
        Destroy(hand.gameObject);
        SortHand();
        GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, GetHandCardNum());
    }

    public int GetHandCardNum()
    {
        int count = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].card != null)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }

    public void SortHand()
    {
        hands.RemoveAll(hand =>
        {
            if (hand.card == null)
            {
                Destroy(hand.gameObject);
                return true;
            }
            return false;
        });
        int i = 0;
        foreach (HandCard hand in hands)
        {
            hand.gameObject.transform.position = new Vector3(-75+i, -30);
            i += 20;
        }
    }

    void ToggleCardSelection(GameObject card)
    {
        HandCard handCard = card.gameObject.GetComponent<HandCard>();
        handCard.isSelected = !handCard.isSelected;
        selectedHand = handCard.isSelected ? handCard : null;
    }
    void ToggleCardSelection(HandCard handCard)
    {
        handCard.isSelected = !handCard.isSelected;
        selectedHand = handCard.isSelected ? handCard : null;
    }


    public bool DrawCard(Card drawCard)
    {
        Debug.LogError("Call DrawCard");
        if (drawCard != null)
        {
            tmpHand = Instantiate(HandPrefab, transform).GetComponent<HandCard>();
            tmpHand.name = "HandCard" + hands.Count;
            tmpHand.SetCard(drawCard);
            //이 시점까진 데이터 정상
            hands.Add(tmpHand);
            Debug.Log(tmpHand.card.cardName);
            SortHand();
            return true;
        }
        return false;
    }
}
