using UnityEngine;
using UnityEngine.Events;
using Presentation.Audio;

public class PhysicalButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private FmodAudioService _audioService;
    private void OnMouseDown()
    {   
        if (_audioService != null)
        {
            // Play the UI On Click (or change UISound.ThemeMusic to whatever sound enum you want)
            _audioService.Play(UISound.OnClickDrum);
        }
        else
        {
            Debug.LogWarning("FmodAudioService reference is missing on " + gameObject.name, this);
        }
        OnClick.Invoke();
    }
}
