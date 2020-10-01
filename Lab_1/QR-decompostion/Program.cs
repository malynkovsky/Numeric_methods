using System;

namespace QR_decompostion
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
            return Math.Sqrt(ScalatMulti(a,a));
        }

        static double[,] CreateIdentity(int n)//создание единичной матрицы n x n
        {
            double[,] E = new double[n, n];
            for (int i = 0; i < n; i++)
                for ( int j = 0; j < n; j++)
                {
                    if (i == j)
                        E[i, j] = 1.0;
                    else
                        E[i, j] = 0;
                }
            return E;
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
                    res[i, j] = k * A[i,j];
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
        static double[] DivideNum(double[] A, double k)//
        {
            double[] res = new double[A.GetLength(0)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                res[i] = A[i] / k;
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
        // транспонирование матрицы
        static double[,] transposeMatrix(double[,] matrix)
        {
            double[,] newMatrix = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newMatrix[j, i] = matrix[i, j];
                }
            }
            return newMatrix;
        }
        //проверка не являются ли нулями все элементы в стоблике под текущим
        static bool CheckZeros(double[] A,int num)
        {
            bool r = true;
            for (int i = num+1; i < A.GetLength(0); i++)
                if (Math.Abs(A[i]) > 1e-9)
                {
                    r = false;
                    break;
                }
            return r;
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
            double[,] U;
            double[,] E = CreateIdentity(A.GetLength(0));
            
            double[,] temp;
            double[] y = new double[A.GetLength(0)];
            double[] X = new double[A.GetLength(0)];
            for (int i = 0; i < size; i++)
                X[i] = 0;
            for (int t = 0; t < A.GetLength(1); t++)
            {
                //if (CheckZeros(A, t)) continue;//если под диагональным элементом все нули то идём к следующей итерации
                //Высчитываем ортгональную матрицу U и умножаем её на A и B 
                for (int i = 0; i < t; i++) y[i] = 0;
                for (int i = t; i < A.GetLength(0); i++) y[i] = A[i, t];
                if (CheckZeros(y,t)) continue;
                double alpha = FindNorma(y);
                Console.WriteLine("альфа {0} ", alpha);
                double[] e = new double[A.GetLength(0)];
                e[t] = 1;
                y = addMatrix(y, MultipleNum(e, alpha));
                y = DivideNum(y,FindNorma(y));
                double[,] w = new double[A.GetLength(0), A.GetLength(0)];
                double[,] wt;
                for (int i = 0; i < A.GetLength(0); i++)
                    w[i, t] = y[i];
                wt = transposeMatrix(w);
                temp = MultipleMatrix(w, wt);
                temp = MultipleNum(temp, 2.0);
                U = substractMatrix(E, temp);
                A = MultipleMatrix(U, A);
                B = MultipleMatrix(U, B);
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    for (int j = 0; j < A.GetLength(0); j++)
                    {
                        Console.Write("{0:N3}      ", A[i, j]);
                    }
                    Console.WriteLine(B[i]);
                }
            }
            double summa = 0;
            X[A.GetLength(0) - 1] = B[A.GetLength(0) - 1] / A[A.GetLength(0) - 1, A.GetLength(0) - 1];
            for (int k = A.GetLength(0) - 2; k >= 0; k--)
            {

                for (int r = k + 1; r < A.GetLength(1); r++)
                {
                    summa += A[k, r] * X[r];
                }
                //обратный ход
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
