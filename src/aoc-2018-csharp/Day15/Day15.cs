using System.Text;

namespace aoc_2018_csharp.Day15;

public static class Day15
{
    private static readonly string[] Input = File.ReadAllLines("Day15/day15.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    private record Coordinate(int Row, int Col);

    private class Unit
    {
        public Unit(int id, Coordinate position, int hitPoints, int attackPower, char type)
        {
            this.Id = id;
            this.Position = position;
            this.HitPoints = hitPoints;
            this.AttackPower = attackPower;
            this.Type = type;
        }

        public override string ToString() => $"{Type}({HitPoints})";
        public int Id { get; init; }
        public Coordinate Position { get; set; }
        public int HitPoints { get; set; }
        public int AttackPower { get; init; }
        public char Type { get; init; }

        // public void Deconstruct(out int id, out Coordinate position, out int hitPoints, out int attackPower, out char type)
        // {
        //     id = this.Id;
        //     position = this.Position;
        //     hitPoints = this.HitPoints;
        //     attackPower = this.AttackPower;
        //     type = this.Type;
        // }
    }

    public static int Solve1(string[] input)
    {
        var maxRow = input.Length - 1;
        var maxCol = input[0].Length - 1;
        Dictionary<Coordinate, char> grid = BuildGrid(input);
        var units = BuildUnits(input).ToList();
        var round = 1;

        Console.WriteLine("Initially:");
        DrawGrid(grid, units);

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
                        var s1 = round > 1 ? "s" : string.Empty;
                        Console.WriteLine($"After {round} round{s1}:");
                        DrawGrid(grid, units);
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

            var s = round > 1 ? "s" : string.Empty;
            Console.WriteLine($"After {round} round{s}:");
            DrawGrid(grid, units);

            round++;
        }

        return round * units.Sum(x => x.HitPoints);
    }

