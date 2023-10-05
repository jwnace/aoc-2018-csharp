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
        private readonly char[,] _grid;
        private int _maxY;
        private int _minY = int.MaxValue;

        public Solver(string[] input)
        {
            _input = input;
            _grid = new char[3000, 3000];
        }

        public int Solve(int part)
        {
            int x;
            int y;

            foreach (var line in _input)
            {
                var l = line.Split('=', ',', '.');

                if (l[0] == "x")
                {
                    x = int.Parse(l[1]);
                    y = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = y; a <= len; a++)
                    {
                        _grid[x, a] = '#';
                    }
                }
                else
                {
                    y = int.Parse(l[1]);
                    x = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = x; a <= len; a++)
                    {
                        _grid[a, y] = '#';
                    }
                }

                if (y > _maxY)
                {
                    _maxY = y;
                }

                if (y < _minY)
                {
                    _minY = y;
                }
            }

            const int springX = 500;
            const int springY = 0;

            // fill with water
            GoDown(springX, springY);

            // count spaces with water
            var t = 0;
            for (y = _minY; y < _grid.GetLength(1); y++)
            {
                for (x = 0; x < _grid.GetLength(0); x++)
                {
                    if (part == 1 && (_grid[x, y] == '~' || _grid[x, y] == '|'))
                    {
                        t++;
                    }
                    else if (_grid[x, y] == '~')
                    {
                        t++;
                    }
                }
            }

            return t;
        }

        private bool SpaceTaken(int x, int y)
        {
            return _grid[x, y] == '#' || _grid[x, y] == '~';
        }

        private void GoDown(int x, int y)
        {
            _grid[x, y] = '|';
            while (_grid[x, y + 1] != '#' && _grid[x, y + 1] != '~')
            {
                y++;
                if (y > _maxY)
                {
                    return;
                }

                _grid[x, y] = '|';
            }

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

                    _grid[minX, y] = '|';

                    if (SpaceTaken(minX - 1, y))
                    {
                        break;
                    }
                }

                int maxX;
                for (maxX = x; maxX < _grid.GetLength(0); maxX++)
                {
                    if (SpaceTaken(maxX, y + 1) == false)
                    {
                        goDownRight = true;

                        break;
                    }

                    _grid[maxX, y] = '|';

                    if (SpaceTaken(maxX + 1, y))
                    {
                        break;
                    }
                }

                // handle water falling
                if (goDownLeft)
                {
                    if (_grid[minX, y] != '|')
                    {
                        GoDown(minX, y);
                    }
                }

                if (goDownRight)
                {
                    if (_grid[maxX, y] != '|')
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
                    _grid[a, y] = '~';
                }

                y--;
            } while (true);
        }
    }
}
