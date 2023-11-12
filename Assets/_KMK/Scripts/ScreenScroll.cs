using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScroll : MonoBehaviour
{
    public Camera cam;

    public float distance;

    public int distanceRange = 50;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftArrow"))
        {
            if(distance >= distanceRange)
            {
                distance--;
            }
        }
        else if (Input.GetButtonDown("RightArrow"))
        {
            if(distance <= distanceRange)
            {
                distance++;
            }             
        }
    }
}
