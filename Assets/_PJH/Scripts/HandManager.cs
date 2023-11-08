using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using Photon.Pun;
using Unity.VisualScripting;

public class HandManager : MonoBehaviour
{
    [SerializeField] public HandCard[] cards;
    [HideInInspector] public HandCard selectedHand;
    public static HandManager Instance;
    bool isDragging = false;
    GameObject draggingObject;

    Vector3 startPos;
    bool usingSelectedCard = true;

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

    // Start is called before the first frame update
    void Start()
    {
        cards = GetComponentsInChildren<HandCard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedHand == null)
        {
            usingSelectedCard = false;
        }
        if(GameManager.Instance.gamePhase == GamePhase.ActionPhase&& GameManager.Instance.canAct)
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
                    Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
                    if (draggingObject != null)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.gameObject.layer == 7)
                            {
                                usingSelectedCard =  FieldManager.Instance.SelectField(collider.GetComponent<Field>());
                                GameManager.Instance.photonView.RPC("SelectFieldForPun", RpcTarget.Others, mousePos);
                            }
                        }
                        draggingObject.transform.position = startPos;
                        if(usingSelectedCard == false)
                            ToggleCardSelection(selectedHand);
                    }
                    isDragging = false;
                    draggingObject = null;
                }

                if (isDragging)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (draggingObject != null)
                    {
                        draggingObject.transform.position = mousePos;
                    }
                }
            }
        }
    }

    public void RemoveHand()
    {
        selectedHand.RemoveCard();
        ToggleCardSelection(selectedHand.gameObject);
        selectedHand = null;
        SortHand();
        GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, GetHandCardNum());
    }

    public int GetHandCardNum()
    {
        int count = 0;
        for(int i = 0; i < cards.Length; i++)
        {
            if (cards[i].card != null)
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
        int i = 0;
        while (cards[i].card != null)
        {
            i++;
            if (i == cards.Length - 1) return;
        }
        if (cards[i].card != null) return;
        cards[i].RemoveCard();
        while (i < cards.Length - 1)
        {
            if (cards[i].card == null)
            {
                cards[i].SetCard(cards[i + 1].card);
                cards[i + 1].RemoveCard();
            }
            i++;
        }
        cards[cards.Length - 1].RemoveCard();
        foreach (HandCard card in cards)
        {
            if (card.card == null)
            {
                card.RemoveCard();
            }
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
        Card newCard = drawCard;
        foreach (HandCard card in cards)
        {
            if (card.isEmpty)
            {
                card.SetCard(newCard);
                return true;
            }
        }
        return false;
    }
}
