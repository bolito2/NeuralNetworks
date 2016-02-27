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
            //Ejemplo(XOR)
            NeuralNetwork XOR = new NeuralNetwork(3, new int[] {3, 2, 1 });

            XOR.Connect(XOR.map(0, 0), XOR.map(1, 1), -45f);
            XOR.Connect(XOR.map(0, 1), XOR.map(1, 1), 30f);
            XOR.Connect(XOR.map(0, 2), XOR.map(1, 1), 30f);
            XOR.Connect(XOR.map(0, 1), XOR.map(2, 0), 1f);
            XOR.Connect(XOR.map(0, 2), XOR.map(2, 0), 1f);
            XOR.Connect(XOR.map(1, 1), XOR.map(2, 0), -10f);
            
            Console.WriteLine("Input x1 = 0, x2 = 0 :");
            XOR.SetInput(new int[] { 0, 0 });
            XOR.FeedForward();

            Console.WriteLine("Input x1 = 1, x2 = 0 :");
            XOR.SetInput(new int[] { 1, 0 });
            XOR.FeedForward();

            Console.WriteLine("Input x1 = 0, x2 = 1 :");
            XOR.SetInput(new int[] { 0, 1 });
            XOR.FeedForward();

            Console.WriteLine("Input x1 = 1, x2 = 1 :");
            XOR.SetInput(new int[] { 1, 1 });
            XOR.FeedForward();

            Console.ReadKey();
        }
    }
}
