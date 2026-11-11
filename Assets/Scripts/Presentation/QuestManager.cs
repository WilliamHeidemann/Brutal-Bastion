using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Quests;
using Game.Presentation.Audio;
using Presentation;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestList _questList;
    [SerializeField] private GameObject QuestIndicator;
    [SerializeField] private int questsToDraw = 3;
    [SerializeField] private Transform canvas;
    [SerializeField] private List<Spawner> spawners = new()
    {
        new() {region = Region.Orchid},
        new() {region = Region.Ashmound},
        new () {region = Region.Argot},
        new() {region = Region.Barrow},
    };
    private Dictionary<Region, Spawner> _spawners  = new();
    [SerializeField] private float minDistanceBetweenIndicators = 50f;
    private List<GameObject> indicators = new List<GameObject>();
    public FmodAudioService _audioService;

    private void Start()
    {
        foreach (var spawner in spawners)
            _spawners[spawner.region] = spawner;
        PlayManager.Instance.PlayCoordinator.OnNewDay += GenerateQuests;
        GenerateQuests();
    }

    public void GenerateQuests()
    {
        var quests = new List<Quest>();
        var validQuests = _questList.Quests.Prune(PlayManager.Instance.PlayCoordinator);

        while (quests.Count < questsToDraw && validQuests.Count != 0)
        {
            var q = validQuests.Draw();
            quests.Add(q.Convert());
            validQuests.Remove(q);
        }
        
        PlayManager.Instance.PlayCoordinator.SetQuestsForTheDay(quests);
        
        // This should probably be moved later to control timing from elsewhere.
        UpdateQuests();
    }

    public void UpdateQuests()
    {
        // Get rid of old indicators.
        foreach (var indicator in indicators) 
            Destroy(indicator);
        indicators.Clear();
        
        // Spawn indicators for new quests.
        var quests = PlayManager.Instance.PlayCoordinator.GetOfferedQuests();
        foreach (var quest in quests)
            SpawnIndicator(quest);
    }

    private void SpawnIndicator(Quest quest)
    {
        var go = Instantiate(QuestIndicator, _spawners[quest.Region].position);
        indicators.Add(go);
        
        var r = Random.insideUnitCircle * _spawners[quest.Region].radius;
        go.transform.localPosition = new Vector3(r.x, r.y, 0);
        /*float minDist = minDistanceBetweenIndicators;
        while (indicators.Any(i => Vector3.Distance(i.transform.position, go.transform.position) < minDist))
        {
            // Indicator was spawned too close to other indicator. Try again.
            r = Random.insideUnitCircle * _spawners[quest.Region].radius;
            go.transform.localPosition = new Vector3(r.x, r.y, 0);
            // Lower threshold, to ensure eventual valid position is found, even at cost of overlap.
            minDist *= .99f;
        }*/
        
        var qi = go.GetComponent<QuestIndicator>();
        qi.Quest = quest;
        qi.canvas = canvas;
        qi._audioService  = _audioService;
    }

    public void OnDrawGizmos()
    {
        foreach (var spawner in spawners)
        {
            Gizmos.color = spawner.region.Color();
            Gizmos.DrawSphere(spawner.position.position, spawner.radius);
        }
    }
}

[Serializable]
public class Spawner
{
    public Region region;
    public RectTransform position;
    public float radius;
}
