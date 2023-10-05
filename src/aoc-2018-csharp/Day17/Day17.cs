using aoc_2018_csharp.Extensions;

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
        private int _maxY = int.MinValue;
        private int _minY = int.MaxValue;

        public Solver(string[] input)
        {
            _input = input;
            _grid = new char[3000, 3000];
        }

        public int Solve(int part)
        {
            ParseInput();
            Fall(500, 0);

            return CountWater(part);
        }

        private void ParseInput()
        {
            foreach (var line in _input)
            {
                var (leftKey, leftValue, _, rightStart, rightEnd) =
                    line.Split(new[] { "=", ", ", ".." }, StringSplitOptions.RemoveEmptyEntries);

                if (leftKey == "x")
                {
                    var x = int.Parse(leftValue);
                    var yStart = int.Parse(rightStart);
                    var yEnd = int.Parse(rightEnd);

                    for (var i = yStart; i <= yEnd; i++)
                    {
                        _grid[x, i] = '#';
                    }

                    _minY = Math.Min(yStart, _minY);
                    _maxY = Math.Max(yEnd, _maxY);
                }
                else
                {
                    var y = int.Parse(leftValue);
                    var xStart = int.Parse(rightStart);
                    var xEnd = int.Parse(rightEnd);

                    for (var i = xStart; i <= xEnd; i++)
                    {
                        _grid[i, y] = '#';
                    }

                    _minY = Math.Min(y, _minY);
                    _maxY = Math.Max(y, _maxY);
                }
            }
        }

        private void Fall(int x, int y)
        {
            UpdateCell(x, y, '|');

            while (IsSpaceVacant(x, y + 1))
            {
                y++;

                if (y > _maxY)
                {
                    return;
                }

                UpdateCell(x, y, '|');
            }

            while (true)
            {
                var leftSideCanFall = false;
                var rightSideCanFall = false;
                int left;
                int right;

                for (left = x; left >= 0; left--)
                {
                    if (IsSpaceVacant(left, y + 1))
                    {
                        leftSideCanFall = true;
                        break;
                    }

                    UpdateCell(left, y, '|');

                    if (IsSpaceTaken(left - 1, y))
                    {
                        break;
                    }
                }

                for (right = x; right < _grid.GetLength(0); right++)
                {
                    if (IsSpaceVacant(right, y + 1))
                    {
                        rightSideCanFall = true;
                        break;
                    }

                    UpdateCell(right, y, '|');

                    if (IsSpaceTaken(right + 1, y))
                    {
                        break;
                    }
                }

                if (leftSideCanFall && _grid[left, y] != '|')
                {
                    Fall(left, y);
                }

                if (rightSideCanFall && _grid[right, y] != '|')
                {
                    Fall(right, y);
                }

                if (leftSideCanFall || rightSideCanFall)
                {
                    return;
                }

                for (var i = left; i <= right; i++)
                {
                    UpdateCell(i, y, '~');
                }

                y--;
            }
        }

        private int CountWater(int part)
        {
            var count = 0;

            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                for (var y = _minY; y < _grid.GetLength(1); y++)
                {
                    if (part == 1 && (_grid[x, y] == '~' || _grid[x, y] == '|'))
                    {
                        count++;
                    }
                    else if (_grid[x, y] == '~')
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void UpdateCell(int x, int y, char c) => _grid[x, y] = c;

        private bool IsSpaceTaken(int x, int y) => _grid[x, y] is '#' or '~';

        private bool IsSpaceVacant(int x, int y) => !IsSpaceTaken(x, y);
    }
}
