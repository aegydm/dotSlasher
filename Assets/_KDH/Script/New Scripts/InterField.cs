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
    float distanceX;

    private void Start()
    {
        if (originField != null)
            distanceX = transform.position.x - originField.transform.position.x;
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Enter");
        if (trigger == false && PlayerActionManager.instance.isDrag & PlayerActionManager.instance.dirtyForInter == false)
        {
            if (FieldManagerTest.instance.GetAdditionalField() == null)
            {
                Debug.Log("모든 필드를 사용했습니다.");
                return;
            }
            PlayerActionManager.instance.NewFieldAction += CancelInterWithNewField;
            PlayerActionManager.instance.CancelAction += CancelInter;
            Vector3 pos = transform.position;
            PlayerActionManager.instance.dirtyForInter = true;
            trigger = true;
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
        if (trigger == false && (PlayerActionManager.instance.isDrag || FieldManagerTest.instance.isOpenDirection))
        {
            Debug.Log("DIRTYON");
            PlayerActionManager.instance.dirtyForInter = true;
        }
    }

    private void OnMouseExit()
    {
        if (PlayerActionManager.instance.isDrag)
        {
            Debug.Log("mouseExit");
            CancelInter();
        }
    }

    public void CancelInterWithNewField()
    {
        Debug.Log("CANCELNEW");
        if (trigger == true && (PlayerActionManager.instance.isDrag || FieldManagerTest.instance.isOpenDirection) && additionalGO != null && PlayerActionManager.instance.dirtyForInter == true)
        {
            interLine.SetActive(true);
            PlayerActionManager.instance.dirtyForInter = false;
            trigger = false;
            if (originField == null)
            {

                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                //while (temp != null)
                //{
                //    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                //    temp = temp.Next;
                //}
                this.transform.localPosition = new Vector3(-0.55f, 0, 0);
                //additionalGO.SetActive(false);
                PlayerActionManager.instance.field = null;
                additionalGO = null;
            }
            else
            {

                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                //for (int i = 0; i <= FieldManagerTest.instance.battleField.FindIndex(originField); i++)
                //{
                //    temp.gameObject.transform.position += new Vector3(0.825f, 0, 0);
                //    temp = temp.Next;
                //}
                //while (temp != null)
                //{
                //    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                //    temp = temp.Next;
                //}
                this.transform.localPosition = new Vector3(0.55f, 0, 0);
                //additionalGO.SetActive(false);
                PlayerActionManager.instance.field = null;
                additionalGO = null;
            }
        }
    }

    public void CancelInter()
    {
        //Debug.Log("CANCELNormal1");
        //Debug.Log(trigger);
        //Debug.Log(" " + (PlayerActionManager.instance.isDrag || FieldManagerTest.instance.isOpenDirection));
        //Debug.Log(" " + (additionalGO != null));
        //Debug.Log(" " + (PlayerActionManager.instance.dirtyForInter == true));
        if (trigger == true && (PlayerActionManager.instance.isDrag || FieldManagerTest.instance.isOpenDirection) && additionalGO != null && PlayerActionManager.instance.dirtyForInter == true)
        {
            Debug.Log("CANCELNormal2");
            interLine.SetActive(true);
            Debug.Log(interLine.activeSelf);
            PlayerActionManager.instance.dirtyForInter = false;
            PlayerActionManager.instance.field.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            trigger = false;
            if (originField == null)
            {

                FieldCardObjectTest temp = FieldManagerTest.instance.battleField.First;
                while (temp != null)
                {
                    temp.gameObject.transform.position -= new Vector3(0.825f, 0, 0);
                    temp = temp.Next;
                }
                this.transform.localPosition = new Vector3(-0.55f, 0, 0);
                PlayerActionManager.instance.field = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Prev = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Next = null;
                additionalGO.SetActive(false);
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
                this.transform.localPosition = new Vector3(0.55f, 0, 0);
                PlayerActionManager.instance.field = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Prev = null;
                additionalGO.GetComponent<FieldCardObjectTest>().Next = null;
                additionalGO.SetActive(false);
                additionalGO = null;
            }
        }
    }

    private void TransMatch()
    {
        this.transform.position = originField.transform.position + new Vector3(0.825f, 0, 0);
    }
}
