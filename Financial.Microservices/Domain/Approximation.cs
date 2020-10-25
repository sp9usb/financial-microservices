using System;

namespace Domain
{
    public class Approximation
    {
        private readonly double[] _factorCollection;

        public Approximation(double[] factorCollection)
        {
            _factorCollection = factorCollection;
        }

        public double ComputeResultFor(double x)
        {
            // Compute the polynomial result for x
            // W(x)= a[0]*x^n-1 + x[1]*x^n-2 + ... + a[n-2]*x + a[n-1]
            double sum = _factorCollection[_factorCollection.Length - 2] * x + _factorCollection[_factorCollection.Length - 1];
            for (int i = 0; i < _factorCollection.Length - 2; i++)
                sum += _factorCollection[i] * Math.Pow(x, _factorCollection.Length - i - 1);
            return sum;
        }
    }
}
