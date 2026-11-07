using Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation
{
    public class MissionPresenter : MonoBehaviour
    {
        [SerializeField] private PlayManager _playManager;
        
        [SerializeField] private PanelRenderer _missions;
        [SerializeField] private VisualTreeAsset _missionTemplate;
        private ScrollView _missionsScrollView;
        
        public void Initialize()
        {
            _missions.RegisterUIReloadCallback(RegisterMissions);
        }

        public void AddMission(Mission mission)
        {
            VisualElement missionElement = _missionTemplate.Instantiate();

            missionElement.Q<Label>("DaysToCompleteValue").text = mission.DaysToComplete.ToString();
            missionElement.Q<Label>("RequirementValue").text = mission.Requirement.ToString();
            missionElement.Q<Label>("RewardValue").text = mission.Reward.ToString();
            missionElement.Q<Label>("SizePenaltyValue").text = mission.SizePenalty.ToString();
            missionElement.Q<Label>("HealthPenaltyValue").text = mission.HealthPenalty.ToString();

            missionElement.Q<Button>("Accept").clicked += () =>
            {
                _playManager.PlayCoordinator.Accept(mission);
                _missionsScrollView.Remove(missionElement);
            };
            
            _missionsScrollView.Add(missionElement);
        }

        private void RegisterMissions(PanelRenderer panelRenderer, VisualElement rootElement, int version)
        {
            _missionsScrollView = rootElement.Q<ScrollView>("MissionsScrollView");
        }
    }
}