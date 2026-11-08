using System;
using Domain;
using Domain.Quests;
using Game.Presentation.Audio;
using Presentation;
using UnityEngine;

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
        Quest = QuestGenerator.Generate();
        _audioService.Play(Sound.DrawQuest);
    }
    
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    private void Toggle()
    {
        _accepted = !_accepted;
        GetComponent<SpriteRenderer>().sprite = _accepted ? AcceptSprite : DeclineSprite;
    }
}
