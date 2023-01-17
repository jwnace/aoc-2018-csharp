using System.Text;

namespace aoc_2018_csharp.Day10;

public static class Day10
{
    private static readonly string[] Input = File.ReadAllLines("Day10/day10.txt");

    public static string Part1() => Solve().Message;

    public static int Part2() => Solve().Time;

    private static (string Message, int Time) Solve()
    {
        var points = GetPoints();

        for (var i = 1; i < int.MaxValue; i++)
        {
            foreach (var point in points)
            {
                point.Position.X += point.Velocity.X;
                point.Position.Y += point.Velocity.Y;
            }

            var minY = points.Min(p => p.Position.Y);
            var maxY = points.Max(p => p.Position.Y);

            if (maxY - minY < 10)
            {
                return (DrawGrid(points), i);
            }
        }

        return ("", 0);
    }

    private static List<Point> GetPoints()
    {
        var points = new List<Point>();

        foreach (var line in Input)
        {
            var values = line.Split('<', '>').Select(x => x.Split(',')).ToArray();

            var pX = int.Parse(values[1][0]);
            var pY = int.Parse(values[1][1]);
            var vX = int.Parse(values[3][0]);
            var vY = int.Parse(values[3][1]);

            var position = new Vector(pX, pY);
            var velocity = new Vector(vX, vY);

            points.Add(new Point(position, velocity));
        }

        return points;
    }

    private static string DrawGrid(List<Point> points)
    {
        var minX = points.Min(p => p.Position.X);
        var maxX = points.Max(p => p.Position.X);
        var minY = points.Min(p => p.Position.Y);
        var maxY = points.Max(p => p.Position.Y);

        if (maxY - minY > 9)
        {
            return "";
        }

        var builder = new StringBuilder();

        for (var y = minY; y <= maxY; y++)
        {
            builder.Append(Environment.NewLine);

            for (var x = minX; x <= maxX; x++)
            {
                builder.Append(points.Any(p => p.Position.X == x && p.Position.Y == y) ? '#' : ' ');
            }
        }

        return builder.ToString();
    }

    private record Point(Vector Position, Vector Velocity);

    private class Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
