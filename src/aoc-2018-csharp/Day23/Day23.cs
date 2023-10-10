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

    public static int Solve2(string[] input) => new Solver(input).SolvePart2();

    private record Nanobot(int X, int Y, int Z, int Radius)
    {
        public static Nanobot Parse(string line)
        {
            var parts = line.Split('<', ',', '>', '=').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToArray();
            var (x, y, z, radius) = parts;

            return new Nanobot(x, y, z, radius);
        }

        public int CountInRange(IEnumerable<Nanobot> nanobots) => nanobots.Count(InRange);

        public bool InRange(Nanobot other)
        {
            var distance = GetDistance(other);
            return distance <= Radius;
        }

        public int GetDistance(Nanobot other)
        {
            // get the manhattan distance between the two bots
            var distance = Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
            return distance;
        }
    }

    private class Solver
    {
        private readonly string[] _input;

        public Solver(string[] input)
        {
            _input = input;
        }

        public int SolvePart2()
        {
            (int x, int y, int z) bestLocation = (0, 0, 0);
            int inRange = 0, maxInRange = 0, bestSum = 0;
            (int minX, int minY, int minZ, int maxX, int maxY, int maxZ) limits;
            int grain = (int)Math.Pow(2, 26);

            var bots = _input.Select(Bot.Parse);

            Bot biggestBot = bots.OrderByDescending(b => b.Radius).FirstOrDefault();
            limits = (bots.Min(bot => bot.Location.X), bots.Min(bot => bot.Location.Y), bots.Min(bot => bot.Location.Z),
                bots.Max(bot => bot.Location.X), bots.Max(bot => bot.Location.Y), bots.Max(bot => bot.Location.Z));
            int xRange = limits.maxX - limits.minX,
                yRange = limits.maxY - limits.minY,
                zRange = limits.maxZ - limits.minZ;

            int inRangeOfBiggest = bots.Count(bot => biggestBot.InRange(bot));

            if (true)
                do
                {
                    maxInRange = 0;
                    bestSum = int.MaxValue;
                    for (int x = limits.minX; x < limits.maxX; x += grain)
                    for (int y = limits.minY; y < limits.maxY; y += grain)
                    for (int z = limits.minZ; z < limits.maxZ; z += grain)
                        if ((inRange = bots.Count(bot => bot.InRange(x, y, z))) > maxInRange || inRange == maxInRange &&
                            Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < bestSum)
                        {
                            maxInRange = inRange;
                            bestLocation = (x, y, z);
                            bestSum = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                        }

                    //Debug.Print("Grain {0}: Location: {1}, {2}, {3} InRange: {4} Sum: {5}", grain, bestLocation.x, bestLocation.y, bestLocation.z, maxInRange, bestSum);
                    grain /= 2;
                    xRange /= 2;
                    yRange /= 2;
                    zRange /= 2;
                    limits = (bestLocation.x - xRange / 2, bestLocation.y - yRange / 2, bestLocation.z - zRange / 2,
                        bestLocation.x + xRange / 2, bestLocation.y + yRange / 2, bestLocation.z + zRange / 2);
                } while (grain >= 1);

            return bestSum;
        }

        private class Bot
        {
            public (int X, int Y, int Z) Location { get; private set; }
            public int Radius { get; private set; }

            public static Bot Parse(string line)
            {
                var parts = line.Split('<', ',', '>', '=').Where(l => int.TryParse(l, out _)).Select(int.Parse).ToArray();
                var (x, y, z, radius) = parts;

                return new Bot((x, y, z), radius);
            }

            public Bot((int x, int y, int z) location, int radius)
            {
                Location = location;
                Radius = radius;
            }

            public bool InRange(int x, int y, int z) => Distance(x, y, z) <= Radius;
            public bool InRange(Bot otherBot) => InRange(otherBot.Location.X, otherBot.Location.Y, otherBot.Location.Z);

            public int Distance(int x, int y, int z) =>
                Math.Abs(Location.X - x) + Math.Abs(Location.Y - y) + Math.Abs(Location.Z - z);

            public int Distance(Bot otherBot) =>
                Distance(otherBot.Location.X, otherBot.Location.Y, otherBot.Location.Z);
        }
    }
}
