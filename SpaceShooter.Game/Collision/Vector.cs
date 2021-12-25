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

    public float Magnitude
    {
        get { return (float)Math.Sqrt(X * X + Y * Y); }
    }

    public void Normalize()
    {
        float magnitude = Magnitude;
        _x /= magnitude;
        _y /= magnitude;
    }

    public float DotProduct(Vector vector)
    {
        return X * vector.X + Y * vector.Y;
    }

    public float DistanceTo(Vector vector)
    {
        return (float)Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2));
    }

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }

    public static Vector operator -(Vector a)
    {
        return new Vector(-a.X, -a.Y);
    }

    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y);
    }

    public static Vector operator *(Vector a, float b)
    {
        return new Vector(a.X * b, a.Y * b);
    }

    public static Vector operator *(Vector a, int b)
    {
        return new Vector(a.X * b, a.Y * b);
    }

    public static Vector operator *(Vector a, double b)
    {
        return new Vector((float)(a.X * b), (float)(a.Y * b));
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override string ToString()
    {
        return X + "," + Y;
    }

    public string ToString(bool rounded)
    {
        if (rounded)
        {
            return (int)Math.Round(X) + "," + (int)Math.Round(Y);
        }
        else
        {
            return ToString();
        }
    }


}