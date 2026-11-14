using FMODUnity;
using UnityEngine;

namespace Game.Presentation.Audio
{
    public enum UISound
    {
        Ambient,
        ThemeMusic,
        OnClickDrum
    }

    [CreateAssetMenu(fileName = "UIAudioConfig", menuName = "Audio/UI Audio Config")]
    public class UIAudioConfig : ScriptableObject
    {
        [field: SerializeField] public EventReference _ambient { get; private set; }
        [field: SerializeField] public EventReference _themeMusic { get; private set; }
        [field: SerializeField] public EventReference _onClickDrum { get; private set; }
    

        public EventReference Get(UISound sound) => sound switch
        {
            UISound.Ambient => _ambient,
            UISound.ThemeMusic => _themeMusic,
            UISound.OnClickDrum => _onClickDrum,

            _ => default
        };
    }
}