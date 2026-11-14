using System;
using System.Collections;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

public class SoldierBar : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject soldierParticle;
    [SerializeField] private AnimationCurve soldierDrain;
    [SerializeField] [Range(0, 3)] private float timeToDrain = 1.0f;
    [SerializeField] [Range(5, 50)] private int drainSteps = 5;

    private int maxSoldiers = 1000;
    private int soldiers = 0;

    private int displaySoldiers = 0;

    private void Start()
    {
        soldiers = maxSoldiers;
        displaySoldiers = soldiers;
        PlayManager.Instance.PlayCoordinator.OnArmySizeChange += UpdateSoldiers;
        PlayManager.Instance.PlayCoordinator.OnNewDay += NewDay;
        UpdateDisplays();
    }

    private void UpdateSoldiers()
    {
        int newSoldiers = maxSoldiers - PlayManager.Instance.PlayCoordinator.GetCommitedSoldiers();
        int change = newSoldiers - soldiers;
        soldiers = newSoldiers;

        // Copy so we don't get dereferenced.
        Vector2 pos = new(PlayManager.Instance.lastInteractedQuestPosition.x, PlayManager.Instance.lastInteractedQuestPosition.y);
        StartCoroutine(Drain(change, pos));
    }

    private IEnumerator Drain(int amount, Vector2 pos)
    {
        int mult = amount > 0 ? 1 : -1;
        amount *= mult;
        float time = 0.0f;
        const float waitTime = 1f / 60f;
        int soldiersAccountedFor = 0;
        
        while (time < timeToDrain)
        {
            time += waitTime;

            int soldiers = (int)(soldierDrain.Evaluate(time/timeToDrain) * amount);
            int soldiersMissing = soldiers - soldiersAccountedFor;
            int stepsMissing = soldiersMissing / drainSteps;

            for (int i = 0; i < stepsMissing; i++)
            {
                displaySoldiers += mult * drainSteps;
                soldiersAccountedFor += drainSteps;
                UpdateDisplays();

                var go = Instantiate(soldierParticle, transform);
                go.transform.position = mult == 1 ? pos : slider.transform.position;
                go.GetComponent<MoveAndDie>().SetTarget(mult == 1 ? slider.transform.position : pos);
            }
            
            yield return new WaitForSeconds(waitTime);
        }
        
        if (soldiersAccountedFor != amount)
        {
            displaySoldiers += amount - soldiersAccountedFor;
            UpdateDisplays();
        } 
    }

    private void NewDay()
    {
        maxSoldiers = PlayManager.Instance.PlayCoordinator.GetArmySize();
        UpdateSoldiers();
    }

    private void UpdateDisplays()
    {
        text.text = $"{displaySoldiers}/{maxSoldiers}";
        slider.maxValue = maxSoldiers;
        slider.value = displaySoldiers;
    }
}
