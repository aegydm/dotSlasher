using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaseSpriteRender : MonoBehaviour
{
    public List<Sprite> offSprite;
    public List<GameObject> onSprite;
    public GameManager manager;

    public GamePhase gamePhase;

    void Start()
    {

    }

    void Update()
    {
        GamePhaseRender();
    }

    public void GamePhaseRender()
    {
        if(gamePhase != manager.gamePhase)
        {
            gamePhase = manager.gamePhase;

            for(int i = 0; i < onSprite.Count; i++)
            {
                onSprite[i].SetActive(false);
            }
            if(gamePhase == GamePhase.DrawPhase)
            {
                onSprite[1].SetActive(true);
            }
            else if(gamePhase == GamePhase.ActionPhase)
            {
                onSprite[2].SetActive(true);
            }
            else if(gamePhase == GamePhase.BattlePhase)
            {
                onSprite[3].SetActive(true);
            }
            else if(gamePhase == GamePhase.ExecutionPhase)
            {
                onSprite[4].SetActive(true);
            }
            else if(gamePhase == GamePhase.EndPhase)
            {
                onSprite[5].SetActive(true);
            }
            else
            {
                Debug.Log("처리 가능한 페이즈가 아닙니다.");
            }

            PhaseChange();
        }
    }

    public void PhaseChange()
    {
        gamePhase = manager.gamePhase;
    }
}
