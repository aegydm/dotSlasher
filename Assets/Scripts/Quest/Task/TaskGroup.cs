using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroup_State
{
    InActive, Running, Complete
}

[Serializable]
public class TaskGroup
{
    [SerializeField] private Task[] task_group;

    public TaskGroup(TaskGroup copy)
    {
        task_group = copy.Tasks.Select(x => GameObject.Instantiate(x)).ToArray();
    }
    #region 프로퍼티
    //읽기 전용 리스트
    public IReadOnlyList<Task> Tasks => task_group;
    //읽기 전용 퀘스트(private set)
    public Quest Owner { get; private set; }
    public bool AutoComplete => task_group.All(x => x.IsComplete);
    public TaskGroup_State State { get; private set; }
    public bool IsComplete => State == TaskGroup_State.Complete;
    #endregion

    public void Setup(Quest owner)
    {
        Owner = owner;
        foreach (var task in task_group)
        {
            task.Setup(owner);
        }
    }  
    public void Start()
    {
        State = TaskGroup_State.Running;
        foreach (var task in task_group)
        {
            task.Start();
        }
    }
    public void End()
    {
        State = TaskGroup_State.Complete;
        foreach (var task in task_group)
        {
            task.End();
        }
    }

    public void Complete() 
    {
        //그룹에서 Complete 처리가 되어있다면 작업되지 않도록
        if (IsComplete)
            return;

        State = TaskGroup_State.Complete;
        foreach (var task in task_group)
        {
            if(!task.IsComplete)
                task.Complete();
        }

    }


    public void Receive(string category, object target, int S_count)
    {
        foreach (var task in task_group)
        {
            if (task.IsTarget(category, target))
                task.Receive(S_count);
        }
    }
}
