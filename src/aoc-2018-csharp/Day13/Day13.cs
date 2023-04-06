namespace aoc_2018_csharp.Day13;

public static class Day13
{
    private static readonly string[] Input = File.ReadAllLines("Day13/day13.txt");

    public static string Part1()
    {
        var carts = GetCarts();

        while (true)
        {
            var orderedCarts = GetOrderedCarts(carts);

            foreach (var cart in orderedCarts)
            {
                if (cart.TryMoveForward(carts) == false)
                {
                    return $"{cart.Col},{cart.Row}";
                }
            }
        }
    }

    public static string Part2()
    {
        var carts = GetCarts();

        while (true)
        {
            var orderedCarts = GetOrderedCarts(carts);

            foreach (var cart in orderedCarts)
            {
                cart.TryMoveForward(carts);
            }

            if (carts.Count == 1)
            {
                return $"{carts[0].Col},{carts[0].Row}";
            }
        }
    }

    private static List<Cart> GetCarts()
    {
        var carts = new List<Cart>();

        for (var row = 0; row < Input.Length; row++)
        {
            for (var col = 0; col < Input[row].Length; col++)
            {
                if (Input[row][col] is '<' or '>' or '^' or 'v')
                {
                    carts.Add(new Cart(row, col, Input[row][col]));
                }
            }
        }

        return carts;
    }

    private static List<Cart> GetOrderedCarts(List<Cart> carts) =>
        carts.OrderBy(c => c.Row)
            .ThenBy(c => c.Col)
            .ToList();

    private class Cart
    {
        private int _completedTurns;
        private char _direction;

        public int Row { get; private set; }
        public int Col { get; private set; }

        public Cart(int row, int col, char direction)
        {
            Row = row;
            Col = col;
            _direction = direction;
        }

        public bool TryMoveForward(List<Cart> carts)
        {
            (Row, Col) = _direction switch
            {
                '<' => (Row, Col - 1),
                '>' => (Row, Col + 1),
                '^' => (Row - 1, Col),
                'v' => (Row + 1, Col),
                _ => throw new ArgumentOutOfRangeException()
            };

            // check for collisions
            if (carts.Any(c => (c.Row, c.Col) == (Row, Col) && c != this))
            {
                carts.RemoveAll(c => (c.Row, c.Col) == (Row, Col));
                return false;
            }

            // check for intersections
            if (Input[Row][Col] == '+')
            {
                Turn();
            }
            else
            {
                _direction = _direction switch
                {
                    '<' when Input[Row][Col] == '/' => 'v',
                    '>' when Input[Row][Col] == '/' => '^',
                    '^' when Input[Row][Col] == '/' => '>',
                    'v' when Input[Row][Col] == '/' => '<',
                    '<' when Input[Row][Col] == '\\' => '^',
                    '>' when Input[Row][Col] == '\\' => 'v',
                    '^' when Input[Row][Col] == '\\' => '<',
                    'v' when Input[Row][Col] == '\\' => '>',
                    _ => _direction
                };
            }

            return true;
        }

        private void Turn()
        {
            _direction = (_completedTurns % 3) switch
            {
                // turn left
                0 when _direction is '<' => 'v',
                0 when _direction is '>' => '^',
                0 when _direction is '^' => '<',
                0 when _direction is 'v' => '>',
                // turn right
                2 when _direction is '<' => '^',
                2 when _direction is '>' => 'v',
                2 when _direction is '^' => '>',
                2 when _direction is 'v' => '<',
                // go straight
                _ => _direction
            };

            _completedTurns++;
        }
    }
}
