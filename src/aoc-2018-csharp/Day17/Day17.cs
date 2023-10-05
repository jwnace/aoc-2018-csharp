using System.Net;
using System.Runtime.Serialization;
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
        var water = new HashSet<(int X, int Y)>();
        var clay = GetClay(input);
        var seenDown = new HashSet<(int X, int Y)>();
        var seenLeft = new HashSet<(int X, int Y)>();
        var seenRight = new HashSet<(int X, int Y)>();

        var minX = clay.Min(x => x.X);
        var maxX = clay.Max(x => x.X);
        var minY = clay.Min(x => x.Y);
        var maxY = clay.Max(x => x.Y);

        Console.WriteLine($"x: {minX}..{maxX}, y: {minY}..{maxY}");
        Console.WriteLine(DrawGrid(clay, water));
        //Console.ReadLine();

        while (!water.Contains((500, 1)))
        {
            var drop = new Drop(500, 0);

            DoTheThing(drop, clay, water, seenDown, seenLeft, seenRight);
        }

        return 1;
    }

    private static void DoTheThing(
        Drop drop,
        IReadOnlySet<(int X, int Y)> clay,
        HashSet<(int X, int Y)> water,
        HashSet<(int X, int Y)> seenDown,
        HashSet<(int X, int Y)> seenLeft,
        HashSet<(int X, int Y)> seenRight)
    {
        while (drop.TryMoveDown(clay, water, seenDown, out var d))
        {
            drop = d;
        }

        if (!drop.TryMoveLeft(clay, water, seenLeft, out _) && !drop.TryMoveRight(clay, water, seenRight, out _))
        {
            water.Add((drop.X, drop.Y));

            Console.WriteLine(drop);
            Console.WriteLine(DrawGrid(clay, water));
            //Console.ReadLine();

            return;
        }

        var left = drop;

        while (left.TryMoveLeft(clay, water, seenLeft, out var l))
        {
            left = l;
        }

        if (left != drop)
        {
            if (seenLeft.Contains((left.X - 1, left.Y)))
            {
                seenLeft.Add((left.X, left.Y));
            }
            else
            {
                if (!left.TryMoveDown(clay, water, seenDown, out _))
                {
                    // if i can't go left anymore, and i can't go down, and i'm in a column that falls forever...
                    if (water.Contains((left.X + 1, left.Y + 1)) && !clay.Any(x => x.X == left.X && x.Y > left.Y))
                    {
                        seenLeft.Add((left.X, left.Y));
                        return;
                    }

                    water.Add((left.X, left.Y));

                    Console.WriteLine(left);
                    Console.WriteLine(DrawGrid(clay, water));
                    //Console.ReadLine();

                    return;
                }

                DoTheThing(left, clay, water, seenDown, seenLeft, seenRight);
            }
        }

        var right = drop;

        while (right.TryMoveRight(clay, water, seenRight, out var r))
        {
            right = r;
        }

        if (right != drop)
        {
            if (seenRight.Contains((right.X + 1, right.Y)))
            {
                seenRight.Add((right.X, right.Y));
                return;
            }

            if (!right.TryMoveDown(clay, water, seenDown, out _))
            {
                // if i can't go right anymore, and i can't go down, and i'm in a column that falls forever...
                if (water.Contains((right.X - 1, right.Y + 1)) && !clay.Any(x => x.X == right.X && x.Y > right.Y))
                {
                    seenRight.Add((right.X, right.Y));
                    return;
                }

                water.Add((right.X, right.Y));

                Console.WriteLine(right);
                Console.WriteLine(DrawGrid(clay, water));
                //Console.ReadLine();

                return;
            }

            DoTheThing(right, clay, water, seenDown, seenLeft, seenRight);
        }
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
