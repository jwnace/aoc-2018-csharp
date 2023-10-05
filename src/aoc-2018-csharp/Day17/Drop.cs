namespace aoc_2018_csharp.Day17;

public record Drop(int X, int Y)
{
    public static implicit operator (int X, int Y)(Drop drop) => (drop.X, drop.Y);

    public bool TryMoveDown(Dictionary<(int X, int Y), char> grid, out Drop down)
    {
        if (Y >= grid.Keys.Max(x => x.Y))
        {
            down = this;
            return false;
        }

        down = this with { Y = Y + 1 };
        return !grid.TryGetValue(down, out _);
    }

    public bool TryMoveLeft(Dictionary<(int X, int Y), char> grid, out Drop left)
    {
        var below = this with { Y = Y + 1 };

        // if there is flowing water below, don't try to move left or right
        if (grid.TryGetValue(below, out var v) && v is '|')
        {
            left = this;
            return false;
        }

        if (TryMoveDown(grid, out _))
        {
            left = this;
            return false;
        }

        left = this with { X = X - 1 };
        return !grid.TryGetValue(left, out _);
    }

    public bool TryMoveRight(Dictionary<(int X, int Y), char> grid, out Drop right)
    {
        var below = this with { Y = Y + 1 };

        // if there is flowing water below, don't try to move left or right
        if (grid.TryGetValue(below, out var v) && v is '|')
        {
            right = this;
            return false;
        }

        if (TryMoveDown(grid, out _))
        {
            right = this;
            return false;
        }

        right = this with { X = X + 1 };
        return !grid.TryGetValue(right, out _);
    }
}
