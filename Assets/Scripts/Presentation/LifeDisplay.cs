using Presentation;
using UnityEngine;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnLivesChanged += UpdateLives;
        UpdateLives();
    }

    private void UpdateLives()
    {
        text.text = $"{PlayManager.Instance.PlayCoordinator.GetLives()} hearts";
    }
}
