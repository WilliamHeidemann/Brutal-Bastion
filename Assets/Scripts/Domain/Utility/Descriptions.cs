using System.Collections.Generic;
using System.Linq;
using Domain.Modifiers;
using UnityEngine;

namespace Domain.Utility
{
    public static class Descriptions
    {
        public static string Get(IncentiveModifier modifier)
        {
            if (modifier == null)
            {
                return "Apply a temporary quest modifier.";
            }

            string regions = modifier.ToAffect.Format();
            string effects = FormatEffects(modifier.Changes);
            string duration = FormatDuration(modifier.Duration);

            return $"For {regions} quests, {effects} {duration}";
        }
        
        private static string FormatEffects(List<ValueChange> changes)
        {
            if (changes == null || changes.Count == 0)
            {
                return "quest outcomes are modified.";
            }

            return string.Join("; ", changes.Select(FormatChange)) + ".";
        }

        private static string FormatChange(ValueChange change)
        {
            string timing = change.TimeOfEffect switch
            {
                TimeOfEffect.OnReward => "on reward",
                TimeOfEffect.OnPenalty => "on penalty",
                _ => "at an event"
            };

            string attribute = change.Attribute switch
            {
                Attribute.Lives => "lives",
                Attribute.ArmySize => "army size",
                Attribute.TurnRequirement => "turn requirement",
                _ => "value"
            };

            string amount = change.ChangeType switch
            {
                ChangeType.Value => $"{FormatSigned(change.Value)}",
                ChangeType.Percentage when change.Value != 0 => $"{FormatSigned(change.Value):0.#}%",
                ChangeType.Percentage => "0%",
                _ => "0"
            };

            return $"{timing}: {attribute} {amount}";
        }

        private static string FormatDuration(Duration duration)
        {
            return duration switch
            {
                LimitedDuration limited =>
                    $"Lasts {limited.TurnsLeft} turn{(limited.TurnsLeft == 1 ? string.Empty : "s")}.",
                InfiniteDuration => "Lasts indefinitely.",
                _ => "Lasts for an unknown duration."
            };
        }

        private static string FormatSigned(float value)
        {
            return value > 0 ? $"+{value:0.#}" : $"{value:0.#}";
        }

        private static string FormatColor(Color color, string text)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }

        public static string FormatSoldiers(int amount)
        {
            return FormatColor(
                Color.blue,
                $"{amount} \ue803"
            );
        }

        public static string FormatLives(int amount)
        {
            string s = "";
            if (amount == 1)
                s = "\ue806";
            else if (amount == -1)
                s = "-\ue806";
            else 
                s = $"{amount} \ue806";

            return FormatColor(
                Color.darkRed,
                s
            );
        }

        public static string FormatDays(int amount)
        {
            return FormatColor(
                Color.blueViolet,
                $"{amount} \ue800"
            );
        }

        public static string ModifierIcon()
        {
            return FormatColor(
                Color.chartreuse,
                "\ueF0D0"
            );
        }
    }
}