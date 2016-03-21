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

        public Training()
        {
            InitializeComponent();
        }

        public void OnProgress(int progress)
        {
            progressBar1.Value = 10 + progress * 9;
        }

        public void OnEnd(float init, float final)
        {
            MessageBox.Show("Cost gone from " + init + " to " + final, "Results!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Ended();
            Hide();
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

        private void button1_Click(object sender, EventArgs e)
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
                int numIter = int.Parse(textBox2.Text);

                float[][] input = new float[samples][];
                float[][] output = new float[samples][];

                label5.Text = "Reading database...";

                using(StreamReader sr = new StreamReader("database.dat"))
                {
                    string[] pixels;
                    int lastPercent = 0;

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
                        if ((int)Math.Floor((double)(sample * 10) / samples) > lastPercent)
                        {
                            lastPercent = (int)Math.Floor((double)(sample * 10) / samples);
                            Console.WriteLine((lastPercent * 10).ToString() + "%");
                            progressBar1.Value = lastPercent;
                        }
                    }
                }
                net.BackPropagation(input, output, learningRate, lambda, numIter);
            }
        }

        private void Training_FormClosed(object sender, FormClosedEventArgs e)
        {
            Ended();
            Hide();
        }
    }
}
