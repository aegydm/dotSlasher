using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using Unity.VisualScripting;

public class Hero : Unit
{
    public int savedDMG;

    public override void GetDamage(int damageVal)
    {
        //������ savedDMG��ŭ ī�带 ������ �ڵ�;
        savedDMG += damageVal;
        Debug.Log(savedDMG);
        savedDMG = 0;
    }
}
