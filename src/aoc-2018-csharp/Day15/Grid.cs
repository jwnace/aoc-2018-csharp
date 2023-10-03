using System.Text;

namespace aoc_2018_csharp.Day15;

public record Grid(Dictionary<Coordinate, char> Map, List<Unit> Units)
{
    public static implicit operator Dictionary<Coordinate, char>(Grid grid) => grid.Map;

    public static Grid Parse(string[] input, int elfAttackPower = 3)
    {
        var grid = BuildGrid(input);
        var units = BuildUnits(input, elfAttackPower).ToList();

        return new Grid(grid, units);
    }

    private static Dictionary<Coordinate, char> BuildGrid(IReadOnlyList<string> input)
    {
        var grid = new Dictionary<Coordinate, char>();

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[new Coordinate(row, col)] = input[row][col] is 'E' or 'G' ? '.' : input[row][col];
            }
        }

        return grid;
    }

    private static IEnumerable<Unit> BuildUnits(string[] input, int elfAttackPower = 3)
    {
        var id = 0;

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] is not ('E' or 'G'))
                {
                    continue;
                }

                id++;

                var attackPower = input[row][col] == 'E' ? elfAttackPower : 3;

                yield return new Unit(id, new Coordinate(row, col), 200, attackPower, input[row][col]);
            }
        }
    }

    public (int Distance, Coordinate FirstStep) FindShortestPath(Coordinate start, Coordinate end)
    {
        var nodes = new Dictionary<(Coordinate Position, Coordinate FirstStep), int>();
        var queue = new Queue<(Coordinate Position, Coordinate FirstStep)>();
        var initialState = (start, start);
        nodes[initialState] = 0;
        queue.Enqueue(initialState);

        while (queue.Any())
        {
            var current = queue.Dequeue();

            var (row, col) = current.Position;

            var neighbors = new[]
            {
                new Coordinate(row - 1, col),
                new Coordinate(row, col - 1),
                new Coordinate(row, col + 1),
                new Coordinate(row + 1, col)
            };

            foreach (var neighbor in neighbors)
            {
                if (Map[neighbor] != '.' || Units.Any(u => u.Position == neighbor))
                {
                    continue;
                }

                if (nodes.ContainsKey((neighbor, current.FirstStep)))
                {
                    continue;
                }

                var newState = (neighbor, current.FirstStep);

                if (newState.FirstStep == start)
                {
                    newState.FirstStep = neighbor;
                }

                nodes[(neighbor, newState.FirstStep)] = nodes[current] + 1;
                queue.Enqueue((neighbor, newState.FirstStep));
            }
        }

        var paths = nodes.Where(n => n.Key.Position == end).ToList();

        if (paths.Any())
        {
            var shortestPath = paths.OrderBy(n => n.Value)
                .ThenBy(x => x.Key.FirstStep.Row)
                .ThenBy(x => x.Key.FirstStep.Col)
                .First();

            return (shortestPath.Value, shortestPath.Key.FirstStep);
        }

        return (int.MaxValue, start);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        var maxRow = Map.Max(g => g.Key.Row);
        var maxCol = Map.Max(g => g.Key.Col);

        for (var row = 0; row <= maxRow; row++)
        {
            for (var col = 0; col <= maxCol; col++)
            {
                builder.Append(Units.Any(x => x.Position == new Coordinate(row, col))
                    ? Units.Single(x => x.Position == new Coordinate(row, col)).Type
                    : Map[new Coordinate(row, col)]);
            }

            var units = string.Join(", ", Units.Where(u => u.Position.Row == row).OrderBy(u => u.Position.Col))
                .PadRight(80, ' ');

            builder.Append($"   {units}{Environment.NewLine}");
        }

        builder.Append(Environment.NewLine);
        return builder.ToString();
    }
}
