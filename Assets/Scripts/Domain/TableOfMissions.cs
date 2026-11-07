using System.Collections.Generic;

namespace Domain
{
    public class TableOfMissions
    {
        private readonly Dictionary<Mission, int> _activeMissions = new();

        public void Add(Mission mission)
        {
            _activeMissions.Add(mission, mission.DaysToComplete);
        }
        
        public List<Mission> PassDay()
        {
            List<Mission> completedMissions = new();
            foreach (Mission mission in _activeMissions.Keys)
            {
                _activeMissions[mission] -= 1;
                var daysLeft = _activeMissions[mission];
                if (daysLeft <= 0)
                {
                    completedMissions.Add(mission);
                }
            }
            
            foreach (Mission completedMission in completedMissions)
            {
                _activeMissions.Remove(completedMission);
            }
            
            return completedMissions;
        }
    }
}