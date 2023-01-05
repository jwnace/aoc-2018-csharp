namespace aoc_2018_csharp.Day03;

public static class Day03
{
    private static readonly string[] Input = File.ReadAllLines("Day03/day03.txt");

    public static int Part1()
    {
        var result = 0;
        var claims = GetClaims();

        for (var i = 0; i < claims.Count - 1; i++)
        {
            for (var j = i + 1; j < claims.Count; j++)
            {
                var first = claims[i];
                var second = claims[j];

                if (first.Overlaps(second))
                {
                    result += first.GetOverlappingArea(second);
                }
            }
        }
        
        return result;
    }
    
    public static int Part2() => 2;

    private static List<Claim> GetClaims()
    {
        var claims = new List<Claim>();

        foreach (var line in Input)
        {
            var values = line.Split(' ');
            var start = values[2][..^1].Split(',').Select(int.Parse).ToArray();
            var size = values[3].Split('x').Select(int.Parse).ToArray();
            var (startX, startY) = (start[0], start[1]);
            var (endX, endY) = (startX + size[0], startY + size[1]);

            claims.Add(new Claim(startX, startY, endX, endY));
        }

        return claims;
    }

    private record Claim(int StartX, int StartY, int EndX, int EndY)
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

        public int GetOverlappingArea(Claim other)
        {
            var startX = Math.Max(StartX, other.StartX);
            var endX = Math.Min(EndX, other.EndX);
            var startY = Math.Max(StartY, other.StartY);
            var endY = Math.Min(EndY, other.EndY);

            var dx = endX - startX;
            var dy = endY - startY;

            return dx * dy;
        }
    }
}
