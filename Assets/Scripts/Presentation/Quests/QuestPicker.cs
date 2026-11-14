using Domain;
using Domain.Quests;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

public class QuestPicker : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private TMPro.TextMeshProUGUI region;
    [SerializeField] private RewardShower reward;
    [SerializeField] private PenaltyShower penalty;
    [SerializeField] private Sprite acceptSprite;
    [SerializeField] private Sprite declineSprite;
    [SerializeField] private Image acceptImage;
    [SerializeField] private Image declineImage;
    
    private Quest _quest;
    private bool _accepted;

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        title.text = quest.Title;
        region.text = quest.Region.Format();
        reward.SetQuest(quest);
        penalty.SetQuest(quest);
        Decline();
    }

    public void Accept()
    {
        if (_accepted)
            return;
        _accepted = true;
        PlayManager.Instance.PlayCoordinator.Accept(_quest);
        acceptImage.sprite = acceptSprite;
        declineImage.sprite = declineSprite;
    }

    public void Decline()
    {
        if (!_accepted)
            return;
        _accepted = false;
        PlayManager.Instance.PlayCoordinator.Decline(_quest);
        acceptImage.sprite = declineSprite;
        declineImage.sprite = acceptSprite;
    }
}
