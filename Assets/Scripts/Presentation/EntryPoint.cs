using UnityEngine;

namespace Presentation
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private QuestGiver _questGiver;
        [SerializeField] private QuestPresenter _questPresenter;

        private void Start()
        {
            _questPresenter.Initialize();
            _questGiver.GiveQuests(5);
        }
    }
}