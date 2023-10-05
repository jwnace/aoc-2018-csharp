using System.Net;
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

        Console.WriteLine();
        Console.WriteLine(DrawGrid(clay, water));
        // //Console.ReadLine();

        while (!water.Contains((500, 1)))
        {
            var drop = new Drop(500, 0);

            Fall(drop, clay, water);
        }

        return 1;
    }

    private static void Fall(Drop drop, HashSet<(int x, int y)> clay, HashSet<(int X, int Y)> water)
    {
        var canMoveDown = drop.TryMoveDown(clay, water, out var down);
        var canMoveLeft = drop.TryMoveLeft(clay, water, out var left);
        var canMoveRight = drop.TryMoveRight(clay, water, out var right);

        if (!canMoveDown && !canMoveLeft && !canMoveRight)
        {
            water.Add((drop.X, drop.Y));
            Console.WriteLine(drop);
            Console.WriteLine(DrawGrid(clay, water));
            //Console.ReadLine();
            return;
        }

        if (canMoveDown)
        {
            Fall(down, clay, water);
        }

        if (canMoveLeft)
        {
            while (left.TryMoveLeft(clay, water, out left))
            {
                if (left.TryMoveDown(clay, water, out left))
                {
                    Fall(left, clay, water);
                    return;
                }

                water.Add((left.X, left.Y - 1));
                Console.WriteLine(left);
                Console.WriteLine(DrawGrid(clay, water));
                //Console.ReadLine();
            }
        }

        if (canMoveRight)
        {
            while (right.TryMoveRight(clay, water, out right))
            {
                if (right.TryMoveDown(clay, water, out right))
                {
                    Fall(right, clay, water);
                    return;
                }

                water.Add((right.X, right.Y - 1));
                Console.WriteLine(right);
                Console.WriteLine(DrawGrid(clay, water));
                //Console.ReadLine();
            }
        }
    }

    // private static void Fall(Drop drop, HashSet<(int x, int y)> clay, HashSet<(int X, int Y)> water)
    // {
    //     // move down until we hit clay or water
    //     while (drop.TryMoveDown(clay, water, out var down))
    //     {
    //         drop = down;
    //     }
    //
    //     var left = drop;
    //
    //     // move left until we hit clay or there is no clay directly under us
    //     while (left.TryMoveLeft(clay, water, out left) &&
    //            (clay.Contains((left.X, left.Y + 1)) || water.Contains((left.X, left.Y + 1))))
    //     {
    //         // water.Add((left.X, left.Y));
    //         // Console.WriteLine(left);
    //         // Console.WriteLine(DrawGrid(clay, water));
    //         // //Console.ReadLine();
    //     }
    //
    //     // if there is no clay or water directly under us, fall
    //     if (!clay.Contains((left.X, left.Y + 1)) && !water.Contains((left.X, left.Y + 1)))
    //     {
    //         Fall(left, clay, water);
    //         return;
    //     }
    //
    //     var right = drop;
    //
    //     // move right until we hit clay or there is no clay directly under us
    //     while (right.TryMoveRight(clay, water, out right) &&
    //            (clay.Contains((right.X, right.Y + 1)) || water.Contains((right.X, right.Y + 1))))
    //     {
    //         // water.Add((right.X, right.Y));
    //         // Console.WriteLine(right);
    //         // Console.WriteLine(DrawGrid(clay, water));
    //         // //Console.ReadLine();
    //     }
    //
    //     // if there is no clay or water directly under us, fall
    //     if (!clay.Contains((right.X, right.Y + 1)) && !water.Contains((right.X, right.Y + 1)))
    //     {
    //         Fall(right, clay, water);
    //         return;
    //     }
    //
    //     water.Add((drop.X, drop.Y));
    //     Console.WriteLine(drop);
    //     Console.WriteLine(DrawGrid(clay, water));
    //     //Console.ReadLine();
    // }

    // public static int Solve1(string[] input)
    // {
    //     var water = new HashSet<(int X, int Y)>();
    //     var clay = GetClay(input);
    //
    //     Console.WriteLine(DrawGrid(clay, water));
    //     // Thread.Sleep(1000);
    //
    //     var drop = new Drop(500, 0);
    //     Fill(drop, clay, water);
    //
    //     return 1;
    // }

    private static void Fill(Drop drop, HashSet<(int x, int y)> clay, HashSet<(int X, int Y)> water)
    {
        var canMoveDown = drop.TryMoveDown(clay, water, out var down);
        var canMoveLeft = drop.TryMoveLeft(clay, water, out var left);
        var canMoveRight = drop.TryMoveRight(clay, water, out var right);

        if (!canMoveDown && !canMoveLeft && !canMoveRight)
        {
            return;
        }

        if (canMoveDown)
        {
            water.Add((down.X, down.Y));
            Console.WriteLine(drop.ToString());
            Console.WriteLine(DrawGrid(clay, water));
            //Console.ReadLine();

            Fill(down, clay, water);
        }

        // if there is no clay below me, don't bother moving left or right since I can fall forever
        if (clay.Any(c => c.x == drop.X && c.y > drop.Y))
        {
            if (canMoveLeft && !water.Contains((left.X, left.Y)))
            {
                water.Add((left.X, left.Y));
                Console.WriteLine(drop.ToString());
                Console.WriteLine(DrawGrid(clay, water));
                //Console.ReadLine();

                Fill(left, clay, water);
            }

            if (canMoveRight && !water.Contains((right.X, right.Y)))
            {
                water.Add((right.X, right.Y));
                Console.WriteLine(drop.ToString());
                Console.WriteLine(DrawGrid(clay, water));
                //Console.ReadLine();

                Fill(right, clay, water);
            }
        }

        // Console.WriteLine(drop.ToString());
        // Console.WriteLine(DrawGrid(clay, water));
        // // Thread.Sleep(2_000);
    }

    public static int Solve2(string[] input)
    {
        throw new NotImplementedException();
    }

    private static HashSet<(int x, int y)> GetClay(IEnumerable<string> input)
    {
        var clay = new HashSet<(int x, int y)>();

        foreach (var line in input)
        {
            var (left, right) = line.Split(", ");
            var (leftKey, leftValue) = left.Split("=");
            var (rightKey, rightValue) = right.Split("=");
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

    private static string DrawGrid(IReadOnlySet<(int x, int y)> clay, IReadOnlyCollection<(int X, int Y)> water)
    {
        var (minX, maxX, minY, maxY) = (clay.Min(x => x.x), clay.Max(x => x.x), clay.Min(x => x.y), clay.Max(x => x.y));

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

    private record Drop(int X, int Y)
    {
        public bool TryMoveDown(IReadOnlySet<(int x, int y)> clay, IReadOnlySet<(int X, int Y)> water, out Drop newDrop)
        {
            var (newX, newY) = (X, Y + 1);
            var newPosition = (newX, newY);
            newDrop = new Drop(newX, newY);

            return !clay.Contains(newPosition) &&
                   !water.Contains(newPosition) &&
                   newY <= clay.Max(x => x.y);
        }

        public bool TryMoveLeft(IReadOnlySet<(int x, int y)> clay, IReadOnlySet<(int X, int Y)> water, out Drop newDrop)
        {
            var (newX, newY) = (X - 1, Y);
            var newPosition = (newX, newY);
            newDrop = new Drop(newX, newY);

            return !clay.Contains(newPosition) &&
                   !water.Contains(newPosition) &&
                   (clay.Contains((newX, newY + 1)) || water.Contains((newX, newY+1))) &&
                   newY < clay.Max(x => x.y);
        }

        public bool TryMoveRight(IReadOnlySet<(int x, int y)> clay, IReadOnlySet<(int X, int Y)> water, out Drop newDrop)
        {
            var (newX, newY) = (X + 1, Y);
            var newPosition = (newX, newY);
            newDrop = new Drop(newX, newY);

            return !clay.Contains(newPosition) &&
                   !water.Contains(newPosition) &&
                   (clay.Contains((newX, newY + 1)) || water.Contains((newX, newY+1))) &&
                   newY < clay.Max(x => x.y);
        }
    }
}
