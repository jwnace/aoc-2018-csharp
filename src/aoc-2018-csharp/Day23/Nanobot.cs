using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day23;

record Nanobot(int X, int Y, int Z, int Radius)
{
    public static Nanobot Parse(string line)
    {
        var parts = line.Split('<', ',', '>', '=').Where(s => int.TryParse(s, out _)).Select(int.Parse).ToArray();
        var (x, y, z, radius) = parts;

        return new Nanobot(x, y, z, radius);
    }

    public bool IsInRange(Nanobot other) => IsInRange(other.X, other.Y, other.Z);

    public bool IsInRange(int x, int y, int z) => GetDistance(x, y, z) <= Radius;

    private int GetDistance(int x, int y, int z) => Math.Abs(X - x) + Math.Abs(Y - y) + Math.Abs(Z - z);
}
