using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Modifiers;
using Domain.Quests;
using Presentation;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    [SerializeField] private GameObject modifierPresenter;
    [SerializeField] private Vector2 spaceBetweenModifiers = new Vector2(0, 50);
    [SerializeField] private Transform modifierPresenterParent;
    
    private Dictionary<IncentiveModifier, GameObject> activeModifiers = new();
    
    private void Start()
    {
        PlayManager.Instance.PlayCoordinator.OnModifiersChanged += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        var newmodifiers = PlayManager.Instance.PlayCoordinator.GetActiveModifiers().ToList();
        var allmodifiers = newmodifiers.Union(activeModifiers.Keys).ToList();
        foreach (var modifier in allmodifiers)
        {
            if (!newmodifiers.Contains(modifier) && activeModifiers.ContainsKey(modifier))
            {
                activeModifiers[modifier].GetComponent<FlipSprite>().FlipDestroy();
                activeModifiers.Remove(modifier);
                continue;
            }

            if (newmodifiers.Contains(modifier) && !activeModifiers.ContainsKey(modifier))
            {
                activeModifiers[modifier] = Instantiate(modifierPresenter, modifierPresenterParent);
            }
            
            activeModifiers[modifier].GetComponent<ModifierViewer>().SetModifier(modifier);
        }
        
        var orderedIndicators = activeModifiers
            .OrderBy(q => q.Key.ToAffect[0])
            .Select(q => q.Value)
            .ToList();
        
        for (int i = 0;  i < orderedIndicators.Count; i++)
        {
            var dest = i * spaceBetweenModifiers;
            orderedIndicators[i]
                .GetComponent<MoveTo2D>()
                .SetTarget(modifierPresenterParent.position + new Vector3(dest.x, dest.y, 0));
        }
    }
}