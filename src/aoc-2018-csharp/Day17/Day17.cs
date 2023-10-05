#define DEBUG
#undef DEBUG

using System.Text;
using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day17;

public static class Day17
{
    private static readonly string[] Input = File.ReadAllLines("Day17/day17.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input)
    {
        var grid = BuildGrid(input);

        var minX = grid.Min(g => g.Key.X);
        var maxX = grid.Max(g => g.Key.X);
        var minY = grid.Min(g => g.Key.Y);
        var maxY = grid.Max(g => g.Key.Y);

#if DEBUG
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"x: {minX}..{maxX}, y: {minY}..{maxY}");
        Console.WriteLine(DrawGrid(grid));
        //Console.ReadLine();
#endif

        while (!grid.ContainsKey((500, 1)))
        {
            Flow(new Drop(500, 0), grid);
        }

        Console.WriteLine(DrawGrid(grid));

        return grid.Count(x => x.Key.Y >= minY && x.Key.Y <= maxY && x.Value is '|' or '~');
    }

    private static void Flow(Drop drop, Dictionary<(int X, int Y), char> grid)
    {
        // move down as far as possible
        while (drop.TryMoveDown(grid, out var d))
        {
            drop = d;
        }

        // if we hit the bottom of the world, don't try to move left or right
        if (drop.Y >= grid.Keys.Max(x => x.Y))
        {
            AddDropToGrid(drop, grid);
            return;
        }

        var below = drop with { Y = drop.Y + 1 };

        // if we hit flowing water, don't try to move left or right
        if (grid.TryGetValue(below, out var v) && v == '|')
        {
            AddDropToGrid(drop, grid);
            return;
        }

        var canMoveLeft = drop.TryMoveLeft(grid, out _);
        var canMoveRight = drop.TryMoveRight(grid, out _);

        // if we can't move left or right, we're done
        if (!canMoveLeft && !canMoveRight)
        {
            AddDropToGrid(drop, grid);
            TryStabilizeRow(drop, grid); // determine if the water at this level is stable
            return;
        }

        if (canMoveLeft)
        {
            var left = drop;

            while (left.TryMoveLeft(grid, out var l))
            {
                left = l;
            }

            if (!left.TryMoveDown(grid, out _))
            {
                AddDropToGrid(left, grid);
                return;
            }

            Flow(left, grid);
            return;
        }

        var right = drop;

        while (right.TryMoveRight(grid, out var r))
        {
            right = r;
        }

        if (!right.TryMoveDown(grid, out _))
        {
            AddDropToGrid(right, grid);
            return;
        }

        Flow(right, grid);
    }

    private static bool TryStabilizeRow(Drop drop, Dictionary<(int X, int Y), char> grid)
    {
        // TODO: this logic will likely break if there are two falling columns of water adjacent to each other
        var left = drop;
        while (grid.ContainsKey(left))
        {
            left = left with { X = left.X - 1 };
        }

        var right = drop;
        while (grid.ContainsKey(right))
        {
            right = right with { X = right.X + 1 };
        }

        left = left with { X = left.X + 1 };
        right = right with { X = right.X - 1 };

        if (grid[left] == '#' && grid[right] == '#')
        {
            for (var i = left.X + 1; i < right.X; i++)
            {
                grid[(i, drop.Y)] = '~';
            }

#if DEBUG
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(drop);
            Console.WriteLine(DrawGrid(grid));
            //Console.ReadLine();
#endif

            return true;
        }

        return false;
    }

    private static void AddDropToGrid(Drop drop, Dictionary<(int X, int Y), char> grid)
    {
        // TODO: all water is flowing initially, but it may become stable later
        grid[drop] = '|';

        var count = grid.Count(x => x.Value is '|' or '~');

        if (count % 100 == 0 || count > 1500)
        {
            Console.WriteLine($"{count,6} water tiles");
        }

#if DEBUG
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(drop);
        Console.WriteLine(DrawGrid(grid));
        //Console.ReadLine();
#endif
    }

