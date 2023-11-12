using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterField : MonoBehaviour
{
    [SerializeField] FieldCardObjectTest originField;
    [SerializeField] GameObject interLine;
    GameObject additionalGO;
    bool trigger = false;

    private void OnMouseEnter()
    {
        //Debug.Log("Enter");
        if (trigger == false && PlayerActionManager.instance.isDrag & PlayerActionManager.instance.dirtyForInter == false)
        {
            Vector3 pos = transform.position;
            PlayerActionManager.instance.dirtyForInter = true;
            trigger = true;
            PlayerActionManager.instance.CancelAction += OnMouseExit;
            if (originField == null)
            {
                additionalGO = FieldManagerTest.instance.GetAdditionalField().gameObject;
                if (additionalGO == null)
                {
                    return;
                }
                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                while (temp != null)
                {
                    temp.gameObject.transform.position += new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                this.transform.position = pos;
                PlayerActionManager.instance.field = additionalGO.GetComponent<FieldCardObjectTest>();
                additionalGO.transform.position = pos;
                additionalGO.GetComponent<FieldCardObjectTest>().Next = FieldManagerTest.instance.battleField.First;
                additionalGO.SetActive(true);
                //GetComponent<BoxCollider2D>().enabled = false;
                interLine.SetActive(false);
            }
            else
            {
                additionalGO = FieldManagerTest.instance.GetAdditionalField().gameObject;
                if (additionalGO == null)
                {
                    return;
                }
                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                for (int i = 0; i <= FieldManagerTest.instance.battleField.FindIndex(originField); i++)
                {
                    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                while (temp != null)
                {
                    temp.gameObject.transform.position += new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                this.transform.position = pos;
                PlayerActionManager.instance.field = additionalGO.GetComponent<FieldCardObjectTest>();
                additionalGO.transform.position = pos;
                additionalGO.GetComponent<FieldCardObjectTest>().Prev = originField;
                additionalGO.SetActive(true);
                //GetComponent<BoxCollider2D>().enabled = false;
                interLine.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        if (trigger == true && PlayerActionManager.instance.isDrag && additionalGO != null && PlayerActionManager.instance.dirtyForInter == true)
        {
            PlayerActionManager.instance.dirtyForInter = false;
            trigger = false;
            PlayerActionManager.instance.CancelAction -= OnMouseExit;
            if (originField == null)
            {
                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                while (temp != null)
                {
                    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                this.transform.position -= new Vector3(0.825f, 0, 0);
                PlayerActionManager.instance.field = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Prev = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Next = null;
                additionalGO.SetActive(false);
                interLine.SetActive(true);
                additionalGO = null;
            }
            else
            {
                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                for (int i = 0; i <= FieldManagerTest.instance.battleField.FindIndex(originField); i++)
                {
                    temp.gameObject.transform.position += new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                while (temp != null)
                {
                    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                Debug.Log(transform.position);
                this.transform.position -= new Vector3(0.825f, 0, 0);
                Debug.Log(transform.position);
                PlayerActionManager.instance.field = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Prev = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Next = null;
                additionalGO.SetActive(false);
                interLine.SetActive(true);
                additionalGO = null;
            }
        }
    }

    private void TransMatch()
    {
        this.transform.position = originField.transform.position + new Vector3(0.825f, 0, 0);
    }
}
