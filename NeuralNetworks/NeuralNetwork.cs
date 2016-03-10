using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public struct Node
    {
        public float value;
        public int id;
        public bool bias;

        public Node(int id)
        {
            this.id = id;
            value = 0;

            bias = false;
        }
    }
    public class NeuralNetwork
    {
        //Input id, output id
        public float[][,] connections;
        //Layer, lid
        public int nextId = 0;

        public int[] maxNodesPerLayer;
        public int L;

        public Node[][] nodes;

        public int totalNodes = 0;

        Random random = new Random();
        const float eps = 0.4f;
        const float lim = 0.0001f;

        float[][,] addToTheta(float[][,] Theta,float add)
        {
            float[][,] addCons = new float[L - 1][,];

            for(int l = 0; l < L - 1; l++)
            {
                addCons[l] = new float[maxNodesPerLayer[l], maxNodesPerLayer[l + 1]];
                for (int i = 0; i < maxNodesPerLayer[l]; i++)
                {
                    for (int j = 0; j < maxNodesPerLayer[l + 1]; j++)
                    {
                        if(!nodes[l + 1][j].bias)
                            addCons[l][i, j] = Theta[l][i, j] + add;
                    }
                }
            }

            return addCons;
        }
        float[][,] addToTheta(float[][,] Theta, float add, int layer, int input, int output)
        {
            float[][,] addCons = new float[L - 1][,];

            for (int l = 0; l < L - 1; l++)
            {
                addCons[l] = new float[maxNodesPerLayer[l], maxNodesPerLayer[l + 1]];
                for (int i = 0; i < maxNodesPerLayer[l]; i++)
                {
                    for (int j = 0; j < maxNodesPerLayer[l + 1]; j++)
                    {
                        if (!nodes[l + 1][j].bias)
                        {
                            if (l == layer && i == input && j == output)
                                addCons[l][i, j] = Theta[l][i, j] + add;
                            else
                                addCons[l][i, j] = Theta[l][i, j];
                        }
                    }
                }
            }

            return addCons;
        }
        float lol = 0;
        int xd = 0;
        public float[] gradient(float[][] input, float[] output, float[] theta,float lambda)
        {
            float[][,] Theta = rollConnections(theta);

            float[][] delta = new float[L][];
            float[] grad = new float[totalConnections];

            int contador = 0;

            delta[L - 1] = new float[maxNodesPerLayer[L - 1]];

            for (int s = 0; s < output.Length; s++)
            {
                SetInput(input[s]);
                float h = FeedForward(Theta);

                delta[L - 1][0] = h - output[s];

                //BackPropagate delta
                for (int l = L - 2; l > 0; l--)
                {
                    delta[l] = new float[maxNodesPerLayer[l]];
                    for(int i = Convert.ToInt32(nodes[l][0].bias); i < maxNodesPerLayer[l]; i++)
                    {
                        for(int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                        {
                            delta[l][i] += delta[l + 1][j] * Theta[l][i, j];
                        }
                        delta[l][i] *= nodes[l][i].value * (1 - nodes[l][i].value);
                    }
                }
                contador = 0;
                for(int l = 0; l < L - 1; l++)
                {
                    
                    for (int i = 0; i < maxNodesPerLayer[l]; i++)
                    {
                        //contador += Convert.ToInt32(nodes[l + 1][0].bias);
                        for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                        {
                            SetInput(input[s]);
                            grad[contador] += delta[l + 1][j] * nodes[l][i].value;
                            contador++;
                        }
                    }
                }
                contador = 0;
            }

            for(int g = 0; g < grad.Length; g++)
            {
                grad[g] /= -output.Length;

                //Console.WriteLine("Gradient computed : " + grad[g] + ", Gradient expected : " + check[g]);
            }
            //Console.WriteLine("/////");
            for (int g = 0; g < theta.Length; g++)
            {
                //Console.WriteLine("Theta#" + g +": " + theta[g]);
            }

            return grad;
        }

        public float[] unrollConnections()
        {
            float[] theta = new float[totalConnections];
            int cont = 0;
            for(int l = 0; l < L - 1; l++)
            {
                for(int i = 0; i < maxNodesPerLayer[l]; i++)
                {
                    for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                    {
                        theta[cont] = connections[l][i, j];
                        cont++;
                    }
                }
            }
            return theta;
        }

        public float[] unrollConnections(float[][,] connections)
        {
            float[] theta = new float[totalConnections];
            int cont = 0;
            for (int l = 0; l < L - 1; l++)
            {
                for (int i = 0; i < maxNodesPerLayer[l]; i++)
                {
                    for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                    {
                        theta[cont] = connections[l][i, j];
                        cont++;
                    }
                }
            }
            return theta;
        }

        public float[][,] rollConnections(float[] theta)
        {
            float[][,] result = new float[L - 1][,];

            int cont = 0;
            for(int l = 0; l < L - 1; l++)
            {
                result[l] = new float[maxNodesPerLayer[l], maxNodesPerLayer[l + 1]];
                for(int i = 0; i < maxNodesPerLayer[l]; i++)
                {
                    for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                    {
                        result[l][i, j] = theta[cont];
                        cont++;
                    }
                }
            }

            return result;
        }

        int totalConnections = 0;

        public NeuralNetwork(int[] maxNodesPerLayer, bool startConnected = true)
        {
            this.maxNodesPerLayer = maxNodesPerLayer;
            L = maxNodesPerLayer.Length;

            nodes = new Node[L][];
            for(int l = 0; l < L; l++)
            {
                nodes[l] = new Node[maxNodesPerLayer[l]];
            }
            
            for(int l = 0; l < L; l++)
            {
                for(int n = 0; n < maxNodesPerLayer[l]; n++)
                {
                    totalNodes++;
                    nodes[l][n].id = nextId;
                    nextId++;

                    if (l != L - 1 && n == 0)
                    {
                        nodes[l][0].value = 1;
                        nodes[l][0].bias = true;
                    }
                }
            }
            connections = new float[L - 1][,];

            for(int l = 0; l < L - 1; l++)
            {
                connections[l] = new float[maxNodesPerLayer[l], maxNodesPerLayer[l + 1]];
                totalConnections += maxNodesPerLayer[l] * maxNodesPerLayer[l + 1];
            }

            if (startConnected)
            {
                for(int l = 0; l < L - 1; l++)
                {
                    for (int i = 0; i < maxNodesPerLayer[l]; i++)
                    {
                        for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                        {
                            connections[l][i, j] = (float)(eps * random.NextDouble() - eps * random.NextDouble());
                        }
                    }
                }
            }
        }
        public void Connect(int layer, int input, int output,float weight)
        {
            connections[layer][input, output] = weight;
        }

        public void SetInput(float[] values)
        {
            for(int i = 1; i < maxNodesPerLayer[0]; i++)
            {
                nodes[0][i].value = values[i - 1];
            }
        }

        public float ComputeCost(float[][] input, float[] output, float[] theta, float lambda)
        {
            float[][,] Theta = rollConnections((float[])theta.Clone());

            float J = 0;
            float h = 0;
            for (int i = 0; i < output.Length; i++)
            {
                SetInput(input[i]);
                h = FeedForward(Theta);

                //Without reg
                J += (float)(output[i] * Math.Log(h) + (1 - output[i]) * Math.Log(1 - h));
            }
            J /= -output.Length;

            if (lambda > 0)
            {
                //reg
                float reg = 0;
                for (int l = 0; l < L - 1; l++)
                {
                    for (int i = 0; i < maxNodesPerLayer[l]; i++)
                    {
                        for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                        {
                            reg += (float)Math.Pow(Theta[l][i, j], 2);
                        }
                    }
                }
                reg *= lambda / (2 * output.Length);

                J += reg;
            }

            return J;
        }
        public void BackPropagation(float[][] input, float[] output, float learningRate,float lambda, int maxIter)
        {
            float[] theta = unrollConnections();
            GradientDescent.Start(gradient, ComputeCost, input, output, ref theta, learningRate, lambda, maxIter);
            connections = rollConnections(theta);
        }

        public float FeedForward(float[][,] connections)
        {                     
            for (int l = 0; l < L - 1; l++)
            {
                for (int j = Convert.ToInt32(nodes[l + 1][0].bias); j < maxNodesPerLayer[l + 1]; j++)
                {
                    nodes[l + 1][j].value = 0;
                    for (int i = 0; i < maxNodesPerLayer[l]; i++)
                    {
                        nodes[l + 1][j].value += nodes[l][i].value * connections[l][i, j];
                    }

                    nodes[l + 1][j].value = sigmoid(nodes[l + 1][j].value);
                }
            }

            return nodes[L - 1][0].value;
        }
        public float sigmoid(float z)
        {
            return (float)(1 / (1 + Math.Exp(-z)));
        }
    }
}
