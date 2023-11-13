using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckImageRender : MonoBehaviour
{
    public Deck deck;
    public List<Image> deckImage;
    public List<Image> graveImage;
    public Sprite emptySprite;
    public int deckCount;
    public int graveCount;
    
    public int preDeckCount;

    private void Start()
    {
        DeckCountRefresh();
        GraveCountRefresh();
        preDeckCount = deckCount;
    }

    // Update is called once per frame
    void Update()
    {
        DeckCountRefresh();
        GraveCountRefresh();
        if(preDeckCount != deckCount)
        {
            preDeckCount = deckCount;
            DeckImageRend();
        }
    }

    public void DeckImageRend()
    {
        int deckImageCount = (deckCount + 2) / 3;
        
        for (int i = 0; i < deckImage.Count; i++)
        {
            if (i < deckImageCount)
            {
                deckImage[i].gameObject.SetActive(true);
            }
            else
            {
                deckImage[i].gameObject.SetActive(false);
            }
        }
        DeckCountRefresh();
    }
    public void GraveImageRend()
    {
        int graveImageCount = (deckCount + 2) / 3;

        for (int i = 0; i < graveImage.Count; i++)
        {
            if (i < graveImageCount)
            {
                graveImage[i].gameObject.SetActive(true);
            }
            else
            {
                graveImage[i].gameObject.SetActive(false);
            }
        }
        GraveCountRefresh();
    }

    private void GraveCountRefresh()
    {
        graveCount = deck.countOfGrave;
    }

    public void DeckCountRefresh()
    {
        deckCount = deck.countOfDeck;
    }
}