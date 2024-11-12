
namespace WindowsFormsApp1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtFoot = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.txt_Form = new System.Windows.Forms.TextBox();
            this.txt_kondisi = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonRecSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.buttonRecStop = new System.Windows.Forms.Button();
            this.buttonDokter = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panelPatientData = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Record_OBS = new System.Windows.Forms.Button();
            this.btn_patient = new System.Windows.Forms.Button();
            this.buttonRecStart = new System.Windows.Forms.Button();
            this.btn_Capture = new System.Windows.Forms.Button();
            this.textBoxPrint = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblRec1 = new System.Windows.Forms.Label();
            this.picRec1 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelBawah = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panelKiri = new System.Windows.Forms.Panel();
            this.panelKanan = new System.Windows.Forms.Panel();
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panelPatientData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRec1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelBawah.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFoot
            // 
            this.txtFoot.Location = new System.Drawing.Point(-21, 11);
            this.txtFoot.Name = "txtFoot";
            this.txtFoot.Size = new System.Drawing.Size(1, 10);
            this.txtFoot.TabIndex = 64;
            this.txtFoot.TextChanged += new System.EventHandler(this.txtFoot_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(21, 98);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(1, 10);
            this.textBox2.TabIndex = 66;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.Color.White;
            this.lblCode.Location = new System.Drawing.Point(62, 37);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(53, 16);
            this.lblCode.TabIndex = 63;
            this.lblCode.Text = "No. RM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(4, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 61;
            this.label4.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 60;
            this.label1.Text = "No. RM";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(4, 108);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(0, 0);
            this.button4.TabIndex = 59;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // txt_Form
            // 
            this.txt_Form.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Form.Location = new System.Drawing.Point(41, 20);
            this.txt_Form.Name = "txt_Form";
            this.txt_Form.Size = new System.Drawing.Size(0, 10);
            this.txt_Form.TabIndex = 55;
            this.txt_Form.Text = "1";
            // 
            // txt_kondisi
            // 
            this.txt_kondisi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_kondisi.Location = new System.Drawing.Point(5, 668);
            this.txt_kondisi.Name = "txt_kondisi";
            this.txt_kondisi.Size = new System.Drawing.Size(0, 10);
            this.txt_kondisi.TabIndex = 18;
            this.txt_kondisi.TextChanged += new System.EventHandler(this.txt_kondisi_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(3, 580);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(0, 10);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "1";
            // 
            // buttonRecSave
            // 
            this.buttonRecSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.buttonRecSave.FlatAppearance.BorderSize = 0;
            this.buttonRecSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecSave.ForeColor = System.Drawing.Color.White;
            this.buttonRecSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonRecSave.Image")));
            this.buttonRecSave.Location = new System.Drawing.Point(173, 486);
            this.buttonRecSave.Name = "buttonRecSave";
            this.buttonRecSave.Size = new System.Drawing.Size(145, 117);
            this.buttonRecSave.TabIndex = 7;
            this.buttonRecSave.Text = "Mulai Rekam";
            this.buttonRecSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRecSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonRecSave.UseVisualStyleBackColor = false;
            this.buttonRecSave.Click += new System.EventHandler(this.btn_Record_Click_1);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.panel2.Controls.Add(this.pictureBox3);
            this.panel2.Controls.Add(this.buttonRecStop);
            this.panel2.Controls.Add(this.buttonDokter);
            this.panel2.Controls.Add(this.txtFoot);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.panelPatientData);
            this.panel2.Controls.Add(this.btn_Record_OBS);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.txt_Form);
            this.panel2.Controls.Add(this.txt_kondisi);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.btn_patient);
            this.panel2.Controls.Add(this.buttonRecStart);
            this.panel2.Controls.Add(this.btn_Capture);
            this.panel2.Controls.Add(this.buttonRecSave);
            this.panel2.Controls.Add(this.textBoxPrint);
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(1588, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(358, 1016);
            this.panel2.TabIndex = 63;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(4, 17);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(324, 77);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 77;
            this.pictureBox3.TabStop = false;
            // 
            // buttonRecStop
            // 
            this.buttonRecStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.buttonRecStop.FlatAppearance.BorderSize = 0;
            this.buttonRecStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecStop.ForeColor = System.Drawing.Color.White;
            this.buttonRecStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonRecStop.Image")));
            this.buttonRecStop.Location = new System.Drawing.Point(173, 336);
            this.buttonRecStop.Name = "buttonRecStop";
            this.buttonRecStop.Size = new System.Drawing.Size(145, 117);
            this.buttonRecStop.TabIndex = 76;
            this.buttonRecStop.Text = "Hentikan Kamera";
            this.buttonRecStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRecStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonRecStop.UseVisualStyleBackColor = false;
            this.buttonRecStop.Click += new System.EventHandler(this.buttonRecStop_Click);
            // 
            // buttonDokter
            // 
            this.buttonDokter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.buttonDokter.FlatAppearance.BorderSize = 0;
            this.buttonDokter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDokter.ForeColor = System.Drawing.Color.White;
            this.buttonDokter.Image = ((System.Drawing.Image)(resources.GetObject("buttonDokter.Image")));
            this.buttonDokter.Location = new System.Drawing.Point(16, 636);
            this.buttonDokter.Name = "buttonDokter";
            this.buttonDokter.Size = new System.Drawing.Size(145, 117);
            this.buttonDokter.TabIndex = 73;
            this.buttonDokter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDokter.UseVisualStyleBackColor = false;
            this.buttonDokter.Click += new System.EventHandler(this.buttonDokter_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(173, 786);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(145, 117);
            this.button3.TabIndex = 70;
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // panelPatientData
            // 
            this.panelPatientData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panelPatientData.Controls.Add(this.label7);
            this.panelPatientData.Controls.Add(this.richTextBox1);
            this.panelPatientData.Controls.Add(this.label5);
            this.panelPatientData.Controls.Add(this.label3);
            this.panelPatientData.Controls.Add(this.label2);
            this.panelPatientData.Controls.Add(this.label1);
            this.panelPatientData.Controls.Add(this.label4);
            this.panelPatientData.Controls.Add(this.lblCode);
            this.panelPatientData.Location = new System.Drawing.Point(4, 98);
            this.panelPatientData.Name = "panelPatientData";
            this.panelPatientData.Size = new System.Drawing.Size(325, 130);
            this.panelPatientData.TabIndex = 68;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(62, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 16);
            this.label7.TabIndex = 70;
            this.label7.Text = "Name";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(64, 64);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(248, 43);
            this.richTextBox1.TabIndex = 69;
            this.richTextBox1.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(54, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 16);
            this.label5.TabIndex = 67;
            this.label5.Text = ":";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(54, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 16);
            this.label3.TabIndex = 66;
            this.label3.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(103, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 18);
            this.label2.TabIndex = 65;
            this.label2.Text = "DATA PASIEN";
            // 
            // btn_Record_OBS
            // 
            this.btn_Record_OBS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.btn_Record_OBS.FlatAppearance.BorderSize = 0;
            this.btn_Record_OBS.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Record_OBS.ForeColor = System.Drawing.Color.White;
            this.btn_Record_OBS.Image = ((System.Drawing.Image)(resources.GetObject("btn_Record_OBS.Image")));
            this.btn_Record_OBS.Location = new System.Drawing.Point(16, 786);
            this.btn_Record_OBS.Name = "btn_Record_OBS";
            this.btn_Record_OBS.Size = new System.Drawing.Size(145, 117);
            this.btn_Record_OBS.TabIndex = 67;
            this.btn_Record_OBS.Text = "Rekam OBS";
            this.btn_Record_OBS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Record_OBS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Record_OBS.UseVisualStyleBackColor = false;
            this.btn_Record_OBS.Click += new System.EventHandler(this.btn_Record_OBS_Click);
            // 
            // btn_patient
            // 
            this.btn_patient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.btn_patient.FlatAppearance.BorderSize = 0;
            this.btn_patient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_patient.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_patient.Image = ((System.Drawing.Image)(resources.GetObject("btn_patient.Image")));
            this.btn_patient.Location = new System.Drawing.Point(173, 636);
            this.btn_patient.Name = "btn_patient";
            this.btn_patient.Size = new System.Drawing.Size(145, 117);
            this.btn_patient.TabIndex = 15;
            this.btn_patient.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_patient.UseVisualStyleBackColor = false;
            this.btn_patient.Click += new System.EventHandler(this.btn_patient_Click);
            // 
            // buttonRecStart
            // 
            this.buttonRecStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.buttonRecStart.FlatAppearance.BorderSize = 0;
            this.buttonRecStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecStart.ForeColor = System.Drawing.Color.White;
            this.buttonRecStart.Image = ((System.Drawing.Image)(resources.GetObject("buttonRecStart.Image")));
            this.buttonRecStart.Location = new System.Drawing.Point(16, 336);
            this.buttonRecStart.Name = "buttonRecStart";
            this.buttonRecStart.Size = new System.Drawing.Size(145, 117);
            this.buttonRecStart.TabIndex = 10;
            this.buttonRecStart.Text = "Mulai Kamera";
            this.buttonRecStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRecStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonRecStart.UseVisualStyleBackColor = false;
            this.buttonRecStart.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Capture
            // 
            this.btn_Capture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(150)))));
            this.btn_Capture.FlatAppearance.BorderSize = 0;
            this.btn_Capture.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Capture.ForeColor = System.Drawing.Color.White;
            this.btn_Capture.Image = ((System.Drawing.Image)(resources.GetObject("btn_Capture.Image")));
            this.btn_Capture.Location = new System.Drawing.Point(16, 486);
            this.btn_Capture.Name = "btn_Capture";
            this.btn_Capture.Size = new System.Drawing.Size(145, 117);
            this.btn_Capture.TabIndex = 9;
            this.btn_Capture.Text = "Ambil Foto";
            this.btn_Capture.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Capture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Capture.UseVisualStyleBackColor = false;
            this.btn_Capture.Click += new System.EventHandler(this.btn_Capture_Click);
            // 
            // textBoxPrint
            // 
            this.textBoxPrint.Location = new System.Drawing.Point(150, 539);
            this.textBoxPrint.Name = "textBoxPrint";
            this.textBoxPrint.Size = new System.Drawing.Size(1, 10);
            this.textBoxPrint.TabIndex = 71;
            this.textBoxPrint.TextChanged += new System.EventHandler(this.textBoxPrint_TextChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // lblRec1
            // 
            this.lblRec1.AutoSize = true;
            this.lblRec1.BackColor = System.Drawing.Color.Transparent;
            this.lblRec1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRec1.Location = new System.Drawing.Point(124, 771);
            this.lblRec1.Name = "lblRec1";
            this.lblRec1.Size = new System.Drawing.Size(38, 13);
            this.lblRec1.TabIndex = 75;
            this.lblRec1.Text = "Timer";
            // 
            // picRec1
            // 
            this.picRec1.BackColor = System.Drawing.Color.Transparent;
            this.picRec1.Image = ((System.Drawing.Image)(resources.GetObject("picRec1.Image")));
            this.picRec1.Location = new System.Drawing.Point(98, 768);
            this.picRec1.Name = "picRec1";
            this.picRec1.Size = new System.Drawing.Size(23, 20);
            this.picRec1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRec1.TabIndex = 76;
            this.picRec1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(773, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 0);
            this.pictureBox1.TabIndex = 73;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(629, 9);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(0, 0);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 66;
            this.pictureBox2.TabStop = false;
            // 
            // panelBawah
            // 
            this.panelBawah.BackColor = System.Drawing.Color.Black;
            this.panelBawah.Controls.Add(this.pictureBox4);
            this.panelBawah.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelBawah.Location = new System.Drawing.Point(0, 803);
            this.panelBawah.Name = "panelBawah";
            this.panelBawah.Size = new System.Drawing.Size(1588, 214);
            this.panelBawah.TabIndex = 80;
            this.panelBawah.WrapContents = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Location = new System.Drawing.Point(3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(0, 50);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // panelKiri
            // 
            this.panelKiri.BackColor = System.Drawing.Color.Black;
            this.panelKiri.Location = new System.Drawing.Point(0, 1);
            this.panelKiri.Name = "panelKiri";
            this.panelKiri.Size = new System.Drawing.Size(79, 802);
            this.panelKiri.TabIndex = 82;
            // 
            // panelKanan
            // 
            this.panelKanan.BackColor = System.Drawing.Color.Black;
            this.panelKanan.Location = new System.Drawing.Point(1509, 1);
            this.panelKanan.Name = "panelKanan";
            this.panelKanan.Size = new System.Drawing.Size(79, 802);
            this.panelKanan.TabIndex = 83;
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.BackColor = System.Drawing.SystemColors.Control;
            this.videoSourcePlayer.Location = new System.Drawing.Point(79, 2);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(1430, 801);
            this.videoSourcePlayer.TabIndex = 69;
            this.videoSourcePlayer.Text = resources.GetString("videoSourcePlayer.Text");
            this.videoSourcePlayer.VideoSource = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1924, 1016);
            this.Controls.Add(this.panelKanan);
            this.Controls.Add(this.panelKiri);
            this.Controls.Add(this.panelBawah);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.picRec1);
            this.Controls.Add(this.lblRec1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.videoSourcePlayer);
            this.Controls.Add(this.pictureBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Form1";
            this.Text = "Endoscope";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panelPatientData.ResumeLayout(false);
            this.panelPatientData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRec1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelBawah.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtFoot;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txt_Form;
        private System.Windows.Forms.TextBox txt_kondisi;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_patient;
        private System.Windows.Forms.Button buttonRecStart;
        private System.Windows.Forms.Button btn_Capture;
        private System.Windows.Forms.Button buttonRecSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_Record_OBS;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelPatientData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picRec1;
        private System.Windows.Forms.Label lblRec1;
        private System.Windows.Forms.TextBox textBoxPrint;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonDokter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRecStop;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel panelBawah;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panelKiri;
        private System.Windows.Forms.Panel panelKanan;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
    }
}

