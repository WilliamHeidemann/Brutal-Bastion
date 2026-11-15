using Domain;
using Domain.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class QuestPicker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _region;
        [SerializeField] private RewardShower _reward;
        [SerializeField] private PenaltyShower _penalty;
        [SerializeField] private Sprite _acceptSprite;
        [SerializeField] private Sprite _declineSprite;
        [SerializeField] private Image _acceptImage;
        [SerializeField] private Image _declineImage;
        private Quest _quest;
        private bool _accepted;

        public void SetQuest(Quest quest)
        {
            _quest = quest;
            _title.text = quest.Title;
            _region.text = quest.Region.Format();
            _reward.SetQuest(quest);
            _penalty.SetQuest(quest);

            _accepted = false;
            UpdateVisuals();
            PlayManager.Instance.PlayCoordinator.Decline(_quest);
        }

        public void Accept()
        {
            if (_accepted) return;
            if (_quest.Requirement > PlayManager.Instance.AvailableSoldiers)
                return;

            _accepted = true;
            PlayManager.Instance.PlayCoordinator.Accept(_quest);
            UpdateVisuals();
        }

        public void Decline()
        {
            if (!_accepted) return;

            _accepted = false;
            PlayManager.Instance.PlayCoordinator.Decline(_quest);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            _acceptImage.sprite = _accepted ? _acceptSprite : _declineSprite;
            _declineImage.sprite = _accepted ? _declineSprite : _acceptSprite;
        }
    }
}