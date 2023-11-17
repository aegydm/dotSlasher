using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using Unity.VisualScripting;

public class SavedDeck : MonoBehaviour
{
    public List<Card> deck;
    public string deckName;

    public SpriteRenderer selectionSquare;

    private void OnMouseDown()
    {
        if (DeckMaker.instance.isDeckMaking) return;
        BuildManager.instance.SelectedSavedDeck = this;
    }

    private void Awake()
    {
        gameObject.name = deckName;
    }

    private void Update()
    {
        if(BuildManager.instance.SelectedSavedDeck == this)
        {
            selectionSquare.enabled = true;
        }
        else
        {
            selectionSquare.enabled = false;
        }
    }
}
