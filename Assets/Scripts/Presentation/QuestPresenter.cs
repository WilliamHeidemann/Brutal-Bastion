using Domain;
using Domain.Quests;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation
{
    public class QuestPresenter : MonoBehaviour
    {
        [SerializeField] private PlayManager _playManager;
        
        [SerializeField] private PanelRenderer _quests;
        [SerializeField] private VisualTreeAsset _questTemplate;
        private ScrollView _questsScrollView;
        
        public void Initialize()
        {
            _quests.RegisterUIReloadCallback(RegisterQuests);
        }

        public void AddQuest(Quest quest)
        {
            VisualElement questElement = _questTemplate.Instantiate();

            questElement.Q<Label>("DaysToCompleteValue").text = quest.DaysToComplete.ToString();
            questElement.Q<Label>("RequirementValue").text = quest.Requirement.ToString();
            questElement.Q<Label>("RewardValue").text = quest.Reward.GetDescription();
            questElement.Q<Label>("SizePenaltyValue").text = quest.Penalty.GetDescription();

            questElement.Q<Button>("Accept").clicked += 
                () => _playManager.PlayCoordinator.offeredQuests.Add(quest);
            
            _questsScrollView.Add(questElement);
        }

        private void RegisterQuests(PanelRenderer panelRenderer, VisualElement rootElement, int version)
        {
            _questsScrollView = rootElement.Q<ScrollView>("QuestsScrollView");
        }
    }
}