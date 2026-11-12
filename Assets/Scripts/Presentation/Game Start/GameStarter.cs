using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private float waitBeforeStart = 2.0f;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameStartAfterDelay;
    
    public void StartGame()
    {
        OnGameStart.Invoke();
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(waitBeforeStart);
        OnGameStartAfterDelay.Invoke();
    }
}
