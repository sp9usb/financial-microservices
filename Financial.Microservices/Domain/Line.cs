namespace Domain
{
    public class Line
    {
        public double A { get; } = 1;
        public double B { get; }

        public Line(double a, double b)
        {
            A = a;
            B = b;
        }

        public Line(double b) : this(1, b) { }
    }
}
