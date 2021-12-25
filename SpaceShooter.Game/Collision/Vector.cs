namespace SpaceShooter.Game.Collision;

public record Vector
{
    public float OffsetPositionX { get; set; } = 0;
    public float OffsetPositionY { get; set; } = 0;

    private float _x;
    private float _y;

    public float X => OffsetPositionX + _x;
    public float Y => OffsetPositionY + _y;

    public Vector(float x, float y)
    {
        _x = x;
        _y = y;
    }

    public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);

    public void Normalize()
    {
        float magnitude = Magnitude;
        _x /= magnitude;
        _y /= magnitude;
    }

    public float DotProduct(Vector vector) => X * vector.X + Y * vector.Y;

    public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

    public override string ToString() => (int)Math.Round(X) + "," + (int)Math.Round(Y);
}