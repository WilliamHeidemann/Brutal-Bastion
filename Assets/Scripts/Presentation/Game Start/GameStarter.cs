using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Presentation.Audio;
using FMODUnity; // Added for RuntimeManager check

public class GameStarter : MonoBehaviour
{
    [SerializeField] private FmodAudioService _audioService;
    [SerializeField] private GameObject canvas;
    [SerializeField] private float waitBeforeStart = 2.0f;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameStartAfterDelay;

    private IEnumerator Start()
    {
        if (_audioService == null)
        {
            Debug.LogError("FmodAudioService reference is missing on GameStarter!");
            yield break;
        }

        // 1. Wait for banks to finish loading
        while (!RuntimeManager.HasBankLoaded("Master") || !RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        // 2. Force FMOD Studio to finish building internal bus/event models
        RuntimeManager.StudioSystem.flushCommands();

        _audioService.Play(UISound.Ambient);
    }
    
    public void StartGame()
    {
        OnGameStart.Invoke();
        StartCoroutine(StartRoutine());
        try
        {
            _audioService.Play(UISound.ThemeMusic);
            _audioService.Stop(UISound.Ambient);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(waitBeforeStart);
        OnGameStartAfterDelay.Invoke();
    }
}