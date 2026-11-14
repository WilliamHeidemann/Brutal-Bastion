using System;
using FMODUnity;
using UnityEngine;

namespace Presentation.Audio
{
    public enum Sound
    {
        DrawQuest
    }

    [CreateAssetMenu(fileName = "Audio Container", menuName = "Audio/Audio Container")]
    public class AudioContainer : ScriptableObject
    {
        [SerializeField] private EventReference _drawQuest;


        public EventReference Get(Sound sound) => sound switch
        {
            Sound.DrawQuest => _drawQuest,

            _ => throw new ArgumentOutOfRangeException(nameof(sound), sound, null)
        };
    }
}