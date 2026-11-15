using Domain.Quests;
using Domain.Utility;
using UnityEngine;

public class RewardShower : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text cost;
    [SerializeField] TMPro.TMP_Text reward;
    
    private Quest _quest;
    
    public void SetQuest(Quest quest)
    {
        _quest = quest;
        cost.text = $"Assign {Descriptions.FormatSoldiers(quest.Requirement)} warriors. In {Descriptions.FormatDays(quest.DaysToComplete)}";
        reward.text = quest.Reward.GetDescription();
    }
}
