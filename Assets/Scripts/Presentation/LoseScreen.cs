using Presentation;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject overlay;
    
    void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnOutOfLives += ShowScreen;
    }

    void ShowScreen()
    {
        overlay.SetActive(true);
    }
}
