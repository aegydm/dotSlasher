using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDeckBuildingScene()
    {
        SceneManager.LoadScene("DeckBuildingScene");
    }

    public void LoadLobbyScene()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Additive);
        List<Card> deckData = BuildManager.instance.SelectedSavedDeck.deck;
        string deckNameData = BuildManager.instance.SelectedSavedDeck.deckName;
        // 씬이 로딩되는 동안 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameObject targetObject = GameObject.Find("DeckData");
        if (targetObject != null)
        {
            targetObject.GetComponent<DeckData>().deck = deckData;
            targetObject.GetComponent<DeckData>().deckName = deckNameData;
        }

        SceneManager.UnloadSceneAsync("DeckBuildingScene");
    }
}
