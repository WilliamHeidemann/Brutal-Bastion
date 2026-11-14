using Domain;
using Domain.Quests;
using Presentation;
using UnityEngine;

public class QuestPicker : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private TMPro.TextMeshProUGUI region;
    [SerializeField] private RewardShower reward;
    [SerializeField] private PenaltyShower penalty;
    
    private Quest _quest;
    private bool _accepted;

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        title.text = quest.Title;
        region.text = quest.Region.Format();
        reward.SetQuest(quest);
        penalty.SetQuest(quest);
    }

    public void Accept()
    {
        if (_accepted)
            return;
        _accepted = true;
        PlayManager.Instance.PlayCoordinator.Accept(_quest);
    }

    public void Decline()
    {
        if (!_accepted)
            return;
        _accepted = false;
        PlayManager.Instance.PlayCoordinator.Decline(_quest);
    }
}
