using Domain.Quests;
using UnityEngine;

public class RewardShower : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text cost;
    [SerializeField] TMPro.TMP_Text reward;
    
    private Quest _quest;
    
    public void SetQuest(Quest quest)
    {
        _quest = quest;
        cost.text = $"Assign {quest.Requirement.ToString()} warriors";
        reward.text = quest.Reward.GetDescription();
    }
}
