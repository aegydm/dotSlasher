using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using Photon.Pun;

public class HandManager : MonoBehaviour
{
    [SerializeField] public HandCard[] cards;
    [HideInInspector] public HandCard selectedHand;
    public static HandManager Instance;
    private Vector2 startPos;

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
        if (GameManager.Instance.gamePhase == GamePhase.ActionPhase && GameManager.Instance.canAct)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (rayhit.collider != null)
                {
                    if (rayhit.collider.GetComponent<HandCard>() != null)
                    {
                        if (selectedHand == null)
                        {
                            ToggleCardSelection(rayhit.collider.gameObject);
                        }
                        else if (rayhit.collider.gameObject == selectedHand.gameObject)
                        {
                            ToggleCardSelection(rayhit.collider.gameObject);
                        }
                        else
                        {
                            ToggleCardSelection(selectedHand.gameObject, selectedHand);
                            ToggleCardSelection(rayhit.collider.gameObject);
                        }
                        selectedHand = rayhit.collider.GetComponent<HandCard>();
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (rayhit.collider != null)
                {

                }
            }
        }
    }

    public void RemoveHand()
    {
        ToggleCardSelection(selectedHand.gameObject);
        selectedHand.RemoveCard();
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
        Vector3 scale = card.transform.localScale;
        card.transform.localScale = handCard.isSelected ? new Vector3(scale.x, scale.y / 1.1f, scale.z) : new Vector3(scale.x, scale.y * 1.1f, scale.z);
        handCard.isSelected = !handCard.isSelected;
    }
    void ToggleCardSelection(GameObject card, HandCard handCard)
    {
        Vector3 scale = card.transform.localScale;
        card.transform.localScale = handCard.isSelected ? new Vector3(scale.x, scale.y / 1.1f, scale.z) : new Vector3(scale.x, scale.y * 1.1f, scale.z);
        handCard.isSelected = !handCard.isSelected;
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
