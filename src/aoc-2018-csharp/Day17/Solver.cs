using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day17;

public class Solver
{
    private readonly Dictionary<(int X, int Y), char> _grid = new();

    private int _minX = int.MaxValue;
    private int _maxX = int.MinValue;
    private int _maxY = int.MinValue;
    private int _minY = int.MaxValue;

    public Dictionary<(int X, int Y), char> Solve(string[] input)
    {
        ParseInput(input);
        Fall(500, 0);

        return _grid;
    }

    private void ParseInput(string[] input)
    {
        foreach (var line in input)
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
                    _grid[(x, i)] = '#';
                }

                _minX = Math.Min(x - 1, _minX);
                _maxX = Math.Max(x + 1, _maxX);

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
                    _grid[(i, y)] = '#';
                }

                _minX = Math.Min(xStart - 1, _minX);
                _maxX = Math.Max(xEnd + 1, _maxX);

                _minY = Math.Min(y, _minY);
                _maxY = Math.Max(y, _maxY);
            }
        }
    }

    private void Fall(int x, int y)
    {
        UpdateCell(x, y, '|');

        while (SpaceIsVacant(x, y + 1))
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

            for (left = x; left >= _minX; left--)
            {
                if (SpaceIsVacant(left, y + 1))
                {
                    leftSideCanFall = true;
                    break;
                }

                UpdateCell(left, y, '|');

                if (SpaceIsTaken(left - 1, y))
                {
                    break;
                }
            }

            for (right = x; right <= _maxX; right++)
            {
                if (SpaceIsVacant(right, y + 1))
                {
                    rightSideCanFall = true;
                    break;
                }

                UpdateCell(right, y, '|');

                if (SpaceIsTaken(right + 1, y))
                {
                    break;
                }
            }

            if (leftSideCanFall && !SpaceHasFallingWater(left, y))
            {
                Fall(left, y);
            }

            if (rightSideCanFall && !SpaceHasFallingWater(right, y))
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

    private void UpdateCell(int x, int y, char c)
    {
        if (x < _minX || x > _maxX || y < _minY || y > _maxY)
        {
            return;
        }

        _grid[(x, y)] = c;
    }

    private bool SpaceIsVacant(int x, int y) => !SpaceIsTaken(x, y);

    private bool SpaceIsTaken(int x, int y) => _grid.TryGetValue((x, y), out var v) && v is '#' or '~';

    private bool SpaceHasFallingWater(int x, int y) => _grid.TryGetValue((x, y), out var v) && v is '|';
}
