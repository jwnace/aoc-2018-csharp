using System.Text;

namespace aoc_2018_csharp.Day15;

public static class Day15
{
    private static readonly string[] Input = File.ReadAllLines("Day15/day15.txt");

    public static int Part1()
    {
        var rows = Input.Length;
        var cols = Input[0].Length;

        var grid = BuildGrid(rows, cols);

        DrawGrid(grid);

        var inCombat = true;

        while (inCombat)
        {
            // iterate in reading order
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    // if there is a unit at this location, it should take its turn
                    if (grid[(r, c)] is not ('E' or 'G'))
                    {
                        continue;
                    }

                    // find all targets
                    var targetType = grid[(r, c)] == 'E' ? 'G' : 'E';
                    var targets = grid.Where(g => g.Value == targetType).ToList();

                    // if no targets, combat ends
                    if (!targets.Any())
                    {
                        inCombat = false;
                        throw new NotImplementedException("Combat ends");
                    }

                    // if no open squares adjacent to any targets, unit ends turn
                    // if open squares adjacent to any targets, unit moves
                    // if unit is in range of a target, unit attacks

                    throw new NotImplementedException("Unit takes turn");
                }
            }
        }

        return 1;
    }

    public static int Part2() => 2;

    private static Dictionary<(int, int), char> BuildGrid(int rows, int cols)
    {
        var grid = new Dictionary<(int, int), char>();

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                grid[(r, c)] = Input[r][c];
            }
        }

        return grid;
    }

    private static void DrawGrid(IReadOnlyDictionary<(int Row, int Col), char> grid)
    {
        var rows = grid.Max(g => g.Key.Row) + 1;
        var cols = grid.Max(g => g.Key.Col) + 1;

        for (var r = 0; r < rows; r++)
        {
            var line = new StringBuilder();

            for (var c = 0; c < cols; c++)
            {
                line.Append(grid[(r, c)]);
            }

            Console.WriteLine(line);
        }
    }
}
