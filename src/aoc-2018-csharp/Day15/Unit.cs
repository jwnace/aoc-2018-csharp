namespace aoc_2018_csharp.Day15;

public class Unit
{
    public Unit(int id, Coordinate position, int hitPoints, int attackPower, char type)
    {
        Id = id;
        Position = position;
        HitPoints = hitPoints;
        AttackPower = attackPower;
        Type = type;
    }

    public override string ToString() => $"{Type}({HitPoints})";
    public int Id { get; }
    public Coordinate Position { get; set; }
    public int HitPoints { get; set; }
    public int AttackPower { get; }
    public char Type { get; }

    // public void Deconstruct(out int id, out Coordinate position, out int hitPoints, out int attackPower, out char type)
    // {
    //     id = this.Id;
    //     position = this.Position;
    //     hitPoints = this.HitPoints;
    //     attackPower = this.AttackPower;
    //     type = this.Type;
    // }
}
