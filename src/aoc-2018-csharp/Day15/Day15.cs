using System.Diagnostics;
using System.Text;

namespace aoc_2018_csharp.Day15;

public static class Day15
{
    private static readonly string[] Input = File.ReadAllLines("Day15/day15.txt");

    public static int Part1() => Solve1(Input, animate: false);

    public static int Part2() => Solve2(Input, elfAttackPower: 29);

    public static int Solve1(string[] input, bool animate = false)
    {
        var maxRow = input.Length - 1;
        var maxCol = input[0].Length - 1;
        Dictionary<Coordinate, char> grid = BuildGrid(input);
        var units = BuildUnits(input).ToList();
        var round = 1;

        var (top, left) = (Console.CursorTop, Console.CursorLeft);

        if (animate)
        {
            Console.WriteLine("Initially:");
            DrawGrid(grid, units);
        }

        while (true)
        {
            var alreadyMovedThisTurn = new HashSet<int>();

            for (var row = 0; row <= maxRow; row++)
            {
                for (var col = 0; col <= maxCol; col++)
                {
                    var unit = units.FirstOrDefault(x => x.Position == new Coordinate(row, col));

                    if (unit is null)
                    {
                        continue;
                    }

                    if (alreadyMovedThisTurn.Contains(unit.Id))
                    {
                        continue;
                    }

                    var targets = FindTargets(unit, units).ToList();

                    if (!targets.Any())
                    {
                        if (animate)
                        {
                            var s1 = round > 1 ? "s" : string.Empty;
                            Console.SetCursorPosition(left, top);
                            Console.WriteLine($"After {round} round{s1}:");
                            DrawGrid(grid, units);
                        }

                        return (round - 1) * units.Sum(x => x.HitPoints);
                    }

                    var neighbors = new[]
                    {
                        (row - 1, col),
                        (row, col - 1),
                        (row, col + 1),
                        (row + 1, col)
                    };

                    var inRange = targets.Where(t => neighbors.Any(n => new Coordinate(n.Item1, n.Item2) == t.Position))
                        .ToList();

                    if (inRange.Any())
                    {
                        alreadyMovedThisTurn.Add(unit.Id);

                        var target = inRange.OrderBy(x => x.HitPoints)
                            .ThenBy(x => x.Position.Row)
                            .ThenBy(x => x.Position.Col)
                            .First();

                        target.HitPoints -= unit.AttackPower;

                        if (target.HitPoints <= 0)
                        {
                            units.Remove(target);
                        }
                    }
                    else
                    {
                        var openSquares = FindOpenSquares(targets, grid, units);

                        // if no open squares adjacent to any targets, unit ends turn
                        if (!openSquares.Any())
                        {
                            continue;
                        }

                        var paths = new List<((int Distance, Coordinate FirstStep), Coordinate End)>();

                        // if open squares adjacent to any targets, unit moves
                        foreach (var openSquare in openSquares)
                        {
                            var start = new Coordinate(row, col);
                            var end = openSquare;

                            var path = FindShortestPath(grid, start, end, units);

                            paths.Add((path, end));
                        }

                        var chosenPath = paths.OrderBy(x => x.Item1.Distance)
                            .ThenBy(x => x.End.Row)
                            .ThenBy(x => x.End.Col)
                            .ThenBy(x => x.Item1.FirstStep.Row)
                            .ThenBy(x => x.Item1.FirstStep.Col)
                            .First();

                        if (chosenPath.Item1.Distance == int.MaxValue)
                        {
                            continue;
                        }

                        // move
                        unit.Position = chosenPath.Item1.FirstStep;
                        alreadyMovedThisTurn.Add(unit.Id);

                        // try to attack again
                        {
                            // if unit is in range of a target, unit attacks
                            var newNeighbors = new[]
                            {
                                new Coordinate(unit.Position.Row - 1, unit.Position.Col),
                                new Coordinate(unit.Position.Row, unit.Position.Col - 1),
                                new Coordinate(unit.Position.Row, unit.Position.Col + 1),
                                new Coordinate(unit.Position.Row + 1, unit.Position.Col)
                            };

                            var newInRange = targets.Where(t => newNeighbors.Any(n => n == t.Position))
                                .ToList();

                            if (newInRange.Any())
                            {
                                // this is redundant at this point
                                // alreadyMovedThisTurn.Add(unit.Id);

                                var target = newInRange.OrderBy(x => x.HitPoints)
                                    .ThenBy(x => x.Position.Row)
                                    .ThenBy(x => x.Position.Col)
                                    .First();

                                target.HitPoints -= unit.AttackPower;

                                if (target.HitPoints <= 0)
                                {
                                    units.Remove(target);
                                }
                            }
                        }
                    }
                }
            }

            if (animate)
            {
                var s = round > 1 ? "s" : string.Empty;
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"After {round} round{s}:");
                DrawGrid(grid, units);
            }

            round++;
        }
    }

    public static int Solve2(string[] input, int elfAttackPower = 4)
    {
        var elfDied = true;

        while (elfDied)
        {
            elfDied = false;

            // Console.WriteLine($"Trying elf attack power {elfAttackPower}");

            var maxRow = input.Length - 1;
            var maxCol = input[0].Length - 1;
            var grid = BuildGrid(input);
            var units = BuildUnits(input, elfAttackPower).ToList();
            var inCombat = true;
            var round = 1;

            // Console.WriteLine("Initially:");
            // DrawGrid(grid, units);

            while (inCombat)
            {
                var alreadyMovedThisTurn = new HashSet<int>();

                // Console.WriteLine($"round {round}");

                for (var row = 0; row <= maxRow; row++)
                {
                    for (var col = 0; col <= maxCol; col++)
                    {
                        var unit = units.FirstOrDefault(x => x.Position == new Coordinate(row, col));

                        if (unit is null)
                        {
                            continue;
                        }

                        if (alreadyMovedThisTurn.Contains(unit.Id))
                        {
                            continue;
                        }

                        var targets = FindTargets(unit, units).ToList();

                        if (!targets.Any())
                        {
                            if (!elfDied)
                            {
                                // var s1 = round > 1 ? "s" : string.Empty;
                                // Console.WriteLine($"After {round} round{s1}:");
                                // DrawGrid(grid, units);
                                // Console.WriteLine("Combat ended without an elf dying (early return)");
                                return (round - 1) * units.Sum(x => x.HitPoints);
                            }

                            inCombat = false;
                            row = int.MaxValue - 1;
                            col = int.MaxValue - 1;
                            break;
                        }

                        var neighbors = new[]
                        {
                            (row - 1, col),
                            (row, col - 1),
                            (row, col + 1),
                            (row + 1, col)
                        };

                        var inRange = targets
                            .Where(t => neighbors.Any(n => new Coordinate(n.Item1, n.Item2) == t.Position))
                            .ToList();

                        if (inRange.Any())
                        {
                            alreadyMovedThisTurn.Add(unit.Id);

                            var target = inRange.OrderBy(x => x.HitPoints)
                                .ThenBy(x => x.Position.Row)
                                .ThenBy(x => x.Position.Col)
                                .First();

                            target.HitPoints -= unit.AttackPower;

                            if (target.HitPoints <= 0)
                            {
                                if (target.Type == 'E')
                                {
                                    elfDied = true;
                                    // inCombat = false;
                                    // row = int.MaxValue - 1;
                                    // col = int.MaxValue - 1;
                                    // break;
                                }

                                units.Remove(target);
                            }
                        }
                        else
                        {
                            var openSquares = FindOpenSquares(targets, grid, units);

                            // if no open squares adjacent to any targets, unit ends turn
                            if (!openSquares.Any())
                            {
                                continue;
                            }

                            var paths = new List<((int Distance, Coordinate FirstStep), Coordinate End)>();

                            // if open squares adjacent to any targets, unit moves
                            foreach (var openSquare in openSquares)
                            {
                                var start = new Coordinate(row, col);
                                var end = openSquare;

                                var path = FindShortestPath(grid, start, end, units);

                                paths.Add((path, end));
                            }

                            var chosenPath = paths.OrderBy(x => x.Item1.Distance)
                                .ThenBy(x => x.End.Row)
                                .ThenBy(x => x.End.Col)
                                .ThenBy(x => x.Item1.FirstStep.Row)
                                .ThenBy(x => x.Item1.FirstStep.Col)
                                .First();

                            if (chosenPath.Item1.Distance == int.MaxValue)
                            {
                                continue;
                            }

                            // move
                            unit.Position = chosenPath.Item1.FirstStep;
                            alreadyMovedThisTurn.Add(unit.Id);

                            // try to attack again
                            {
                                // if unit is in range of a target, unit attacks
                                var newNeighbors = new[]
                                {
                                    new Coordinate(unit.Position.Row - 1, unit.Position.Col),
                                    new Coordinate(unit.Position.Row, unit.Position.Col - 1),
                                    new Coordinate(unit.Position.Row, unit.Position.Col + 1),
                                    new Coordinate(unit.Position.Row + 1, unit.Position.Col)
                                };

                                var newInRange = targets.Where(t => newNeighbors.Any(n => n == t.Position))
                                    .ToList();

                                if (newInRange.Any())
                                {
                                    // this is redundant at this point
                                    // alreadyMovedThisTurn.Add(unit.Id);

                                    var target = newInRange.OrderBy(x => x.HitPoints)
                                        .ThenBy(x => x.Position.Row)
                                        .ThenBy(x => x.Position.Col)
                                        .First();

                                    target.HitPoints -= unit.AttackPower;

                                    if (target.HitPoints <= 0)
                                    {
                                        if (target.Type == 'E')
                                        {
                                            elfDied = true;
                                            // inCombat = false;
                                            // row = int.MaxValue - 1;
                                            // col = int.MaxValue - 1;
                                            // continue;
                                        }

                                        units.Remove(target);
                                    }
                                }
                            }
                        }
                    }
                }

                // var s = round > 1 ? "s" : string.Empty;
                // Console.WriteLine($"After {round} round{s}:");
                // DrawGrid(grid, units);

                round++;
            }

            if (!elfDied)
            {
                // var s1 = round > 1 ? "s" : string.Empty;
                // Console.WriteLine($"After {round} round{s1}:");
                // DrawGrid(grid, units);
                // Console.WriteLine("Combat ended without an elf dying (early return)");
                return (round - 1) * units.Sum(x => x.HitPoints);
            }

            elfAttackPower++;
        }

        throw new Exception("No solution found");
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

                var attackPower = input[row][col] == 'E' ? elfAttackPower : 3;

                id++;
                yield return new Unit(id, new Coordinate(row, col), 200, attackPower, input[row][col]);
            }
        }
    }

    private static (int Distance, Coordinate FirstStep) FindShortestPath(Dictionary<Coordinate, char> grid,
        Coordinate start, Coordinate end, List<Unit> units)
    {
        // find the shortest path from start to end
        // return the first step in the path

        var nodes = new Dictionary<(Coordinate Position, Coordinate FirstStep), int>();
        var queue = new Queue<(Coordinate Position, Coordinate FirstStep)>();

        var initialState = (start, start);

        nodes[initialState] = 0;
        queue.Enqueue(initialState);

        while (queue.Any())
        {
            var current = queue.Dequeue();

            var neighbors = new[]
            {
                new Coordinate(current.Position.Row - 1, current.Position.Col),
                new Coordinate(current.Position.Row, current.Position.Col - 1),
                new Coordinate(current.Position.Row, current.Position.Col + 1),
                new Coordinate(current.Position.Row + 1, current.Position.Col)
            };

            foreach (var neighbor in neighbors)
            {
                if (grid[neighbor] != '.' || units.Any(u => u.Position == neighbor))
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

    private static IEnumerable<Unit> FindTargets(Unit player, List<Unit> units)
    {
        var targetType = player.Type == 'E' ? 'G' : 'E';
        return units.Where(u => u.Type == targetType);
    }

    private static List<Coordinate> FindOpenSquares(
        List<Unit> targets,
        Dictionary<Coordinate, char> grid,
        List<Unit> units)
    {
        var openSquares = new List<Coordinate>();

        foreach (var target in targets)
        {
            var targetRow = target.Position.Row;
            var targetCol = target.Position.Col;

            // check up
            if (grid[new Coordinate(targetRow - 1, targetCol)] == '.' &&
                units.All(u => u.Position != new Coordinate(targetRow - 1, targetCol)))
            {
                openSquares.Add(new Coordinate(targetRow - 1, targetCol));
            }

            // check left
            if (grid[new Coordinate(targetRow, targetCol - 1)] == '.' &&
                units.All(u => u.Position != new Coordinate(targetRow, targetCol - 1)))
            {
                openSquares.Add(new Coordinate(targetRow, targetCol - 1));
            }

            // check right
            if (grid[new Coordinate(targetRow, targetCol + 1)] == '.' &&
                units.All(u => u.Position != new Coordinate(targetRow, targetCol + 1)))
            {
                openSquares.Add(new Coordinate(targetRow, targetCol + 1));
            }

            // check down
            if (grid[new Coordinate(targetRow + 1, targetCol)] == '.' &&
                units.All(u => u.Position != new Coordinate(targetRow + 1, targetCol)))
            {
                openSquares.Add(new Coordinate(targetRow + 1, targetCol));
            }
        }

        return openSquares;
    }

    private static Dictionary<Coordinate, char> BuildGrid(string[] input)
    {
        var grid = new Dictionary<Coordinate, char>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] is ('E' or 'G'))
                {
                    grid[new Coordinate(row, col)] = '.';
                }
                else
                {
                    grid[new Coordinate(row, col)] = input[row][col];
                }
            }
        }

        return grid;
    }

    private static void DrawGrid(IReadOnlyDictionary<Coordinate, char> grid, List<Unit> units)
    {
        var maxRow = grid.Max(g => g.Key.Row);
        var maxCol = grid.Max(g => g.Key.Col);

        for (var row = 0; row <= maxRow; row++)
        {
            var line = new StringBuilder();

            for (var col = 0; col <= maxCol; col++)
            {
                if (units.Any(x => x.Position == new Coordinate(row, col)))
                {
                    line.Append(units.Single(x => x.Position == new Coordinate(row, col)).Type);
                }
                else
                {
                    line.Append(grid[new Coordinate(row, col)]);
                }
            }

            line.Append("   ");

            var temp = string.Join(", ", units.Where(u => u.Position.Row == row).OrderBy(u => u.Position.Col))
                .PadRight(80, ' ');

            line.Append(temp);

            Console.WriteLine(line);
        }

        Console.WriteLine();
    }
}
