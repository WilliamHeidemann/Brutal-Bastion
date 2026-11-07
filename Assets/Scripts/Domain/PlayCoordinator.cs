using System.Collections.Generic;

namespace Domain
{
    public class PlayCoordinator
    {
        public readonly TableOfMissions TableOfMissions = new();
        public readonly Army Army = new();

        public void Accept(Mission mission)
        {
            TableOfMissions.Add(mission);
        }

        public void Discard(Mission mission)
        {
            Army.Size -= mission.SizePenalty;
            Army.Health -= mission.HealthPenalty;
        }

        public void PassDay()
        {
            foreach (Mission completedMission in TableOfMissions.PassDay())
            {
                Army.Size += completedMission.Requirement + completedMission.Reward;
            }
        }
    }
}