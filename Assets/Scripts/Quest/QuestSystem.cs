using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 퀘스트 데이터베이스를 통해 퀘스트를 처리할 메인 시스템
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
                //Find  : 오브젝트 이름을 통해서 찾는 방법
                //FindObjectOfType : 스크립트 이름으로 찾기
                //FindGameObjectWithTag : 태그를 이용해 찾기
                //FindGameObjectsWithTag : 오브젝트 배열(묶음)으로 부터 태그 검색
                if(instance == null)
                {
                    instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    //퀘스트 시스템이란 이름으로 Quest System 컴포넌트를 추가해 생성합니다.
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
        //JSON 형태로 SavedData를 변환해주고, JSON Array에 추가하는 코드
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


    #region Contains (퀘스트 포함 여부)
    public bool ContainsInActiveQuests(Quest quest) => activedQuests.Any(x=>x.Id == quest.Id);
    public bool ContainsInCompleteQuests(Quest quest) => completedQuests.Any(x=>x.Id == quest.Id);

    public bool ContainsInActiveAchievement(Quest quest) => activeAchievements.Any(x => x.Id == quest.Id);
    public bool ContainsInCompleteAchievement(Quest quest) => completedAchievements.Any(x => x.Id == quest.Id);
    #endregion
    #region Callback

    //내부 시스템에서 사용할 Receive
    private void Receive(List<Quest> quests, string category, object target, int S_count)
    {
        //Quest가 for문을 돌리는 중에 Complete 처리가 되서 목록에서 제거가 될 경우
        //범위 이탈 문제가 발생할 수 있기 때문
        foreach(var quest in quests.ToArray())
            quest.Receive(category, target, S_count);
    }
    //외부에서 사용할 Receive
    public void Receive(string category, object target, int S_count)
    {
        //퀘스트와 업적 목록을 넣고 Receive를 작동
        Receive(activedQuests, category, target, S_count);
        Receive(activeAchievements, category, target, S_count);
    }
    //직접적으로 받아서 사용할 Receive
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
        //데이터의 저장은 프로그램에서 정말 중요하고,
        //어느 시점에서 문제가 생겨서 해당 작업이 처리되지 않고
        //자료가 날라갈 가능성도 있음.
        //현 프로젝트에서는 종료 시 저장 또는 UI를 통한 저장 등을 고려하고 있고
        //실제 프로젝트에서 응용하실 때는 특정 시점, 특정 상황 등에서 Save를 적절하게
        //적용할 것
    }
    #endregion
}
