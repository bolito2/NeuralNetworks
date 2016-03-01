using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class GradientDescent
    {
        public delegate float Gradient(float[][] input, float[] output,float[] theta,float lambda);

        public static void Start(Gradient gradient, float[] initialTheta, float learningRate)
        {
            
        }
    }
}
