using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day22;

public static class Day22
{
    private static readonly string[] Input = File.ReadAllLines("Day22/day22.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input) => new Solver(input).Solve1();

    public static int Solve2(string[] input) => new Solver(input).Solve2();

    private class Solver
    {
        private readonly string[] _input;
        private readonly Dictionary<(int X, int Y), int> _geologicIndexes = new();
        private readonly Dictionary<(int geologicIndex, int depth), int> _erosionLevels = new();

        public Solver(string[] input)
        {
            _input = input;
        }

        public int Solve1()
        {
            var depth = int.Parse(_input[0].Split(": ")[1]);
            var (targetX, targetY) = _input[1].Split(": ")[1].Split(",").Select(int.Parse).ToArray();
            var riskLevel = 0;

            for (var x = 0; x <= targetX; x++)
            {
                for (var y = 0; y <= targetY; y++)
                {
                    var geologicIndex = GetGeologicIndex(x, y, targetX, targetY, depth);
                    var erosionLevel = GetErosionLevel(geologicIndex, depth);
                    var type = GetRegionType(erosionLevel);

                    riskLevel += type;
                }
            }

            return riskLevel;
        }

        public int Solve2()
        {
            var depth = int.Parse(_input[0].Split(": ")[1]);
            var (targetX, targetY) = _input[1].Split(": ")[1].Split(",").Select(int.Parse).ToArray();

            var queue = new PriorityQueue<Node, int>();
            var visited = new Dictionary<Node, int>();

            var start = new Node(0, 0, 1);
            queue.Enqueue(start, 0);
            visited[start] = 0;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (node.X == targetX && node.Y == targetY && node.Tool == 1)
                {
                    return visited[node];
                }

                var geologicIndex = GetGeologicIndex(node.X, node.Y, targetX, targetY, depth);
                var erosionLevel = GetErosionLevel(geologicIndex, depth);
                var regionType = GetRegionType(erosionLevel);
                var tools = GetTools(regionType);

                foreach (var tool in tools)
                {
                    var cost = visited[node] + 1;

                    if (tool != node.Tool)
                    {
                        cost += 7;
                    }

                    var neighbors = new[]
                    {
                        (node.X - 1, node.Y),
                        (node.X + 1, node.Y),
                        (node.X, node.Y - 1),
                        (node.X, node.Y + 1)
                    };

                    foreach (var neighbor in neighbors.Where(n => IsValidNeighbor(n.Item1, n.Item2, targetX, targetY, depth, tool)))
                    {
                        var neighborNode = new Node(neighbor.Item1, neighbor.Item2, tool);

                        if (visited.TryGetValue(neighborNode, out var visitedCost) && visitedCost <= cost)
                        {
                            continue;
                        }

                        queue.Enqueue(neighborNode, cost);
                        visited[neighborNode] = cost;
                    }
                }
            }

            throw new Exception("No path found");
        }

        private bool IsValidNeighbor(int x, int y, int targetX, int targetY, int depth, int tool)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }

            var geologicIndex = GetGeologicIndex(x, y, targetX, targetY, depth);
            var erosionLevel = GetErosionLevel(geologicIndex, depth);
            var regionType = GetRegionType(erosionLevel);
            var tools = GetTools(regionType);

            return tools.Contains(tool);
        }

        private IEnumerable<int> GetTools(int regionType)
        {
            return regionType switch
            {
                0 => new[] { 1, 2 },
                1 => new[] { 0, 2 },
                2 => new[] { 0, 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(regionType))
            };
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

    private record Node(int X, int Y, int Tool);
}
