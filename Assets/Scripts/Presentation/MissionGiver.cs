using System;
using Domain;
using UnityEngine;

namespace Presentation
{
    public class MissionGiver : MonoBehaviour
    {
        [SerializeField] private MissionPresenter _missionPresenter;

        public void GiveMissions(int count)
        {
            MissionGenerator missionGenerator = new();
            for (int i = 0; i < count; i++)
            {
                Mission mission = missionGenerator.Generate();
                _missionPresenter.AddMission(mission);
            }
        }
    }
}