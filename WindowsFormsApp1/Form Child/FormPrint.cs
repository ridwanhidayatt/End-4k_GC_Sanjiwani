using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormPrint : Form
    { 
        public delegate void TransfDelegate(String value);
        public event TransfDelegate TransfEventtttt;
        public event TransfDelegate TransfEventPrint1; 

        public FormPrint()
        {
            InitializeComponent();
        }

        private void FormPrint_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Format 1")
            {
                //comboBox1.SelectedIndex = -1;
                //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                //int kondisi = 1;
                //TransfEventPrint1(kondisi.ToString());
                //this.Close();

                comboBox1.SelectedIndex = -1;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                int kondisi = 8;
                TransfEventPrint1(kondisi.ToString());
                this.Close();
            }
            else if (comboBox1.Text == "Format 2")
            {
                //comboBox1.SelectedIndex = -1;
                //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                //int sendFormUtama = 4;
                //TransfEventPrint1(sendFormUtama.ToString());
                //this.Close();

                comboBox1.SelectedIndex = -1;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                int sendFormUtama = 9;
                TransfEventPrint1(sendFormUtama.ToString());
                this.Close();
            }
            //else if (comboBox1.Text == "Format 3")
            //{
            //    comboBox1.SelectedIndex = -1;
            //    comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            //    int sendFormUtama = 8;
            //    TransfEventPrint1(sendFormUtama.ToString());
            //    this.Close();
            //}
            //else if (comboBox1.Text == "Format 4")
            //{
            //    comboBox1.SelectedIndex = -1;
            //    comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            //    int sendFormUtama = 9;
            //    TransfEventPrint1(sendFormUtama.ToString());
            //    this.Close();
            //}
            else
            {
                MessageBox.Show("Pilih format print");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            int kondisi = 6;
            TransfEventtttt(kondisi.ToString());
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (panel1.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 2;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.Black, thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness, halfThickness,panel1.ClientSize.Width - thickness,panel1.ClientSize.Height - thickness));
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
