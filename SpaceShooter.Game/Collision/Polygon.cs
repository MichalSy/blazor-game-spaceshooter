namespace SpaceShooter.Game.Collision;

public class Polygon
{
    private List<Vector> _points = new();
    private readonly List<Vector> _edges = new();

    public void BuildEdges()
    {
        Vector p1;
        Vector p2;
        _edges.Clear();
        for (int i = 0; i < _points.Count; i++)
        {
            p1 = _points[i];
            if (i + 1 >= _points.Count)
            {
                p2 = _points[0];
            }
            else
            {
                p2 = _points[i + 1];
            }
            _edges.Add(p2 - p1);
        }
    }

    public List<Vector> Edges
    {
        get { return _edges; }
    }

    public List<Vector> Points
    {
        get { return _points; }
        set { _points = value; }
    }



    public void Offset(float x, float y)
    {
        for (int i = 0; i < _points.Count; i++)
        {
            Vector p = _points[i];
            p.OffsetPositionX = x;
            p.OffsetPositionY = y;
            //_points[i] = new Vector(p.X + x, p.Y + y);
        }
    }

    public string ToSvgString()
    {
        string result = "";

        for (int i = 0; i < _points.Count; i++)
        {
            if (result != "") result += " ";
            result += _points[i].ToString(true) + " ";
        }

        return result.TrimEnd();
    }

}