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
                Attribute.Lives => change.ChangeType switch
                {
                    ChangeType.Value => FormatLives(change.Value),
                    ChangeType.Percentage => FormatLives(change.Value, true),
                    _ => FormatLives(change.Value),
                },
                Attribute.ArmySize => change.ChangeType switch
                {
                    ChangeType.Value => FormatSoldiers(change.Value),
                    ChangeType.Percentage => FormatSoldiers(change.Value, true),
                    _ => FormatSoldiers(change.Value),
                },
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

            return $"{timing}: {attribute}";
        }

        private static string FormatDuration(Duration duration)
        {
            return duration switch
            {
                LimitedDuration limited =>
                    $"Lasts {FormatDays(limited.TurnsLeft)}",
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

        public static string FormatSoldiers(int amount, bool percent = false)
        {
            var perc = percent ? "%" : string.Empty;
            return FormatColor(
                Color.blue,
                $"{amount}{perc} \ue803"
            );
        }

        public static string FormatLives(int amount, bool percent = false)
        {
            var perc = percent ? "%" : string.Empty;
            string s = "";
            if (amount == 1 && !percent)
                s = "\ue806";
            else if (amount == -1  && !percent)
                s = "-\ue806";
            else 
                s = $"{amount}{perc} \ue806";

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