using System.Threading;
using FMODUnity;
using UnityEngine;

namespace Game.Presentation.Audio
{
    public interface IAudioService
    {
        // Gameplay (Enum-based)
        void Play(Sound sound);
        Awaitable PlayAsync(Sound sound, float delaySeconds, CancellationToken cancellationToken = default);
        void Stop(Sound sound, bool allowFadeOut = true);

        // UI (Enum-based)
        void Play(UISound uiSound);
        void Stop(UISound uiSound, bool allowFadeOut = true);

        // Direct FMOD overrides (when designers assign custom events directly)
        void Play(EventReference eventRef);
        void Stop(EventReference eventRef, bool allowFadeOut = true);

        // 3D positional audio
        void PlayAttached(Sound sound, GameObject target);

        // Global control
        void StopAll(bool allowFadeOut = true);
    }
}