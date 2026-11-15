using Presentation;
using Presentation.Audio;
using UnityEngine;
using UnityEngine.Events;

public class NextDay : MonoBehaviour
{
    public UnityEvent OnDayEnd;

    public void EndDay()
    {
        PlayManager.Instance.PlayCoordinator.PassDay();
        OnDayEnd.Invoke();

        int lives = PlayManager.Instance.PlayCoordinator.GetLives();
        Debug.Log($"[NextDay] Lives: {lives}, AudioService: {FmodAudioService.Instance != null}");

        if (lives > 0)
        {
            FmodAudioService.Instance?.Play(Sound.EndOfDay);
        }
    }
}