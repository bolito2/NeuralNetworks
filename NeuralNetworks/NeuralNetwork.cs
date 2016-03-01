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

    public struct Connection
    {
        public float weight;
        public bool active;

        public Connection(float weight)
        {
            this.weight = weight;
            active = true;
        }
    }
    public class NeuralNetwork
    {
        //Input id, output id
        public Connection[,] connections;
        //Layer, lid
        public Node[,] nodes;

        public int nextId = 0;

        private int[] maxNodesPerLayer;
        public int L;

        public int totalNodes = 0;

        Random random = new Random();
        const float eps = 0.04f;

        public NeuralNetwork(int L, int[] maxNodesPerLayer, bool startConnected)
        {
            this.L = L;
            this.maxNodesPerLayer = maxNodesPerLayer;

            nodes = new Node[L, maxNodesPerLayer.Max()];
            
            for(int l = 0; l < L; l++)
            {
                for(int n = 0; n < maxNodesPerLayer[l]; n++)
                {
                    totalNodes++;
                    nodes[l, n].id = nextId;
                    nextId++;

                    if (l != L - 1 && n == 0)
                    {
                        nodes[l, 0].value = 1;
                        nodes[l, 0].bias = true;
                    }
                }
            }
            connections = new Connection[totalNodes, totalNodes];

            if (startConnected)
            {
                for(int c1 = 0; c1 < totalNodes; c1++)
                {
                    for (int c2 = 0; c2 < totalNodes; c2++)
                    {
                        if(layer(c2) == L -1 || lid(c2) != 0)
                        {
                            connections[c1, c2] = new Connection((float)(-eps + 2*eps*random.NextDouble()));
                        }
                    }
                }
            }
        }
        public void Connect(int inputId, int outputId, float weight)
        {
            connections[inputId, outputId] = new Connection(weight);
        }

        public void SetInput(float[] values)
        {
            for(int i = 1; i < maxNodesPerLayer[0]; i++)
            {
                nodes[0, i].value = values[i - 1];
            }
        }

        public float ComputeCost(float[][] input, float[] output, float lambda)
        {
            float J = 0;
            float h = 0;
            for (int i = 0; i < output.Length; i++)
            {
                SetInput(input[i]);
                h = FeedForward();

                //Without reg
                J += (float)(output[i] * Math.Log(h) + (1 - output[i]) * Math.Log(1 - h));
            }
            J /= -output.Length;

            //reg
            float reg = 0;
            for (int c1 = 0; c1 < totalNodes; c1++)
            {
                for (int c2 = 0; c2 < totalNodes; c2++)
                {
                    if (connections[c1, c2].active)
                    {
                        reg += (float)Math.Pow(connections[c1, c2].weight, 2);
                    }
                }
            }
            reg *= lambda / (2 * output.Length);

            J += reg;

            return J;
        }

        public void BackPropagation(float lambda)
        {

        }

        public float FeedForward()
        {
            for(int l = 1; l < L; l++)
            {
                for(int n = 0; n < maxNodesPerLayer[l]; n++)
                {
                    for(int c = 0; c < totalNodes; c++)
                    {
                        if (connections[c, map(l, n)].active && !nodes[l, n].bias)
                        {
                            //Console.WriteLine("Conexion entre l:" + l + ", nodo:" + n + " y l:" + layer(c) + ", nodo:" + lid(c));
                            nodes[l, n].value += connections[c, map(l, n)].weight * nodes[layer(c), lid(c)].value;
                        }
                    }
                    if(!nodes[l, n].bias)
                        nodes[l, n].value = sigmoid(nodes[l, n].value);
                    }
            }

            return nodes[L - 1, 0].value;
        }

        public int map(int layer, int lid)
        {
            return nodes[layer, lid].id;
        }

        public int layer(int id)
        {
            for(int l = 0; l < L; l++)
            {
                for(int n = 0; n < maxNodesPerLayer[l]; n++)
                {
                    if(nodes[l, n].id == id)
                    {
                        return l;
                    }
                }
            }
            return -1;
        }

        public int lid(int id)
        {
            for (int l = 0; l < L; l++)
            {
                for (int n = 0; n < maxNodesPerLayer[l]; n++)
                {
                    if (nodes[l, n].id == id)
                    {
                        return n;
                    }
                }
            }
            return -1;
        }
        public float sigmoid(float z)
        {
            return (float)(1 / (1 + Math.Exp(-z)));
        }
    }
}
