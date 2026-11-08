using FMODUnity;
using UnityEngine;

namespace Game.Presentation.Audio
{
    public enum UISound
    {
        MenuTheme
    }

    [CreateAssetMenu(fileName = "UIAudioConfig", menuName = "Audio/UI Audio Config")]
    public class UIAudioConfig : ScriptableObject
    {
        [field: SerializeField] public EventReference MenuTheme { get; private set; }

        public EventReference Get(UISound sound) => sound switch
        {
            UISound.MenuTheme => MenuTheme,

            _ => default
        };
    }
}