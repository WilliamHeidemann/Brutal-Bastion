using Presentation;
using Presentation.Audio;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject overlay;
    [SerializeField] private TMPro.TextMeshProUGUI stats;

    void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnOutOfLives += ShowScreen;
    }

    private void OnDestroy()
    {
        if (PlayManager.Instance != null && PlayManager.Instance.PlayCoordinator != null)
        {
            PlayManager.Instance.PlayCoordinator.OnOutOfLives -= ShowScreen;
        }
    }

    void ShowScreen()
    {
        // Stop all currently playing audio immediately (or set allowFadeOut: true if preferred)
        FmodAudioService.Instance?.StopAll(allowFadeOut: false);

        // Play the losing audio
        FmodAudioService.Instance?.Play(UISound.Failure);

        overlay.SetActive(true);
        UpdateStats();
    }

    private void UpdateStats()
    {
        stats.text = $"Days survived: {PlayManager.Instance.PlayCoordinator.GetDayNumber()}";
    }
}