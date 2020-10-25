using System;

namespace Domain
{
    public class LineComputator
    {
        public double AngleOf(Line first, Line second)
        {
            var radian = 180 / Math.PI;

            return Math.Atan(Math.Abs((second.A - first.A) / (1 + first.A * second.A))) * radian;
        }
    }
}
