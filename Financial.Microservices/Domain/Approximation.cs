using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Approximation
    {
        public double[] Polyval(double[] x, double[] a)
        {
            var result = new List<double>();
            for (int i = 0; i < x.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < a.Length; j++)
                    if (j > 1)
                        sum += a[j] * Math.Pow(x[i], a.Length - j);
                    else if (j == 1)
                        sum += a[j] * x[i];
                    else
                        sum += a[j];
                result.Add(sum);
            }
            return result.ToArray();
        }

        public static double[] Polyfit(double[] x, double[] y, int degree)
        {
            // Vandermonde matrix
            var v = new DenseMatrix(x.Length, degree + 1);
            for (int i = 0; i < v.RowCount; i++)
                for (int j = 0; j <= degree; j++) v[i, j] = Math.Pow(x[i], j);
            var yv = new DenseVector(y).ToColumnMatrix();
            var qr = v.QR();
            // Math.Net doesn't have an "economy" QR, so:
            // cut R short to square upper triangle, then recompute Q
            var r = qr.R.SubMatrix(0, degree + 1, 0, degree + 1);
            var q = v.Multiply(r.Inverse());
            var p = r.Inverse().Multiply(q.TransposeThisAndMultiply(yv));
            return p.Column(0).ToArray();
        }
    }
}
