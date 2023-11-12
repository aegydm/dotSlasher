using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTestManager : MonoBehaviour
{
    public static HandTestManager instance;
    public bool isDrag;
    private void Awake()
    {
        instance = this;
    }
}
