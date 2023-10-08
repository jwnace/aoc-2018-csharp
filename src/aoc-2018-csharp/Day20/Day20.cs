using System.Text;

namespace aoc_2018_csharp.Day20;

public static class Day20
{
    private static readonly string Input = File.ReadAllText("Day20/day20.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string input)
    {
        var rooms = new List<Room>();
        var (row, col) = (0, 0);
        var room = new Room(row, col);
        rooms.Add(room);

        for (var i = 1; i < input.Length - 1; i++)
        {
            switch (input[i])
            {
                case 'N':
                {
                    row--;
                    var newRoom = new Room(row, col);
                    room.North = newRoom;
                    newRoom.South = room;
                    rooms.Add(newRoom);
                    room = newRoom;
                    break;
                }
                case 'S':
                {
                    row++;
                    var newRoom = new Room(row, col);
                    room.South = newRoom;
                    newRoom.North = room;
                    rooms.Add(newRoom);
                    room = newRoom;
                    break;
                }
                case 'E':
                {
                    col++;
                    var newRoom = new Room(row, col);
                    room.East = newRoom;
                    newRoom.West = room;
                    rooms.Add(newRoom);
                    room = newRoom;
                    break;
                }
                case 'W':
                {
                    col--;
                    var newRoom = new Room(row, col);
                    room.West = newRoom;
                    newRoom.East = room;
                    rooms.Add(newRoom);
                    room = newRoom;
                    break;
                }
                case '(':
                    throw new NotImplementedException();
                    break;
                case ')':
                    throw new NotImplementedException();
                    break;
                case '|':
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        Console.WriteLine(DrawMap(rooms));

        return 0;
    }

    public static int Solve2(string input)
    {
        throw new NotImplementedException();
    }

    private static string DrawMap(List<Room> rooms)
    {
        var minCol = rooms.Min(r => r.Col);
        var maxCol = rooms.Max(r => r.Col);
        var minRow = rooms.Min(r => r.Row);
        var maxRow = rooms.Max(r => r.Row);

        var builder = new StringBuilder();

        for (var row = minRow; row <= maxRow; row++)
        {
            for (var i = 0; i < 3; i++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    var room = rooms.FirstOrDefault(room => room.Row == row && room.Col == col);

                    for (var c1 = 0; c1 < 3; c1++)
                    {
                        if (room == null)
                        {
                            builder.Append(' ');
                        }
                        else
                        {
                            var roomString = room.ToString();
                            var foo = roomString.Split(Environment.NewLine);
                            var bar = foo[i];
                            var baz = bar[c1];
                            builder.Append(baz);
                        }
                    }
                }

                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private class Room
    {
        public int Row { get; }
        public int Col { get; }

        public Room? North { get; set; }
        public Room? South { get; set; }
        public Room? East { get; set; }
        public Room? West { get; set; }

        public Room(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append('#');
            builder.Append(North != null ? '^' : '#');
            builder.Append('#');

            builder.AppendLine();

            builder.Append(West != null ? '<' : '#');
            builder.Append('.');
            builder.Append(East != null ? '>' : '#');

            builder.AppendLine();

            builder.Append('#');
            builder.Append(South != null ? 'v' : '#');
            builder.Append('#');

            return builder.ToString();
        }
    }
}
