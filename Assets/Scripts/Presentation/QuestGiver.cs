using System;
using Domain;
using Domain.Quests;
using UnityEngine;

namespace Presentation
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private QuestPresenter _questPresenter;

        public void GiveQuests(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Quest quest = QuestGenerator.Generate();
                _questPresenter.AddQuest(quest);
            }
        }
    }
}