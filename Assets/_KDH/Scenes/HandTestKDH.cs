using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandTestKDH : MonoBehaviour
{
    [SerializeField] GameObject spriteGO;
    private Vector3 originPos;
    private bool isDrag;

    private void Update()
    {
        if (HandTestManager.instance.isDrag && isDrag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            transform.position = mousePos;
        }
    }

    private void Start()
    {
        originPos = transform.position;
    }

    private void OnMouseOver()
    {
        if (HandTestManager.instance.isDrag == false)
        {
            spriteGO.transform.localScale = new(1.3f, 1.3f, 1.3f);
        }
    }

    private void OnMouseExit()
    {
        if(HandTestManager.instance.isDrag == false)
        {
            spriteGO.transform.localScale = new(1, 1, 1);
        }
    }

    private void OnMouseDown()
    {
        isDrag = true;
        HandTestManager.instance.isDrag = true;
    }

    private void OnMouseUp()
    {
        isDrag = false;
        HandTestManager.instance.isDrag = false;
        transform.position = originPos;
    }
}
