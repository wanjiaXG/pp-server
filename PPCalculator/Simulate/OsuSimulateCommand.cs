// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;

namespace PPCalculator.Simulate
{
    [Command(Name = "osu", Description = "Computes the performance (pp) of a simulated osu! play.")]
    public class OsuSimulateCommand : SimulateCommand
    {
        [UsedImplicitly]
        [Required, FileExists]
        [Argument(0, Name = "beatmap", Description = "Required. The beatmap file (.osu).")]
        public override string Beatmap { get; set; }

        [UsedImplicitly]
        [Option(Template = "-a|--accuracy <accuracy>", Description = "Accuracy. Enter as decimal 0-100. Defaults to 100."
                                                                     + " Scales hit results as well and is rounded to the nearest possible value for the beatmap.")]
        public override double Accuracy { get; set; } = 100;

        [UsedImplicitly]
        [Option(Template = "-c|--combo <combo>", Description = "Maximum combo during play. Defaults to beatmap maximum.")]
        public override int? Combo { get; set; }

        [UsedImplicitly]
        [Option(Template = "-C|--percent-combo <combo>", Description = "Percentage of beatmap maximum combo achieved. Alternative to combo option."
                                                                       + " Enter as decimal 0-100.")]
        public override double PercentCombo { get; set; } = 100;

        [UsedImplicitly]
        [Option(CommandOptionType.MultipleValue, Template = "-m|--mod <mod>", Description = "One for each mod. The mods to compute the performance with."
                                                                                            + " Values: hr, dt, hd, fl, ez, etc...")]
        public override string[] Mods { get; set; }

        [UsedImplicitly]
        [Option(Template = "-X|--misses <misses>", Description = "Number of misses. Defaults to 0.")]
        public override int Misses { get; set; }

        [UsedImplicitly]
        [Option(Template = "-M|--mehs <mehs>", Description = "Number of mehs. Will override accuracy if used. Otherwise is automatically calculated.")]
        public override int? Mehs { get; set; }

        [UsedImplicitly]
        [Option(Template = "-G|--goods <goods>", Description = "Number of goods. Will override accuracy if used. Otherwise is automatically calculated.")]
        public override int? Goods { get; set; }

        public override Ruleset Ruleset => new OsuRuleset();

        protected override int GetMaxCombo(IBeatmap beatmap) => beatmap.HitObjects.Count + beatmap.HitObjects.OfType<Slider>().Sum(s => s.NestedHitObjects.Count - 1);

        protected override Dictionary<HitResult, int> GenerateHitResults(double accuracy, IBeatmap beatmap, int countMiss, int? countMeh, int? countGood)
        {
            int countGreat;

            var totalResultCount = beatmap.HitObjects.Count;

            if (countMeh != null || countGood != null)
            {
                countGreat = totalResultCount - (countGood ?? 0) - (countMeh ?? 0) - countMiss;
            }
            else
            {
                // Let Great=6, Good=2, Meh=1, Miss=0. The total should be this.
                var targetTotal = (int)Math.Round(accuracy * totalResultCount * 6);

                // Start by assuming every non miss is a meh
                // This is how much increase is needed by greats and goods
                var delta = targetTotal - (totalResultCount - countMiss);

                // Each great increases total by 5 (great-meh=5)
                countGreat = delta / 5;
                // Each good increases total by 1 (good-meh=1). Covers remaining difference.
                countGood = delta % 5;
                // Mehs are left over. Could be negative if impossible value of amountMiss chosen
                countMeh = totalResultCount - countGreat - countGood - countMiss;
            }

            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, countGreat },
                { HitResult.Good, countGood ?? 0 },
                { HitResult.Meh, countMeh ?? 0 },
                { HitResult.Miss, countMiss }
            };
        }

        protected override double GetAccuracy(Dictionary<HitResult, int> statistics)
        {
            var countGreat = statistics[HitResult.Great];
            var countGood = statistics[HitResult.Good];
            var countMeh = statistics[HitResult.Meh];
            var countMiss = statistics[HitResult.Miss];
            var total = countGreat + countGood + countMeh + countMiss;

            return (double)((6 * countGreat) + (2 * countGood) + countMeh) / (6 * total);
        }

        protected override void WritePlayInfo(ScoreInfo scoreInfo, IBeatmap beatmap)
        {
            WriteAttribute("Acc", (scoreInfo.Accuracy * 100).ToString(CultureInfo.InvariantCulture) + "%");
            WriteAttribute("Combo", FormattableString.Invariant($"{scoreInfo.MaxCombo} ({Math.Round(100.0 * scoreInfo.MaxCombo / GetMaxCombo(beatmap), 2)}%)"));

            foreach (var statistic in scoreInfo.Statistics)
            {
                WriteAttribute(Enum.GetName(typeof(HitResult), statistic.Key), statistic.Value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
