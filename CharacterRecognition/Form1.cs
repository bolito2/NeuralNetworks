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

namespace CharacterRecognition
{
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(400, 400);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Black, 20f);

            sw = new StreamWriter("database.txt");      
        }

        bool painting = false;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            painting = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting)
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
            if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 ||
                e.KeyCode == Keys.D3 || e.KeyCode == Keys.D4 || e.KeyCode == Keys.D5 ||
                e.KeyCode == Keys.D6 || e.KeyCode == Keys.D7 || e.KeyCode == Keys.D8 ||
                e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.NumPad1 ||
                e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.NumPad4 ||
                e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad7 ||
                e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad9)
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
                        if (bitmap.GetPixel(i, j).Name == "ff000000")
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
                    gra.DrawImage(bitmap, new Rectangle(0, 0, dimension, dimension), new Rectangle(hcentro - (int)Math.Floor((double)dimension / 2), vcentro - (int)Math.Floor((double)dimension / 2), dimension, dimension), GraphicsUnit.Pixel);
                }

                Bitmap small = new Bitmap(25, 25);
                using (var g = Graphics.FromImage(small))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.DrawImage(cut, new Rectangle(0, 0, 25, 25));
                }
                pictureBox1.Image = small;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

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
                        pixels += " " + small.GetPixel(i, j).ToArgb().ToString();
                    }
                }

                sw.WriteLine(pixels);
                prueba();
            }
        }

        private void prueba()
        {
            sw.Close();

            using(StreamReader sr = new StreamReader("database.txt"))
            {
                string text = sr.ReadLine();
                string[] pixels = text.Split(' ');

                int contador = 1;
                Console.WriteLine("number = " + pixels[0]);

                Bitmap bit2 = new Bitmap(25, 25);
                for(int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        bit2.SetPixel(i, j, Color.FromArgb(int.Parse(pixels[contador])));
                        contador++;
                    }
                }
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox2.Image = bit2;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sw.Close();
        }
    }
}
