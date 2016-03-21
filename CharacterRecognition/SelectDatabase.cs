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
    public partial class SelectDatabase : Form
    {
        public SelectDatabase()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DATABASE|*.dat";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
        StreamWriter dataWriter;

        public void setStream(StreamWriter sw)
        {
            dataWriter = sw;
        }

        Form1 form1;

        public void setForm(Form1 form)
        {
            form1 = form;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Enter a file path.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string data = "";
                using (StreamReader sr = new StreamReader(textBox1.Text))
                {
                    while (!sr.EndOfStream)
                    {
                        data += sr.ReadLine() + "\n";
                    }
                }

                dataWriter.Write(data);
                MessageBox.Show("Database loaded sucessfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
                form1.Show();
            }
        }

        private void SelectDatabase_Load(object sender, EventArgs e)
        {

        }

        private void SelectDatabase_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Show();
        }
    }
}
