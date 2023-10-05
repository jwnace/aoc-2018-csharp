namespace aoc_2018_csharp.Day17;

public static class Day17
{
    private static readonly string[] Input = File.ReadAllLines("Day17/day17.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input)
    {
        var solver = new Solver(input);
        return solver.Solve(1);
    }

    public static int Solve2(string[] input)
    {
        var solver = new Solver(input);
        return solver.Solve(2);
    }

    private class Solver
    {
        private readonly string[] _input;
        private char[,] grid;
        private int maxY = 0;
        private int minY = int.MaxValue;

        public Solver(string[] input)
        {
            _input = input;
        }

        public int Solve(int part)
        {
            var x = 3000;
            var y = 3000;

            grid = new char[x, y];

            foreach (var line in _input)
            {
                var l = line.Split(new[] { '=', ',', '.' });

                if (l[0] == "x")
                {
                    x = int.Parse(l[1]);
                    y = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = y; a <= len; a++)
                    {
                        grid[x, a] = '#';
                    }
                }
                else
                {
                    y = int.Parse(l[1]);
                    x = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = x; a <= len; a++)
                    {
                        grid[a, y] = '#';
                    }
                }

                if (y > maxY)
                {
                    maxY = y;
                }

                if (y < minY)
                {
                    minY = y;
                }
            }

            var springX = 500;
            var springY = 0;

            // fill with water
            GoDown(springX, springY);

            // count spaces with water
            var t = 0;
            for (y = minY; y < grid.GetLength(1); y++)
            {
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    if (part == 1 && (grid[x, y] == '~' || grid[x, y] == '|'))
                    {
                        t++;
                    }
                    else if (grid[x,y] == '~')
                    {
                        t++;
                    }
                }
            }

            return t;
        }

        private bool SpaceTaken(int x, int y)
        {
            return grid[x, y] == '#' || grid[x, y] == '~';
        }

        public void GoDown(int x, int y)
        {
            grid[x, y] = '|';
            while (grid[x, y + 1] != '#' && grid[x, y + 1] != '~')
            {
                y++;
                if (y > maxY)
                {
                    return;
                }

                grid[x, y] = '|';
            }

            ;

            do
            {
                bool goDownLeft = false;
                bool goDownRight = false;

                // find boundaries
                int minX;
                for (minX = x; minX >= 0; minX--)
                {
                    if (SpaceTaken(minX, y + 1) == false)
                    {
                        goDownLeft = true;
                        break;
                    }

                    grid[minX, y] = '|';

                    if (SpaceTaken(minX - 1, y))
                    {
                        break;
                    }
                }

                int maxX;
                for (maxX = x; maxX < grid.GetLength(0); maxX++)
                {
                    if (SpaceTaken(maxX, y + 1) == false)
                    {
                        goDownRight = true;

                        break;
                    }

                    grid[maxX, y] = '|';

                    if (SpaceTaken(maxX + 1, y))
                    {
                        break;
                    }
                }

                // handle water falling
                if (goDownLeft)
                {
                    if (grid[minX, y] != '|')
                    {
                        GoDown(minX, y);
                    }
                }

                if (goDownRight)
                {
                    if (grid[maxX, y] != '|')
                    {
                        GoDown(maxX, y);
                    }
                }

                if (goDownLeft || goDownRight)
                {
                    return;
                }

                // fill row
                for (int a = minX; a < maxX + 1; a++)
                {
                    grid[a, y] = '~';
                }

                y--;
            } while (true);
        }
    }
}
