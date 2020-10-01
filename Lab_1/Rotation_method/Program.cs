using System;

namespace Rotation_method
{
    class Program
    {
        // проверка на нулевые элементы
        static bool CheckZeros(double num)
        {
            double eps = 1e-9;
            if (Math.Abs(num) < eps)
                return true;
            else
                return false;
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
            // Результирующая матрица
            
            double[] X = new double[A.GetLength(0)];
            for (int i = 0; i < size; i++)
                X[i] = 0;
            for (int t = 0; t < A.GetLength(0); t++)
            {
                //защита от деления на ноль
                for (int k = t; k < A.GetLength(0); k++)
                    if (!CheckZeros(A[k,t]))
                    {
                        double temp;
                        for (int i = 0; i < A.GetLength(0); i++)
                        {
                            temp = A[t, i];
                            A[t, i] = A[k, i];
                            A[k, i] = temp;
                            temp = B[k];
                            B[k] = B[t];
                            B[t] = B[k];
                        }
                        break;
                    }

                for (int k = t + 1; k < A.GetLength(0); k++ )
                {
                    double c = A[t, t] / Math.Sqrt(A[t, t] * A[t, t] + A[k, t] * A[k, t]);
                    double s = A[k, t] / Math.Sqrt(A[t, t] * A[t, t] + A[k, t] * A[k, t]);
                    double[] A1 = new double[A.GetLength(0)];
                    double[] A2 = new double[A.GetLength(0)];
                    double B1 = B[t]; double B2 = B[k];
                    for (int i = 0; i < A.GetLength(0); i++)
                    {
                        A1[i] = A[t, i] * c + A[k, i] * s;
                        A2[i] = A[t, i] * (-s) + A[k, i] * c;
                        B1 = B[t] * c + B[k] * s;
                        B2 = B[t] * (-s) + B[k] * c;
                    }
                    for (int i = 0; i < A.GetLength(0); i++)
                    {
                        A[t, i] = A1[i];
                        A[k, i] = A2[i];
                    }
                    B[t] = B1; B[k] = B2;
                }
            }
            //обратный ход
            double summa = 0;
            X[A.GetLength(0) - 1] = B[A.GetLength(0) - 1] / A[A.GetLength(0) - 1, A.GetLength(0) - 1];
            for (int k = A.GetLength(0) - 2; k >= 0; k--)
            {
                for (int r = k + 1; r < A.GetLength(1); r++)
                {
                    summa += A[k, r] * X[r];
                }
                X[k] = (B[k] - summa) / A[k, k];
                summa = 0;
            }

            Console.WriteLine("Было получено следующее решение: ");
            for (int i = 0; i < X.GetLength(0); i++)
                Console.Write("{0:N4}    ", X[i]);// После N меняем цифру, это показывает сколько знаков после запятой выводить
            Console.WriteLine();

        }
    }
}
