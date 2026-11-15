using System;
using FMODUnity;
using UnityEngine;

namespace Presentation.Audio
{
    public enum Sound
    {
        ClickOnQuest,
        ClickOffQuest,
        DeploySoldiers,
    }

    [CreateAssetMenu(fileName = "Audio Container", menuName = "Audio/Audio Container")]
    public class AudioContainer : ScriptableObject
    {
        [SerializeField] private EventReference _clickOnQuest;
        [SerializeField] private EventReference _clickOffQuest;
        [SerializeField] private EventReference _deploySoldiers;

        public EventReference Get(Sound sound) => sound switch
        {   
            Sound.ClickOnQuest => _clickOnQuest,
            Sound.ClickOffQuest => _clickOffQuest,
            Sound.DeploySoldiers => _deploySoldiers,

            _ => throw new ArgumentOutOfRangeException(nameof(sound), sound, null)
        };
    }
}