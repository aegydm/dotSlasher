using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/String", fileName = "S_Target_")]
public class StringTarget : TaskTarget
{
    [SerializeField] private string value;

    public override object Value => value;

    public override bool IsTarget(object target)
    {
        string targetString = target as string;
        if(targetString != null) //비어있지 않다면? 같음.
            return value == targetString;
        return false;
    }
}

