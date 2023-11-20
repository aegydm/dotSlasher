using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public TMP_Text deckNameText;

    private void Start()
    {
        deckNameText.text = "Deck : " + NetworkManager.instance.deckName;
    }

    public void LoadDeckScene()
    {
        SceneLoadManager.LoadScene("DeckBuild");
    }

    public void LoadGameScene()
    {
        SceneLoadManager.LoadScene("MatchMaking");
    }


}
