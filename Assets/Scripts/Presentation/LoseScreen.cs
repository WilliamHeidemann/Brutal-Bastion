using Presentation;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject overlay;
    [SerializeField] private TMPro.TextMeshProUGUI stats;
    
    void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnOutOfLives += ShowScreen;
    }

    void ShowScreen()
    {
        overlay.SetActive(true);
        UpdateStats();
    }

    private void UpdateStats()
    {
        stats.text = $"Days survived: {PlayManager.Instance.PlayCoordinator.GetDayNumber()}";
    }
}
