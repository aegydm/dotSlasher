using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskAction : ScriptableObject
{
    public abstract int Run(Task task, int success, int s_count);
}
