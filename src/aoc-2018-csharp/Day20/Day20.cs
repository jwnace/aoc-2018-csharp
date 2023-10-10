namespace aoc_2018_csharp.Day20;

public static class Day20
{
    private static readonly string Input = File.ReadAllText("Day20/day20.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string input) => BuildMap(input).Max(GetShortestPath);

    private static int Solve2(string input) => BuildMap(input).Select(GetShortestPath).Count(x => x >= 1000);

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

    private static int GetShortestPath(Room room)
    {
        var queue = new Queue<(Room room, int steps)>();
        var visited = new HashSet<Room>();
        queue.Enqueue((room, 0));

        while (queue.Any())
        {
            var (currentRoom, steps) = queue.Dequeue();
            visited.Add(currentRoom);

            if ((currentRoom.Row, currentRoom.Col) == (0, 0))
            {
                return steps;
            }

            if (currentRoom.North != null && !visited.Contains(currentRoom.North))
            {
                queue.Enqueue((currentRoom.North, steps + 1));
            }

            if (currentRoom.South != null && !visited.Contains(currentRoom.South))
            {
                queue.Enqueue((currentRoom.South, steps + 1));
            }

            if (currentRoom.East != null && !visited.Contains(currentRoom.East))
            {
                queue.Enqueue((currentRoom.East, steps + 1));
            }

            if (currentRoom.West != null && !visited.Contains(currentRoom.West))
            {
                queue.Enqueue((currentRoom.West, steps + 1));
            }
        }

        throw new Exception("No path found");
    }
}
