using System;
using UnityEngine;
using UnityEngine.Events;
using Presentation.Audio;

public class PhysicalButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private FmodAudioService _audioService;
    private void OnMouseDown()
    {   
        OnClick.Invoke();
        try
        {
            // Play the UI On Click (or change UISound.ThemeMusic to whatever sound enum you want)
            _audioService.Play(UISound.OnClickDrum);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            throw;
        }
    }
}
