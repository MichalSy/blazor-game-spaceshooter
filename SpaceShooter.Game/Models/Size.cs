namespace SpaceShooter.Game.Models;

public record Size
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Size(int x = 1, int y = 1)
        => (Width, Height) = (x, y);

    public override string ToString()
    {
        return $"{Width}, {Height}";
    }
}
