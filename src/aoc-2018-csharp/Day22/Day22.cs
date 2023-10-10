using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day22;

public static class Day22
{
    private static readonly string[] Input = File.ReadAllLines("Day22/day22.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => throw new NotImplementedException();

    public static int Solve1(string[] input) => new Solver(input).Solve();

    private class Solver
    {
        private readonly string[] _input;
        private readonly Dictionary<(int X, int Y), int> _geologicIndexes = new();
        private readonly Dictionary<(int geologicIndex, int depth), int> _erosionLevels = new();

        public Solver(string[] input)
        {
            _input = input;
        }

        public int Solve()
        {
            var depth = int.Parse(_input[0].Split(": ")[1]);
            var (targetX, targetY) = _input[1].Split(": ")[1].Split(",").Select(int.Parse).ToArray();
            var riskLevel = 0;

            for (var x = 0; x <= targetX; x++)
            {
                for (var y = 0; y <= targetY; y++)
                {
                    Console.WriteLine($"current position: ({x}, {y})");

                    var geologicIndex = GetGeologicIndex(x, y, targetX, targetY, depth);
                    var erosionLevel = GetErosionLevel(geologicIndex, depth);
                    var type = GetRegionType(erosionLevel);

                    riskLevel += type;
                }
            }

            return riskLevel;
        }

        private int GetRegionType(int erosionLevel)
        {
            return erosionLevel % 3;
        }

        private int GetErosionLevel(int geologicIndex, int depth)
        {
            if (_erosionLevels.TryGetValue((geologicIndex, depth), out var value))
            {
                return value;
            }

            value = (geologicIndex + depth) % 20183;
            _erosionLevels[(geologicIndex, depth)] = value;

            return value;
        }

        private int GetGeologicIndex(int x, int y, int targetX, int targetY, int depth)
        {
            if (_geologicIndexes.TryGetValue((x, y), out var value))
            {
                return value;
            }

            if ((x, y) == (0, 0))
            {
                value = 0;
            }
            else if ((x, y) == (targetX, targetY))
            {
                value = 0;
            }
            else if (y == 0)
            {
                value = x * 16807;
            }
            else if (x == 0)
            {
                value = y * 48271;
            }
            else
            {
                var g1 = GetGeologicIndex(x - 1, y, targetX, targetY, depth);
                var g2 = GetGeologicIndex(x, y - 1, targetX, targetY, depth);

                value = GetErosionLevel(g1, depth) * GetErosionLevel(g2, depth);
            }

            _geologicIndexes[(x, y)] = value;
            return value;
        }
    }
}
