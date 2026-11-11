using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Domain
{
    public enum Region
    {
        Orchid,
        Ashmound,
        Argot,
        Barrow,
        All,
    }

    public static class RegionExtensions
    {
        public static string Name(this Region region) => region switch
        {
            Region.Orchid => "Heaven's Orchid",
            Region.Ashmound => "The Ashmound",
            Region.Argot => "Argot",
            Region.Barrow => "Barrow",
            _ => "All",
        };

        public static Color Color(this Region region) => region switch
        {
            Region.Orchid => new Color(1, 0.867f, 0.294f),
            Region.Ashmound => new Color(1, 0.184f, 0.231f),
            Region.Argot => new Color(0.263f, 1, 0.337f),
            Region.Barrow => new Color(0.247f, 0.973f, 1),
            _ => new Color(1, 1, 1)
        };

        public static string Format(this Region region) => $"<color=#{ColorUtility.ToHtmlStringRGB(region.Color())}>{region.Name()}</color>";

        public static string Format(this IEnumerable<Region> regions)
        {
            if (regions == null || regions.Count() == 0 || regions.Count() == Enum.GetValues(typeof(Region)).Length)
            {
                return Region.All.Format();
            }

            if (regions.Count() == 1)
                return regions.First().Format();

            var ordered = regions.OrderBy(region => region);

            string formatted = string.Join(", ", ordered.SkipLast(1).Select(r => r.Format()));
            formatted += $" and {ordered.Last().Format()}";
            return formatted;
        }

        public static Region Resolve(this Region region)
        {
            List<Region> standardRegions = new List<Region>()
            {
                Region.Orchid,
                Region.Ashmound,
                Region.Argot,
                Region.Barrow,
            };

            return region switch
            {
                Region.All => standardRegions.ElementAt(Random.Range(0, standardRegions.Count)),
                _ => region,
            };
        }
    }
}