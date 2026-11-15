using FMODUnity;
using UnityEngine;

namespace Presentation.Audio
{
    public enum UISound
    {
        Ambient,
        ThemeMusic,
        OnClickDrum,
        Failure,
        Click,
    }

    [CreateAssetMenu(fileName = "UIAudioConfig", menuName = "Audio/UI Audio Config")]
    public class UIAudioConfig : ScriptableObject
    {
        [field: SerializeField] public EventReference _ambient { get; private set; }
        [field: SerializeField] public EventReference _themeMusic { get; private set; }
        [field: SerializeField] public EventReference _onClickDrum { get; private set; }
        [field: SerializeField] public EventReference _failure { get; private set; }
        [field: SerializeField] public EventReference _click { get; private set; }
    

        public EventReference Get(UISound sound) => sound switch
        {
            UISound.Ambient => _ambient,
            UISound.ThemeMusic => _themeMusic,
            UISound.OnClickDrum => _onClickDrum,
            UISound.Failure => _failure,
            UISound.Click => _click,

            _ => default
        };
    }
}