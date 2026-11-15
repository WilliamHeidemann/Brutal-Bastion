using Domain.Utility;
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
        string heart = Descriptions.FormatLives(1);
        text.text = "";
        for (int i = 0; i < PlayManager.Instance.PlayCoordinator.GetLives(); i++)
            text.text += " " + heart;
        
        if (PlayManager.Instance.PlayCoordinator.GetLives() > 8)
            text.text = Descriptions.FormatLives(PlayManager.Instance.PlayCoordinator.GetLives());
    }
}
