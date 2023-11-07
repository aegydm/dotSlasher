using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achivement_")]
public class Achievement : Quest
{
    public override bool IsCancelable => false;
    public override bool IsSaveable => true;
    public override void Cancel()
    {
        Debug.LogAssertion("���� ��� �Ұ� �����Դϴ�.");
    }

}
