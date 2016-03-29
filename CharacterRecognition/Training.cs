using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNetworks;
using System.IO;

namespace CharacterRecognition
{
    public partial class Training : Form
    {
        public delegate void endTraining();
        public event endTraining Ended;

        delegate void ProgressCallBack(int value, int iter, float cost);

        public Training()
        {
            InitializeComponent();
        }

        public void OnProgress(int progress, int iter, float cost)
        {
            this.setProgress(progress, iter, cost);
        }
        private void setProgress(int value, int iter, float cost)
        {
            if (this.progressBar1.InvokeRequired)
            {
                ProgressCallBack pcb = new ProgressCallBack(setProgress);
                this.Invoke(pcb, new object[] { value, iter, cost });
            }
            else
            {
                progressBar1.Value = value;
                label5.Text = "Training, iteration " + (iter + 1) + "/" + numIter + ", cost = " + cost;
            }
        }

        public delegate void endCallBack(float i, float f);

        public void OnEnd(float init, float final)
        {
            this.end(init, final);  
        }

        private void end(float init, float final)
        {
            if (this.InvokeRequired)
            {
                endCallBack callback = new endCallBack(OnEnd);
                Invoke(callback, new object[] { init, final });
            }
            else
            {
                label5.Text = "Completed";
                MessageBox.Show("Cost gone from " + init + " to " + final, "Results!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Ended();
                Hide();
            }
        }

        NeuralNetwork net;

        public void trainNeuralNetwork(ref NeuralNetwork nn)
        {
            nn = new NeuralNetwork(new int[] { 626, 26, 10 });
            net = nn;
            Console.WriteLine(net.Equals(nn));

            GradientDescent.GetEnd += OnEnd;
            GradientDescent.GetProgress += OnProgress;

            Show();
            label1.Text = "Select Parameters";
            label5.Visible = false;
            progressBar1.Visible = false;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        int samples;

        public void Samples(int numNumeros)
        {
            samples = numNumeros;
            Console.WriteLine(samples);
        }

        int numIter = 0;

        private async void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Enter the parameters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Console.WriteLine("XDDDDDDDDDDDDDDDDDDDDD");
                label1.Text = "Training Neural Network...";
                progressBar1.Visible = true;
                label5.Visible = true;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                button1.Enabled = false;
                button1.Visible = false;

                float lambda = float.Parse(textBox3.Text);
                float learningRate = float.Parse(textBox1.Text);
                numIter = int.Parse(textBox2.Text);

                float[][] input = new float[samples][];
                float[][] output = new float[samples][];

                label5.Text = "Reading database...";

                using(StreamReader sr = new StreamReader("database.dat"))
                {
                    string[] pixels;

                    for (int sample = 0; sample < samples; sample++)
                    {
                        input[sample] = new float[625];
                        output[sample] = new float[10];
                        
                        pixels = sr.ReadLine().Split(' ');

                        output[sample][int.Parse(pixels[0])] = 1;
                        for(int i = 0; i < 625; i++)
                        {
                            input[sample][i] = float.Parse(pixels[i + 1]);
                        }
                    }
                }
                label5.Text = "Training, iteration " + 1 + "/" + numIter + ", cost = " + net.ComputeCost(input, output, net.unrollConnections(), lambda);
                Task backProp = new Task(() => net.BackPropagation(input, output, learningRate, lambda, numIter));
                backProp.Start();
            }
        }

        private void Training_FormClosed(object sender, FormClosedEventArgs e)
        {
            Ended();
            Hide();
        }
    }
}
