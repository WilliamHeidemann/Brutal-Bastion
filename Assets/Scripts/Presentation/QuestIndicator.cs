using System;
using System.Linq;
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
        PlayManager.Instance.PlayCoordinator.OnAcceptedQuestsChanged += SetIndicator;
    }
    
    public void OnClick()
    {
        PlayManager.Instance.lastInteractedQuestPosition = GetComponent<RectTransform>().position;
    }

    private void OnDestroy()
    {
        PlayManager.Instance.PlayCoordinator.OnAcceptedQuestsChanged -= SetIndicator;
    }

    private void SetIndicator()
    {
        if (PlayManager.Instance.PlayCoordinator.GetAcceptedQuests().Contains(_quest))
            Accept();
        else
            Decline();
    }

    public void Accept()
    {
        if (_accepted)
            return;
        
        PlayManager.Instance.lastInteractedQuestPosition = GetComponent<RectTransform>().position;
        _accepted = true;
        GetComponent<FlipSprite>().Flip(AcceptSprite);
    }

    public void Decline()
    {
        if (!_accepted)
            return;
        
        PlayManager.Instance.lastInteractedQuestPosition = GetComponent<RectTransform>().position;
        _accepted = false;
        GetComponent<FlipSprite>().Flip(DeclineSprite);
    }
}
