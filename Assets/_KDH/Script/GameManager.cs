using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private Deck deck;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        deck = GetComponent<Deck>();
    }

    public void StartSetting()
    {
        deck.Shuffle();
        deck.Draw(4);
    }
}
