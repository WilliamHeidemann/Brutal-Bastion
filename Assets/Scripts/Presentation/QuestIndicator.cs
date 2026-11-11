using System;
using Domain;
using Domain.Quests;
using Game.Presentation.Audio;
using Presentation;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestIndicator : MonoBehaviour
{
    public Sprite DeclineSprite;
    public Sprite AcceptSprite;
    public Quest Quest;
    public GameObject QuestPresenterPrefab;
    public FmodAudioService _audioService;
    
    private bool _accepted = false;

    private void Start()
    {
        // Random range. TODO: Remove and spawn sensibly elsewhere.
        GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-800, 800), Random.Range(-500, 500), 0);
    }
    
    public void OnClick()
    {
        var go = Instantiate(QuestPresenterPrefab, transform.parent.parent);
        var qp = go.GetComponent<QuestPresentation>();
        qp.indicator = this;
        qp.Quest = Quest;
    }

    private void Toggle()
    {
        _accepted = !_accepted;
        GetComponent<Image>().sprite = _accepted ? AcceptSprite : DeclineSprite;
    }

    public void Accept()
    {
        if (_accepted)
            return;
        
        _accepted = true;
        GetComponent<FlipSprite>().Flip(AcceptSprite);
    }

    public void Decline()
    {
        if (!_accepted)
            return;
        
        _accepted = false;
        GetComponent<FlipSprite>().Flip(DeclineSprite);
    }
}
