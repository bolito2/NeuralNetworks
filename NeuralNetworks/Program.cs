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
            XORExample();

            RollingExample();

            Console.ReadKey();
        }
        static void XORExample()
        {
            NeuralNetwork XOR = new NeuralNetwork(new int[] { 3, 2, 1 }, true);

            XOR.Connect(XOR.map(0, 0), XOR.map(1, 1), -45f);
            XOR.Connect(XOR.map(0, 1), XOR.map(1, 1), 30f);
            XOR.Connect(XOR.map(0, 2), XOR.map(1, 1), 30f);
            XOR.Connect(XOR.map(0, 1), XOR.map(2, 0), 1f);
            XOR.Connect(XOR.map(0, 2), XOR.map(2, 0), 1f);
            XOR.Connect(XOR.map(1, 1), XOR.map(2, 0), -10f);

            Console.WriteLine("Input x1 = 0, x2 = 0 :");
            XOR.SetInput(new float[] { 0, 0 });
            Console.WriteLine(Math.Round(XOR.FeedForward()));

            Console.WriteLine("Input x1 = 1, x2 = 0 :");
            XOR.SetInput(new float[] { 1, 0 });
            Console.WriteLine(Math.Round(XOR.FeedForward()));

            Console.WriteLine("Input x1 = 0, x2 = 1 :");
            XOR.SetInput(new float[] { 0, 1 });
            Console.WriteLine(Math.Round(XOR.FeedForward()));

            Console.WriteLine("Input x1 = 1, x2 = 1 :");
            XOR.SetInput(new float[] { 1, 1 });
            Console.WriteLine(Math.Round(XOR.FeedForward()));

            float cost = XOR.ComputeCost(new float[][] { new float[] { 0, 0 }, new float[] { 1, 0 }, new float[] { 0, 1 }, new float[] { 1, 1 } }, new float[] { 0, 1, 1, 0 }, 0);
            Console.WriteLine(cost);
        }

        static void RollingExample()
        {
            NeuralNetwork rolling = new NeuralNetwork(new int[] { 7, 4, 7, 6, 3, 6, 4 }, true);
            Connection[,] lastCon = rolling.connections;

            rolling.rollConnections(rolling.unrollConnections());
            for (int i = 0; i < rolling.totalNodes; i++)
            {
                for (int j = 0; j < rolling.totalNodes; j++)
                {
                    Console.WriteLine("Connection " + i + "|" + j + " : " + lastCon[i, j].weight + "->" + rolling.connections[i, j].weight);
                }
            }
        }
    }
}
