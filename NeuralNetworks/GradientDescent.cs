using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class GradientDescent
    {
        public delegate float[] Gradient(float[][] input, float[][] output,float[] theta,float lambda);
        public delegate float Cost(float[][] input, float[][] output, float[] theta, float lambda);

        public delegate void progress(int perDec);
        public static event progress GetProgress;

        public delegate void ended(float init, float final);
        public static event ended GetEnd;

        public static void Start(Gradient gradient, Cost cost, float[][] input, float[][] output, ref float[] theta, float learningRate, float lambda, int numIter)
        {
            float initJ = cost(input, output, theta, lambda);
            int lastPercent = 0;

            for (int iter = 0; iter < numIter; iter++)
            {
                Console.WriteLine("Cost at iter " + iter + " = " + cost(input, output, theta, lambda));
                if((int)Math.Floor((double)(iter * 10) / numIter) > lastPercent)
                {
                    lastPercent = (int)Math.Floor((double)(iter * 10) / numIter);
                    Console.WriteLine((lastPercent*10).ToString() + "%");
                    GetProgress(lastPercent);
                }

                float[] grad = gradient(input, output, theta, lambda);
                for(int j = 0; j < theta.Length; j++)
                {
                    theta[j] += learningRate * grad[j];
                }
                //Console.WriteLine(theta[1]);
            }
            Console.WriteLine("Cost : " + initJ + "->" + cost(input, output, theta, lambda));
            GetProgress(10);
            GetEnd(initJ, cost(input, output, theta, lambda));
        }
    }
}
