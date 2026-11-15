using Domain;
using Domain.Quests;
using Domain.Utility;
using TMPro;
using UnityEngine;

public class QuestSummary : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text region;
    [SerializeField] private TMP_Text reward;
    [SerializeField]  private TMP_Text time;
    private Quest _quest;

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        title.text = quest.Title;
        region.text = quest.Region.Format();
        reward.text = quest.Reward.GetShortDescription();
        time.text = $"{Descriptions.FormatDays(quest.DaysToComplete)}";
    }
}
