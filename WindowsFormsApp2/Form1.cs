using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        string namaFont, noRMDefault, namaDaerah;

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



            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);



            string dir = @"D:\GLEndoscope\LogoKOP\";

            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(dir + "logo1.png", ImageFormat.Png);
            }

            //namaDaerah = richTextBox5.Text;


            KoneksiDatabase.con.Open();
            string selectQuery4 = "SELECT * FROM tbl_default WHERE id_default ='1'";
            KoneksiDatabase.cmd = new MySqlCommand(selectQuery4, KoneksiDatabase.con);
            KoneksiDatabase.mdr = KoneksiDatabase.cmd.ExecuteReader();

            if (KoneksiDatabase.mdr.Read())
            {
                noRMDefault = KoneksiDatabase.mdr.GetString("no_rm");
                Name = KoneksiDatabase.mdr.GetString("nama");
            }
            KoneksiDatabase.con.Close();

            KoneksiDatabase.con.Open();
            string selectQuer2 = "SELECT tbl_pasienn.code AS 'Code', tbl_pasienn.no_rm AS 'No RM', tbl_pasienn.nama AS 'Nama', tbl_pasienn.jenis_kelamin AS 'Jenis Kelamin', tbl_pasienn.tanggal_lahir AS 'Tanggal Lahir', tbl_pasienn.umur AS 'Umur', tbl_pasienn.nama_jalan AS 'Nama Jalan',  tbl_pasienn.rt AS 'RT',  tbl_pasienn.rw AS 'RW', tbl_pasienn.desa_kelurahan AS 'Desa / Kelurahan', tbl_pasienn.kecamatan AS 'Kecamatan', tbl_pasienn.kota_kabupaten AS 'Kota / Kabupaten', tbl_pasienn.tanggal_kunjungan AS 'Tanggal Kunjungan', " +
                                 "tbl_pasienn.tindakan AS 'Tindakan', tbl_dokter.nama_dokter AS 'Nama Dokter', tbl_dokter.id_dokter, tbl_pasienn.id_pasien " +
                                 "FROM tbl_dokter " +
                                 "JOIN tbl_pasienn ON tbl_dokter.id_dokter = tbl_pasienn.id_dokter AND tbl_pasienn.no_rm ='" + noRMDefault + "'";
            KoneksiDatabase.cmd = new MySqlCommand(selectQuer2, KoneksiDatabase.con);
            KoneksiDatabase.mdr = KoneksiDatabase.cmd.ExecuteReader();

            if (KoneksiDatabase.mdr.Read())
            {
                string tglKunjungan;
                tglKunjungan = KoneksiDatabase.mdr.GetString("Tanggal Kunjungan");
                namaDaerah = richTextBox5.Text;
            }
            KoneksiDatabase.con.Close();



            var xd =
            new XDocument(
        new XElement(
            "userdata",
            new XElement("logo", comboBox9.Text)));

            xd.Save(@"D:\GLEndoscope\LogoKOP\logo.xml");




            //comboBox9.SelectedIndex = -1;

            //}

            //textBox4.Clear();
            //comboBox5.SelectedIndex = -1;
            //comboBox3.SelectedIndex = -1;
            //comboBox1.SelectedIndex = -1;
            //richTextBox5.Clear();



            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (of.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Image.Dispose();
                //pictureBox1.Image = null;
                pictureBox1.ImageLocation = of.FileName;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            this.ActiveControl = label17;

            string dirr = @"D:\GLEndoscope\FileRTF\";
            if (Directory.Exists(dirr))
            {
                richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
                richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
                richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
                richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
                richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);

            }
            else
            {


            }

            richTextBox1.BackColor = Color.White;
            richTextBox2.BackColor = Color.White;
            richTextBox3.BackColor = Color.White;
            richTextBox4.BackColor = Color.White;

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;

            KoneksiDatabase.con.Open();
            string selectQuery4 = "SELECT * FROM tbl_default WHERE id_default ='1'";
            KoneksiDatabase.cmd = new MySqlCommand(selectQuery4, KoneksiDatabase.con);
            KoneksiDatabase.mdr = KoneksiDatabase.cmd.ExecuteReader();

            if (KoneksiDatabase.mdr.Read())
            {
                noRMDefault = KoneksiDatabase.mdr.GetString("no_rm");
                Name = KoneksiDatabase.mdr.GetString("nama");
            }
            KoneksiDatabase.con.Close();

            KoneksiDatabase.con.Open();
            string selectQuer2 = "SELECT tbl_pasienn.code AS 'Code', tbl_pasienn.no_rm AS 'No RM', tbl_pasienn.nama AS 'Nama', tbl_pasienn.jenis_kelamin AS 'Jenis Kelamin', tbl_pasienn.tanggal_lahir AS 'Tanggal Lahir', tbl_pasienn.umur AS 'Umur', tbl_pasienn.nama_jalan AS 'Nama Jalan',  tbl_pasienn.rt AS 'RT',  tbl_pasienn.rw AS 'RW', tbl_pasienn.desa_kelurahan AS 'Desa / Kelurahan', tbl_pasienn.kecamatan AS 'Kecamatan', tbl_pasienn.kota_kabupaten AS 'Kota / Kabupaten', tbl_pasienn.tanggal_kunjungan AS 'Tanggal Kunjungan', " +
                                 "tbl_pasienn.tindakan AS 'Tindakan', tbl_dokter.nama_dokter AS 'Nama Dokter', tbl_dokter.id_dokter, tbl_pasienn.id_pasien " +
                                 "FROM tbl_dokter " +
                                 "JOIN tbl_pasienn ON tbl_dokter.id_dokter = tbl_pasienn.id_dokter AND tbl_pasienn.no_rm ='" + noRMDefault + "'";
            KoneksiDatabase.cmd = new MySqlCommand(selectQuer2, KoneksiDatabase.con);
            KoneksiDatabase.mdr = KoneksiDatabase.cmd.ExecuteReader();

            if (KoneksiDatabase.mdr.Read())
            {
                string tglKunjungan;
                tglKunjungan = KoneksiDatabase.mdr.GetString("Tanggal Kunjungan");
                namaDaerah = richTextBox5.Text;
            }
            KoneksiDatabase.con.Close();




            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode, xmlnode1;
            int i = 0;
            FileStream fs = new FileStream(@"D:\GLEndoscope\LogoKOP\logo.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("logo");
            String dataXML = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();



            comboBox9.Text = dataXML;


            //FileStream fs = new FileStream(NameTb.Text + ".txt");
            //File.Create(fs);
            fs.Close();


        }
    }
}
