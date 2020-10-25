namespace Domain
{
    public class CrossPointComputator
    {
        public Point TwoLinesCrossPoint(Line first, Line second)
        {
            var x = (second.B - first.B) / (first.A - second.A);
            var y = first.A * x + first.B;
            return new Point(x, y);
        }
    }
}
