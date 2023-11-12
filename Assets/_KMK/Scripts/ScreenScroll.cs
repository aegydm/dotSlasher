using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScroll : MonoBehaviour
{
    public Camera cam;

    public float distance;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftArrow"))
        {
            distance++;

        }
        else if (Input.GetButtonDown("RightArrow"))
        {
            distance--;
             
        }
    }
}
