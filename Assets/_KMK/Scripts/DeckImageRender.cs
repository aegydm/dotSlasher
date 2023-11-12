using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckImageRender : MonoBehaviour
{
    public Deck deck;
    public int deckCount;
    public Image[] deckImage;

    private void Start()
    {
        CountRefresh();
    }

    // Update is called once per frame
    void Update()
    {
        ImageRend();
    }

    private void ImageRend()
    {
        if (deckCount <= 3)
        {
            if(deckCount <= 6)
            {
                if(deckCount <= 9)
                {
                    if(deckCount <= 12)
                    {
                        if(deckCount <= 15)
                        {
                            if(deckCount <= 18)
                            {
                                if(deckCount <= 21)
                                {
                                    if(deckCount <= 24)
                                    {
                                        if(deckCount <= 27)
                                        {
                                            if(deckCount <= 30)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        CountRefresh();
    }

    public void CountRefresh()
    {
        deckCount = deck.countOfDeck;
    }
}
/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckImageRender : MonoBehaviour
{
    public Deck deck;
    public int deckCount;
    public Image deckImage;
    public Sprite[] sprites; // 카드 수량에 따른 이미지를 저장

    private void Start()
    {
        CountRefresh();
        ImageRend();
    }

    // Update is called once per frame
    void Update()
    {
        CountRefresh();
        ImageRend();
    }

    private void ImageRend()
    {
        int index = deckCount / 3; // 카드 수량에 따른 이미지 인덱스를 계산

        // 인덱스가 이미지 배열의 범위를 벗어나지 않도록 체크
        if (index >= sprites.Length)
        {
            index = sprites.Length - 1;
        }

        // 이미지를 변경
        deckImage.sprite = sprites[index];
    }

    public void CountRefresh()
    {
        deckCount = deck.countOfDeck;
    }
}*/
