using System.Text;

namespace aoc_2018_csharp.Day18;

public static class Day18
{
    private static readonly string[] Input = File.ReadAllLines("Day18/day18.txt");

    public static int Part1() => Solve(Input);

    public static int Part2() => Solve(Input, 1_000_000_000);

    public static int Solve(string[] input, int iterations = 10)
    {
        var grid = BuildGrid(input);
        var newGrid = new Dictionary<(int Row, int Col), char>();
        var seen = new Dictionary<string, int> { [DrawGrid(grid)] = 0 };
        var minRow = grid.Keys.Min(x => x.Row);
        var maxRow = grid.Keys.Max(x => x.Row);
        var minCol = grid.Keys.Min(x => x.Col);
        var maxCol = grid.Keys.Max(x => x.Col);
        var foundCycle = false;

        for (var i = 0; i < iterations; i++)
        {
            for (var row = minRow; row <= maxRow; row++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    var (trees, lumberyards) = GetNeighborCounts(grid, row, col);

                    var newValue = grid[(row, col)] switch
                    {
                        '.' => trees >= 3 ? '|' : '.',
                        '|' => lumberyards >= 3 ? '#' : '|',
                        '#' => lumberyards >= 1 && trees >= 1 ? '#' : '.',
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    newGrid[(row, col)] = newValue;
                }
            }

            var gridString = DrawGrid(newGrid);

            if (!foundCycle && seen.TryGetValue(gridString, out var value))
            {
                var cycleLength = i - value;
                var remainingIterations = iterations - i - 1;
                var remainingCycles = remainingIterations / cycleLength;

                i += remainingCycles * cycleLength;
                foundCycle = true;
            }
            else
            {
                seen[gridString] = i;
            }

            (grid, newGrid) = (newGrid, grid);
        }

        return grid.Values.Count(x => x is '|') * grid.Values.Count(x => x is '#');
    }

    private static Dictionary<(int Row, int Col), char> BuildGrid(IReadOnlyList<string> input)
    {
        var grid = new Dictionary<(int Row, int Col), char>();

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[(row, col)] = input[row][col];
            }
        }

        return grid;
    }

    private static (int Trees, int Lumberyards) GetNeighborCounts(
        IReadOnlyDictionary<(int Row, int Col), char> grid, int row, int col)
    {
        var trees = 0;
        var lumberyards = 0;

        for (var r = row - 1; r <= row + 1; r++)
        {
            for (var c = col - 1; c <= col + 1; c++)
            {
                if (r == row && c == col)
                {
                    continue;
                }

                if (!grid.TryGetValue((r, c), out var value))
                {
                    continue;
                }

                trees += value is '|' ? 1 : 0;
                lumberyards += value is '#' ? 1 : 0;
            }
        }

        return (trees, lumberyards);
    }

    private static string DrawGrid(Dictionary<(int Row, int Col), char> grid)
    {
        var sb = new StringBuilder();

        var minRow = grid.Keys.Min(x => x.Row);
        var maxRow = grid.Keys.Max(x => x.Row);
        var minCol = grid.Keys.Min(x => x.Col);
        var maxCol = grid.Keys.Max(x => x.Col);

        for (var row = minRow; row <= maxRow; row++)
        {
            for (var col = minCol; col <= maxCol; col++)
            {
                sb.Append(grid[(row, col)]);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
