using System.Collections.Generic;
using Domain.Quests;
using Game.Presentation.Audio;
using Presentation;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject QuestIndicator;
    private List<GameObject> indicators = new List<GameObject>();
    public FmodAudioService _audioService;

    private void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnNewDay += GenerateQuests;
        GenerateQuests();
    }

    public void GenerateQuests()
    {
        List<Quest> quests = new List<Quest>();
        for (int i = 0; i < 5; i++)
            quests.Add(QuestGenerator.Generate());
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
