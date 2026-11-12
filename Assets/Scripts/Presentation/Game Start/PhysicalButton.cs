using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnClick;
    private void OnMouseDown()
    {
        OnClick.Invoke();
    }
}
