using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Presentation.Audio;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private FmodAudioService _audioService;
    [SerializeField] private GameObject canvas;
    [SerializeField] private float waitBeforeStart = 2.0f;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameStartAfterDelay;

    private void Start()
    {
        if (_audioService != null)
        {
            _audioService.Play(UISound.Ambient);
        }
        else
        {
            Debug.LogError("FmodAudioService reference is missing on GameStarter!");
        }
    }

    public void StartGame()
    {
        OnGameStart.Invoke();
        {
            _audioService.Play(UISound.ThemeMusic);
            _audioService.Stop(UISound.Ambient);
        }
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(waitBeforeStart);
        OnGameStartAfterDelay.Invoke();
    }
}