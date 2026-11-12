using Presentation;
using UnityEngine;
using UnityEngine.Events;

public class NextDay : MonoBehaviour
{
    public UnityEvent OnDayEnd;
    
    public void EndDay()
    {
        OnDayEnd.Invoke();
        PlayManager.Instance.PlayCoordinator.PassDay();
    }
}
