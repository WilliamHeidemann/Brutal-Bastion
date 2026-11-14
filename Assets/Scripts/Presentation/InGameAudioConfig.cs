using System;
using FMODUnity;
using UnityEngine;

namespace Game.Presentation.Audio
{
    public enum Sound
    {
        StartButton,
        DrawQuest
    }

    [CreateAssetMenu(fileName = "Audio Container", menuName = "Audio/Audio Container")]
    public class AudioContainer : ScriptableObject
    {
        [SerializeField] private EventReference _startButton;
        [SerializeField] private EventReference _drawQuest;


        public EventReference Get(Sound sound) => sound switch
        {   
            Sound.StartButton => _startButton,
            Sound.DrawQuest => _drawQuest,
            _ => throw new ArgumentOutOfRangeException(nameof(sound), sound, null)
        };
    }
}