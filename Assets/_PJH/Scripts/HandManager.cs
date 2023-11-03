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
        if(Instance == null)
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
                        SelectCard(rayhit.collider.gameObject);
                    }
                    else if (rayhit.collider.gameObject == selectedHand.gameObject)
                    {
                        SelectCard(rayhit.collider.gameObject);
                    }
                    else
                    {
                        SelectCard(selectedHand.gameObject, selectedHand);
                        SelectCard(rayhit.collider.gameObject);
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
        selectedHand.RemoveCard();
        selectedHand = null;
    }

    void SelectCard(GameObject card)
    {
        HandCard handCard = card.gameObject.GetComponent<HandCard>();
        Vector3 scale = card.transform.localScale;
        card.transform.localScale = handCard.isSelected ? new Vector3(scale.x, scale.y / 1.1f, scale.z) : new Vector3(scale.x, scale.y * 1.1f, scale.z);
        handCard.isSelected = !handCard.isSelected;
    }
    void SelectCard(GameObject card, HandCard handCard)
    {
        Vector3 scale = card.transform.localScale;
        card.transform.localScale = handCard.isSelected ? new Vector3(scale.x, scale.y / 1.1f, scale.z) : new Vector3(scale.x, scale.y * 1.1f, scale.z);
        handCard.isSelected = !handCard.isSelected;
    }

    public bool DrawCard(Card newCard)
    {
        foreach(HandCard card in cards)
        {
            if (card.isEmpty)
            {
                card.SetCard(newCard);
                Debug.Log(newCard.cardName);
                return true;
            }
        }
        return false;

    }
}
