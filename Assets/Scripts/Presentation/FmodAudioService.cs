using System;
using System.Collections.Generic;
using System.Threading;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Game.Presentation.Audio
{
    public class FmodAudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioContainer _audioContainer;
        [SerializeField] private UIAudioConfig _uiAudioConfig;

        // Tracks active instances by their EventReference GUID so they can be stopped
        private readonly Dictionary<FMOD.GUID, List<EventInstance>> _activeInstances = new();

        public void Play(Sound sound)
        {
            if (_audioContainer == null) return;
            Play(_audioContainer.Get(sound));
        }

        public void Play(UISound uiSound)
        {
            if (_uiAudioConfig == null) return;
            Play(_uiAudioConfig.Get(uiSound));
        }

        public void Play(EventReference eventRef)
        {
            if (eventRef.IsNull) return;
            CreateAndStartInstance(eventRef);
        }

        public async Awaitable PlayAsync(Sound sound, float delaySeconds, CancellationToken cancellationToken = default)
        {
            if (_audioContainer == null) return;
            EventReference eventRef = _audioContainer.Get(sound);
            if (eventRef.IsNull) return;

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                destroyCancellationToken,
                cancellationToken
            );

            try
            {
                await Awaitable.WaitForSecondsAsync(delaySeconds, cancellationToken: linkedCts.Token);
                CreateAndStartInstance(eventRef);
            }
            catch (OperationCanceledException)
            {
                // Clean exit if canceled or GameObject destroyed
            }
        }

        public void PlayAttached(Sound sound, GameObject target)
        {
            if (_audioContainer == null || target == null) return;
            EventReference eventRef = _audioContainer.Get(sound);
            if (eventRef.IsNull) return;

            EventInstance instance = RuntimeManager.CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(instance, target);
            instance.start();
            instance.release();

            TrackInstance(eventRef.Guid, instance);
        }

        public void Stop(Sound sound, bool allowFadeOut = true)
        {
            if (_audioContainer == null) return;
            Stop(_audioContainer.Get(sound), allowFadeOut);
        }

        public void Stop(UISound uiSound, bool allowFadeOut = true)
        {
            if (_uiAudioConfig == null) return;
            Stop(_uiAudioConfig.Get(uiSound), allowFadeOut);
        }

        public void Stop(EventReference eventRef, bool allowFadeOut = true)
        {
            if (eventRef.IsNull) return;
            if (!_activeInstances.TryGetValue(eventRef.Guid, out List<EventInstance> instances)) return;

            STOP_MODE stopMode = allowFadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

            for (int i = instances.Count - 1; i >= 0; i--)
            {
                EventInstance instance = instances[i];
                if (instance.isValid())
                {
                    instance.stop(stopMode);
                    instance.release();
                }
            }

            instances.Clear();
        }

        public void StopAll(bool allowFadeOut = true)
        {
            STOP_MODE stopMode = allowFadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

            // Stops everything on the Master Bus (including any untracked OneShots)
            if (RuntimeManager.StudioSystem.getBus("bus:/", out Bus masterBus) == FMOD.RESULT.OK)
            {
                masterBus.stopAllEvents(stopMode);
            }

            _activeInstances.Clear();
        }

        private void CreateAndStartInstance(EventReference eventRef)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventRef);
            instance.start();
            instance.release(); // Tells FMOD to destroy the instance automatically once playback finishes

            TrackInstance(eventRef.Guid, instance);
        }

        private void TrackInstance(FMOD.GUID guid, EventInstance instance)
        {
            if (!_activeInstances.TryGetValue(guid, out List<EventInstance> list))
            {
                list = new List<EventInstance>();
                _activeInstances[guid] = list;
            }

            // Prune finished/invalidated instances to avoid unbounded list growth
            list.RemoveAll(i => !IsPlaying(i));
            list.Add(instance);
        }

        private static bool IsPlaying(EventInstance instance)
        {
            if (!instance.isValid()) return false;
            instance.getPlaybackState(out PLAYBACK_STATE state);
            return state != PLAYBACK_STATE.STOPPED;
        }

        private void OnDestroy()
        {
            StopAll(allowFadeOut: false);
        }
    }
}