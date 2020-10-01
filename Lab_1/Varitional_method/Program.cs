using System;

namespace Varitional_method
{
    class Program
    {
        static double ScalatMulti(double[] a, double[] b)
        {
            double temp = 0.0;
            for (int i = 0; i < a.GetLength(0); i++)
                temp += a[i] * b[i];
            return temp;
        }
        static double FindNorma(double[] a)
        {
            return Math.Sqrt(ScalatMulti(a, a));
        }
        

        static double[,] MultipleMatrix(double[,] A, double[,] B)// перемножение матрицы
        {
            if (A.GetLength(1) != B.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[,] res = new double[A.GetLength(0), B.GetLength(0)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    for (int k = 0; k < B.GetLength(0); k++)
                    {
                        res[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return res;
        }
        static double[] MultipleMatrix(double[,] A, double[] B)// перемножение матрицы
        {
            if (A.GetLength(1) != B.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[] res = new double[B.GetLength(0)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(0); j++)
                {
                    res[i] += A[i, j] * B[j];
                }
            }
            return res;
        }
        // умножение матрицы на число
        static double[,] MultipleNum(double[,] A, double k)// перемножение матрицы
        {
            double[,] res = new double[A.GetLength(0), A.GetLength(0)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    res[i, j] = k * A[i, j];
                }
            }
            return res;
        }
        static double[] MultipleNum(double[] A, double k)//
        {
            double[] res = new double[A.GetLength(0)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                res[i] = k * A[i];
            }
            return res;
        }
        // сложение матриц
        static double[,] addMatrix(double[,] A, double[,] B)
        {
            double[,] C = new double[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }
        static double[] addMatrix(double[] A, double[] B)
        {
            double[] C = new double[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                C[i] = A[i] + B[i];
            }
            return C;
        }
        // вычитание матриц
        static double[,] substractMatrix(double[,] A, double[,] B)
        {
            double[,] C = new double[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    C[i, j] = A[i, j] - B[i, j];
                }
            }
            return C;
        }
        static double[] substractMatrix(double[] A, double[] B)
        {
            double[] C = new double[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                C[i] = A[i] - B[i];
            }
            return C;
        }

        static void Main(string[] args)
        {
            Console.Write("Введите размер матрицы A: ");
            int size = int.Parse(Console.ReadLine());
            Console.WriteLine();
            double[,] A = new double[size, size];
            double[] B = new double[size];
            Console.WriteLine("Введите элементы матрицы A построчно, разделяя элементы пробелом: ");
            for (int i = 0; i < A.GetLength(0); i++)
            {
                string Matrix = Console.ReadLine();
                string[] massiveMatrix = Matrix.Split(new Char[] { ' ' });
                for (int j = 0; j < massiveMatrix.Length; j++)
                {
                    A[i, j] = double.Parse(massiveMatrix[j]);
                }
            }
            Console.WriteLine("Введите элементы столбца B построчно, разделяя элементы пробелом: ");
            string enterString = Console.ReadLine();
            string[] massiveString = enterString.Split(new Char[] { ' ' });
            for (int i = 0; i < B.GetLength(0); i++)
            {
                B[i] = double.Parse(massiveString[i]);
            }
            Console.Write("Задайте точность вычисления: ");
            double accuracy = double.Parse(Console.ReadLine());
            Console.Write("Введите максимально допустимое количество итераций: ");
            int iterations = int.Parse(Console.ReadLine());
            int k = 0;
            // Результирующая матрица
            double[] X = new double[A.GetLength(0)];
            for (int i = 0; i < size; i++)
                X[i] = 0;
            
            double[] previousValues = new double[A.GetLength(0)];//хранит значения на предыдущей итерации 
            double[] discrepancy = new double[A.GetLength(0)];//невязка
            for (int i = 0; i < previousValues.GetLength(0); i++)
            {
                previousValues[i] = 0.0;
            }
            discrepancy = substractMatrix(MultipleMatrix(A, previousValues),B);
            for (int i = 0; i < A.GetLength(0); i++)
                Console.Write("{0}  ", discrepancy[i]);
            Console.WriteLine();
            while (true)
            {
                double[] currentValues = new double[A.GetLength(0)];
                double tau = ScalatMulti(discrepancy, discrepancy) / ScalatMulti(MultipleMatrix(A, discrepancy),discrepancy);
                currentValues = addMatrix(MultipleNum(discrepancy, tau), previousValues);
                discrepancy = substractMatrix(B, MultipleMatrix(A, currentValues));
                k++;
                Console.WriteLine("Итерация № {0}", k);
                for (int i = 0; i < currentValues.GetLength(0); i++)
                    Console.Write("{0:N4}    ", currentValues[i]);// После N меняем цифру, это показывает сколько знаков после запятой выводить
                Console.WriteLine();
                double max = 0.0;
                for (int i = 0; i < X.Length; i++)
                {
                    X[i] = currentValues[i] - previousValues[i];
                    if (Math.Abs(X[i]) > max)
                        max = Math.Abs(X[i]);
                }
                    
                if ((max <= accuracy) || (k > iterations))
                {
                    previousValues = currentValues;
                    break;
                }
                previousValues = currentValues;
            }
            X = previousValues;
            if (k > iterations)
            {
                Console.WriteLine("Приблизиться к решений с заданной точностью не удалось");
            }
            else
            {
                Console.WriteLine("Было получено следующее решение: ");
                for (int i = 0; i < X.GetLength(0); i++)
                    Console.Write("{0:N4}    ", X[i]);// После N меняем цифру, это показывает сколько знаков после запятой выводить
                Console.WriteLine();
            }
        }
    }
}
