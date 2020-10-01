using System;

namespace Gradient_Descent_method
{
    class Program
    {
        static double Loss_function(double[] A)//целевая функция 
        {
            double x = A[0];
            double y = A[1];
            double z = A[2];
            double F = (2 * Math.Exp(x) - Math.Log(y) + z) * (2 * Math.Exp(x) - Math.Log(y) + z) + (Math.Sin(x) + 4 * y - z * z) * (Math.Sin(x) + 4 * y - z * z) + (x + y * y * y + 0.5 * z) * (x + y * y * y + 0.5 * z);
            //F = (x * x + y * y + 4 - z) * (x * x + y * y + 4 - z) + (x * z - 6 * y) * (x * z - 6 * y) + (2 * x * x * x - 8 * y + z) * (2 * x * x * x - 8 * y + z);
            return F;
        }
        static double[] Gradient(double[] A)
        {
            double[] result = new double[3];
            double h = 0.00005;
            double[] B = new double[3];
            for (int i = 0; i < 3; i++)
                B[i] = A[i];
            for (int i = 0; i < 3; i++)
            {
                B[i] += h;
                result[i] = (Loss_function(B) - Loss_function(A)) / h;
                //Console.WriteLine(result[i]);
                B[i] = A[i];
            }
            return result;
        }
        
        static double[] Gradient_analitic(double[] A)
        {
            double x = A[0];
            double y = A[1];
            double z = A[2];
            double X = 4 * Math.Exp(x) * (2 * Math.Exp(x) - Math.Log(y) + z) + 2 * Math.Cos(x) * (Math.Sin(x) + 4 * y - z * z) + 2 * (x + y * y * y + 0.5 * z);
            //X = 4 * x * (x * x + y * y + 4 - z) + 2 * z*(x * z - 6 * y) + 12 * x * x*(2 * x * x * x - 8 * y + z);
            double Y = 2 * ((-1.0) / y) * (2 * Math.Exp(x) - Math.Log(y) + z) + 8 * (Math.Sin(x) + 4 * y - z * z) + 6 * y * y * (x + y * y * y + 0.5 * z);
            //Y = 4 * y * (x * x + y * y + 4 - z) - 12 * (x * z - 6 * y) - 16 * (2 * x * x * x - 8 * y + z);
            double Z = 2 * (2 * Math.Exp(x) - Math.Log(y) + z) - 4 * z * (Math.Sin(x) + 4 * y - z * z) + (x + y * y * y + 0.5 * z);
            //Z = -2 * (x * x + y * y + 4 - z) + 2 * x * (x * z - 6 * y) + 2 * (2 * x * x * x - 8 * y + z);
            double[] res = new double[3];
            res[0] = X; res[1] = Y; res[2] = Z;
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
                X[i] = 0.0;
            X[1] = 3.0;
            double alpha = 0.005;
            int k = 0;
            double[] previousValues = X;
            double[] currentValues = X;
            double accuracy = 0.0000001;
            int iterations = 15000;
            //int index;
            double[] P = new double[3];
            double[] temp = new double[3];

            while (true)
            {
                bool t = false;
                temp = substractMatrix(previousValues, MultipleNum( Gradient_analitic(previousValues),alpha));
                if (Loss_function(temp) < Loss_function(previousValues))
                {
                    currentValues = temp;
                    t = true;
                }
                k++;
                if (currentValues == previousValues)
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
                //Console.WriteLine("Loss  {0}   ", Loss_function(previousValues));
            }
            X = currentValues;
            for (int i = 0; i < 3; i++)
                Console.Write("{0:N4}  ", X[i]);
            Console.WriteLine(Loss_function(new double[3] { 0, 1, -2 }));
        }
    }
}
