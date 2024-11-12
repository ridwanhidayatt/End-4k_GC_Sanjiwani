using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string noRMDefault, namaDaerah;

        // Simpan path gambar saat ini
        private string currentImagePath, currentImagePath2;

        private string dirLogo = @"D:\GLEndoscope\LogoKOP\logo1.png";
        private string dirLogo2 = @"D:\GLEndoscope\LogoKOP\logo2.png";

        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string dirr = @"D:\GLEndoscope\FileRTF\";

            string fileName = dirr + "RtfFile.rtf";
            richTextBox1.SaveFile(fileName, RichTextBoxStreamType.RichText);
            richTextBox1.Clear();


            string fileName1 = dirr + "RtfFile1.rtf";
            richTextBox2.SaveFile(fileName1, RichTextBoxStreamType.RichText);
            richTextBox2.Clear();


            string fileName2 = dirr + "RtfFile2.rtf";
            richTextBox3.SaveFile(fileName2, RichTextBoxStreamType.RichText);
            richTextBox3.Clear();


            string fileName3 = dirr + "RtfFile3.rtf";
            richTextBox4.SaveFile(fileName3, RichTextBoxStreamType.RichText);
            richTextBox4.Clear();

            string fileName4 = dirr + "RtfFile4.rtf";
            richTextBox5.SaveFile(fileName4, RichTextBoxStreamType.RichText);
            richTextBox5.Clear();

            string fileName5 = dirr + "RtfFile5.rtf";
            richTextBox6.SaveFile(fileName5, RichTextBoxStreamType.RichText);
            richTextBox6.Clear();



            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
            richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);




            string dir = @"D:\GLEndoscope\LogoKOP";
            string fileNameLogo1 = "logo1.png";
            string fileNameLogo2 = "logo2.png";

            // Ensure the directory exists
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                if (comboBox9.SelectedItem != null)
                {
                    int selectedValue = Convert.ToInt32(comboBox9.SelectedItem);

                    if (selectedValue == 1)
                    {
                        // Save pictureBox1 and logo1
                        SaveImage(pictureBox1.Image, Path.Combine(dir, fileNameLogo1));
                    }
                    else if (selectedValue == 2)
                    {
                        // Save pictureBox1, pictureBox2, logo1, and logo2
                        SaveImage(pictureBox1.Image, Path.Combine(dir, fileNameLogo1));
                        SaveImage(pictureBox2.Image, Path.Combine(dir, fileNameLogo2));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure resources are released
                pictureBox1.Image?.Dispose();
                pictureBox2.Image?.Dispose();
            }

            var xd = new XDocument(new XElement("userdata", new XElement("logo", comboBox9.Text), new XElement("jenis", comboBox1.Text)));

            xd.Save(@"D:\GLEndoscope\LogoKOP\logo.xml");

            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
            richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);

            MessageBox.Show("Data telah tersimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void SaveImage(Image image, string filePath)
        {
            try
            {
                using (Image img = image.Clone() as Image)
                {
                    if (img != null)
                    {
                        // Convert the image to PNG format before saving
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            img.Save(fs, ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure resources are released
                image?.Dispose();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Buka OpenFileDialog untuk memilih gambar baru
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = "C:\\";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simpan path gambar yang baru dipilih
                    currentImagePath = openFileDialog.FileName;

                    // Ganti gambar di PictureBox
                    pictureBox1.Image = Image.FromFile(currentImagePath);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadImage();
            button2.Enabled = false;
            button5.Enabled = true;
        }


        private void comboBox9_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Menyembunyikan semua PictureBox terlebih dahulu
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            button8.Visible = false;
            button1.Visible = false;

            // Memeriksa pilihan ComboBox dan menampilkan PictureBox yang sesuai
            switch (comboBox9.SelectedIndex)
            {
                case 0:
                    pictureBox1.Visible = true;
                    button8.Visible = true;
                    break;
                case 1:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    button8.Visible = true;
                    button1.Visible = true;
                    break;
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Menyembunyikan semua PictureBox terlebih dahulu
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            button8.Visible = false;
            button1.Visible = false;

            // Memeriksa pilihan ComboBox dan menampilkan PictureBox yang sesuai
            switch (comboBox9.SelectedIndex)
            {
                case 0:
                    pictureBox1.Visible = true;
                    button8.Visible = true;
                    break;
                case 1:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    button8.Visible = true;
                    button1.Visible = true;
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Memeriksa pilihan ComboBox1 dan mengubah ukuran PictureBox sesuai
            if (comboBox1.SelectedIndex == 0) // Jika memilih persegi
            {
                pictureBox1.Size = new Size(142, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(142, 142);
                pictureBox2.Location = new Point(1061, 11);

                richTextBox1.Size = new Size(896, 41);
                richTextBox1.Location = new Point(160, 11);

                richTextBox2.Size = new Size(896, 41);
                richTextBox2.Location = new Point(160, 54);

                richTextBox3.Size = new Size(896, 28);
                richTextBox3.Location = new Point(160, 96);

                richTextBox4.Size = new Size(896, 28);
                richTextBox4.Location = new Point(160, 126);

            }
            else if (comboBox1.SelectedIndex == 1) // Jika memilih persegi panjang
            {
                pictureBox1.Size = new Size(270, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(270, 142);
                pictureBox2.Location = new Point(934, 11);

                richTextBox1.Size = new Size(642, 41);
                richTextBox1.Location = new Point(288, 11);

                richTextBox2.Size = new Size(642, 41);
                richTextBox2.Location = new Point(288, 54);

                richTextBox3.Size = new Size(642, 28);
                richTextBox3.Location = new Point(288, 96);

                richTextBox4.Size = new Size(642, 28);
                richTextBox4.Location = new Point(288, 126);
            }
        }

        private void comboBox9_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            // Menyembunyikan semua PictureBox terlebih dahulu
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            button8.Visible = false;
            button1.Visible = false;

            // Memeriksa pilihan ComboBox dan menampilkan PictureBox yang sesuai
            switch (comboBox9.SelectedIndex)
            {
                case 0:
                    pictureBox1.Visible = true;
                    button8.Visible = true;
                    break;
                case 1:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    button8.Visible = true;
                    button1.Visible = true;
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Memeriksa pilihan ComboBox1 dan mengubah ukuran PictureBox sesuai
            if (comboBox1.SelectedIndex == 0) // Jika memilih persegi
            {
                pictureBox1.Size = new Size(142, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(142, 142);
                pictureBox2.Location = new Point(1061, 11);

                richTextBox1.Size = new Size(896, 41);
                richTextBox1.Location = new Point(160, 11);

                richTextBox2.Size = new Size(896, 41);
                richTextBox2.Location = new Point(160, 54);

                richTextBox3.Size = new Size(896, 28);
                richTextBox3.Location = new Point(160, 96);

                richTextBox4.Size = new Size(896, 28);
                richTextBox4.Location = new Point(160, 126);

            }
            else if (comboBox1.SelectedIndex == 1) // Jika memilih persegi panjang
            {
                pictureBox1.Size = new Size(270, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(270, 142);
                pictureBox2.Location = new Point(934, 11);

                richTextBox1.Size = new Size(642, 41);
                richTextBox1.Location = new Point(288, 11);

                richTextBox2.Size = new Size(642, 41);
                richTextBox2.Location = new Point(288, 54);

                richTextBox3.Size = new Size(642, 28);
                richTextBox3.Location = new Point(288, 96);

                richTextBox4.Size = new Size(642, 28);
                richTextBox4.Location = new Point(288, 126);
            }
        }

        private void comboBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            // Memeriksa pilihan ComboBox1 dan mengubah ukuran PictureBox sesuai
            if (comboBox1.SelectedIndex == 0) // Jika memilih persegi
            {
                pictureBox1.Size = new Size(142, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(142, 142);
                pictureBox2.Location = new Point(1061, 11);

                richTextBox1.Size = new Size(896, 41);
                richTextBox1.Location = new Point(160, 11);

                richTextBox2.Size = new Size(896, 41);
                richTextBox2.Location = new Point(160, 54);

                richTextBox3.Size = new Size(896, 28);
                richTextBox3.Location = new Point(160, 96);

                richTextBox4.Size = new Size(896, 28);
                richTextBox4.Location = new Point(160, 126);

            }
            else if (comboBox1.SelectedIndex == 1) // Jika memilih persegi panjang
            {
                pictureBox1.Size = new Size(270, 142);
                pictureBox1.Location = new Point(12, 11);
                pictureBox2.Size = new Size(270, 142);
                pictureBox2.Location = new Point(934, 11);

                richTextBox1.Size = new Size(642, 41);
                richTextBox1.Location = new Point(288, 11);

                richTextBox2.Size = new Size(642, 41);
                richTextBox2.Location = new Point(288, 54);

                richTextBox3.Size = new Size(642, 28);
                richTextBox3.Location = new Point(288, 96);

                richTextBox4.Size = new Size(642, 28);
                richTextBox4.Location = new Point(288, 126);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Buka OpenFileDialog untuk memilih gambar baru
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = "C:\\";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simpan path gambar yang baru dipilih
                    currentImagePath = openFileDialog.FileName;

                    // Ganti gambar di PictureBox
                    pictureBox1.Image = Image.FromFile(currentImagePath);
                }
            }
        }

        private void LoadImage()
        {
            // Use the default path that you specified
            currentImagePath = dirLogo;
            currentImagePath2 = dirLogo2;

            // Dispose of the existing image, if any
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            else if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
            }

            // Introduce a small delay before accessing the file
            System.Threading.Thread.Sleep(100);

            // Load the new image into PictureBox
            using (FileStream stream = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read))
            {
                pictureBox1.Image = Image.FromStream(stream);
            }

            using (FileStream stream = new FileStream(currentImagePath2, FileMode.Open, FileAccess.Read))
            {
                pictureBox2.Image = Image.FromStream(stream);
            }

            string dirr = @"D:\GLEndoscope\FileRTF\";
            if (Directory.Exists(dirr))
            {
                richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
                richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
                richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
                richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
                richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
                richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);

            }

            else
            {

            }


        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label17;
            comboBox1.SelectedIndex = 0;



            richTextBox1.BackColor = Color.White;
            richTextBox2.BackColor = Color.White;
            richTextBox3.BackColor = Color.White;
            richTextBox4.BackColor = Color.White;

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;

            //noRMDefault = KoneksiDatabase.mdr.GetString("no_rm");
            //Name = KoneksiDatabase.mdr.GetString("nama"); 
            //string tglKunjungan;
            //tglKunjungan = KoneksiDatabase.mdr.GetString("Tanggal Kunjungan");
            //namaDaerah = richTextBox5.Text; 




            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode, xmlnode1;
            int i = 0;
            FileStream fs = new FileStream(@"D:\GLEndoscope\LogoKOP\logo.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("logo");
            xmlnode1 = xmldoc.GetElementsByTagName("jenis");
            String dataXML = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
            String dataXML1 = xmlnode1[0].ChildNodes.Item(0).InnerText.Trim();



            comboBox9.Text = dataXML;
            comboBox1.Text = dataXML1;


            //FileStream fs = new FileStream(NameTb.Text + ".txt");
            //File.Create(fs);
            fs.Close();


            button5.Enabled = false;

            button8.Visible = true;
            button1.Visible = false;
            pictureBox2.Visible = false;

        }
    }
}
