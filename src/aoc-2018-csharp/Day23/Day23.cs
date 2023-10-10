using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day23;

public static class Day23
{
    private static readonly string[] Input = File.ReadAllLines("Day23/day23.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input)
    {
        var nanobots = input.Select(Nanobot.Parse).ToList();

        var strongest = nanobots.MaxBy(x => x.Radius)!;

        return strongest.CountInRange(nanobots);
    }

    public static int Solve2(string[] input) => throw new NotImplementedException();

    private record Nanobot(int X, int Y, int Z, int Radius)
    {
        public static Nanobot Parse(string line)
        {
            var parts = line.Split('<', ',', '>', '=').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToArray();
            var (x, y, z, radius) = parts;

            return new Nanobot(x, y, z, radius);
        }

        public int CountInRange(IEnumerable<Nanobot> nanobots) => nanobots.Count(InRange);

        private bool InRange(Nanobot other)
        {
            var distance = GetDistance(other);
            return distance <= Radius;
        }

        private int GetDistance(Nanobot other)
        {
            // get the manhattan distance between the two bots
            var distance = Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
            return distance;
        }
    }
}
