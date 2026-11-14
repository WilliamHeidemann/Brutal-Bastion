using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlipSprite : MonoBehaviour
{
    private bool flipping = false;
    private bool destroy = false;
    private Image _image;
    private Sprite newSprite;
    private Sprite oldSprite;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float timeToFlip;
    private float timeSinceStart = 0.0f;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
    }
    
    public void Flip(Sprite sprite)
    {
        timeSinceStart =  0.0f;
        flipping = true;
        oldSprite = _image.sprite;
        newSprite = sprite;
    }

    public void FlipDestroy()
    {
        timeSinceStart =  0.0f;
        flipping = true;
        oldSprite = _image.sprite;
        newSprite = null;
        destroy = true;
    }

    private void Update()
    {
        if (!flipping)
            return;
        
        timeSinceStart = Mathf.Clamp(timeSinceStart + Time.deltaTime,0.0f, timeToFlip);
        float progress =  timeSinceStart / timeToFlip;
        float degrees = Mathf.Lerp(0, 180, curve.Evaluate(progress));

        if (degrees > 90)
        {
            _image.sprite = newSprite;
            degrees -= 180;
            _image.color = Color.white;
            if (destroy)
                Destroy(gameObject);
        }
        else
        {
            _image.sprite = oldSprite;
            if (oldSprite == null)
                _image.color = new Color(0, 0, 0, 0);
        }
        
        transform.localEulerAngles = new Vector3(0, degrees, 0);

        if (timeSinceStart >= timeToFlip)
        {
            flipping = false;
        }
    }
}
