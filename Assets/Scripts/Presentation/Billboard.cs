using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool OnStart = true;
    public bool OnUpdate = true;

    private void Start()
    {
        if  (OnStart)
            transform.forward = Camera.main.transform.forward;
    }

    // Update is called once per frame
    private void Update()
    {
        if  (OnUpdate)
            transform.forward = Camera.main.transform.forward;
    }
}
