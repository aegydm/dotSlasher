using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Condition : ScriptableObject
{
    [SerializeField]
    private string description;

    public abstract bool IsPass(Quest quest);
}

