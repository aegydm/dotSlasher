using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class HandCard : MonoBehaviour
{
    [SerializeField] Card[] cards;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (rayhit.collider != null)
            {
                if (rayhit.collider.gameObject.tag is "Field" && rayhit.collider.gameObject.GetComponent<Field>().isEmpty)
                {
                    rayhit.collider.gameObject.GetComponent<Field>().card = cards[0];
                    rayhit.collider.gameObject.GetComponent<Field>().isEmpty = false;
                    if (GameManager.Instance.red)
                    {
                        rayhit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        rayhit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    GameManager.Instance.AddCard(cards[0], rayhit.collider.gameObject.GetComponent<Field>());
                    rayhit.collider.gameObject.GetComponent<Field>().spriteRenderer.sprite = cards[0].cardSprite;

                    for (int i = 1; i < cards.Length; i++)
                    {
                        cards[i - 1] = cards[i];
                    }
                    cards[cards.Length - 1] = null;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (rayhit.collider != null)
            {
                if (rayhit.collider.gameObject.tag is "Field" && rayhit.collider.gameObject.GetComponent<Field>().isEmpty)
                {
                    rayhit.collider.gameObject.GetComponent<Field>().card = cards[0];
                    rayhit.collider.gameObject.GetComponent<Field>().isEmpty = false;
                    rayhit.collider.gameObject.GetComponent<Field>().card.lookingLeft = !rayhit.collider.gameObject.GetComponent<Field>().card.lookingLeft;
                    if (GameManager.Instance.red)
                    {
                        rayhit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        rayhit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    GameManager.Instance.AddCard(cards[0], rayhit.collider.gameObject.GetComponent<Field>());
                    rayhit.collider.gameObject.GetComponent<Field>().spriteRenderer.sprite = cards[0].cardSprite;
                    rayhit.collider.gameObject.GetComponent<Field>().spriteRenderer.flipX = true;
                    for (int i = 1; i < cards.Length; i++)
                    {
                        cards[i - 1] = cards[i];
                    }
                    cards[cards.Length - 1] = null;
                }
            }
        }
    }
}
