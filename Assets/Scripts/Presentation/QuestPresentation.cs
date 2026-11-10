using Domain.Quests;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

public class QuestPresentation : MonoBehaviour
{
    private Quest _quest;
    public Quest Quest
    {
        get => _quest;
        set
        {
            _quest = value;
            Debug.Log($"Quest Cost: {Quest.Requirement}");
            Debug.Log($"Quest Time: {Quest.DaysToComplete}");
            Debug.Log($"Quest Reward: {Quest.Reward.GetDescription()}");
            Debug.Log($"Quest Penalty: {Quest.Penalty.GetDescription()}");
            _cost.text = $"Cost: {Quest.Requirement.ToString()} soldiers";
            _duration.text = $"Duration: {Quest.DaysToComplete.ToString()} days";
            _reward.text = Quest.Reward.GetDescription();
            _penalty.text = Quest.Penalty.GetDescription();
            if (Quest.Requirement > PlayManager.Instance.AvailableSoldiers)
                AcceptButton.interactable = false;
        }
    }
    public QuestIndicator indicator;
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _cost;
    [SerializeField] private TMPro.TextMeshProUGUI _duration;
    [SerializeField] private TMPro.TextMeshProUGUI _reward;
    [SerializeField] private TMPro.TextMeshProUGUI _penalty;
    [SerializeField] private Button DeclineButton;
    [SerializeField] private Button AcceptButton;

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
