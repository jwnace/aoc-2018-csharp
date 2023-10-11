namespace aoc_2018_csharp.Day23;

public static class Day23
{
    private static readonly string[] Input = File.ReadAllLines("Day23/day23.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(IEnumerable<string> input)
    {
        var nanobots = input.Select(Nanobot.Parse).ToList();
        var strongest = nanobots.MaxBy(x => x.Radius)!;

        return nanobots.Count(strongest.IsInRange);
    }

    public static int Solve2(IEnumerable<string> input)
    {
        var nanobots = input.Select(Nanobot.Parse).ToList();

        var minX = nanobots.Min(bot => bot.X);
        var maxX = nanobots.Max(bot => bot.X);
        var minY = nanobots.Min(bot => bot.Y);
        var maxY = nanobots.Max(bot => bot.Y);
        var minZ = nanobots.Min(bot => bot.Z);
        var maxZ = nanobots.Max(bot => bot.Z);

        var dx = maxX - minX;
        var dy = maxY - minY;
        var dz = maxZ - minZ;

        var best = (X: 0, Y: 0, Z: 0);
        var precision = dx / 8;

        while (precision >= 1)
        {
            var max = 0;

            for (var x = minX; x < maxX; x += precision)
            {
                for (var y = minY; y < maxY; y += precision)
                {
                    for (var z = minZ; z < maxZ; z += precision)
                    {
                        var count = nanobots.Count(bot => bot.IsInRange(x, y, z));

                        if (count <= max)
                        {
                            continue;
                        }

                        max = count;
                        best = (x, y, z);
                    }
                }
            }

            precision /= 2;

            dx /= 2;
            dy /= 2;
            dz /= 2;

            minX = best.X - dx / 2;
            minY = best.Y - dy / 2;
            minZ = best.Z - dz / 2;
            maxX = best.X + dx / 2;
            maxY = best.Y + dy / 2;
            maxZ = best.Z + dz / 2;
        }

        return best.X + best.Y + best.Z;
    }
}
