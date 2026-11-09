using System;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

public class SoldierBar : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    private int maxSoldiers = 1000;
    private int soldiers = 0;
    public int Soldiers
    {
        set
        {
            soldiers = value;
            text.text = $"{soldiers}/{maxSoldiers}";
            slider.maxValue = maxSoldiers;
            slider.value = maxSoldiers;
        }
    }

    private void Start()
    {
        Soldiers = maxSoldiers;
        PlayManager.Instance.PlayCoordinator.OnArmySizeChange += UpdateSoldiers;
        PlayManager.Instance.PlayCoordinator.OnNewDay += NewDay;
    }

    private void UpdateSoldiers()
    {
        Soldiers = maxSoldiers - PlayManager.Instance.PlayCoordinator.GetCommitedSoldiers();
    }

    private void NewDay()
    {
        maxSoldiers = PlayManager.Instance.PlayCoordinator.GetArmySize();
        UpdateSoldiers();
    }
}
