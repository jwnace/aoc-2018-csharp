namespace aoc_2018_csharp.Day24;

public static class Day24
{
    private static readonly string Input = File.ReadAllText("Day24/day24.txt").Trim();

    public static int Part1() => Solve(Input);

    public static int Part2() => Solve(Input, int.MaxValue);

    public static int Solve(string input, int maxBoost = 0)
    {
        List<Group> immuneSystem = null!;
        List<Group> infection = null!;

        for (var boost = 0; boost <= maxBoost; boost++)
        {
            immuneSystem = GetImmuneSystem(input, boost);
            infection = GetInfection(input);

            var round = 0;

            while (immuneSystem.Any(g => g.Units > 0) && infection.Any(g => g.Units > 0))
            {
                round++;

                if (round > 10_000)
                {
                    break;
                }

                var allGroups = immuneSystem.Union(infection)
                    .OrderByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.Initiative)
                    .ToList();

                var targets = ProcessTargetSelection(allGroups, immuneSystem, infection);

                ProcessAttacks(allGroups, targets, immuneSystem, infection);
            }

            if (immuneSystem.Any(g => g.Units > 0) && infection.Any(g => g.Units > 0))
            {
                continue;
            }

            if (immuneSystem.Any(g => g.Units > 0))
            {
                break;
            }
        }

        return immuneSystem.Any(g => g.Units > 0)
            ? immuneSystem.Sum(g => g.Units)
            : infection.Sum(g => g.Units);
    }

    private static List<Group> GetInfection(string input)
    {
        return input.Split("\n\n")[1].Split("\n").Skip(1).Select(Group.Parse).ToList();
    }

    private static List<Group> GetImmuneSystem(string input, int boost)
    {
        var immuneSystem = input.Split("\n\n")[0].Split("\n").Skip(1).Select(Group.Parse).ToList();
        immuneSystem.ForEach(g => g.Boost = boost);

        return immuneSystem;
    }

    private static Dictionary<Group, Group> ProcessTargetSelection(
        List<Group> allGroups,
        List<Group> immuneSystem,
        List<Group> infection)
    {
        var targets = new Dictionary<Group, Group>();

        foreach (var group in allGroups)
        {
            var potentialTargets = immuneSystem.Contains(group) ? infection : immuneSystem;

            var target = potentialTargets
                .Where(g => !targets.ContainsValue(g))
                .OrderByDescending(g => group.DamageTo(g))
                .ThenByDescending(g => g.EffectivePower)
                .ThenByDescending(g => g.Initiative)
                .FirstOrDefault();

            if (target is null || group.DamageTo(target) == 0)
            {
                continue;
            }

            targets.Add(group, target);
        }

        return targets;
    }

    private static void ProcessAttacks(
        List<Group> allGroups,
        Dictionary<Group, Group> targets,
        List<Group> immuneSystem,
        List<Group> infection)
    {
        foreach (var attacker in allGroups.OrderByDescending(g => g.Initiative))
        {
            if (!targets.TryGetValue(attacker, out var target))
            {
                continue;
            }

            var damage = attacker.DamageTo(target);
            var unitsKilled = Math.Min(damage / target.HitPoints, target.Units);
            target.Units -= unitsKilled;

            if (target.Units > 0)
            {
                continue;
            }

            immuneSystem.Remove(target);
            infection.Remove(target);
        }
    }
}
