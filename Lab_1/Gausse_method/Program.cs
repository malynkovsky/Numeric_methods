using System;

namespace Gausse_method
{
    class Program
    {
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
            int[] Memory = new int[size];
            for (int i = 0; i < size; i++)
                Memory[i] = i;//В массиве Memory будем хранить какой столбец какому корню соотвествуеет, что понадибиться
            //при перестовках столбцов в момента поиска максимального элемента в текущей нетреугольной части матрицы
            double[] X = new double[size];
            for (int i = 0; i < size; i++)
                X[i] = 0;
            double t = 0;
            bool q = false;
            for (int k  =0; k < size-1; k++)
            {
                //поиск наибольшего элмента в оставшейся нетреугольной части матрицы
                double max = A[k, k];
                int index_i = k, index_j = k;
                for (int r = k; r < size; r++)
                {
                    for (int w = k; w < size; w++)
                    {
                        if (A[r,w] > max)
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
                for (int i = k + 1 ; i < size; i++)
                {
                    t = A[i, k] / A[k, k];
                    B[i] = B[i] - t * B[k];
                    for (int j = k  ; j < size; j++)
                    {
                        A[i, j] = A[i, j] - t * A[k, j];
                    }
                }
            }
            X[size - 1] = B[size - 1] / A[size - 1, size - 1];
            double summa = 0;
            for (int k = size - 2; k >= 0; k--)
            {
                
                for (int r = k + 1; r < size; r++ )
                {
                    summa += A[k, r] * X[r];
                    
                }
                //обратный ход
                X[k] = (B[k] - summa)/A[k,k];
                summa = 0;
            }
            //Расставляем корни по местам 
            if (!q)
            {
                double[] Ans = new double[size];
                for (int i = 0; i < size; i++)
                    Ans[Memory[i]] = X[i];   
                Console.WriteLine("Получены следующие корни: ");
                for (int i = 0; i < size; i++)
                    Console.Write("{0:N3}    ", Ans[i]);
                Console.WriteLine();
            }
        }

    }
}

