namespace SpaceShooter.Game.Models;
public record MousePoint
{
    public int MouseX { get; set; }
    public int MouseY { get; set; }

    public MousePoint(int mouseX = 0, int mouseY = 0)
        => (MouseX, MouseY) = (mouseX, mouseY);

    public override string ToString()
    {
        return $"{MouseX}, {MouseY}";
    }
}