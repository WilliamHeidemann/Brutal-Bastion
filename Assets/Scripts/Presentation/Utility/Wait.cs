using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Wait : MonoBehaviour
{
    [SerializeField] private float seconds;
    public UnityEvent OnComplete;


    public void Trigger()
    {
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(seconds);
        OnComplete.Invoke();
    }
}
