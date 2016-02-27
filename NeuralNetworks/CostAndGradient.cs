using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public static class CostAndGradient
    {
        public static float ComputeCost(NeuralNetwork nn, float[][] input, float[] output, float lambda)
        {
            float J = 0;
            float h = 0;
            for(int i = 0; i < output.Length; i++)
            {
                nn.SetInput(input[i]);
                h = nn.FeedForward();

                //Without reg
                J += (float)(output[i] * Math.Log(h) + (1 - output[i]) * Math.Log(1 - h));
            }
            J /= -output.Length;

            //reg
            float reg = 0;
            for (int c1 = 0; c1 < nn.totalNodes; c1++)
            {
                for (int c2 = 0; c2 < nn.totalNodes; c2++)
                {
                    if (nn.connections[c1, c2].active)
                    {
                        reg += (float)Math.Pow(nn.connections[c1, c2].weight, 2);
                    }
                }
            }
            reg *= lambda / (2 * output.Length);

            J += reg;

            return J;
        }
    }
}