    private static string DrawGrid(IReadOnlyDictionary<(int X, int Y), char> grid)
    {
        var builder = new StringBuilder();
        var minX = grid.Min(g => g.Key.X);
        var maxX = grid.Max(g => g.Key.X);
        var minY = grid.Min(g => g.Key.Y);
        var maxY = grid.Max(g => g.Key.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                builder.Append(grid.TryGetValue((x, y), out var v) ? v : '.');
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    private static Dictionary<(int X, int Y), char> BuildGrid(IEnumerable<string> input)
    {
        var grid = new Dictionary<(int X, int Y), char>();

        foreach (var line in input)
        {
            var (left, right) = line.Split(", ");
            var (leftKey, leftValue) = left.Split("=");
            var (_, rightValue) = right.Split("=");
            var (rightValueStart, rightValueEnd) = rightValue.Split("..");

            if (leftKey == "x")
            {
                var x = int.Parse(leftValue);
                var y1 = int.Parse(rightValueStart);
                var y2 = int.Parse(rightValueEnd);

                for (var y = y1; y <= y2; y++)
                {
                    // if it doesn't exist, add it
                    if (!grid.TryGetValue((x, y), out var v))
                    {
                        grid.Add((x, y), '#');
                        continue;
                    }

                    // if it's already clay, skip it
                    if (v == '#')
                    {
                        continue;
                    }

                    // if it already exists and it's not clay, wtf?
                    throw new Exception("wtf");
                }
            }
            else
            {
                var y = int.Parse(leftValue);
                var x1 = int.Parse(rightValueStart);
                var x2 = int.Parse(rightValueEnd);

                for (var x = x1; x <= x2; x++)
                {
                    // if it doesn't exist, add it
                    if (!grid.TryGetValue((x, y), out var v))
                    {
                        grid.Add((x, y), '#');
                        continue;
                    }

                    // if it's already clay, skip it
                    if (v == '#')
                    {
                        continue;
                    }

                    // if it already exists and it's not clay, wtf?
                    throw new Exception("wtf");
                }
            }
        }

        return grid;
    }

    public static int Solve2(string[] input)
    {
        throw new NotImplementedException();
    }

    private static HashSet<(int X, int Y)> GetClay(IEnumerable<string> input)
    {
        var clay = new HashSet<(int X, int Y)>();

        foreach (var line in input)
        {
            var (left, right) = line.Split(", ");
            var (leftKey, leftValue) = left.Split("=");
            var (_, rightValue) = right.Split("=");
            var (rightValueStart, rightValueEnd) = rightValue.Split("..");

            if (leftKey == "x")
            {
                var x = int.Parse(leftValue);
                var y1 = int.Parse(rightValueStart);
                var y2 = int.Parse(rightValueEnd);

                for (var y = y1; y <= y2; y++)
                {
                    clay.Add((x, y));
                }
            }
            else
            {
                var y = int.Parse(leftValue);
                var x1 = int.Parse(rightValueStart);
                var x2 = int.Parse(rightValueEnd);

                for (var x = x1; x <= x2; x++)
                {
                    clay.Add((x, y));
                }
            }
        }

        return clay;
    }

    private static string DrawGrid(IReadOnlySet<(int X, int Y)> clay, IReadOnlySet<(int X, int Y)> water)
    {
        var minX = clay.Min(x => x.X);
        var maxX = clay.Max(x => x.X);
        var minY = clay.Min(x => x.Y);
        var maxY = clay.Max(x => x.Y);

        var builder = new StringBuilder();

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (clay.Contains((x, y)))
                {
                    builder.Append('#');
                }
                else if (water.Any(w => w.X == x && w.Y == y))
                {
                    builder.Append('~');
                }
                else
                {
                    builder.Append('.');
                }
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}
