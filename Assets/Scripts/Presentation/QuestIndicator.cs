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
    public Transform canvas;
    
    private bool _accepted = false;

    private void Start()
    {
        //GetComponent<Image>().sprite = null;
        GetComponent<FlipSprite>().Flip(AcceptSprite);
    }
    
    public void OnClick()
    {
        var go = Instantiate(QuestPresenterPrefab, canvas);
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
