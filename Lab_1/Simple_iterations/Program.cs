using System;

namespace Simple_iterations
{
    class Program
    {
        static double FindNorma(double[] a, double[] b)
        {
            double temp = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                temp += a[i] * b[i];
            }
            return Math.Sqrt(temp);
        }

        static void Main(string[] args)
        {
            Console.Write("Введите размер матрицы A: ");
            int size = int.Parse(Console.ReadLine());
            Console.WriteLine();
            double[,] A = new double[size, size];
            double[] B = new double[size];
            Console.WriteLine("Введите элементы матрицы A построчно, разделяя элементы пробелом: ");
            double max_denum = Math.Abs(A[0, 0]);
            for (int i = 0; i < A.GetLength(0); i++)
            {
                string Matrix = Console.ReadLine();
                string[] massiveMatrix = Matrix.Split(new Char[] { ' ' });
                for (int j = 0; j < massiveMatrix.Length; j++)
                {
                    A[i, j] = double.Parse(massiveMatrix[j]);
                    if (Math.Abs(A[i, j]) > max_denum)
                        max_denum = A[i, j];
                }
            }
            Console.WriteLine("Введите элементы столбца B построчно, разделяя элементы пробелом: ");
            string enterString = Console.ReadLine();
            string[] massiveString = enterString.Split(new Char[] { ' ' });
            for (int i = 0; i < B.GetLength(0); i++)
            {
                B[i] = double.Parse(massiveString[i]);
                if (Math.Abs(B[i]) > max_denum)
                    max_denum = B[i];
            }
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < size; j++)
                {
                    A[i, j] /= max_denum;
                    //Console.Write("{0} ", A[i, j]);
                }
                B[i] /= max_denum;
                //Console.WriteLine("       {0}", B[i]);

            }
            // Результирующая матрица
            double[,] matrix;
            double[] X;

            Console.Write("Задайте точность вычисления: ");
            double accuracy = double.Parse(Console.ReadLine());
            Console.Write("Введите максимально допустимое количество итераций: ");
            int iterations = int.Parse(Console.ReadLine());
            int k = 0;
            matrix = new double[A.GetLength(0), A.GetLength(1) + 1];
            X = new double[A.GetLength(0)];
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                {
                    
                    matrix[i, j] = -A[i, j];
                }
                matrix[i, i] = 1.0 - A[i, i];
                matrix[i, matrix.GetLength(1) - 1] = B[i];
            }
            
            double[] previousValues = new double[matrix.GetLength(0)];//хранит значения на предыдущей итерации 
            for (int i = 0; i < previousValues.GetLength(0); i++)
            {
                previousValues[i] = 0.0;
            }

            while (true)
            {
                // Введем вектор значений неизвестных на текущем шаге
                double[] currentValues = new double[matrix.GetLength(0)];

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    currentValues[i] = matrix[i, matrix.GetLength(1)-1];

                    for (int j = 0; j < matrix.GetLength(1)-1; j++)
                    {
                        currentValues[i] += matrix[i, j] * previousValues[j];
                        
                    }

                }
                k++;
                Console.WriteLine("Итерация № {0}", k);
                for (int i = 0; i < currentValues.GetLength(0); i++)
                    Console.Write("{0:N4}    ", currentValues[i]);// После N меняем цифру, это показывает сколько знаков после запятой выводить
                Console.WriteLine();
                for (int i = 0; i < X.Length; i++)
                    X[i] = currentValues[i] - previousValues[i];
                if ((FindNorma(X, X) <= accuracy) || (k > iterations))
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
