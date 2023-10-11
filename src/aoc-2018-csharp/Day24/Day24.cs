using System.Text.RegularExpressions;

namespace aoc_2018_csharp.Day24;

public static partial class Day24
{
    private static readonly string Input = File.ReadAllText("Day24/day24.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string input)
    {
        var sections = input.Split("\n\n");
        var immuneSystem = sections[0].Split("\n").Skip(1).Select(Group.Parse).ToList();
        var infection = sections[1].Split("\n").Skip(1).Select(Group.Parse).ToList();

        while (immuneSystem.Any(g => g.Units > 0) && infection.Any(g => g.Units > 0))
        {
            var allGroups = immuneSystem.Union(infection)
                .OrderByDescending(g => g.EffectivePower)
                .ThenByDescending(g => g.Initiative)
                .ToList();

            var targets = new Dictionary<Group, Group>();

            foreach (var group in allGroups)
            {
                var potentialTargets = immuneSystem.Contains(group) ? infection : immuneSystem;

                var target = potentialTargets
                    // TODO: I don't think the `AttackType` is relevant here...
                    // .Where(g => g.Units > 0 && g.AttackType != group.AttackType && !targets.ContainsValue(g))
                    .Where(g => g.Units > 0)
                    .Where(g => !targets.ContainsValue(g))
                    .OrderByDescending(g => group.DamageTo(g))
                    .ThenByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.Initiative)
                    .FirstOrDefault();

                // TODO: do I need the `group.DamageTo(target) == 0` check?
                if (target is null || group.DamageTo(target) == 0)
                {
                    continue;
                }

                targets.Add(group, target);
            }

            foreach (var attacker in allGroups.OrderByDescending(g => g.Initiative))
            {
                // TODO: do I need the `attacker.Units > 0` check?
                if (attacker.Units > 0 && targets.TryGetValue(attacker, out var target))
                {
                    var damage = attacker.DamageTo(target);
                    var unitsKilled = Math.Min(damage / target.HitPoints, target.Units);
                    target.Units -= unitsKilled;

                    if (target.Units <= 0)
                    {
                        // we're not sure which group the target is so just try to remove it from both
                        immuneSystem.Remove(target);
                        infection.Remove(target);
                    }
                }
            }
        }

        return immuneSystem.Any(g => g.Units > 0)
            ? immuneSystem.Sum(g => g.Units)
            : infection.Sum(g => g.Units);
    }

    public static int Solve2(string input)
    {
        throw new NotImplementedException();
    }

    private partial class Group
    {
        private Group(int units,
            int hitPoints,
            int attackDamage,
            string attackType,
            int initiative,
            string[] weaknesses,
            string[] immunities)
        {
            Units = units;
            HitPoints = hitPoints;
            AttackDamage = attackDamage;
            AttackType = attackType;
            Initiative = initiative;
            Weaknesses = weaknesses;
            Immunities = immunities;
        }

        public static Group Parse(string line)
        {
            var match = GroupRegex().Match(line);
            var units = int.Parse(match.Groups["units"].Value);
            var hitPoints = int.Parse(match.Groups["hitPoints"].Value);
            var attackDamage = int.Parse(match.Groups["attackDamage"].Value);
            var attackType = match.Groups["attackType"].Value;
            var initiative = int.Parse(match.Groups["initiative"].Value);
            var modifiers = match.Groups["modifiers"].Value;
            var weaknesses = Array.Empty<string>();
            var immunities = Array.Empty<string>();

            if (!string.IsNullOrEmpty(modifiers))
            {
                var modifierMatches = ModifiersRegex().Matches(modifiers);

                foreach (Match modifierMatch in modifierMatches)
                {
                    var type = modifierMatch.Groups["type"].Value;
                    var list = modifierMatch.Groups["list"].Value;

                    if (type == "weak")
                    {
                        weaknesses = list.Split(", ");
                    }
                    else if (type == "immune")
                    {
                        immunities = list.Split(", ");
                    }
                }
            }

            return new Group(units, hitPoints, attackDamage, attackType, initiative, weaknesses, immunities);
        }

        public int EffectivePower => Units * AttackDamage;
        public int Units { get; set; }
        public int HitPoints { get; }
        public int AttackDamage { get; }
        public string AttackType { get; }
        public int Initiative { get; }
        public string[] Weaknesses { get; }
        public string[] Immunities { get; }

        public override string ToString()
        {
            var weaknesses = Weaknesses.Length > 0 ? $"weak to {string.Join(", ", Weaknesses)}" : "";
            var immunities = Immunities.Length > 0 ? $"immune to {string.Join(", ", Immunities)}" : "";
            var modifiers = string.Join("; ", new[] { weaknesses, immunities }.Where(s => !string.IsNullOrEmpty(s)));
            return
                $"{Units} units each with {HitPoints} hit points {modifiers}with an attack that does {AttackDamage} {AttackType} damage at initiative {Initiative}";
        }

        [GeneratedRegex(
            @"(?<units>\d+) units each with (?<hitPoints>\d+) hit points (\((?<modifiers>.*)\) )?with an attack that does (?<attackDamage>\d+) (?<attackType>\w+) damage at initiative (?<initiative>\d+)")]
        private static partial Regex GroupRegex();

        [GeneratedRegex(@"(?<type>\w+) to (?<list>[^;]+)")]
        private static partial Regex ModifiersRegex();

        public void Deconstruct(out int units, out int hitPoints, out int attackDamage, out string attackType,
            out int initiative, out string[] weaknesses, out string[] immunities)
        {
            units = Units;
            hitPoints = HitPoints;
            attackDamage = AttackDamage;
            attackType = AttackType;
            initiative = Initiative;
            weaknesses = Weaknesses;
            immunities = Immunities;
        }

        public int DamageTo(Group other)
        {
            if (other.Immunities.Contains(AttackType))
            {
                return 0;
            }

            if (other.Weaknesses.Contains(AttackType))
            {
                return EffectivePower * 2;
            }

            return EffectivePower;
        }
    }
}
