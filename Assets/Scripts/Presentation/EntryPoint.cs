using UnityEngine;

namespace Presentation
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private MissionGiver _missionGiver;
        [SerializeField] private MissionPresenter _missionPresenter;

        private void Start()
        {
            _missionPresenter.Initialize();
            _missionGiver.GiveMissions(5);
        }
    }
}