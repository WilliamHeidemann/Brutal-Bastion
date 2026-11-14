using UnityEngine;
using UnityEngine.Events;

public class MoveTo2D : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] [Range(0, 1)] private float time = .2f;
    public UnityEvent OnFinish;
    
    private float distance;
    private Vector2 direction;
    private float lastStep = 0f;
    private bool go = false;
    private float elapsedTime = 0f;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTarget(Vector2 target)
    {
        Vector2 pos = rectTransform.position;
        distance = (target - pos).magnitude;
        direction = (target - pos).normalized;
        lastStep = 0f;
        go = true;
    }

    private void Update()
    {
        if (!go)
            return;
        
        elapsedTime += Time.deltaTime;
        float eval = speedCurve.Evaluate(elapsedTime/time);

        Vector2 move = direction * (distance * (eval - lastStep));
        rectTransform.position += new Vector3(move.x, move.y, 0);

        lastStep = eval;
        
        if (elapsedTime >= time)
        {
            OnFinish.Invoke();
            go = false;
        }
    }
}
