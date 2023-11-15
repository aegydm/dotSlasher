using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class SavedDeck : MonoBehaviour
{
    public List<Card> deck;
    public string deckName;

    private void OnMouseDown()
    {
        BuildManager.instance.SelectedSavedDeck = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
