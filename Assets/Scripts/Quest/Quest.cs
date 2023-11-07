using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//���� �ȵ� ����, ���� �� , �Ϸ�, ���� , �Ϸ� ó��
public enum Q_State
{
    Inactive, Running, Complete, Cancel,OnComplete
}
[CreateAssetMenu(menuName = "Quest/Quest", fileName ="Quest_")]
public class Quest : ScriptableObject
{
    public delegate void OnCompleted(Quest quest, Task task, int current_success, int pre_success);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void TaskGroupHandler(Quest quest,TaskGroup current_TaskGroup, 
        TaskGroup pre_TaskGroup);

    public event OnCompleted oncompleted;
    public event CompletedHandler completedhandler;
    public event CancelHandler cancelhandler;
    public event TaskGroupHandler taskgrouphandler;

    [Header("Category")]
    [SerializeField] private Category category;
    [SerializeField] private Sprite icon;
    [Header("ID")]
    [SerializeField] private string id;
    [SerializeField] private string ingame_id;
    [SerializeField,TextArea] private string description;
    [Header("Settings")]
    [SerializeField] private bool auto_complete;
    [SerializeField] private bool isSaveable;
    [Header("Task")]
    [SerializeField] TaskGroup[] taskGroups;
    [Header("Reward")]
    [SerializeField] Reward[] rewards;
    [Header("Condition")]
    [SerializeField] Condition[] acceptionConditions;
    [SerializeField] Condition[] cancelConditions;

    private int idx;
    private bool isCancelable;

    public Category Category  => category;
    public Sprite Icon => icon; 
    public string Id => id; 
    public string Ingame_id  => ingame_id; 
    public string Description  => description; 

    public bool AutoComplete => auto_complete;
  


    public Q_State State { get; private set; }
    public TaskGroup Current_Group => taskGroups[idx];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;

    //���¿� ���� ������Ƽ
    public bool InActive => State != Q_State.Inactive;
    public bool InComplete => State == Q_State.Complete;
    public bool InOnComplete => State == Q_State.OnComplete;
    public bool InCancel => State == Q_State.Cancel;

    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    public virtual bool IsSaveable => isSaveable;

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();
        return clone;
    }
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            ID = id,
            state = State,
            idx = idx,
            taskSuccessCounts = Current_Group.Tasks.Select(x => x.Current_Success).ToArray()
            //Select�� �� �ϳ��� �䱸�ϴ� ���� ���
        };
    }

    public void LoadData(QuestSaveData savedata)
    {
        State = savedata.state;
        idx = savedata.idx;

        //�ε��� ������ŭ �ݺ� �����ؼ� �׷��� ���� Start
        for (int i = 0; i < idx; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        //�۾� Ƚ����ŭ ���� �׷쿡 ���� ó�� ����
        for(int i = 0; i < savedata.taskSuccessCounts.Length; i++)
        {
            Current_Group.Start();
            Current_Group.Tasks[i].Current_Success = savedata.taskSuccessCounts[i];
        }
    }



    public void Active()
    {
        Debug.Assert(!InActive, "�� ����Ʈ�� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");

        //�׷� �������� �׷쿡 ���� ����
        foreach (var taskGroup in taskGroups)
        {
            //Setup ����
            taskGroup.Setup(this);
            //�׷� ������ �۾� ���ٿ� �۾��� ����
            foreach (var task in taskGroup.Tasks)
                task.onCompleted += OnTaskCompleted;
        }
        State = Q_State.Running;
        Current_Group.Start();
    }

    //onCompleted �̺�Ʈ�� �������� �Լ� �ۼ�
    private void OnTaskCompleted(Task task, int current_success, int pre_success)
    => oncompleted?.Invoke(this, task, current_success, pre_success);

    public void Receive(string category, object target, int S_count)
    {
        //�Ϸ� ������ ��쿡�� �۾����� �ʽ��ϴ�.
        if (InComplete)
            return;
        Current_Group.Receive(category,target,S_count);
        //��� �۾��� ������ �� ���¿���
        if (Current_Group.IsComplete)
        {
            //idx + 1 �� �׷��� ���̿� ���ٸ�
            if (idx + 1 == taskGroups.Length)
            {
                State = Q_State.OnComplete;
                if (auto_complete)
                    Complete();
            }
            else
            {
                //���� ��ġ�� �׷쿡 ���� ����
                //�� �� �ε��� 1 ����
                var pre_Group = taskGroups[idx++];
                pre_Group.End();
                Current_Group.Start();
                taskgrouphandler?.Invoke(this, Current_Group, pre_Group);
            }
        }
        else
            State = Q_State.Running;
    }

    public void Complete()
    {
        foreach(var taskGroup in taskGroups)
            taskGroup.Complete();

        State = Q_State.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        completedhandler?.Invoke(this);

        oncompleted = null;
        cancelhandler = null; 
        completedhandler = null;
        taskgrouphandler = null;
    }

    /// <summary>
    /// �Ϲ� ��ӿ��� �ٸ� Ŭ������ ���� ��, �߻�(abstract)�� �ƴ� ������� ���� ���ؼ�
    /// �������̵��� ������ ��� virtual�� ���� �ڽ� ���� ���� ������ �� �ְ� ó�����ݴϴ�.
    /// </summary>
    public virtual void Cancel()
    {
        State = Q_State.Cancel;
        cancelhandler?.Invoke(this);
    }



}
