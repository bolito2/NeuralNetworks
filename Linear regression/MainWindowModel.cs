using System;
using System.ComponentModel;
namespace Linear_regression
{
    using System;
    using System.IO;

    using OxyPlot;
    using OxyPlot.Series;

    public class MainWindowModel
    {
        public float RegressionGradient(float[][] input, float[] output, float[] theta, float lambda)
        {
            //without reg
            float grad = 0;


            return grad;
        }

        public MainWindowModel()
        {
            model = new PlotModel() { Title = "Ejemplo"};
            ScatterSeries series = new ScatterSeries();

            double maxX = 0;

            StreamReader sr = new StreamReader("data.txt");
            while (!sr.EndOfStream)
            {
                string[] xy = sr.ReadLine().Split(' ');

                int x = int.Parse(xy[0]);
                int y = int.Parse(xy[1]);

                series.Points.Add(new ScatterPoint(x, y));

                if (x > maxX)
                    maxX = x;
            }
            model.Series.Add(series);

            //TODO: Calcular hipotesis pambi

            double[] theta = {0, 0, 1};
            int steps = 6;

            LineSeries hypothesis = new LineSeries();

            for (int i = 0; i <= steps; i++)
            {
                double x = (i * maxX) / steps;

                double y = 0;
                for (int j = 0; j < theta.Length; j++)
                {
                    y += Math.Pow(x, j) * theta[j];
                }
                hypothesis.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(hypothesis);
        }
        public PlotModel model { get; private set; }
    }
}
