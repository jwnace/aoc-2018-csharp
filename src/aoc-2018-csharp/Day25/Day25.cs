using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day25;

public static class Day25
{
    private static readonly string[] Input = File.ReadAllLines("Day25/day25.txt");

    public static int Part1() => Solve1(Input);

    public static string Part2() => "Merry Christmas!";

    public static int Solve1(string[] input)
    {
        var constellations = new List<Constellation>();

        foreach (var line in input)
        {
            var (x, y, z, t) = line.Split(',').Select(int.Parse).ToArray();
            var point = new Point(x, y, z, t);
            var set = new HashSet<Point> { point };
            var constellation = new Constellation(set);

            constellations.Add(constellation);
        }

        var shouldContinue = true;

        while(shouldContinue)
        {
            shouldContinue = false;

            for (var i = 0; i < constellations.Count - 1; i++)
            {
                for (var j = i + 1; j < constellations.Count; j++)
                {
                    var constellation1 = constellations[i];
                    var constellation2 = constellations[j];

                    if (constellation1.Points.Any(p => constellation2.Points.Any(p.IsInRange)))
                    {
                        constellation1.Points.UnionWith(constellation2.Points);
                        constellations.RemoveAt(j);
                        j--;
                        shouldContinue = true;
                    }
                }
            }
        }

        return constellations.Count;
    }

    private record Constellation(HashSet<Point> Points);

    private record Point(int X, int Y, int Z, int T)
    {
        public bool IsInRange(Point other)
        {
            var distance = Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z) + Math.Abs(T - other.T);
            return distance <= 3;
        }
    }
}
