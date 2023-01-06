namespace aoc_2018_csharp.Day06;

public static class Day06
{
    private static readonly string[] Input = File.ReadAllLines("Day06/day06.txt");

    public static int Part1()
    {
        // 4345 was too high
        // 4578 was too high
        var coordinates = GetCoordinates();

        // var candidates = new List<Coordinate>();

        var best = 0;
        var areas = new List<int>();

        foreach (var coordinate in coordinates)
        {
            var area1 = GetArea(coordinate, coordinates, 0);
            var area2 = GetArea(coordinate, coordinates, 1);

            if (area1 == -1 || area1 != area2)
            {
                // Console.WriteLine($"area is infinite! coordinate: {coordinate}");
                continue;
            }

            areas.Add(area1);
            best = Math.Max(best, area1);

            // candidates.Add(coordinate);
        }

        // Console.WriteLine($"candidates: {candidates.Count}");
        // Console.WriteLine(string.Join(Environment.NewLine, candidates));

        // DrawGrid(coordinates, candidates);

        var query = areas.OrderDescending().ToList();
        
        return best;
    }

    public static int Part2() => 2;

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

    private static void DrawGrid(List<Coordinate> grid, List<Coordinate> candidates)
    {
        var minX = grid.Min(c => c.X);
        var maxX = grid.Max(c => c.X);
        var minY = grid.Min(c => c.Y);
        var maxY = grid.Max(c => c.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var isCoordinate = grid.Contains(new Coordinate(x, y));
                var isCandidate = candidates.Contains(new Coordinate(x, y));

                var value = isCoordinate switch
                {
                    true when isCandidate => 'F',
                    true when !isCandidate => 'I',
                    _ => ' ',
                };

                Console.Write(value);
            }

            Console.WriteLine();
        }
    }

    private static int GetArea(Coordinate coordinate, List<Coordinate> coordinates, int padding)
    {
        var (x, y) = coordinate;

        if (coordinates.Any(c => c.X < x) &&
            coordinates.Any(c => c.X > x) &&
            coordinates.Any(c => c.Y < y) &&
            coordinates.Any(c => c.Y > y))
        {
            // TODO: calculate the area for this (x, y) coordinate
            var area = 0;

            var minX = coordinates.Min(c => c.X) - padding;
            var maxX = coordinates.Max(c => c.X) + padding;
            var minY = coordinates.Min(c => c.Y) - padding;
            var maxY = coordinates.Max(c => c.Y) + padding;

            for (var i = minY; i <= maxY; i++)
            {
                for (var j = minX; j <= maxX; j++)
                {
                    var temp = GetClosestCoordinate(i, j, coordinates);

                    if (temp == coordinate)
                    {
                        area++;
                    }
                }
            }

            return area;
        }

        return -1;
    }

    private static Coordinate? GetClosestCoordinate(int x, int y, List<Coordinate> coordinates)
    {
        var minDistance = coordinates.Min(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y));

        // var temp = coordinates
        // .Select(c => new {Coordinate = c, Distance = Math.Abs(c.X - x) + Math.Abs(c.Y - y)})
        // .OrderBy(c => c.Distance)
        // .ToList();

        var query = coordinates.Where(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y) == minDistance).ToList();

        return query.Count() == 1 ? query.First() : null;
    }

    private record Coordinate(int X, int Y);
}
