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
        //덱에서 savedDMG만큼 카드를 버리는 코드;
        savedDMG += damageVal;
        Debug.Log(savedDMG);
        savedDMG = 0;
    }
}
