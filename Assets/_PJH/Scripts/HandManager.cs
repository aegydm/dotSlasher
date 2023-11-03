using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class HandManager : MonoBehaviour
{
    [SerializeField] HandCard[] cards;
    [HideInInspector] public HandCard selectedHand;
    public static HandManager Instance;

    private Vector2 startPos;

    //private void OnMouseDown()
    //{
    //    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //}

    //private void OnMouseDrag()
    //{
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
    //    Debug.Log(1);
    //    if(rayhit.collider.GetComponent<HandCard>() != null )
    //    {
    //        Debug.Log(2);
    //        rayhit.transform.position = mousePos;
    //        rayhit.collider.GetComponent<HandCard>().isSelected = true;
    //        selectedHand = rayhit.collider.GetComponent<HandCard>();
    //    }
    //}

    //private void OnMouseUp()
    //{
    //    selectedHand.transform.position = startPos;
    //    selectedHand = null;
    //}

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

    public void RemoveHand()
    {
        ToggleCardSelection(selectedHand.gameObject);
        selectedHand.RemoveCard();
        selectedHand = null;
        SortHand();
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
        Debug.Log($"{i} + {cards.Length}");
        cards[i].RemoveCard();
        while (i < cards.Length - 1)
        {
            if (cards[i].card == null)
            {
                cards[i].SetCard(cards[i + 1].card);
                cards[i + 1].RemoveCard();
            }
            i++;

            Debug.Log(i);
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

<<<<<<< Updated upstream
    public void DrawCard(Card drawCard)
    {
        Card newCard = drawCard;
        foreach(HandCard card in cards)
=======
    public bool DrawCard(Card drawCard)
    {
        Card newCard = drawCard;
        foreach (HandCard card in cards)
>>>>>>> Stashed changes
        {
            if (card.isEmpty)
            {
                card.SetCard(newCard);
<<<<<<< Updated upstream
                break;
            }
        }
=======
                return true;
            }
        }
        return false;
>>>>>>> Stashed changes
    }
}
