namespace aoc_2018_csharp.Day06;

public static class Day06
{
    private static readonly string[] Input = File.ReadAllLines("Day06/day06.txt");

    public static int Part1()
    {
        var coordinates = GetCoordinates();
        var best = 0;

        foreach (var coordinate in coordinates)
        {
            var area1 = GetArea(coordinate, coordinates, 0);
            var area2 = GetArea(coordinate, coordinates, 1);

            if (area1 == -1 || area1 != area2)
            {
                continue;
            }

            best = Math.Max(best, area1);
        }

        return best;
    }

    public static int Part2()
    {
        var coordinates = GetCoordinates();
        var region = GetSafeRegion(coordinates);

        return region.Count;
    }

    private static List<Coordinate> GetSafeRegion(List<Coordinate> coordinates)
    {
        var region = new List<Coordinate>();
        var minX = coordinates.Min(c => c.X);
        var maxX = coordinates.Max(c => c.X);
        var minY = coordinates.Min(c => c.Y);
        var maxY = coordinates.Max(c => c.Y);

        for (var x = minY; x <= maxY; x++)
        {
            for (var y = minX; y <= maxX; y++)
            {
                var sumOfDistances = coordinates.Sum(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y));

                if (sumOfDistances < 10_000)
                {
                    region.Add(new Coordinate(x, y));
                }
            }
        }

        return region;
    }

    private static List<Coordinate> GetCoordinates()
    {
        var coordinates = new List<Coordinate>();

        foreach (var line in Input)
        {
            var values = line.Split(", ").Select(int.Parse).ToArray();
            var coordinate = new Coordinate(values[0], values[1]);
            coordinates.Add(coordinate);
        }

        return coordinates;
    }

    private static int GetArea(Coordinate coordinate, List<Coordinate> coordinates, int padding)
    {
        var (x, y) = coordinate;

        if (!coordinates.Any(c => c.X < x) || !coordinates.Any(c => c.X > x) ||
            !coordinates.Any(c => c.Y < y) || !coordinates.Any(c => c.Y > y))
        {
            return -1;
        }

        var area = 0;

        var minX = coordinates.Min(c => c.X) - padding;
        var maxX = coordinates.Max(c => c.X) + padding;
        var minY = coordinates.Min(c => c.Y) - padding;
        var maxY = coordinates.Max(c => c.Y) + padding;

        for (var i = minY; i <= maxY; i++)
        {
            for (var j = minX; j <= maxX; j++)
            {
                var temp = GetClosestCoordinate(new Coordinate(i, j), coordinates);

                if (temp == coordinate)
                {
                    area++;
                }
            }
        }

        return area;
    }

    private static Coordinate? GetClosestCoordinate(Coordinate coordinate, List<Coordinate> coordinates)
    {
        var (x, y) = coordinate;

        var query = coordinates
            .GroupBy(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y))
            .OrderBy(g => g.Key)
            .First();

        return query.Count() == 1 ? query.First() : null;
    }

    private record Coordinate(int X, int Y);
}
