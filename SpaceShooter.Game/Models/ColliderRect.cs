namespace SpaceShooter.Game.Models;
public record ColliderRect
{
    public Position Position { get; set; }
    public Size Size { get; set; }

    public ColliderRect(Position position, Size size)
        => (Position, Size) = (position, size);

    public override string ToString()
    {
        return $"{Position} ({Size})";
    }
}
