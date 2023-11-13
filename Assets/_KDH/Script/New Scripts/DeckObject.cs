using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(UIManager.Instance.isPopUI == false)
        {
            UIManager.Instance.PopupCard(TestManager.instance.gameObject.GetComponent<Deck>().useDeck);
        }
    }
}
