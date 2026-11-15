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
        _audioService.Play(Sound.EndOfDay);
        OnDayEnd.Invoke();
        PlayManager.Instance.PlayCoordinator.PassDay();
    }
}
