using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Backward_interpolation_method
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
                resultPoly[maxPoly.HeadPow - 1 - i] = maxPoly[maxPoly.HeadPow - 1 - i] + minPoly[minPoly.HeadPow - 1 - i];

            return resultPoly;
        }

        // Умножает полином на константу
        public static Polynomial operator *(Polynomial poly1, double k)
        {
            Polynomial resultPoly = new Polynomial(poly1.LineCoefficients);

            for (int i = 0; i < poly1.HeadPow; i++) resultPoly[i] *= k;

            return resultPoly;
        }

        public static Polynomial operator /(Polynomial poly1, double k)
        {
            Polynomial resultPoly = new Polynomial(poly1.LineCoefficients);

            for (int i = 0; i < poly1.HeadPow; i++) resultPoly[i] /= k;

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

            for (int i = this.HeadPow - 1; i >= 0; i--)
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
            return (x * x * x - 7*Math.Exp(x-2));
        }
        
        static Polynomial Iteration(double[] X,double[,] A, double y_)
        {
            Polynomial result = new Polynomial(new double[] { X[0] });
            Polynomial[] temp = new Polynomial[A.GetLength(0)];
            for (int i = 1; i < A.GetLength(0); i++)
            {
                temp[i] = new Polynomial(new double[1] { 1 });
                for (int j = 0; j <= i; j++)
                {
                    temp[i] =  temp[i] * (new Polynomial(new double[2] { 1, -X[j] }));
                }
                temp[i] = temp[i] * A[0, i];
            }
            Polynomial t = new Polynomial(new double[] { Fun(X[0])- y_});
            for (int i = 1; i < temp.Length; i++)
                t = t + temp[i];
            t = t / (-A[0, 0]);
            result = result + t;
            return result;

        }
        
        static void Main(string[] args)
        {
            int n = 15;

            double Y_ = 1.0;

            double[] X = new double[n];
            double[] Y = new double[n];
            
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            {
                X[i] = rand.Next(-5, 5) + rand.NextDouble();
            }
            Array.Sort(X);
            for (int i = 0; i < n; i++)
                Y[i] = Fun(X[i]);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0:N3}   {1:N3}", X[i],Y[i]);
            }
            double[,] A = new double[n - 1, n - 1];
            for (int i = 0; i < n - 1; i++)
                A[i, 0] = (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
            for (int j = 1; j < n-1; j++)
            {
                for (int i = 0; i < n - 1 - j; i++)
                {
                    A[i, j] = (A[i + 1, j - 1] - A[i, j - 1]) / (X[i + j + 1] - X[i]);
                }
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n-1; j++)
                {
                    Console.Write("{0:N3}  ", A[i, j]);
                }
                Console.WriteLine();
            }
            Polynomial a = Iteration(X, A, Y_);
            int iterations = 1500;
            double epsilon = 0.00000001;
            int k = 0;
            double res = X[0];
            Console.WriteLine("Итерация № {0}, X = {1:N4}", k, res);
            double temp = X[0];
            double t;
            while (true)
            {
                t = a.GetSolution(temp);
                k++;
                Console.WriteLine("Итерация № {0}, X = {1}", k, t);
                
                if ((Math.Abs(t - temp) <= epsilon) || (k > iterations))
                {
                    temp = t;
                    break;
                }
                temp = t;
            }
            if (k > iterations)
            {
                Console.WriteLine();
                Console.WriteLine("Приблизиться к решений с заданной точностью не удалось");
            }
            else
            {
                Console.WriteLine();
                Console.Write("Было получено следующее решение: ");
                Console.WriteLine("{0:N4}    ", temp);
            }
            Console.WriteLine();
            Console.WriteLine(a);
        }
    }
}