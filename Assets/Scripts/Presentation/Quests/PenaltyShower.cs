using Domain.Quests;
using UnityEngine;

public class PenaltyShower : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI penalty;
    
    private Quest _quest;

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        penalty.text = _quest.Penalty.GetDescription();
    }
}
