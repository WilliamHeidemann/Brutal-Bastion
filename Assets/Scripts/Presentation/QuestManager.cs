using System.Collections.Generic;
using System.Linq;
using Domain.Quests;
using Game.Presentation.Audio;
using Presentation;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestList _questList;
    [SerializeField] private GameObject QuestIndicator;
    private List<GameObject> indicators = new List<GameObject>();
    public FmodAudioService _audioService;

    private void Start()
    {
        Debug.Log("Starting QuestManager");
        PlayManager.Instance.PlayCoordinator.OnNewDay += GenerateQuests;
        Debug.Log("Starting QuestManager2");
        GenerateQuests();
        Debug.Log("Starting QuestManager3");
    }

    public void GenerateQuests()
    {
        Debug.Log("Generating quests");
        List<Quest> quests = new List<Quest>();
        List<Quest> validQuests = _questList.Quests
            .Where(q => PlayManager.Instance.PlayCoordinator.IsQuestValid(q))
            .Select(q => q.Convert())
            .ToList();
        Debug.Log("Generating quest2s");
        
        foreach (Quest quest in validQuests)
            Debug.Log(quest.Title);

        quests = validQuests.Take(3).ToList();
        
        foreach (Quest quest in quests)
            Debug.Log(quest.Title);
        
        PlayManager.Instance.PlayCoordinator.SetQuestsForTheDay(quests);
        
        // This should probably be moved later to control timing from elsewhere.
        UpdateQuests();
    }

    public void UpdateQuests()
    {
        // Get rid of old indicators.
        foreach (var indicator in indicators) 
            Destroy(indicator);
        indicators.Clear();
        
        // Spawn indicators for new quests.
        var quests = PlayManager.Instance.PlayCoordinator.GetOfferedQuests();
        foreach (var quest in quests)
        {
            var go = Instantiate(QuestIndicator, transform);
            indicators.Add(go);
            var qi = go.GetComponent<QuestIndicator>();
            qi.Quest = quest;
            qi._audioService  = _audioService;
        }
    }
}
