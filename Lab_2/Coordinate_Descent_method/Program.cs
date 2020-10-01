using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coordinate_Descent_method
{
    class Program
    {
        static double Loss_function(double[] A)//целевая функция 
        {
            double x = A[0];
            double y = A[1];
            double z = A[2];
            double F = (2 * Math.Exp(x) - Math.Log(y) + z) * (2 * Math.Exp(x) - Math.Log(y) + z) + (Math.Sin(x) + 4 * y - z * z) * (Math.Sin(x) + 4 * y - z * z) + (x + y * y * y + 0.5 * z) * (x + y * y * y + 0.5 * z);
            return F;
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

        static double[] addMatrix(double[] A, double[] B)
        {
            double[] C = new double[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                C[i] = A[i] + B[i];
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            double[] X = new double[3];
            for (int i = 0; i < 3; i++)
                X[i] = 5.0;
            //создадим три вектора с единицами на i-ой позиции(остальные нули)
            double[,] E = new double[3, 3];
            E[0, 0] = 1.0;
            E[1, 1] = 1.0;
            E[2, 2] = 1.0;
            double alpha = 0.5;
            int k = 0;
            double[] previousValues = X;
            double[] currentValues = X;
            double accuracy = 0.00001;
            int iterations = 15000;
            int index;
            double[] P = new double[3];
            double[] temp = new double[3];
            
            while (true)
            {
                bool t = false;
                index = k - 3 * (k / 3);
                for (int i = 0; i < 3; i++)
                    P[i] = E[index, i];
                P = MultipleNum(P, alpha);
                temp = addMatrix(previousValues, P);
                if (Loss_function(temp) < Loss_function(previousValues))
                {
                    currentValues = temp;
                    t = true;
                }
                    
                else
                {
                    temp = substractMatrix(previousValues, P);
                    if (Loss_function(temp) < Loss_function(previousValues))
                    {
                        currentValues = temp;
                        t = true;
                    }
                        
                }
                k++;
                if ((index == 2) && (currentValues == previousValues))
                    alpha /= 2;
                if (t)
                {
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
                }
                previousValues = currentValues;
                Console.Write("plot3(");
                for (int i = 0; i < 3; i++)
                    Console.Write("{0:N4},", currentValues[i]);
                Console.WriteLine("'o');");
            }
            X = currentValues;
            for (int i = 0; i < 3; i++)
                Console.Write("{0:N4}  ", X[i]);
        }
    }
}
