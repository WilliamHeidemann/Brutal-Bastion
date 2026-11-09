using Presentation;
using UnityEngine;

public class NextDay : MonoBehaviour
{
    public void EndDay()
    {
        Debug.Log("Ending day...");
        PlayManager.Instance.PlayCoordinator.PassDay();
    }
}
