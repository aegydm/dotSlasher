using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public void LoadDeckScene()
    {
        SceneLoadManager.LoadScene("DeckBuild");
    }

    public void LoadGameScene()
    {
        SceneLoadManager.LoadScene("MatchMaking");
    }

}
