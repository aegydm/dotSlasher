using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class DeckObject : MonoBehaviour
{
    public enum DeckCategory
    {
        Deck,
        Grave,
        EnemyGrave,
    }

    public DeckCategory category;
    private void OnMouseDown()
    {
        if(UIManager.Instance.isPopUI == false)
        {
            switch (category)
            {
                case DeckCategory.Deck:
                    UIManager.Instance.PopupCard(GameManager.instance.deck.useDeck);
                    break;
                case DeckCategory.Grave:
                    UIManager.Instance.PopupCard(GameManager.instance.deck.grave);
                    break;
                case DeckCategory.EnemyGrave:
                    UIManager.Instance.PopupCard(GameManager.instance.deck.enemyGrave);
                    break;
            }
        }
    }
}
