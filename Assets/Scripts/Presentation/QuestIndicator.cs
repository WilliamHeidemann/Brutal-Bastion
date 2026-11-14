using System;
using Domain;
using Domain.Quests;
using Presentation;
using Presentation.Audio;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestIndicator : MonoBehaviour
{
    public Sprite DeclineSprite;
    public Sprite AcceptSprite;
    private Quest _quest;

    public Quest Quest
    {
        get { return _quest; }
        set
        {
            _quest = value;
            gameObject.name = _quest.Title;
        }
    }
    public GameObject QuestPresenterPrefab;
    public FmodAudioService _audioService;
    public Transform canvas;
    
    [SerializeField] private bool scaleByCost = true;
    
    private bool _accepted = false;

    private void Start()
    {
        if (scaleByCost)
        {
            float scaleFactor = (float)Math.Sqrt((float)Quest.Requirement / 200);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        GetComponent<Image>().sprite = null;
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
        GetComponent<FlipSprite>().Flip(DeclineSprite);
    }
    
    public void OnClick()
    {
        PlayManager.Instance.lastInteractedQuestPosition = GetComponent<RectTransform>().position;
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
