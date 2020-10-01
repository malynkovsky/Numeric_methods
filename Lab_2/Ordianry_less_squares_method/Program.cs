using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Ordianry_less_squares_method
{

    public class Polynomial
    {
        private List<double> _array;
        
        // Инициализирует новый полином заданной строкой коэффициентов
        public Polynomial(IEnumerable<double> arrayCoefficients)
        {
            _array = new List<double>(arrayCoefficients);
        }
        
        // Инициализирует новый полином по старшей степени при этом все коэффициенты будут равны 0
        public Polynomial(int headPow)
        {
            _array = new List<double>(headPow);
            for (int i = 0; i < headPow; i++)
                _array.Add(0D);
        }
        
        // Доступ к коэффициенту полинома по индексу
        double this[int i]
        {
            get
            {
                if (i < 0 || i >= _array.Count) throw new IndexOutOfRangeException();
                return _array[i];
            }

            set
            {
                if (i < 0 || i >= _array.Count) throw new IndexOutOfRangeException();
                _array[i] = value;
            }
        }
        
        // Старшая степень полинома
        public int HeadPow
        {
            get
            {
                return _array.Count;
            }
        }

        // Строка коэффициентов
        public IEnumerable<double> LineCoefficients
        {
            get
            {
                return _array;
            }
        }
        
        // Выполняет сложение двух полиномов
        public static Polynomial operator +(Polynomial poly1, Polynomial poly2)
        {
            Polynomial maxPoly = maxPowPoly(poly1, poly2);
            Polynomial minPoly = minPowPoly(poly1, poly2);

            Polynomial resultPoly = new Polynomial(maxPoly.LineCoefficients);

            for (int i = 0; i < minPoly.HeadPow; i++)
                resultPoly[maxPoly.HeadPow - 1 -i] = maxPoly[maxPoly.HeadPow - 1 - i] + minPoly[minPoly.HeadPow - 1 - i];

            return resultPoly;
        }
        
        // Умножает полином на константу
        public static Polynomial operator *(Polynomial poly1, double k)
        {
            Polynomial resultPoly = new Polynomial(poly1.LineCoefficients);

            for (int i = 0; i < poly1.HeadPow; i++) resultPoly[i] *= k;

            return resultPoly;
        }
        
        // Перемножает между собой два полинома
        public static Polynomial operator *(Polynomial poly1, Polynomial poly2)
        {
            Polynomial resultPoly = new Polynomial(poly1.HeadPow + poly2.HeadPow - 1);
            resultPoly._array.Select(x => 0);

            for (int i = 0; i < poly1.HeadPow; i++)
                for (int j = 0; j < poly2.HeadPow; j++)
                    resultPoly[i + j] += poly1[i] * poly2[j];

            return resultPoly;
        }
        
        // Вычисляет производную от полинома
        public Polynomial Derivative()
        {
            Polynomial result = new Polynomial(this.LineCoefficients);

            for (int i = 0; i < this.HeadPow - 1; i++)
                result[i] = result[i + 1] * (i + 1);

            result._array.RemoveAt(result.HeadPow - 1);
            
            if (result._array.Count == 0) result._array.Add(0);

            return result;
        }

        // Вычисляет интеграл(неопределённый) от полинома
        public Polynomial Integral()
        {
            Polynomial result = new Polynomial(new double[this.HeadPow + 1]);
            //Polynomial result = new Polynomial(this.LineCoefficients);
            result[this.HeadPow] = 0;
            for (int i = 0; i < this.HeadPow; i++)
            {
                result[i] = this[i] / (this.HeadPow - i);
            }
                
            
            if (result._array.Count == 0) result._array.Add(0);

            return result;
        }

        // Преобразует полином в строку
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = this.HeadPow - 1; i >= 0 ; i--)
            {
                //if (this[this.HeadPow - 1 - i] == 0 && this.HeadPow - 1 - i != 0) continue;
                if ((true) && (i != this.HeadPow - 1)) sb.Append("+ ");
                if (i == 0)
                {
                    sb.Append(this[this.HeadPow - 1 - i] + " ");
                    continue;
                }
                if (i == 1)
                {
                    sb.Append(this[this.HeadPow - 1 - i] + "*x ");
                    continue;
                }
                sb.Append(this[this.HeadPow - 1 - i] + "*x^" + i + ' ');
            }

            return sb.ToString();
        }

        // Решает полином подставляя x
        public double GetSolution(double x)
        {
            double res = 0.0;

            for (int i = 0; i < this.HeadPow; i++)
                res += this[i] * Math.Pow(x, this.HeadPow - 1 - i);

            return res;
        }
        
        // Полином большего порядка из двух
        private static Func<Polynomial, Polynomial, Polynomial> maxPowPoly = (x, y) =>
            Math.Max(x.HeadPow, y.HeadPow) == x.HeadPow ? x : y;
        // Полином меньшего порядка из двух
        private static Func<Polynomial, Polynomial, Polynomial> minPowPoly = (x, y) =>
            maxPowPoly(x, y).Equals(x) ? y : x;
    }

    class Program
    {
        static double Fun(double x)
        {
            return Math.Sin(3*x) * x * x / 5;
            //return Math.Sin(3 * x);
        }
        static double Scalar(Polynomial A, Polynomial B)
        {
            Polynomial p = A * B;
            //Console.WriteLine(p);
            double result = p.Integral().GetSolution(3) - p.Integral().GetSolution(1);
            return result;
        }
        static Polynomial[] Make_ort_system(int m)//m число замеров функции
        {
            Polynomial[] pol = new Polynomial[m];
            Polynomial x = new Polynomial(new double[] { 1, 0 });
            pol[0] = new Polynomial(new double[] { 1 });
            pol[1] = x + new Polynomial(new double[1] { -Scalar(x,pol[0])/Scalar(pol[0],pol[0])});
            for (int i = 2; i < m ; i++)
            {
                pol[i] = x * pol[i - 1] + (pol[i - 1] * (-Scalar(x * pol[i - 1], pol[i - 1]) / Scalar(pol[i - 1], pol[i - 1]))) + (pol[i - 2] * (-Scalar(x * pol[i - 1], pol[i - 2]) / Scalar(pol[i - 2], pol[i - 2])));
            }
            return pol;
        }
        static Polynomial SLAU(double[,] A,double[] B)
        {
            
            double[] t = new double[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                t[i] = B[i] / A[i, i];
            }
            return new Polynomial(t);
        }
        static double Integral_(Polynomial n)
        {
            double step = 1000000;
            double h = 2.0 / step;
            double res = Fun(1.0)* n.GetSolution(1.0)*h;
            for (int i = 1; i < step; i++)
            {
                res += Fun(1.0 + h * i) * n.GetSolution(1.0 + h * i) * h;
            }
            return res;

        }
        static void Main(string[] args)
        {
            int step = 9;
            Polynomial[] n = Make_ort_system(step);
            double[,] A = new double[step, step];
            double[] B = new double[step];
            
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLongLength(1); j++)
                    A[i, j] = Scalar(n[i], n[j]);
                B[i] = Integral_(n[i]);
            }
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLongLength(1); j++)
                {
                    Console.Write("{0:N2}  ", A[i, j]);
                }
                Console.WriteLine("        {0:N2} ", B[i]);
            }
            double[] t = new double[A.GetLength(0)];
            Polynomial[] res = new Polynomial[step];
            double[] ttt = new double[step];
            for (int i = 0; i < step; i++)
                ttt[i] = 0;
            Polynomial result = new Polynomial(ttt);
            for (int i = 0; i < A.GetLength(0); i++)
            {
                t[i] = B[i] / A[i, i];
                res[i] = n[i] * t[i];
                result += res[i];
            }
            Console.WriteLine(result);
            Console.WriteLine();
            Console.WriteLine("Analytic function                       Approximant");
            Console.WriteLine("X        Y                              X         Y");

            double[] Y = new double[20];
            double[] X = new double[20];
            double[] Y_ = new double[20];
            X[0] = 1.0; Y[0] = Fun(X[0]);Y_[0] = result.GetSolution(X[0]);
            Console.WriteLine("{0:N4}   {1:N4}                      {2:N4}   {3:N4}", X[0], Y[0], X[0], Y_[0]);
            for (int i = 1; i < Y.GetLength(0); i++)
            {
                X[i] = X[i - 1] + (2.0 / 19);
                Y[i] = Fun(X[i]);
                Y_[i] = result.GetSolution(X[i]);
                Console.WriteLine("{0:N4}   {1:N4}                      {2:N4}   {3:N4}", X[i], Y[i], X[i], Y_[i]);
            }
            Console.WriteLine(Fun(2.173));
            Console.WriteLine(result.GetSolution(2.173));
        }
    }
}