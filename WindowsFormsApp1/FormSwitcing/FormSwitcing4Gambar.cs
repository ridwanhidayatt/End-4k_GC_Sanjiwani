using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Windows.Forms;

namespace WindowsFormsApp1.FormSwitcing
{
    public partial class FormSwitcing4Gambar : Form
    {
        public delegate void TransfDelegate(String value);
        public event TransfDelegate TEFS4Gambar; 

        string splitTahun, splitBulan, tanggal, gabung, noRM;

        public FormSwitcing4Gambar()
        {
            InitializeComponent();
            Timer MyTimer = new Timer(); 
            MyTimer.Interval = 1; // 45 mins
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";
                ReadDataFromCSV(csvFilePath);
                textBox1.Clear();
            }
        }

        private void ReadDataFromCSV(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length < 2)
                {
                    MessageBox.Show("The CSV file is empty or does not contain the expected header.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Extract column names from the header
                string[] headers = lines[0].Split(',');

                // Output header names for debugging
                //MessageBox.Show($"Header names: {string.Join(", ", headers)}", "Debugging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                int noRMIndex = Array.IndexOf(headers, "Rm");
                int nameIndex = Array.IndexOf(headers, "Nama");
                int actionIndex = Array.IndexOf(headers, "JenisPemeriksaan");
                int dateIndex = Array.IndexOf(headers, "Tanggal Kunjungan");
                int tangalLahirIndex = Array.IndexOf(headers, "Tanggal Lahir");
                int umurIndex = Array.IndexOf(headers, "Umur");
                int alamatIndex = Array.IndexOf(headers, "Alamat");
                int namaDokterIndex = Array.IndexOf(headers, "Dokter");

                // Check if all required columns are present in the CSV file
                //if (noRMIndex == -1 || nameIndex == -1 || actionIndex == -1 || dateIndex == -1)
                //{
                //    MessageBox.Show("One or more required columns not found in the CSV file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                string[] values = lines[1].Split(',');
                noRM = values[noRMIndex].Trim();
                Name = values[nameIndex].Trim();
                gabung = noRM + "-" + Name;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.icon;
            pictureBox2.Image = Properties.Resources.icon;
            pictureBox3.Image = Properties.Resources.icon;
            pictureBox4.Image = Properties.Resources.icon;

            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox2.Image = null;
            pictureBox2.Update();
            pictureBox3.Image = null;
            pictureBox3.Update();
            pictureBox4.Image = null;
            pictureBox4.Update();

            int kondisi = 16;
            TEFS4Gambar(kondisi.ToString());
            this.Close();
        }

        private void buttonAdd3_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            of.InitialDirectory = "D:\\GLEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.ImageLocation = of.FileName;
                buttonDelete3.Visible = true;
                buttonAdd3.Visible = false;
            }
        }

        private void buttonDelete3_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
            pictureBox3.Update(); 
            buttonAdd3.Visible = true;
            buttonDelete3.Visible = false;
        }

        private void buttonAdd2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            of.InitialDirectory = "D:\\GLEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = of.FileName;
                buttonDelete2.Visible = true;
                buttonAdd2.Visible = false;
            }
        }

        private void buttonAdd1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            of.InitialDirectory = "D:\\GLEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = of.FileName;
                buttonDelete1.Visible = true;
                buttonAdd1.Visible = false;
            }
        }

        private void buttonDelete2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox2.Update(); 
            buttonAdd2.Visible = true;
            buttonDelete2.Visible = false;
        }

        private void buttonDelete1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update(); 
            buttonAdd1.Visible = true;
            buttonDelete1.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FormSwitcing4Gambar_Load(object sender, EventArgs e)
        {
            buttonDelete1.Visible = false; 
            buttonDelete2.Visible = false; 
            buttonDelete3.Visible = false; 
            buttonDelete4.Visible = false; 
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            tanggal = DateTime.Now.ToString("ddMMyyy"); 
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            of.InitialDirectory = "D:\\GLEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox4.ImageLocation = of.FileName;
                buttonDelete4.Visible = true;
                buttonAdd4.Visible = false;
            }
        }

        private void buttonDelete4_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = null;
            pictureBox4.Update(); 
            buttonAdd4.Visible = true;
            buttonDelete4.Visible = false;
        }
    }
}
