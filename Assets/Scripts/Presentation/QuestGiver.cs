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
            QuestGenerator questGenerator = new();
            for (int i = 0; i < count; i++)
            {
                Quest quest = questGenerator.Generate();
                _questPresenter.AddQuest(quest);
            }
        }
    }
}