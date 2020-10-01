using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rational_Aproximation
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

        //Возводит полином в степень
        public Polynomial Power(int p)
        {
            Polynomial result = this;
            for (int i =2; i <= p; i++)
            {
                result = result * this;
            }
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
        static int Factorial(int n)
        {
            int result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        static double Function(double x)
        {
            //return Math.Exp(x) / (1 - x);
            return Math.Sqrt((1 + 0.5 * x) / (1 + 2 * x));
        }
        //нахождения производных функции n-го порядка для нахождения коэф-то ряда Тейлора
        static double Derivatives(double x, int k)
        {
            double res = 0;
            double h = 0.005;
            if (k == 1)
            {
                res = (Function(x + h) - Function(x - h)) / (2 * h);
                return res;
            }
            else
            {
                res = (Derivatives(x + h, k - 1) - Derivatives(x - h, k - 1)) / (2 * h);
                return res;
            }
            
        }

        static double[] SLAU(double[,] A, double[] B)
        {
            int size = B.Length;
            int[] Memory = new int[size];
            for (int i = 0; i < size; i++)
                Memory[i] = i;//В массиве Memory будем хранить какой столбец какому корню соотвествуеет, что понадибиться
            //при перестовках столбцов в момента поиска максимального элемента в текущей нетреугольной части матрицы
            double[] X = new double[size];
            for (int i = 0; i < size; i++)
                X[i] = 0;
            double t = 0;
            bool q = false;
            for (int k = 0; k < size - 1; k++)
            {
                //поиск наибольшего элмента в оставшейся нетреугольной части матрицы
                double max = A[k, k];
                int index_i = k, index_j = k;
                for (int r = k; r < size; r++)
                {
                    for (int w = k; w < size; w++)
                    {
                        if (A[r, w] > max)
                        {
                            max = A[r, w];
                            index_i = r;
                            index_j = w;
                        }
                    }
                }
                if (max < 1e-15)
                {
                    Console.WriteLine("Однозначного решения нет");
                    q = true;
                    break;
                }
                //обмен строк и столбцов если найден такой элемент
                double temp = 0;
                if ((index_i != k) || (index_j != k))
                {
                    //обмен строк
                    if (index_i != k)
                    {
                        for (int j = k; j < size; j++)
                        {
                            temp = A[k, j];
                            A[k, j] = A[index_i, j];
                            A[index_i, j] = temp;
                        }
                        temp = B[k];
                        B[k] = B[index_i];
                        B[index_i] = temp;
                    }

                    //обмен столбцов
                    if (index_j != k)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            temp = A[i, k];
                            A[i, k] = A[i, index_j];
                            A[i, index_j] = temp;
                        }
                        int u = Memory[k];
                        Memory[k] = Memory[index_j];
                        Memory[index_j] = u;
                    }

                }
                //прямой ход метода
                for (int i = k + 1; i < size; i++)
                {
                    t = A[i, k] / A[k, k];
                    B[i] = B[i] - t * B[k];
                    for (int j = k; j < size; j++)
                    {
                        A[i, j] = A[i, j] - t * A[k, j];
                    }
                }
            }
            //
            //Надо сделать проверку на невырожденность системы
            //
            X[size - 1] = B[size - 1] / A[size - 1, size - 1];
            double summa = 0;
            for (int k = size - 2; k >= 0; k--)
            {

                for (int r = k + 1; r < size; r++)
                {
                    summa += A[k, r] * X[r];

                }
                //обратный ход
                X[k] = (B[k] - summa) / A[k, k];
                summa = 0;
            }
            //Расставляем корни по местам 
            if (!q)
            {
                double[] Ans = new double[size];
                for (int i = 0; i < size; i++)
                    Ans[Memory[i]] = X[i];
                return Ans;
            }
            else
                return new double[] { 0 };
        }

        static void Main(string[] args)
        {
            int L = 1;
            int M = 1;
            double center = 0.0;
            double[] Coefficients = new double[L + M + 1];
            Polynomial Taylor = new Polynomial(new double[] { Function(center) });
            Polynomial temp = new Polynomial(new double[] { 1, -center });
            for (int i = 1; i < L + M + 1; i++)
                Taylor += temp.Power(i) * Derivatives(center, i) * (1.0 / Factorial(i));
            Console.WriteLine("Taylor series:");
            Console.WriteLine(Taylor);
            Coefficients = Taylor.LineCoefficients.ToArray();
            Array.Reverse(Coefficients);
            double[,] A = new double[M, M];
            double[] B = new double[M];
            int index;
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if ((L - M + 1 + i + j) >= 0)
                        A[i, j] = Coefficients[L - M + 1 + i + j];
                    else
                        A[i, j] = 0;
                    A[j, i] = A[i, j];
                }
                if ((L + 1 + i) >= 0)
                    B[i] = -Coefficients[L + 1 + i];
                else
                    B[i] = 0;
            }
            double[] X = SLAU(A, B);
            double[] Denumerator = new double[M + 1];
            for (int i = 0; i < M; i++)
                Denumerator[i] = X[i];
            Denumerator[M] = 1;
            Array.Reverse(Denumerator);
            double[] Numerator = new double[L + 1];
            Numerator[0] = Coefficients[0];
            for (int i = 1; i < L+1; i++)
            {
                Numerator[i] = Coefficients[i];
                for (int j = 1; j <= Math.Min(L, M); j++)
                    if ((i - j)>= 0)
                        Numerator[i] += Denumerator[j] * Coefficients[i - j];

            }
            Array.Reverse(Numerator);
            Array.Reverse(Denumerator);
            Polynomial Num = new Polynomial(Numerator);
            Polynomial Denum = new Polynomial(Denumerator);
            Console.WriteLine();
            Console.WriteLine(Num);
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine(Denum);
            Console.WriteLine();
            Console.WriteLine("Comparing table:");  
            Console.WriteLine("Analytic function      Taylor series      Pade approximation");
            for (double i = 0; i < 10.0; i += 0.2)
            {
                Console.Write("{0:N7}              ",Function(i));
                Console.Write("{0:N7}              ",Taylor.GetSolution(i));
                Console.Write("{0:N7}              ",Num.GetSolution(i) / Denum.GetSolution(i));
                Console.WriteLine();
            }
            
        }
    }
}
