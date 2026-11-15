using Presentation;
using Presentation.Audio;
using UnityEngine;
using UnityEngine.Events;

public class NextDay : MonoBehaviour
{   
    [SerializeField] private FmodAudioService _audioService;
    public UnityEvent OnDayEnd;
    
    public void EndDay()
    {
        PlayManager.Instance.PlayCoordinator.PassDay();
        OnDayEnd.Invoke();

        // Replace with your PlayCoordinator's lives/game-over check (e.g., GetLives() > 0 or !IsGameOver)
        if (PlayManager.Instance.PlayCoordinator.GetLives() > 0)
        {
            _audioService.Play(Sound.EndOfDay);
        }
    }
}