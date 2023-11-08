using CCGCard;
using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TMP_Text turnText;
    [SerializeField] GameObject endScene;
    public bool isGameEnd = false;

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
            if(gamePhase == GamePhase.ActionPhase)
            {
                turnText.text = "Turn : " + _currentTurn.ToString();
            }
        }
    }

    public bool playerEnd
    {
        get
        {
            return _playerEnd;
        }
        set
        {
            _playerEnd = value;
            if (_playerEnd)
            {
                photonView.RPC("CallActionEnd", RpcTarget.Others);
            }
            CheckPhaseEnd();
        }
    }

    public bool enemyEnd
    {
        get
        {
            return _enemyEnd;
        }
        set
        {
            _enemyEnd = value;
            CheckPhaseEnd();
        }
    }

    public bool canAct
    {
        get
        {
            return _canAct;
        }
        set
        {
            _canAct = value;
            if (_canAct == false && gamePhase == GamePhase.ActionPhase)
            {
                currentTurn++;
                photonView.RPC("MatchTurnNum", RpcTarget.Others, currentTurn);
                photonView.RPC("CallSummonEnd", RpcTarget.Others);
            }
        }
    }

    public bool startFirst = false;

    [SerializeField] private bool _canAct;

    private void CheckPhaseEnd()
    {
        if (playerEnd && enemyEnd)
        {
            EndPhase();
        }
    }

    [SerializeField] private int _currentTurn;

    [SerializeField] private bool _playerEnd;

    [SerializeField] private bool _enemyEnd;

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
        startFirst = NetworkManager.instance.first;
        NetworkManager.instance.SetPlayerID();
        ResetState();
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    private void ResetState()
    {
        playerEnd = false;
        enemyEnd = false;
    }

    public void StartSetting()
    {
        if (deck == null)
        {
            deck = GetComponent<Deck>();
            deck.Shuffle();
        }

        if (gamePhase == GamePhase.DrawPhase)
        {
            //deck.Shuffle();
            deck.Draw(4);
            photonView.RPC("EnemyCardChange", RpcTarget.Others, 4);
            playerEnd = true;
        }
        else
        {
            Debug.Log("드로우 페이즈에만 작동합니다.");
        }
    }

    [PunRPC]
    public void EnemyCardChange(int num)
    {
        FieldManager.Instance.enemyCardNum = num;
    }

    public void EndPhase()
    {
        ResetState();
        switch (gamePhase)
        {
            case GamePhase.DrawPhase:
                if (startFirst)
                {
                    canAct = true;
                }
                else
                {
                    canAct = false;
                }
                //가위바위보 결과로 대체 할 코드
                //if (int.Parse(playerID) % 2 == 0)
                //{
                //    canAct = true;
                //}
                //else
                //{
                //    canAct = false;
                //}
                //
                currentTurn = 0;
                gamePhase = GamePhase.ActionPhase;
                break;
            case GamePhase.ActionPhase:
                gamePhase = GamePhase.BattlePhase;
                BattleManager.instance.AttackButton();
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
        turnText.text = gamePhase.ToString();
    }

    private void EndGame()
    {
        Field tmp = FieldManager.Instance.battleFields.First;
        while (tmp.Next != null)
        {
            if (tmp.unitObject.playerName == playerID)
            {
                deck.Refill(tmp.unitObject.cardData);
                tmp.ResetField();
            }
            else
            {
                tmp.ResetField();
            }
            tmp = tmp.Next;
        }
        if (tmp.unitObject.playerName == playerID)
        {
            deck.Refill(tmp.unitObject.cardData);
            tmp.ResetField();
        }
        else
        {
            tmp.ResetField();
        }
        BattleManager.instance.unitList.Clear();
        EndPhase();
    }

    private void ExecuteGame()
    {
        Debug.LogError("처리 페이즈에 진입했습니다.");
        Debug.LogError("영웅 처리가 마무리 되지 않았으므로 3초 뒤 자동으로 페이즈를 넘깁니다.");
        Invoke("EndPhase", 3f);
    }

    [PunRPC]
    public void PlaceCardForPun(Vector2 pos, int cardID, int playerID, bool lookLeft)
    {
        Debug.LogError("TestPlaceCard");
        Vector2 mousePos = pos;
        FieldManager.Instance.instantiatePosition = new Vector3(mousePos.x, 0, 0);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (cardID != 0)
        {
            Card summonCard = FindCardFromID(cardID);
            FieldManager.Instance.PlaceCard(rayhit.collider.GetComponent<Field>(), summonCard, playerID, lookLeft);
        }
    }

    [PunRPC]
    public void SelectFieldForPun(Vector2 pos)
    {
        Vector2 mousePos = pos;
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
        bool usingSelectedCard;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == 7)
            {
                usingSelectedCard = FieldManager.Instance.SelectField(collider.GetComponent<Field>());
            }
        }
    }

    [PunRPC]
    public void AttackPhaseNetwork()
    {
        BattleManager.instance.AttackPhase();
    }

    [PunRPC]
    public void MatchTurnNum(int turnNum)
    {
        currentTurn = turnNum;
    }

    public Card FindCardFromID(int id)
    {
        foreach (var data in CardDB.instance.cards)
        {
            if (data.cardID == id)
            {
                return data;
            }
        }
        return null;
    }

    [PunRPC]
    public void CallActionEnd()
    {
        enemyEnd = true;
    }

    [PunRPC]
    public void CallSummonEnd()
    {
        if (playerEnd)
        {
            canAct = false;
            photonView.RPC("CallSummonEnd", RpcTarget.Others);
        }
        else
        {
            canAct = true;
        }
    }

    [PunRPC]
    public void GameSet()
    {
        if (isGameEnd == false)
        {
            WinnerProcess();
        }
    }

    private void LoserProcess()
    {
        if (isGameEnd == false)
        {

            StopAllCoroutines();
            endScene.SetActive(true);
            endScene.GetComponentInChildren<TMP_Text>().text = "Lose";
        }
    }

    private void WinnerProcess()
    {
        if (isGameEnd == false)
        {

            StopAllCoroutines();
            endScene.SetActive(true);
            endScene.GetComponentInChildren<TMP_Text>().text = "WIN";
        }
    }

    public void Lose()
    {
        if (isGameEnd == false)
        {
            photonView.RPC("GameSet", RpcTarget.Others);
            LoserProcess();

        }
    }

    public void ExitGame()
    {
        if (isGameEnd)
        {
            PhotonNetwork.LeaveRoom();

        }
    }
}
