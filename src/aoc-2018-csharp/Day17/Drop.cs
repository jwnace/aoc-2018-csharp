using System.Linq.Expressions;

namespace aoc_2018_csharp.Day17;

public record Drop(int X, int Y)
{
    public bool TryMoveDown(
        IReadOnlySet<(int X, int Y)> clay,
        IReadOnlySet<(int X, int Y)> water,
        HashSet<(int X, int Y)> seenDown,
        out Drop down)
    {
        var (newX, newY) = (X, Y + 1);
        var newPosition = (newX, newY);

        down = new(newX, newY);

        var canMoveDown = !clay.Contains(newPosition) &&
                          !water.Contains(newPosition) &&
                          newY <= clay.Max(x => x.Y);

        return canMoveDown;
    }

    public bool TryMoveLeft(
        IReadOnlySet<(int X, int Y)> clay,
        IReadOnlySet<(int X, int Y)> water,
        HashSet<(int X, int Y)> seenLeft,
        out Drop left)
    {
        var floor = (X, Y + 1);
        var (newX, newY) = (X - 1, Y);
        var newPosition = (newX, newY);
        var newFloor = (newX, newY + 1);

        left = new(newX, newY);

        var canMoveLeft = !clay.Contains(newPosition) &&
                          !water.Contains(newPosition) &&
                          !seenLeft.Contains(newPosition) &&
                          // stop moving to the side if there is no clay or water directly under us
                          (clay.Contains(floor) || water.Contains(floor)) &&
                          // stop moving to the side if we can fall forever
                          clay.Any(x => x.X == X && x.Y > Y) &&
                          // there is water under me, but nothing under the next position
                          !(water.Contains(floor) && !water.Contains(newFloor) && !clay.Contains(newFloor)) &&
                          newY < clay.Max(x => x.Y);

        if (!canMoveLeft && seenLeft.Contains((newX, newY)))
        {
            seenLeft.Add((X, Y));
        }

        return canMoveLeft;
    }

    public bool TryMoveRight(
        IReadOnlySet<(int X, int Y)> clay,
        IReadOnlySet<(int X, int Y)> water,
        HashSet<(int X, int Y)> seenRight,
        out Drop right)
    {
        var floor = (X, Y + 1);
        var (newX, newY) = (X + 1, Y);
        var newPosition = (newX, newY);
        var newFloor = (newX, newY + 1);

        right = new(newX, newY);

        var canMoveRight = !clay.Contains(newPosition) &&
                           !water.Contains(newPosition) &&
                           !seenRight.Contains(newPosition) &&
                           // stop moving to the side if there is no clay or water directly under us
                           (clay.Contains(floor) || water.Contains(floor)) &&
                           // stop moving to the side if we can fall forever
                           clay.Any(x => x.X == X && x.Y > Y) &&
                           // there is water under me, but nothing under the next position
                           !(water.Contains(floor) && !water.Contains(newFloor) && !clay.Contains(newFloor)) &&
                           newY < clay.Max(x => x.Y);

        if (!canMoveRight && seenRight.Contains((newX, newY)))
        {
            seenRight.Add((X, Y));
        }

        return canMoveRight;
    }
}
