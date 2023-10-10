namespace aoc_2018_csharp.Day20;

public class Room
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
}
