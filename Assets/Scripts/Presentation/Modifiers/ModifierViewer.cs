using System;
using Domain.Modifiers;
using Domain.Utility;
using Presentation;
using UnityEngine;

public class ModifierViewer : MonoBehaviour
{
    private IncentiveModifier _modifier;
    [SerializeField] private TMPro.TextMeshProUGUI _text;

    public void SetModifier(IncentiveModifier modifier)
    {
        _modifier = modifier;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = Descriptions.Get(_modifier);
    }
}