    public static int Solve2(string[] input)
    {
        var elfDied = true;
        var elfAttackPower = 3;

        while (elfDied)
        {
            elfAttackPower++;
            elfDied = false;

            Console.WriteLine($"Trying elf attack power {elfAttackPower}");

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

                Console.WriteLine($"round {round}");

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
                                var s1 = round > 1 ? "s" : string.Empty;
                                Console.WriteLine($"After {round} round{s1}:");
                                DrawGrid(grid, units);
                                Console.WriteLine("Combat ended without an elf dying (early return)");
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
                var s1 = round > 1 ? "s" : string.Empty;
                Console.WriteLine($"After {round} round{s1}:");
                DrawGrid(grid, units);
                Console.WriteLine("Combat ended without an elf dying (early return)");
                return (round - 1) * units.Sum(x => x.HitPoints);
            }
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

    // public static int Solve2(string[] input)
    // {
    //     var elfDied = true;
    //     var elfAttackPower = 200;
    //
    //     while (elfDied)
    //     {
    //         elfAttackPower++;
    //
    //         Console.WriteLine($"Trying elf attack power {elfAttackPower}");
    //
    //         elfDied = false;
    //         var maxRow = input.Length - 1;
    //         var maxCol = input[0].Length - 1;
    //         var grid = BuildGrid(input);
    //         var hitPoints = new Dictionary<(int, int), int>();
    //
    //         for (var row = 0; row <= maxRow; row++)
    //         {
    //             for (var col = 0; col <= maxCol; col++)
    //             {
    //                 if (grid[(row, col)] is ('E' or 'G'))
    //                 {
    //                     hitPoints[(row, col)] = 200;
    //                 }
    //             }
    //         }
    //
    //         // DrawGrid(grid, hitPoints);
    //
    //         var inCombat = true;
    //         var round = 1;
    //
    //         while (inCombat)
    //         {
    //             var alreadyMoved = new HashSet<(int Row, int Col)>();
    //
    //             // iterate in reading order
    //             for (var row = 0; row <= maxRow; row++)
    //             {
    //                 for (var col = 0; col <= maxCol; col++)
    //                 {
    //                     if (alreadyMoved.Contains((row, col)))
    //                     {
    //                         continue;
    //                     }
    //
    //                     // if there is a unit at this location, it should take its turn
    //                     if (grid[(row, col)] is not ('E' or 'G'))
    //                     {
    //                         continue;
    //                     }
    //
    //                     // find all targets
    //                     var targets = FindTargets(grid, row, col);
    //
    //                     // if no targets, combat ends
    //                     if (!targets.Any())
    //                     {
    //                         if (!elfDied)
    //                         {
    //                             var s = round > 1 ? "s" : string.Empty;
    //                             Console.WriteLine($"After {round} round{s}:");
    //                             DrawGrid(grid, hitPoints);
    //                             Console.WriteLine("Combat ended without an elf dying (early return)");
    //                             return (round - 1) * hitPoints.Sum(x => x.Value);
    //                         }
    //
    //                         inCombat = false;
    //
    //                         // HACK: this is a hack to break out of the nested for loops
    //                         row = int.MaxValue - 1;
    //                         col = int.MaxValue - 1;
    //                         continue;
    //                     }
    //
    //                     // if unit is in range of a target, unit attacks
    //                     var neighbors = new[]
    //                     {
    //                         (row - 1, col),
    //                         (row, col - 1),
    //                         (row, col + 1),
    //                         (row + 1, col)
    //                     };
    //
    //                     var inRange = neighbors.Where(n => targets.Any(t => t.Key == n)).ToList();
    //
    //                     if (inRange.Any())
    //                     {
    //                         alreadyMoved.Add((row, col));
    //
    //                         // attack
    //                         var target = inRange.OrderBy(x => hitPoints[x])
    //                             .ThenBy(x => x.Item1)
    //                             .ThenBy(x => x.Item2)
    //                             .First();
    //
    //                         hitPoints[target] -= grid[target] == 'G' ? elfAttackPower : 3;
    //
    //                         if (hitPoints[target] <= 0)
    //                         {
    //                             if (grid[target] == 'E')
    //                             {
    //                                 elfDied = true;
    //                             }
    //
    //                             grid[target] = '.';
    //                             hitPoints.Remove(target);
    //                         }
    //                     }
    //                     else
    //                     {
    //                         var openSquares = FindOpenSquares(targets, grid);
    //
    //                         // if no open squares adjacent to any targets, unit ends turn
    //                         if (!openSquares.Any())
    //                         {
    //                             continue;
    //                         }
    //
    //                         var paths = new List<(int Distance, (int Row, int Col) FirstStep)>();
    //
    //                         // if open squares adjacent to any targets, unit moves
    //                         foreach (var openSquare in openSquares)
    //                         {
    //                             // find shortest path to each open square
    //                             var start = (row, col);
    //                             var end = openSquare;
    //
    //                             var path = FindShortestPath(grid, start, end);
    //
    //                             // if multiple shortest paths, choose reading order
    //                             // if multiple shortest paths in reading order, choose first step in reading order
    //                             paths.Add(path);
    //                         }
    //
    //                         var chosenPath = paths.OrderBy(x => x.Distance)
    //                             .ThenBy(x => x.FirstStep.Row)
    //                             .ThenBy(x => x.FirstStep.Col)
    //                             .First();
    //
    //                         if (chosenPath.Distance == int.MaxValue)
    //                         {
    //                             continue;
    //                         }
    //
    //                         // move
    //                         grid[chosenPath.FirstStep] = grid[(row, col)];
    //                         grid[(row, col)] = '.';
    //                         hitPoints[chosenPath.FirstStep] = hitPoints[(row, col)];
    //                         hitPoints.Remove((row, col));
    //                         alreadyMoved.Add(chosenPath.FirstStep);
    //
    //                         // try to attack again
    //                         {
    //                             // if unit is in range of a target, unit attacks
    //                             var newNeighbors = new[]
    //                             {
    //                                 (chosenPath.FirstStep.Row - 1, chosenPath.FirstStep.Col),
    //                                 (chosenPath.FirstStep.Row, chosenPath.FirstStep.Col - 1),
    //                                 (chosenPath.FirstStep.Row, chosenPath.FirstStep.Col + 1),
    //                                 (chosenPath.FirstStep.Row + 1, chosenPath.FirstStep.Col)
    //                             };
    //
    //                             var newInRange = newNeighbors.Where(n => targets.Any(t => t.Key == n)).ToList();
    //
    //                             if (newInRange.Any())
    //                             {
    //                                 // this is redundant at this point
    //                                 // alreadyMoved.Add((row, col));
    //
    //                                 // attack
    //                                 var target = newInRange.OrderBy(x => hitPoints[x])
    //                                     .ThenBy(x => x.Item1)
    //                                     .ThenBy(x => x.Item2)
    //                                     .First();
    //
    //                                 hitPoints[target] -= grid[target] == 'G' ? elfAttackPower : 3;
    //
    //                                 if (hitPoints[target] <= 0)
    //                                 {
    //                                     if (grid[target] == 'E')
    //                                     {
    //                                         elfDied = true;
    //                                     }
    //
    //                                     grid[target] = '.';
    //                                     hitPoints.Remove(target);
    //                                 }
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //
    //             // var s = round > 1 ? "s" : string.Empty;
    //             // Console.WriteLine($"After {round} round{s}:");
    //             // DrawGrid(grid, hitPoints);
    //             round++;
    //         }
    //
    //         if (!elfDied)
    //         {
    //             var s = round > 1 ? "s" : string.Empty;
    //             Console.WriteLine($"After {round} round{s}:");
    //             DrawGrid(grid, hitPoints);
    //             Console.WriteLine("Combat ended without an elf dying (end of loop)");
    //             return (round - 1) * hitPoints.Sum(x => x.Value);
    //         }
    //     }
    //
    //     throw new Exception("No solution found");
    // }

    // private static int GetHitPoints((int, int) coordinate, Dictionary<(int, int), int> hitPoints)
    // {
    //     var hp = hitPoints.TryGetValue(coordinate, out var value) ? value : 200;
    //     hitPoints[coordinate] = hp;
    //     return hp;
    // }

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

            for (var col = 0; col <= maxCol; col++)
            {
                if (units.Any(u => u.Position == new Coordinate(row, col)))
                {
                    line.Append(units.Single(u => u.Position == new Coordinate(row, col)));
                    line.Append(' ');
                }
            }

            Console.WriteLine(line);
        }

        Console.WriteLine();
        // Thread.Sleep(2000);
    }
}
