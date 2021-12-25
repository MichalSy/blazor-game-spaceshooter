namespace SpaceShooter.Game.Collision;

public class Polygon
{
    #region Member - Edges
    private readonly List<Vector> _edges = new();
    public List<Vector> Edges
    {
        get { return _edges; }
    } 
    #endregion

    #region Member - Points
    private List<Vector> _points = new();
    public List<Vector> Points
    {
        get { return _points; }
        set { _points = value; }
    } 
    #endregion

    public void Offset(float x, float y)
    {
        foreach (var point in _points)
        {
            point.OffsetPositionX = x;
            point.OffsetPositionY = y;
        }
    }

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

    public string ToSvgString() => string.Join(' ', _points);
}