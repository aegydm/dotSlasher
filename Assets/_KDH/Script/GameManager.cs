using CCGCard;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TMP_Text text;

    //2인 플레이 테스트 용 임시 코드
    public string playerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
            //photonView.ViewID = int.Parse(value);
        }
    }

    [SerializeField] private string _playerID = "-1";

    public PhotonView photonView;
    //End

    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
        private set { _gamePhase = value; }
    }

    public int currentTurn
    {
        get
        {
            return _currentTurn;
        }
        set
        {
            _currentTurn = value;
        }
    }

    public bool myTurn
    {
        get
        {
            return _myTurn;
        }
        set
        {
            _myTurn = value;
        }
    }

    private int _currentTurn;

    private bool _myTurn;

    private Deck deck;

    [SerializeField] private GamePhase _gamePhase;

    private void Awake()
    {
        if (Instance == null)
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
        //photonView = GetComponent<PhotonView>();
        //playerID = photonView.ViewID.ToString();
        //Debug.LogError(photonView.ViewID);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    playerID = "1";
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    playerID = "2";
        //}
    }

    public void StartSetting()
    {
        if(deck == null)
        {
            deck = GetComponent<Deck>();
        }

        if (gamePhase == GamePhase.DrawPhase)
        {
            deck.Shuffle();
            deck.Draw(4);
            EndPhase();
        }
        else
        {
            Debug.Log("드로우 페이즈에만 작동합니다.");
        }
    }

    public void EndPhase()
    {
        switch (gamePhase)
        {
            case GamePhase.DrawPhase:
                gamePhase = GamePhase.ActionPhase;
                break;
            case GamePhase.ActionPhase:
                gamePhase = GamePhase.BattlePhase;
                break;
            case GamePhase.BattlePhase:
                gamePhase = GamePhase.ExecutionPhase;
                ExecuteGame();
                break;
            case GamePhase.ExecutionPhase:
                gamePhase = GamePhase.EndPhase;
                EndGame();
                break;
            case GamePhase.EndPhase:
                gamePhase = GamePhase.DrawPhase;
                break;
        }
    }

    private void EndGame()
    {

    }

    private void ExecuteGame()
    {

    }

    [PunRPC]
    public void PlaceCardForPun(Vector3 pos, int cardID, int playerID)
    {
        Debug.LogError("TestPlaceCard");
        Vector2 mousePos = pos;
        Vector3 newPosition = new Vector3(mousePos.x, 0, 0);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if(cardID != 0)
        {
            Card summonCard = FindCardFromID(cardID);
            FieldManager.Instance.PlaceCard(rayhit.collider.GetComponent<Field>(), summonCard, playerID);
        }
    }

    [PunRPC]
    public void AttackPhaseNetwork()
    {
        BattleManager.instance.AttackPhase();
    }

    public Card FindCardFromID(int id)
    {
        foreach(var data in CardDB.instance.cards)
        {
            if(data.cardID == id)
            {
                return data;
            }
        }
        return null;
    }
}
