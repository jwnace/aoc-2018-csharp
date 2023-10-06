using System.Text;

namespace aoc_2018_csharp.Day18;

public static class Day18
{
    private static readonly string[] Input = File.ReadAllLines("Day18/day18.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input)
    {
        var grid = BuildGrid(input);
        var newGrid = new Dictionary<(int Row, int Col), char>();

        var minRow = grid.Keys.Min(x => x.Row);
        var maxRow = grid.Keys.Max(x => x.Row);
        var minCol = grid.Keys.Min(x => x.Col);
        var maxCol = grid.Keys.Max(x => x.Col);

        for (var i = 0; i < 10; i++)
        {
            for (var row = minRow; row <= maxRow; row++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    var value = grid[(row, col)];
                    var neighbors = GetNeighbors(grid, row, col).ToArray();

                    var newValue = value switch
                    {
                        '.' => neighbors.Count(n => n is '|') >= 3 ? '|' : '.',
                        '|' => neighbors.Count(n => n is '#') >= 3 ? '#' : '|',
                        '#' => neighbors.Any(n => n is '#') && neighbors.Any(a => a is '|') ? '#' : '.',
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    newGrid[(row, col)] = newValue;
                }
            }

            // grid = newGrid;

            (grid, newGrid) = (newGrid, grid);

            // var s = i > 0 ? "s" : "";
            // Console.WriteLine($"After {i + 1} minute{s}:");
        }

        // Console.WriteLine(DrawGrid(grid));
        return grid.Values.Count(x => x is '|') * grid.Values.Count(x => x is '#');
    }

    private static Dictionary<(int Row, int Col), char> BuildGrid(string[] input)
    {
        var grid = new Dictionary<(int Row, int Col), char>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[(row, col)] = input[row][col];
            }
        }

        return grid;
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

    private static IEnumerable<char> GetNeighbors(IReadOnlyDictionary<(int Row, int Col), char> grid, int row, int col)
    {
        for (var r = row - 1; r <= row + 1; r++)
        {
            for (var c = col - 1; c <= col + 1; c++)
            {
                if (r == row && c == col)
                {
                    continue;
                }

                if (grid.TryGetValue((r, c), out var value))
                {
                    yield return value;
                }
            }
        }

        // var neighbors = new[]
        // {
        //     (row - 1, col - 1),
        //     (row - 1, col),
        //     (row - 1, col + 1),
        //     (row, col - 1),
        //     (row, col + 1),
        //     (row + 1, col - 1),
        //     (row + 1, col),
        //     (row + 1, col + 1),
        // };
        //
        // return neighbors.Where(grid.ContainsKey).Select(x => grid[x]).ToArray();
    }

    public static int Solve2(string[] input)
    {
        var grid = new Dictionary<(int Row, int Col), char>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[(row, col)] = input[row][col];
            }
        }

        var newGrid = new Dictionary<(int Row, int Col), char>();
        var seen = new Dictionary<string, int> { [DrawGrid(grid)] = 0 };

        // Console.WriteLine($"Initial state:");
        // Console.WriteLine(DrawGrid(grid));

        var minRow = grid.Keys.Min(x => x.Row);
        var maxRow = grid.Keys.Max(x => x.Row);
        var minCol = grid.Keys.Min(x => x.Col);
        var maxCol = grid.Keys.Max(x => x.Col);

        var foundCycle = false;

        for (var i = 0; i < 1_000_000_000; i++)
        {
            if ((i + 1) % 1_000 == 0)
            {
                Console.WriteLine($"Minute {(i + 1),13:N0}");
            }

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

            if (seen.ContainsKey(gridString) && !foundCycle)
            {
                var cycleLength = i - seen[gridString];
                var remaining = 1_000_000_000 - i - 1;
                var remainingCycles = remaining / cycleLength;
                var remainingMinutes = remaining % cycleLength;

                Console.WriteLine($"Found a cycle at (i = {i}) with length {cycleLength}");
                Console.WriteLine($"Remaining minutes: {remainingMinutes}");
                Console.WriteLine($"Remaining cycles: {remainingCycles}");

                i += remainingCycles * cycleLength;
                foundCycle = true;
            }
            else
            {
                seen[gridString] = i;
            }

            (grid, newGrid) = (newGrid, grid);

            // var s = i > 0 ? "s" : "";
            // Console.WriteLine($"After {i + 1} minute{s}:");
            // Console.WriteLine(DrawGrid(grid));
        }

        return grid.Values.Count(x => x is '|') * grid.Values.Count(x => x is '#');
    }

    private static (int trees, int lumberyards) GetNeighborCounts(
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

                if (grid.TryGetValue((r, c), out var value))
                {
                    if (value is '|')
                    {
                        trees++;
                    }
                    else if (value is '#')
                    {
                        lumberyards++;
                    }
                }
            }
        }

        return (trees, lumberyards);
    }
}
