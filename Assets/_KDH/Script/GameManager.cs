using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    DrawPhase,
    ActionPhase,
    BattlePhase,
    ExecutionPhase,
    EndPhase,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
    }
    
    private Deck deck;

    private GamePhase _gamePhase;

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
