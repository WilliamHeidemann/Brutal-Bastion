using Presentation;
using UnityEngine;

public class NextDay : MonoBehaviour
{
    public void EndDay()
    {
        PlayManager.Instance.PlayCoordinator.PassDay();
    }
}
