using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScroll : MonoBehaviour
{
    public Camera cam;

    public float distance;

    public int distanceRange = 50;
    [Header("이동속도")]
    public float speedlimite;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(distance >= -distanceRange)
            {
                distance -= speedlimite;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if(distance <= distanceRange)
            {
                distance += speedlimite;
            }             
        }

        CameraPosition();
    }

    private void CameraPosition()
    {
        Vector3 camPosition = new Vector3(distance, 0, 0);
        cam.transform.position = camPosition;
    }
}
