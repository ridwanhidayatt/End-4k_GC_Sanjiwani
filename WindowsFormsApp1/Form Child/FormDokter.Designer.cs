
namespace WindowsFormsApp1.Form_Utama
{
    partial class FormDokter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDokter));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.radioButtonWanita = new System.Windows.Forms.RadioButton();
            this.radioButtonPria = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_NipDokter = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_Search1 = new System.Windows.Forms.TextBox();
            this.btn_Search1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.txtCombo = new System.Windows.Forms.TextBox();
            this.txt_NamaDokter = new System.Windows.Forms.TextBox();
            this.txt_AlamatDokter = new System.Windows.Forms.TextBox();
            this.btn_DeleteForm = new System.Windows.Forms.Button();
            this.panelUser = new System.Windows.Forms.Panel();
            this.lbl_error = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.textBoxAkhir = new System.Windows.Forms.TextBox();
            this.textBoxAwal = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            // 
            // radioButtonWanita
            // 
            this.radioButtonWanita.AutoSize = true;
            this.radioButtonWanita.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonWanita.Location = new System.Drawing.Point(328, 166);
            this.radioButtonWanita.Name = "radioButtonWanita";
            this.radioButtonWanita.Size = new System.Drawing.Size(111, 22);
            this.radioButtonWanita.TabIndex = 84;
            this.radioButtonWanita.TabStop = true;
            this.radioButtonWanita.Text = "Perempuan";
            this.radioButtonWanita.UseVisualStyleBackColor = true;
            // 
            // radioButtonPria
            // 
            this.radioButtonPria.AutoSize = true;
            this.radioButtonPria.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonPria.Location = new System.Drawing.Point(181, 165);
            this.radioButtonPria.Name = "radioButtonPria";
            this.radioButtonPria.Size = new System.Drawing.Size(99, 22);
            this.radioButtonPria.TabIndex = 83;
            this.radioButtonPria.TabStop = true;
            this.radioButtonPria.Text = "Laki - laki";
            this.radioButtonPria.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(43, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 18);
            this.label10.TabIndex = 82;
            this.label10.Text = "Jenis Kelamin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(43, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 18);
            this.label9.TabIndex = 77;
            this.label9.Text = "NIP ";
            // 
            // txt_NipDokter
            // 
            this.txt_NipDokter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_NipDokter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_NipDokter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_NipDokter.Location = new System.Drawing.Point(179, 98);
            this.txt_NipDokter.MaxLength = 25;
            this.txt_NipDokter.Name = "txt_NipDokter";
            this.txt_NipDokter.Size = new System.Drawing.Size(582, 24);
            this.txt_NipDokter.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txt_Search1);
            this.panel1.Controls.Add(this.btn_Search1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.btn_Refresh);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 363);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1534, 550);
            this.panel1.TabIndex = 72;
            // 
            // txt_Search1
            // 
            this.txt_Search1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Search1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Search1.Location = new System.Drawing.Point(1150, 11);
            this.txt_Search1.Name = "txt_Search1";
            this.txt_Search1.Size = new System.Drawing.Size(288, 24);
            this.txt_Search1.TabIndex = 65;
            // 
            // btn_Search1
            // 
            this.btn_Search1.Image = ((System.Drawing.Image)(resources.GetObject("btn_Search1.Image")));
            this.btn_Search1.Location = new System.Drawing.Point(1448, 7);
            this.btn_Search1.Name = "btn_Search1";
            this.btn_Search1.Size = new System.Drawing.Size(30, 30);
            this.btn_Search1.TabIndex = 64;
            this.btn_Search1.UseVisualStyleBackColor = true;
            this.btn_Search1.Click += new System.EventHandler(this.btn_Search1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(185, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 71;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 59);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1496, 480);
            this.dataGridView1.TabIndex = 68;
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_Refresh.Image")));
            this.btn_Refresh.Location = new System.Drawing.Point(1487, 8);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(30, 30);
            this.btn_Refresh.TabIndex = 67;
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(1326, 951);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(232, 36);
            this.button3.TabIndex = 69;
            this.button3.Text = "Keluar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(434, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 31);
            this.label5.TabIndex = 68;
            this.label5.Text = "Data Dokter";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(43, 205);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 18);
            this.label7.TabIndex = 58;
            this.label7.Text = "Alamat";
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(129)))), ((int)(((byte)(13)))));
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Save.Location = new System.Drawing.Point(620, 281);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(141, 38);
            this.btn_Save.TabIndex = 60;
            this.btn_Save.Text = "Simpan";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 44;
            this.label1.Text = "Nama";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(179, 281);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(141, 36);
            this.btn_Cancel.TabIndex = 61;
            this.btn_Cancel.Text = "Batal";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // txtCombo
            // 
            this.txtCombo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCombo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCombo.Location = new System.Drawing.Point(362, 128);
            this.txtCombo.Name = "txtCombo";
            this.txtCombo.Size = new System.Drawing.Size(0, 20);
            this.txtCombo.TabIndex = 70;
            // 
            // txt_NamaDokter
            // 
            this.txt_NamaDokter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_NamaDokter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_NamaDokter.Location = new System.Drawing.Point(179, 128);
            this.txt_NamaDokter.MaxLength = 45;
            this.txt_NamaDokter.Name = "txt_NamaDokter";
            this.txt_NamaDokter.Size = new System.Drawing.Size(582, 24);
            this.txt_NamaDokter.TabIndex = 1;
            this.txt_NamaDokter.TextChanged += new System.EventHandler(this.txt_NamaDokter_TextChanged);
            // 
            // txt_AlamatDokter
            // 
            this.txt_AlamatDokter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_AlamatDokter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_AlamatDokter.Location = new System.Drawing.Point(179, 205);
            this.txt_AlamatDokter.Multiline = true;
            this.txt_AlamatDokter.Name = "txt_AlamatDokter";
            this.txt_AlamatDokter.Size = new System.Drawing.Size(582, 60);
            this.txt_AlamatDokter.TabIndex = 3;
            // 
            // btn_DeleteForm
            // 
            this.btn_DeleteForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_DeleteForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DeleteForm.ForeColor = System.Drawing.Color.White;
            this.btn_DeleteForm.Image = ((System.Drawing.Image)(resources.GetObject("btn_DeleteForm.Image")));
            this.btn_DeleteForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_DeleteForm.Location = new System.Drawing.Point(473, 281);
            this.btn_DeleteForm.Name = "btn_DeleteForm";
            this.btn_DeleteForm.Size = new System.Drawing.Size(141, 38);
            this.btn_DeleteForm.TabIndex = 63;
            this.btn_DeleteForm.Text = "Hapus";
            this.btn_DeleteForm.UseVisualStyleBackColor = false;
            this.btn_DeleteForm.Click += new System.EventHandler(this.btn_DeleteForm_Click);
            // 
            // panelUser
            // 
            this.panelUser.BackColor = System.Drawing.Color.White;
            this.panelUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUser.Controls.Add(this.lbl_error);
            this.panelUser.Controls.Add(this.button5);
            this.panelUser.Controls.Add(this.dataGridView2);
            this.panelUser.Controls.Add(this.textBoxAkhir);
            this.panelUser.Controls.Add(this.textBoxAwal);
            this.panelUser.Controls.Add(this.textBox3);
            this.panelUser.Controls.Add(this.textBox2);
            this.panelUser.Controls.Add(this.textBox1);
            this.panelUser.Controls.Add(this.radioButtonWanita);
            this.panelUser.Controls.Add(this.radioButtonPria);
            this.panelUser.Controls.Add(this.label10);
            this.panelUser.Controls.Add(this.label9);
            this.panelUser.Controls.Add(this.txt_NipDokter);
            this.panelUser.Controls.Add(this.panel1);
            this.panelUser.Controls.Add(this.button3);
            this.panelUser.Controls.Add(this.label5);
            this.panelUser.Controls.Add(this.label7);
            this.panelUser.Controls.Add(this.btn_Save);
            this.panelUser.Controls.Add(this.label1);
            this.panelUser.Controls.Add(this.btn_Cancel);
            this.panelUser.Controls.Add(this.txtCombo);
            this.panelUser.Controls.Add(this.txt_NamaDokter);
            this.panelUser.Controls.Add(this.txt_AlamatDokter);
            this.panelUser.Controls.Add(this.btn_DeleteForm);
            this.panelUser.Location = new System.Drawing.Point(12, 12);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(1563, 992);
            this.panelUser.TabIndex = 74;
            // 
            // lbl_error
            // 
            this.lbl_error.AutoSize = true;
            this.lbl_error.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_error.Location = new System.Drawing.Point(767, 134);
            this.lbl_error.Name = "lbl_error";
            this.lbl_error.Size = new System.Drawing.Size(110, 13);
            this.lbl_error.TabIndex = 142;
            this.lbl_error.Text = "*max 45 character";
            this.lbl_error.Visible = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(326, 281);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(141, 38);
            this.button5.TabIndex = 141;
            this.button5.Text = "Ubah";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(742, 93);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(0, 0);
            this.dataGridView2.TabIndex = 92;
            // 
            // textBoxAkhir
            // 
            this.textBoxAkhir.Location = new System.Drawing.Point(847, 260);
            this.textBoxAkhir.Name = "textBoxAkhir";
            this.textBoxAkhir.Size = new System.Drawing.Size(1, 20);
            this.textBoxAkhir.TabIndex = 91;
            // 
            // textBoxAwal
            // 
            this.textBoxAwal.Location = new System.Drawing.Point(814, 260);
            this.textBoxAwal.Name = "textBoxAwal";
            this.textBoxAwal.Size = new System.Drawing.Size(1, 20);
            this.textBoxAwal.TabIndex = 90;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(78, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(1, 20);
            this.textBox3.TabIndex = 89;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(117, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(1, 20);
            this.textBox2.TabIndex = 88;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1, 20);
            this.textBox1.TabIndex = 87;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // FormDokter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1587, 1016);
            this.ControlBox = false;
            this.Controls.Add(this.panelUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDokter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDokter_FormClosed);
            this.Load += new System.EventHandler(this.FormDokter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton radioButtonWanita;
        private System.Windows.Forms.RadioButton radioButtonPria;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_NipDokter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_Search1;
        private System.Windows.Forms.Button btn_Search1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Cancel;
        public System.Windows.Forms.TextBox txtCombo;
        private System.Windows.Forms.TextBox txt_NamaDokter;
        private System.Windows.Forms.TextBox txt_AlamatDokter;
        private System.Windows.Forms.Button btn_DeleteForm;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TextBox textBoxAkhir;
        private System.Windows.Forms.TextBox textBoxAwal;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label lbl_error;
    }
}