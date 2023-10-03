using System.Diagnostics;

namespace aoc_2018_csharp.Day15;

public static class Day15
{
    private static readonly string[] Input = File.ReadAllLines("Day15/day15.txt");

    public static int Part1() => Solve(Input, 1, animate: false);

    public static int Part2() => Solve(Input, 2, elfAttackPower: 29);

    public static int Solve(string[] input, int part, int elfAttackPower = 3, bool animate = false)
    {
        var elfDied = true;

        while (elfDied)
        {
            elfDied = false;

            var (top, left) = (Console.CursorTop, Console.CursorLeft);
            var grid = Grid.Parse(input, elfAttackPower);
            var inCombat = true;
            var round = 1;

            if (animate)
            {
                Console.WriteLine("Initially:");
                Console.WriteLine(grid);
            }

            while (inCombat)
            {
                var alreadyMovedThisTurn = new HashSet<int>();

                for (var row = 0; row <= input.Length - 1; row++)
                {
                    for (var col = 0; col <= input[0].Length - 1; col++)
                    {
                        var unit = grid.Units.FirstOrDefault(x => x.Position == new Coordinate(row, col));

                        if (unit is null)
                        {
                            continue;
                        }

                        if (alreadyMovedThisTurn.Contains(unit.Id))
                        {
                            continue;
                        }

                        alreadyMovedThisTurn.Add(unit.Id);

                        var targets = FindTargets(unit, grid.Units).ToList();

                        if (!targets.Any())
                        {
                            if (animate)
                            {
                                var s1 = round > 1 ? "s" : string.Empty;
                                Console.SetCursorPosition(left, top);
                                Console.WriteLine($"After {round} round{s1}:");
                                Console.WriteLine(grid);
                            }

                            if (!elfDied || part == 1)
                            {
                                return (round - 1) * grid.Units.Sum(x => x.HitPoints);
                            }

                            inCombat = false;
                            row = int.MaxValue - 1;
                            col = int.MaxValue - 1;
                            break;
                        }

                        var targetsInRange = GetTargetsInRange(unit, targets);

                        if (!targetsInRange.Any())
                        {
                            var openSquares = FindOpenSquaresInRangeOfTargets(grid, targets).ToList();

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

                                var path = grid.FindShortestPath(start, end);

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
                        }

                        if (!targetsInRange.Any())
                        {
                            targetsInRange = GetTargetsInRange(unit, targets);
                        }

                        if (!targetsInRange.Any())
                        {
                            continue;
                        }

                        var target = targetsInRange
                            .OrderBy(x => x.HitPoints)
                            .ThenBy(x => x.Position.Row)
                            .ThenBy(x => x.Position.Col)
                            .First();

                        target.HitPoints -= unit.AttackPower;

                        if (target.HitPoints <= 0)
                        {
                            if (target.Type == 'E')
                            {
                                elfDied = true;
                            }

                            grid.Units.Remove(target);
                        }
                    }
                }

                if (animate)
                {
                    var s = round > 1 ? "s" : string.Empty;
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine($"After {round} round{s}:");
                    Console.WriteLine(grid);
                }

                round++;
            }

            if (!elfDied || part == 1)
            {
                return (round - 1) * grid.Units.Sum(x => x.HitPoints);
            }

            elfAttackPower++;
        }

        throw new UnreachableException("No solution found");
    }

    private static List<Unit> GetTargetsInRange(Unit unit, List<Unit> targets)
    {
        var (row, col) = unit.Position;

        var neighbors = new[]
        {
            new Coordinate(row - 1, col),
            new Coordinate(row, col - 1),
            new Coordinate(row, col + 1),
            new Coordinate(row + 1, col)
        };

        return targets.Where(t => neighbors.Any(n => n == t.Position)).ToList();
    }

    private static IEnumerable<Unit> FindTargets(Unit player, List<Unit> units)
    {
        var targetType = player.Type == 'E' ? 'G' : 'E';
        return units.Where(u => u.Type == targetType);
    }

    private static IEnumerable<Coordinate> FindOpenSquaresInRangeOfTargets(Grid grid, List<Unit> targets)
    {
        foreach (var target in targets)
        {
            var (row, col) = target.Position;

            var neighbors = new[]
            {
                new Coordinate(row - 1, col),
                new Coordinate(row, col - 1),
                new Coordinate(row, col + 1),
                new Coordinate(row + 1, col)
            };

            foreach (var neighbor in neighbors)
            {
                if (grid.Map[neighbor] == '.' && grid.Units.All(u => u.Position != neighbor))
                {
                    yield return neighbor;
                }
            }
        }
    }
}
