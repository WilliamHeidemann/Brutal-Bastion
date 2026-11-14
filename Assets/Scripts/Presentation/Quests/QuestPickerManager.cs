using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Quests;
using Presentation;
using UnityEngine;

public class QuestPickerManager : MonoBehaviour
{
    [SerializeField] private GameObject questPickerPrefab;
    [SerializeField] private Vector2 spaceBetweenQuests = new Vector2(80, 0);
    [SerializeField] private Transform questPickerParent;
    
    private Dictionary<Quest, GameObject> activePickers = new();

    public void RemoveQuests()
    {
        foreach (var quest in activePickers.Values)
            Destroy(quest.gameObject);
        activePickers.Clear();
    }
    
    public void UpdateQuests()
    {
        RemoveQuests();
        var quests = PlayManager.Instance.PlayCoordinator.GetOfferedQuests();
        foreach (var quest in quests)
        {
            activePickers[quest] = Instantiate(questPickerPrefab, questPickerParent);
            activePickers[quest].GetComponent<QuestPicker>().SetQuest(quest);
        }
        
        var orderedIndicators = activePickers
            .OrderBy(q => q.Key.DaysToComplete)
            .Select(q => q.Value)
            .ToList();

        for (int i = 0;  i < orderedIndicators.Count; i++)
        {
            var dest = i * spaceBetweenQuests;
            orderedIndicators[i]
                .GetComponent<MoveTo2D>()
                .SetTarget(questPickerParent.position + new Vector3(dest.x, dest.y, 0));
        }
    }
}