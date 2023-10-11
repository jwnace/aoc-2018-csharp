using System.Text.RegularExpressions;

namespace aoc_2018_csharp.Day24;

public partial class Group
{
    public int EffectivePower => Units * (AttackDamage + Boost);
    public int Units { get; set; }
    public int HitPoints { get; }
    public int AttackDamage { get; }
    public string AttackType { get; }
    public int Initiative { get; }
    public string[] Weaknesses { get; }
    public string[] Immunities { get; }
    public int Boost { get; set; }

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

    private Group(
        int units,
        int hitPoints,
        int attackDamage,
        string attackType,
        int initiative,
        string[] weaknesses,
        string[] immunities,
        int boost = 0)
    {
        Units = units;
        HitPoints = hitPoints;
        AttackDamage = attackDamage;
        AttackType = attackType;
        Initiative = initiative;
        Weaknesses = weaknesses;
        Immunities = immunities;
        Boost = boost;
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

    [GeneratedRegex(@"(?<units>\d+) units each with (?<hitPoints>\d+) hit points (\((?<modifiers>.*)\) )?with an attack that does (?<attackDamage>\d+) (?<attackType>\w+) damage at initiative (?<initiative>\d+)")]
    private static partial Regex GroupRegex();

    [GeneratedRegex(@"(?<type>\w+) to (?<list>[^;]+)")]
    private static partial Regex ModifiersRegex();
}
