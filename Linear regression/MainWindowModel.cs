using System;
using System.ComponentModel;
using NeuralNetworks;

namespace Linear_regression
{
    using System;
    using System.IO;

    using OxyPlot;
    using OxyPlot.Series;

    public class MainWindowModel
    { 
        public float RegressionCost(float[][] input, float[] output, float[] theta, float lambda)
        {
            float J = 0;

            for(int i = 0; i < output.Length; i++)
            {
                float prediction = 0;
                for(int j = 0; j < theta.Length; j++)
                {
                    prediction += input[i][j] * theta[j];
                }

                J += (float)Math.Pow(output[i] - prediction, 2);
            }
            J /= output.Length;

            //reg
            float reg = 0;
            for(int j = 1; j < theta.Length; j++)
            {
                reg += (float)Math.Pow(theta[j], 2);
            }
            reg *= (lambda) / (2 * output.Length);

            J += reg;

            return J;
        }
        
        public float[] RegressionGradient(float[][] input, float[] output, float[] theta, float lambda)
        {
            //without reg
            float[] grad = new float[theta.Length];

            for(int i = 0; i < output.Length; i++)
            {
                float h = 0;
                for (int j = 0; j < theta.Length; j++)
                {
                    h += input[i][j] * theta[j];
                }
                for (int j = 0; j < theta.Length; j++)
                {
                    grad[j] += (output[i] - h) * input[i][j];
                }
            }

            for (int j = 0; j < theta.Length; j++)
            {
                grad[j] /= output.Length;
            }

            //reg
            for(int j = 1; j < theta.Length; j++)
            {
                grad[j] += (theta[j] * lambda) / output.Length;
            }
            return grad;
        }

        public MainWindowModel()
        {
            model = new PlotModel() { Title = "Ejemplo"};
            ScatterSeries series = new ScatterSeries();

            double maxX = 0;

            StreamReader sr = new StreamReader("data.txt");
            string[] initParams = sr.ReadLine().Split(' ');
            int m = int.Parse(initParams[0]);
            int n = int.Parse(initParams[1]);

            float[][] input = new float[m][];
            for(int i = 0; i < m; i++)
            {
                input[i] = new float[n];
            }

            float[] output = new float[m];

            for(int i = 0; i < m; i++)
            {
                string[] xy = sr.ReadLine().Split(' ');

                double x = double.Parse(xy[0]);
                double y = double.Parse(xy[1]);

                series.Points.Add(new ScatterPoint(x, y));
                output[i] = (float)y;
                for(int j = 0; j < n; j++)
                {
                    input[i][j] = (float)Math.Pow(x, j);
                }

                if (x > maxX)
                    maxX = x;
            }
            sr.Close();

            model.Series.Add(series);

            float[] theta = new float[n];

            //TODO: Calcular hipotesis pambi
            GradientDescent.Start(RegressionGradient, RegressionCost,input, output,ref theta, .000125f, 0, 500000);
            //
            int steps = 50;

            LineSeries hypothesis = new LineSeries();

            for (int i = 0; i <= steps; i++)
            {
                double[] x = new double[n];

                for (int j = 0; j < n; j++)
                {
                    x[j] = Math.Pow((i * maxX) / steps, j);
                }

                double y = 0;
                for (int j = 0; j < n; j++)
                {
                    y += x[j] * theta[j];
                }
                hypothesis.Points.Add(new DataPoint(x[1], y));
            }

            model.Series.Add(hypothesis);
        }
        public PlotModel model { get; private set; }
    }
}
