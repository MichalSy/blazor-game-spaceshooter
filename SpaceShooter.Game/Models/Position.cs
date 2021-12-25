namespace SpaceShooter.Game.Models;
public record Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x = 0, int y = 0)
        => (X, Y) = (x, y);

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

