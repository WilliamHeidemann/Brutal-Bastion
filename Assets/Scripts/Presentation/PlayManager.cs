using Domain;
using UnityEngine;

namespace Presentation
{
    public class PlayManager : MonoBehaviour
    {
        public PlayCoordinator PlayCoordinator = new();
        private static PlayManager _instance;
        public int AvailableSoldiers => PlayCoordinator.GetArmySize() - PlayCoordinator.GetCommitedSoldiers();

        public static PlayManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("PlayManager");
                    _instance = go.AddComponent<PlayManager>();
                }
                
                return _instance;
            }
        }
    }
}