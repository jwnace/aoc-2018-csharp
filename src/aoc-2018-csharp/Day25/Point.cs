namespace aoc_2018_csharp.Day25;

public record Point(int X, int Y, int Z, int T)
{
    public bool IsInRange(Point other)
    {
        var distance = Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z) + Math.Abs(T - other.T);
        return distance <= 3;
    }
}
