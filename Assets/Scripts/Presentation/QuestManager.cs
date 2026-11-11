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
    [SerializeField] private int questsToDraw = 3;
    private List<GameObject> indicators = new List<GameObject>();
    public FmodAudioService _audioService;

    private void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnNewDay += GenerateQuests;
        GenerateQuests();
    }

    public void GenerateQuests()
    {
        var quests = new List<Quest>();
        var validQuests = _questList.Quests
            .Where(q => PlayManager.Instance.PlayCoordinator.IsQuestValid(q))
            .ToList();

        while (quests.Count < questsToDraw && validQuests.Count != 0)
        {
            int weightSum = validQuests.Sum(q => q.DrawWeight);
            Debug.Log(weightSum);
            int lottery = Random.Range(0, weightSum) + 1;
            for (int i = 0; i < validQuests.Count; i++)
            {
                lottery -= validQuests[i].DrawWeight;
                if (lottery <= 0)
                {
                    quests.Add(validQuests[i].Convert());
                    validQuests.RemoveAt(i);
                    break;
                }
            }
        }
        
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
