using System;
using Domain;
using UnityEngine;

namespace Presentation
{
    public class PlayManager : MonoBehaviour
    {
        public PlayCoordinator PlayCoordinator = new();
        private static PlayManager _instance;
        public int AvailableSoldiers => PlayCoordinator.GetArmySize() - PlayCoordinator.GetCommitedSoldiers();
        public Vector2 lastInteractedQuestPosition;

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

        private void Awake()
        {
            PlayCoordinator.OnNewDay += ResetLastInteractedQuestPosition;
        }
        
        private void ResetLastInteractedQuestPosition()
        {
            // Set last interacted to center of screen.
            lastInteractedQuestPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }
}