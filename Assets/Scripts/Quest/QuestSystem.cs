using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// ����Ʈ �����ͺ��̽��� ���� ����Ʈ�� ó���� ���� �ý���
/// </summary>
public class QuestSystem : MonoBehaviour
{
    #region Save Path
    private const string _RootSavePath = "questSystem";
    private const string _ActiveQuestSavePath = "activeQuests";
    private const string _CompletedQuestSavePath = "completedQuests";
    private const string _ActiveAchievementSavePath = "activeAchievements";
    private const string _CompletedAchievementSavePath = "completedAchievements";
    #endregion

    #region Singleton
    private static QuestSystem instance;
    private static bool isApplicationQuitting; 
    public static QuestSystem Instance
    {
        get
        {
            if(!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                //Find  : ������Ʈ �̸��� ���ؼ� ã�� ���
                //FindObjectOfType : ��ũ��Ʈ �̸����� ã��
                //FindGameObjectWithTag : �±׸� �̿��� ã��
                //FindGameObjectsWithTag : ������Ʈ �迭(����)���� ���� �±� �˻�
                if(instance == null)
                {
                    instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    //����Ʈ �ý����̶� �̸����� Quest System ������Ʈ�� �߰��� �����մϴ�.
                }

                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    #endregion

    #region  Quest List
    private List<Quest> activedQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    private List<Quest> activeAchievements = new List<Quest>();
    private List<Quest> completedAchievements = new List<Quest>();
    #endregion

    #region ReadOnlyList
    public IReadOnlyList<Quest> ActivedQuests => activedQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;
    public IReadOnlyList<Quest> ActiveAchievementQuests => activeAchievements;
    public IReadOnlyList<Quest> CompletedAchievements => completedAchievements;


    #endregion

    #region Quest DATABASE
    private QuestDatabase quest_database;
    private QuestDatabase achievementDatabase;
    #endregion

    #region Delegate & event
    public delegate void QuestActivedHandler(Quest quest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);

    public event QuestActivedHandler QuestActived;
    public event QuestCompletedHandler QuestCompleted;
    public event QuestCanceledHandler QuestCanceled;

    public event QuestActivedHandler AchievementActived;
    public event QuestCompletedHandler AchievementCompleted;
    #endregion


    private void Awake()
    {
        quest_database = Resources.Load<QuestDatabase>("QuestDatabase");
        achievementDatabase = Resources.Load<QuestDatabase>("AchievementDatabase");

        if(!Load())
        {
            foreach (var achievement in achievementDatabase.Quests)
                Active(achievement);
        }
    }


    #region JSON SAVE & LOAD
    //using Unity.Plastic.Newtonsoft.Json.Linq;
    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        //JSON ���·� SavedData�� ��ȯ���ְ�, JSON Array�� �߰��ϴ� �ڵ�
        var saveDatas = new JArray();
        foreach(var quest in quests)
        {
            if(quest.IsSaveable)
                saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
        }
        return saveDatas;
    }
    private void LoadSaveDatas(JToken datasToken, QuestDatabase database ,
        Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach(var data in  datas)
        {
            var saveData = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveData.ID);
            onSuccess.Invoke(saveData,quest);
        }
    }

    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Active(quest);
        newQuest.LoadData(saveData);
    }
    private void LoadCompleteQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Active(quest);
        newQuest.LoadData(saveData);
        if (newQuest is Achievement)
            completedAchievements.Add(newQuest);
        else
            completedQuests.Add(newQuest);


    }

    private void Save()
    {
        var root = new JObject();
        root.Add(_ActiveQuestSavePath, CreateSaveDatas(activedQuests));
        root.Add(_CompletedQuestSavePath, CreateSaveDatas(completedQuests));
        root.Add(_ActiveAchievementSavePath, CreateSaveDatas(ActiveAchievementQuests));
        root.Add(_CompletedAchievementSavePath, CreateSaveDatas(completedAchievements));

        PlayerPrefs.SetString(_RootSavePath, root.ToString());
        PlayerPrefs.Save();
    }
    private bool Load()
    {
        if (PlayerPrefs.HasKey(_RootSavePath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(_RootSavePath));

            LoadSaveDatas(root[_ActiveQuestSavePath], quest_database, LoadActiveQuest);
            LoadSaveDatas(root[_CompletedQuestSavePath], quest_database, LoadCompleteQuest);
            LoadSaveDatas(root[_ActiveAchievementSavePath], quest_database, LoadActiveQuest);
            LoadSaveDatas(root[_CompletedAchievementSavePath], quest_database, LoadCompleteQuest);
            return true;
        }
        return false;
    }
    #endregion


    #region Contains (����Ʈ ���� ����)
    public bool ContainsInActiveQuests(Quest quest) => activedQuests.Any(x=>x.Id == quest.Id);
    public bool ContainsInCompleteQuests(Quest quest) => completedQuests.Any(x=>x.Id == quest.Id);

    public bool ContainsInActiveAchievement(Quest quest) => activeAchievements.Any(x => x.Id == quest.Id);
    public bool ContainsInCompleteAchievement(Quest quest) => completedAchievements.Any(x => x.Id == quest.Id);
    #endregion
    #region Callback

    //���� �ý��ۿ��� ����� Receive
    private void Receive(List<Quest> quests, string category, object target, int S_count)
    {
        //Quest�� for���� ������ �߿� Complete ó���� �Ǽ� ��Ͽ��� ���Ű� �� ���
        //���� ��Ż ������ �߻��� �� �ֱ� ����
        foreach(var quest in quests.ToArray())
            quest.Receive(category, target, S_count);
    }
    //�ܺο��� ����� Receive
    public void Receive(string category, object target, int S_count)
    {
        //����Ʈ�� ���� ����� �ְ� Receive�� �۵�
        Receive(activedQuests, category, target, S_count);
        Receive(activeAchievements, category, target, S_count);
    }
    //���������� �޾Ƽ� ����� Receive
    public void Receive(Category category, TaskTarget target, int S_count)
        => Receive(category.ID, target.Value, S_count);
    
    private void OnQuestCanceled(Quest quest)
    {
        activedQuests.Remove(quest);
        QuestCanceled?.Invoke(quest);
        Destroy(quest, Time.deltaTime);
    }

    private void OnQuestCompleted(Quest quest)
    {
        activedQuests.Remove(quest);
        completedQuests.Add(quest);
        QuestCompleted?.Invoke(quest);
    }

    private void OnAchievementCompleted(Quest achievement)
    {
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);
        AchievementCompleted?.Invoke(achievement);
    }
    #endregion

    #region Method
    public Quest Active(Quest quest)
    {
        var new_Quest = Instantiate(quest);

        if(new_Quest is Achievement)
        {
            new_Quest.completedhandler += OnAchievementCompleted;
            activeAchievements.Add(new_Quest);
            new_Quest.Active();
            AchievementActived?.Invoke(new_Quest);
        }
        else
        {
            new_Quest.completedhandler += OnQuestCompleted;
            new_Quest.cancelhandler += OnQuestCanceled;
            activedQuests.Add(new_Quest);
            new_Quest.Active();
            QuestActived?.Invoke(new_Quest);
        }
        return new_Quest;
    }
    public void OnApplicationQuit()
    {
        isApplicationQuitting = true;
        Save();
        //�������� ������ ���α׷����� ���� �߿��ϰ�,
        //��� �������� ������ ���ܼ� �ش� �۾��� ó������ �ʰ�
        //�ڷᰡ ���� ���ɼ��� ����.
        //�� ������Ʈ������ ���� �� ���� �Ǵ� UI�� ���� ���� ���� ����ϰ� �ְ�
        //���� ������Ʈ���� �����Ͻ� ���� Ư�� ����, Ư�� ��Ȳ ��� Save�� �����ϰ�
        //������ ��
    }
    #endregion
}
