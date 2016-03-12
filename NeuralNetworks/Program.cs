using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class Program
    {
        static void Main(string[] args)
       {
            //XORExample();

            //RollingExample();

            //backPropagationExample();

            invertInput();

            Console.ReadKey();
        }

        static void invertInput()
        {
            NeuralNetwork inv = new NeuralNetwork(new int[] {3, 2});
            float[][] input = new float[][] { new float[] { 0, 0 }, new float[] { 1, 0 }, new float[] { 0, 1 }, new float[] { 1, 1 } };
            float[][] output = new float[][] { new float[] { 1, 1 }, new float[] { 0, 1 }, new float[] { 1, 0 }, new float[] { 0, 0 } };
            inv.BackPropagation(input, output, 1f, 0, 5000);

            /*
            inv.Connect(0, 0, 0, 10f);
            inv.Connect(0, 0, 1, 10f);

            inv.Connect(0, 1, 0, -50f);
            inv.Connect(0, 2, 1, -50f);

            inv.SetInput(new float[] { 0, 0 });
            float[] result1 = inv.FeedForward();
            Console.WriteLine("first output: " + result1[0]);
            Console.WriteLine("second output: " + result1[1]);
            Console.WriteLine("//////");

            inv.SetInput(new float[] { 1, 0 });
            float[] result2 = inv.FeedForward();
            Console.WriteLine("first output: " + result2[0]);
            Console.WriteLine("second output: " + result2[1]);
            Console.WriteLine("//////");

            inv.SetInput(new float[] { 0, 1 });
            float[] result3 = inv.FeedForward();
            Console.WriteLine("first output: " + result3[0]);
            Console.WriteLine("second output: " + result3[1]);
            Console.WriteLine("//////");

            inv.SetInput(new float[] { 1, 1 });
            float[] result4 = inv.FeedForward();
            Console.WriteLine("first output: " + result4[0]);
            Console.WriteLine("second output: " + result4[1]);
            Console.WriteLine("//////");

            */
        }

        static void backPropagationExample()
        {
            NeuralNetwork example = new NeuralNetwork(new int[] { 3, 3, 1 });

            //gradientNetwork.gradient(new float[][] { new float[] { 1, 0 }, new float[] { 0, 0 }, new float[] { 0, 1 }, new float[] { 1, 1 } }, new float[] { 0, 0, 0, 1 }, gradientNetwork.unrollConnections(gradientNetwork.connections), 0);
            example.BackPropagation(new float[][] { new float[]{1, 0}, new float[] { 0, 0 }, new float[] { 0, 1 }, new float[] { 1, 1 } }, new float[][] { new float[]{ 1}, new float[] {0}, new float[] {1}, new float[] {0 } }, 7f, 0, 5000);

            for(int l = 0; l < example.L - 1; l++)
            {
                for(int i = 0; i < example.maxNodesPerLayer[l]; i++)
                {
                    for(int j = Convert.ToInt32(example.nodes[l + 1][0].bias); j < example.maxNodesPerLayer[l + 1]; j++)
                    {
                        Console.WriteLine("l: " + l + ", i:"  + i + ", j:" + j + ", theta: " + example.connections[l][i, j]);
                    }
                }
            }
        }

        static void XORExample()
        {
            NeuralNetwork XOR = new NeuralNetwork(new int[] { 3, 3, 1 }, false);

            XOR.Connect(0, 0, 1, -30f);
            XOR.Connect(0, 0, 2, -10f);
            XOR.Connect(0, 1, 1, 20f);
            XOR.Connect(0, 2, 1, 20f);
            XOR.Connect(0, 1, 2, 20f);
            XOR.Connect(0, 2, 2, 20f);
            XOR.Connect(1, 1, 0, -50f);
            XOR.Connect(1, 2, 0, 20f);
            XOR.Connect(1, 0, 0, -10f);

            Console.WriteLine("Input x1 = 0, x2 = 0 :");
            XOR.SetInput(new float[] { 0, 0 });
            Console.WriteLine(Math.Round(XOR.FeedForward(XOR.connections)[0]));

            Console.WriteLine("Input x1 = 1, x2 = 0 :");
            XOR.SetInput(new float[] { 1, 0 });
            Console.WriteLine(Math.Round(XOR.FeedForward(XOR.connections)[0]));

            Console.WriteLine("Input x1 = 0, x2 = 1 :");
            XOR.SetInput(new float[] { 0, 1 });
            Console.WriteLine(Math.Round(XOR.FeedForward(XOR.connections)[0]));

            Console.WriteLine("Input x1 = 1, x2 = 1 :");
            XOR.SetInput(new float[] { 1, 1 });
            Console.WriteLine(Math.Round(XOR.FeedForward(XOR.connections)[0]));

            float cost = XOR.ComputeCost(new float[][] { new float[] { 0, 0 }, new float[] { 1, 0 }, new float[] { 0, 1 }, new float[] { 1, 1 } }, new float[][] { new float[] { 0}, new float[] {1}, new float[] {1}, new float[] { 0 } }, XOR.unrollConnections(XOR.connections), 0);
            Console.WriteLine(cost);
        }

        static void RollingExample()
        {
            NeuralNetwork rolling = new NeuralNetwork(new int[] { 3, 1 }, true);

            float[] lol = rolling.unrollConnections();

            for (int j = 0; j < lol.Length; j++)
            {
                Console.WriteLine(lol[j]);
            }
            /*
            rolling.rollConnections(rolling.unrollConnections());
            for (int i = 0; i < rolling.totalNodes; i++)
            {
                for (int j = 0; j < rolling.totalNodes; j++)
                {
                    Console.WriteLine("Connection " + i + "|" + j + " : " + lastCon[i, j].weight + "->" + rolling.connections[i, j].weight);
                }
            }
            */
        }
    }
}
