using Domain.Quests;
using Presentation;
using UnityEngine;

public class QuestPresentation : MonoBehaviour
{
    private Quest _quest;
    public Quest Quest
    {
        get => _quest;
        set
        {
            _quest = value;
            _reward.text = _quest.Reward.GetDescription();
            _penalty.text = _quest.Penalty.GetDescription();
        }
    }
    public QuestIndicator indicator;
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _reward;
    [SerializeField] private TMPro.TextMeshProUGUI _penalty;

    public void Accept()
    {
        indicator.Accept();
        PlayManager.Instance.PlayCoordinator.Accept(Quest);
    }

    public void Decline()
    {
        indicator.Decline();
        PlayManager.Instance.PlayCoordinator.Decline(Quest);
    }
}
