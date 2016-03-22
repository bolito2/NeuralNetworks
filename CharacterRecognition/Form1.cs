using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NeuralNetworks;

namespace CharacterRecognition
{
    public enum Mode
    {
        Training,
        Visualizing,
        Guessing
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap bitmap;
        Graphics graphics;
        Pen pen;

        StreamWriter sw;

        public Mode mode;
        int numberSelected = 0;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(400, 400);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Black, 25f);

            trainToolStripMenuItem_Click(this, new EventArgs());
        }

        bool painting = false;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            painting = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting && mode != Mode.Visualizing)
            {
                graphics.FillEllipse(pen.Brush, e.X, e.Y, pen.Width, pen.Width);
                pictureBox1.Image = bitmap;
            }          
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            painting = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(mode == Mode.Guessing)
            {
                if(e.KeyCode == Keys.Space)
                {
                    float[] input = new float[625];
                    Bitmap small = toSmall(bitmap);
                    for(int i = 0; i < 25; i++)
                    {
                        for (int j = 0; j < 25; j++)
                        {
                            input[25 * i + j] = (float)(small.GetPixel(i, j).A)/255;
                        }
                    }

                    net.SetInput(input);
                    float[] output = net.FeedForward();
                    int maxIndex = 0;
                    float maxValue = 0;

                    for(int j = 0; j < output.Length; j++)
                    {
                        if (output[j] > maxValue) { maxIndex = j; maxValue = output[j]; };
                    }

                    label1.Text = "Number is " + maxIndex + "(" + (maxValue * 100).ToString() + "%)";
                }
                else clear();
            }
            if (mode == Mode.Training)
            {
                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 ||
                e.KeyCode == Keys.D3 || e.KeyCode == Keys.D4 || e.KeyCode == Keys.D5 ||
                e.KeyCode == Keys.D6 || e.KeyCode == Keys.D7 || e.KeyCode == Keys.D8 ||
                e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.NumPad1 ||
                e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.NumPad4 ||
                e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad7 ||
                e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad9)
                {
                    Bitmap small = toSmall(bitmap);
                    
                    //pictureBox1.Image = small;
                    //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    string pixels = "";

                    if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                        pixels = "0";
                    if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                        pixels = "1";
                    if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                        pixels = "2";
                    if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                        pixels = "3";
                    if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                        pixels = "4";
                    if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                        pixels = "5";
                    if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                        pixels = "6";
                    if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                        pixels = "7";
                    if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                        pixels = "8";
                    if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                        pixels = "9";

                    Console.WriteLine(pixels);
                    for (int i = 0; i < 25; i++)
                    {
                        for (int j = 0; j < 25; j++)
                        {
                            pixels += " " + (float)small.GetPixel(i, j).A/255;
                }
                    }
                    clear();
                    sw.WriteLine(pixels);
                }
                else
                {
                    clear();
                }
            }
            else if(mode == Mode.Visualizing)
            {
                if(e.KeyCode == Keys.Right)
                {
                    if (numberSelected == numNumeros - 1)
                        numberSelected = 0;
                    else
                        numberSelected++;

                    visualizeNumber(numberSelected);
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (numberSelected == 0)
                        numberSelected = numNumeros - 1;
                    else
                        numberSelected--;

                    visualizeNumber(numberSelected);
                }
                if(e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    sr.Close();

                    File.Copy("database.dat", "copy.dat", true);
                    File.Delete("database.dat");

                    string line = "";
                    int deleted = numberSelected;

                    using(StreamReader reader = new StreamReader("copy.dat"))
                    {
                        using (StreamWriter writer = new StreamWriter("database.dat", true))
                        {
                            int cont = 0;
                            while (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();

                                if (cont != numberSelected)
                                    writer.WriteLine(line);

                                cont++;
                            }
                        }
                    }
                    visualizeToolStripMenuItem_Click(this, new EventArgs());

                    if(deleted == numNumeros)
                        numberSelected = deleted - 1;
                    else
                        numberSelected = deleted;
                    visualizeNumber(numberSelected);
                }
            }
        }
        Bitmap toSmall(Bitmap big)
        {
            int arriba = 0;
            int abajo = 399;
            int derecha = 0;
            int izquierda = 399;

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    //Console.WriteLine("color: " + bitmap.GetPixel(i, j).Name);
                    if (big.GetPixel(i, j).A > 0)
                    {
                        if (i > derecha)
                            derecha = i;
                        if (i < izquierda)
                            izquierda = i;
                        if (j > arriba)
                            arriba = j;
                        if (j < abajo)
                            abajo = j;
                    }
                }
            }

            int ancho = derecha - izquierda;
            int alto = arriba - abajo;
            int dimension = Math.Max(ancho, alto);

            int vcentro = (int)Math.Floor((double)(arriba - abajo) / 2) + abajo;
            int hcentro = (int)Math.Floor((double)(derecha - izquierda) / 2) + izquierda;

            Bitmap cut = new Bitmap(dimension, dimension);
            using (Graphics gra = Graphics.FromImage(cut))
            {
                gra.DrawImage(big, new Rectangle(0, 0, dimension, dimension), new Rectangle(hcentro - (int)Math.Floor((double)dimension / 2), vcentro - (int)Math.Floor((double)dimension / 2), dimension, dimension), GraphicsUnit.Pixel);
            }

            Bitmap small = new Bitmap(25, 25);
            using (var g = Graphics.FromImage(small))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(cut, new Rectangle(0, 0, 25, 25));
            }

            return small;
        }

        private void clear()
        {
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    bitmap.SetPixel(i, j, Color.Transparent);
                }
            }
            pictureBox1.Image = bitmap;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sw.Close();
        }

        private void insertDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectDatabase sd = new SelectDatabase();
            sd.Show();
            sd.setStream(sw);
            sd.setForm(this);
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = Mode.Training;
            pictureBox1.Image = null;
            label1.Text = "Train with your drawings!";
            if(sr != null)
                sr.Close();
            sw = new StreamWriter("database.dat", true);
        }

        StreamReader sr;

        private void visualizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sw.Close();
            sr = new StreamReader("database.dat");
            if (sr.ReadLine() == "")
            {
                Console.WriteLine("HUEHUEHUEHUEHUEHEUEHEUEHEUEEHU");
                MessageBox.Show("Database is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sr.Close();
                sw = new StreamWriter("database.dat", true);
                return;
            }
            sr.Close();
            sr = new StreamReader("database.dat");

            mode = Mode.Visualizing;
            numberSelected = 0;

            databaseLines.Clear();

            int cont = 0;
            while (!sr.EndOfStream)
            {
                databaseLines.Add(sr.ReadLine());
                cont++;
            }
            numNumeros = cont;

            visualizeNumber(0);

            sr.Close();
        }
        int numNumeros;

        int getSamples()
        {
            if(sw != null)
                sw.Close();
            if(sr != null)
                sr.Close();

            int samples = 0;

            using (StreamReader read = new StreamReader("database.dat"))
            {
                while (!read.EndOfStream)
                {
                    read.ReadLine();
                    samples++;
                }
            }

            if (mode == Mode.Visualizing)
                sr = new StreamReader("database.dat");
            else
                sw = new StreamWriter("database.dat", true);

            return samples;
        }

        private void visualizeNumber(int index)
        {
            string[] pixels = databaseLines[index].Split(' ');
            label1.Text = "Number: " + pixels[0] + ", Index: " + index;
            Bitmap bmp = new Bitmap(25, 25);

            int cont = 1;
            for(int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb((int)(float.Parse(pixels[cont]) * 255), 0, 0, 0));
                    //bmp.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                    cont++;
                }
            }

            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        List<string> databaseLines = new List<string>();

        private void guessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (net == null)
            {
                MessageBox.Show("Train or download a neural network before guessing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mode = Mode.Guessing;
            pictureBox1.Image = null;
            label1.Text = "Draw and see your number!";
        }
        NeuralNetwork net;
        public void endTraining()
        {
            if (mode == Mode.Visualizing)
                sr = new StreamReader("database.dat");
            else
                sw = new StreamWriter("database.dat", true);
            Show();
        } 

        private void trainToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Hide();
            Training train = new Training();
            train.Ended += endTraining;
            train.Samples(getSamples());
            if (sw != null)
                sw.Close();
            if (sr != null)
                sr.Close();

            train.trainNeuralNetwork(ref net);           
        }
    }
}
