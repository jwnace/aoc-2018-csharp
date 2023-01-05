namespace aoc_2018_csharp.Day03;

public static class Day03
{
    private static readonly string[] Input = File.ReadAllLines("Day03/day03.txt");

    public static int Part1()
    {
        var grid = GetGrid();
        return grid.Count(x => x.Value > 1);
    }

    public static int Part2()
    {
        var claims = GetClaims();
        return claims.Single(claim => claims.Count(claim.Overlaps) == 1).Id;
    }

    private static Dictionary<(int X, int Y), int> GetGrid()
    {
        var grid = new Dictionary<(int X, int Y), int>();

        foreach (var line in Input)
        {
            var values = line.Split(' ');
            var start = values[2][..^1].Split(',').Select(int.Parse).ToArray();
            var size = values[3].Split('x').Select(int.Parse).ToArray();
            var (startX, startY) = (start[0], start[1]);
            var (endX, endY) = (startX + size[0], startY + size[1]);

            for (var x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    var value = grid.TryGetValue((x, y), out var v) ? v : 0;
                    grid[(x, y)] = value + 1;
                }
            }
        }

        return grid;
    }

    private static List<Claim> GetClaims()
    {
        var claims = new List<Claim>();

        foreach (var line in Input)
        {
            var values = line.Split(' ');
            var id = int.Parse(values[0][1..]);
            var start = values[2][..^1].Split(',').Select(int.Parse).ToArray();
            var size = values[3].Split('x').Select(int.Parse).ToArray();
            var (startX, startY) = (start[0], start[1]);
            var (endX, endY) = (startX + size[0], startY + size[1]);

            claims.Add(new Claim(id, startX, startY, endX, endY));
        }

        return claims;
    }

    private record Claim(int Id, int StartX, int StartY, int EndX, int EndY)
    {
        public bool Overlaps(Claim other)
        {
            if (EndX <= other.StartX || other.EndX <= StartX)
            {
                return false;
            }

            if (EndY <= other.StartY || other.EndY <= StartY)
            {
                return false;
            }

            return true;
        }
    }
}
