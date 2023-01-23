namespace aoc_2018_csharp.Day11;

public static class Day11
{
    private static readonly int SerialNumber = int.Parse(File.ReadAllText("Day11/day11.txt"));

    public static string Part1()
    {
        var grid = BuildGrid();
        var maxPosition = GetMaxPosition(grid, 3);

        return $"{maxPosition.Position.X},{maxPosition.Position.Y}";
    }

    public static string Part2()
    {
        var grid = BuildGrid();
        var maxPosition = GetMaxPosition(grid);

        return $"{maxPosition.Position.X},{maxPosition.Position.Y},{maxPosition.Size}";
    }

    private static Dictionary<(int X, int Y), int> BuildGrid()
    {
        var grid = new Dictionary<(int X, int Y), int>();

        for (var y = 1; y <= 300; y++)
        {
            for (var x = 1; x <= 300; x++)
            {
                grid[(x, y)] = GetPowerLevel(x, y);
            }
        }

        return grid;
    }

    private static int GetPowerLevel(int x, int y)
    {
        var rackId = x + 10;
        var powerLevel = rackId * y;
        powerLevel += SerialNumber;
        powerLevel *= rackId;
        powerLevel = powerLevel > 99 ? powerLevel / 100 % 10 : 0;
        powerLevel -= 5;

        return powerLevel;
    }

    private static ((int X, int Y) Position, int TotalPower, int Size) GetMaxPosition(Dictionary<(int X, int Y), int> grid, int size)
    {
        var max = 0;
        var maxPosition = (X: 0, Y: 0);

        for (var y = 1; y <= 300 - size + 1; y++)
        {
            for (var x = 1; x <= 300 - size + 1; x++)
            {
                var sum = 0;

                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        sum += grid[(x + i, y + j)];
                    }
                }

                if (sum > max)
                {
                    max = sum;
                    maxPosition = (x, y);
                }
            }
        }

        return (maxPosition, max, size);
    }

    private static ((int X, int Y) Position, int TotalPower, int Size) GetMaxPosition(Dictionary<(int X, int Y), int> grid)
    {
        var maxPosition = (Position: (X: 0, Y: 0), TotalPower: 0, Size: 0);

        for (var size = 1; size <= 20; size++)
        {
            var temp = GetMaxPosition(grid, size);

            if (temp.TotalPower > maxPosition.TotalPower)
            {
                maxPosition = temp;
            }
        }

        return maxPosition;
    }
}
