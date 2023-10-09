using System.Text;

namespace aoc_2018_csharp.Day20;

public static class Day20
{
    private static readonly string Input = File.ReadAllText("Day20/day20.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string input)
    {
        var rooms = BuildMap(input);
        var map = DrawMap(rooms);
        Console.WriteLine(map);
        return 0;
    }

    private static List<Room> BuildMap(string input)
    {
        var stack = new Stack<Room>();
        var rooms = new List<Room>();
        var room = new Room(0, 0);
        rooms.Add(room);

        foreach (var c in input)
        {
            switch (c)
            {
                case '^':
                {
                    break;
                }
                case '$':
                {
                    break;
                }
                case 'N':
                {
                    var (row, col) = (room.Row - 1, room.Col);
                    var newRoom = new Room(row, col);
                    var alreadyExists = rooms.Any(x => x.Row == row && x.Col == col);

                    if (alreadyExists)
                    {
                        newRoom = rooms.Single(x => x.Row == row && x.Col == col);
                    }

                    room.North = newRoom;
                    newRoom.South = room;

                    if (!alreadyExists)
                    {
                        rooms.Add(newRoom);
                    }

                    room = newRoom;
                    break;
                }
                case 'S':
                {
                    var (row, col) = (room.Row + 1, room.Col);
                    var newRoom = new Room(row, col);
                    var alreadyExists = rooms.Any(x => x.Row == row && x.Col == col);

                    if (alreadyExists)
                    {
                        newRoom = rooms.Single(x => x.Row == row && x.Col == col);
                    }

                    room.South = newRoom;
                    newRoom.North = room;

                    if (!alreadyExists)
                    {
                        rooms.Add(newRoom);
                    }

                    room = newRoom;
                    break;
                }
                case 'E':
                {
                    var (row, col) = (room.Row, room.Col + 1);
                    var newRoom = new Room(row, col);
                    var alreadyExists = rooms.Any(x => x.Row == row && x.Col == col);

                    if (alreadyExists)
                    {
                        newRoom = rooms.Single(x => x.Row == row && x.Col == col);
                    }

                    room.East = newRoom;
                    newRoom.West = room;

                    if (!alreadyExists)
                    {
                        rooms.Add(newRoom);
                    }

                    room = newRoom;
                    break;
                }
                case 'W':
                {
                    var (row, col) = (room.Row, room.Col - 1);
                    var newRoom = new Room(row, col);
                    var alreadyExists = rooms.Any(x => x.Row == row && x.Col == col);

                    if (alreadyExists)
                    {
                        newRoom = rooms.Single(x => x.Row == row && x.Col == col);
                    }

                    room.West = newRoom;
                    newRoom.East = room;

                    if (!alreadyExists)
                    {
                        rooms.Add(newRoom);
                    }

                    room = newRoom;
                    break;
                }
                case '(':
                {
                    stack.Push(room);
                    break;
                }
                case ')':
                {
                    room = stack.Pop();
                    break;
                }
                case '|':
                {
                    room = stack.Peek();
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(c), c, null);
                }
            }
        }

        return rooms;
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
            builder.Append((Row, Col) == (0, 0) ? 'X' : '.');
            builder.Append(East != null ? '>' : '#');

            builder.AppendLine();

            builder.Append('#');
            builder.Append(South != null ? 'v' : '#');
            builder.Append('#');

            return builder.ToString();
        }
    }
}
