namespace aoc_2018_csharp.Day10;

public static class Day10
{
    private static readonly string[] Input = File.ReadAllLines("Day10/day10.txt");

    public static int Part1()
    {
        foreach (var line in Input)
        {
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var pX = int.Parse(values[1][..^1]);
            var pY = int.Parse(values[2][..^1]);
            var vX = int.Parse(values[4][..^1]);
            var vY = int.Parse(values[5][..^1]);

            var position = new Vector(pX, pY);
            var velocity = new Vector(vX, vY);

            var point = new Point(position, velocity);
        }

        return 1;
    }

    public static int Part2() => 2;

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

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}
