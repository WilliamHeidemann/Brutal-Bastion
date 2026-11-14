using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Quests;
using Presentation;
using UnityEngine;

public class ActiveQuestManager : MonoBehaviour
{
    [SerializeField] private GameObject questIndicatorPrefab;
    [SerializeField] private Vector2 spaceBetweenQuests = new Vector2(0, 50);
    [SerializeField] private Transform questIndicatorParent;
    
    private Dictionary<Quest, GameObject> activeQuests = new();
    
    private void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnQuestsChanged += UpdateQuests;
    }

    private void UpdateQuests()
    {
        var newQuests = PlayManager.Instance.PlayCoordinator.GetActiveQuests();
        var allQuests = newQuests.Union(activeQuests.Keys);
        foreach (var quest in allQuests)
        {
            if (!newQuests.Contains(quest) && activeQuests.ContainsKey(quest))
            {
                activeQuests[quest].GetComponent<FlipSprite>().FlipDestroy();
                activeQuests.Remove(quest);
            }

            if (newQuests.Contains(quest) && !activeQuests.ContainsKey(quest))
            {
                activeQuests[quest] = Instantiate(questIndicatorPrefab, questIndicatorParent);
                activeQuests[quest].GetComponent<QuestIndicator>().Quest = quest;
            }
        }
        
        var orderedIndicators = activeQuests
            .OrderBy(q => q.Key.DaysToComplete)
            .Select(q => q.Value)
            .ToList();

        for (int i = 0;  i < orderedIndicators.Count; i++)
        {
            var dest = i * spaceBetweenQuests;
            orderedIndicators[i]
                .GetComponent<MoveTo2D>()
                .SetTarget(questIndicatorParent.position + new Vector3(dest.x, dest.y, 0));
        }
    }
}
