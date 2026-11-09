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
        GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-800, 800), Random.Range(-600, 600), 0);
    }
    
    public void OnClick()
    {
        var go = Instantiate(QuestPresenterPrefab, transform.parent.parent);
        var qp = go.GetComponent<QuestPresentation>();
        Debug.Log($"Quest Cost: {Quest.Requirement}");
        Debug.Log($"Quest Time: {Quest.DaysToComplete}");
        Debug.Log($"Quest Reward: {Quest.Reward.GetDescription()}");
        Debug.Log($"Quest Penalty: {Quest.Penalty.GetDescription()}");
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
        _accepted = true;
        GetComponent<Image>().sprite = AcceptSprite;
    }

    public void Decline()
    {
        _accepted = false;
        GetComponent<Image>().sprite = DeclineSprite;
    }
}
