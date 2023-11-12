using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance;

    [SerializeField] Deck deck;

    private void Awake()
    {
        Instance = this;
    }
}
