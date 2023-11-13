using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckImageRender : MonoBehaviour
{
    public Deck deck;
    public List<Image> image;
    public Sprite emptySprite;
    public int deckCount;
    
    public int preDeckCount;

    private void Start()
    {
        CountRefresh();
        preDeckCount = deckCount;
    }

    // Update is called once per frame
    void Update()
    {
        CountRefresh();
        if(preDeckCount != deckCount)
        {
            preDeckCount = deckCount;
            ImageRend();
        }
    }

    public void ImageRend()
    {
        int imageCount = (deckCount + 2) / 3;
        
        for (int i = 0; i < image.Count; i++)
        {
            if (i < imageCount)
            {
                image[i].gameObject.SetActive(true);
            }
            else
            {
                image[i].gameObject.SetActive(false);
            }
        }
        CountRefresh();
    }

    public void CountRefresh()
    {
        deckCount = deck.countOfDeck;
    }
}