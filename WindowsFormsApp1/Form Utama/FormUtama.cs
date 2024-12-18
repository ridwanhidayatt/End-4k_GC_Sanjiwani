using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using AForge.Video.FFMPEG;
using AForge.Video.VFW;
using System.Runtime.InteropServices;
using System.Diagnostics;
using AForge.Video;
using System.Drawing.Imaging;
using System.Media;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using WindowsFormsApp1.Form_Utama;
using WindowsFormsApp1.Format_2;
using WindowsFormsApp1.FormSwitcing;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Security.AccessControl;
using DrawingImage = System.Drawing.Image;
using SystemImage = Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System.Image;
using WindowsFormsApp1.Format_3;
using SystemTask = System.Threading.Tasks.Task;
using WindowsFormsApp1.Format_4;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        bool vCamera = false;
        bool vRecord = false; 
        System.Windows.Forms.Timer t1;
        Stopwatch s1;
        private Stopwatch stopWatch = null;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice;
        private Bitmap video;
        public VideoFileWriter FileWriter = new VideoFileWriter();
        private SaveFileDialog saveAvi;
        string tanggal, jam, id, Name, Code, Date, tindakan, action1, gabung, address, tanggalHari, splitBulan, splitTahun, noRM;
        string codeDefault, namaDefault;
        private Bitmap video1;
        private FileSystemWatcher watcher;
        private System.Windows.Forms.Timer timer; 
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_NEXT = 0x22; // Virtual-Key Code for Page Down
        private const int VK_PRIOR = 0x21; // Virtual-Key Code for Page Up
        //private const int VK_1 = 0x31; // Virtual-Key Code for '1'
        private const int VK_NUMPAD1 = 0x61;
        private static bool isHandlingKeyPress = false;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static Form1 _instance;
        private static bool isButtonCapturing = false; // Pastikan ada inisialisasi untuk variabel ini
        private static bool recordingStarted = false; // Pastikan ada inisialisasi untuk variabel ini 
        private Rectangle cropRectangle; 
        private System.Windows.Forms.Timer overlayTimer;

        public Form1()
        {
            InitializeComponent();
            txtFoot.KeyPress += new KeyPressEventHandler(txtFoot_KeyPress);

            // Inisialisasi FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = @"D:\GLEndoscope\Obs";
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = false;

            // Tambahkan event handler untuk kejadian file dibuat
            watcher.Created += new FileSystemEventHandler(OnFileCreated); 
            watcher.EnableRaisingEvents = true;

            // Inisialisasi Timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 3000; // Interval dalam milidetik (misalnya 5000 untuk 5 detik)
            timer.Tick += new EventHandler(Timer_Tick); // Tambahkan event handler untuk event Tick
            timer.Start(); // Mulai Timer

            // Ambil data dari database pertama kali
            UpdateDataFromDatabase();

            panelBawah.AutoScroll = true;
            panelBawah.WrapContents = false;
            panelBawah.FlowDirection = FlowDirection.LeftToRight;
            this.panelBawah.AutoScroll = true;
            this.panelBawah.WrapContents = false;
            this.panelBawah.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelBawah.TabIndex = 1;
            this.panelBawah.Visible = true;
            this.panelBawah.VerticalScroll.Visible = false; // Menghilangkan scrollbar vertikal

            _instance = this;


            SetCropRectangle(-10, 0, 1465, 1200);
            //SetCropRectangle(310, 36, 1300, 1010);


            //videoSourcePlayer.Paint += VideoSourcePlayer_Paint;

        }

        //private void VideoSourcePlayer_Paint(object sender, PaintEventArgs e)
        //{
        //    // Tentukan ukuran kotak
        //    int boxWidth = 100;
        //    int boxHeight = 100;
        //    int boxX = 50; // Posisi X
        //    int boxY = 50; // Posisi Y

        //    // Buat pena untuk menggambar kotak
        //    using (Pen pen = new Pen(Color.Red, 2)) // Warna merah dengan ketebalan 2px
        //    {
        //        // Gambar kotak di dalam VideoSourcePlayer
        //        e.Graphics.DrawRectangle(pen, boxX, boxY, boxWidth, boxHeight);
        //    }
        //}

        private void SetCropRectangle(int x, int y, int width, int height)
        {
            cropRectangle = new Rectangle(x, y, width, height);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam); 

        private static void PerformCaptureClick()
        {
            if (isButtonCapturing)
                return;

            try
            {
                isButtonCapturing = true;
                if (_instance.InvokeRequired)
                {
                    _instance.Invoke(new Action(() => _instance.btn_Capture.PerformClick()));
                    Debug.WriteLine("Capture click invoked");
                }
                else
                {
                    _instance.btn_Capture.PerformClick();
                    Debug.WriteLine("Capture click performed directly");
                }
            }
            finally
            {
                isButtonCapturing = false;
            }
        }


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == VK_NEXT || vkCode == VK_NUMPAD1) // Page Down key or Key '1'
                {
                    Debug.WriteLine("PerformCaptureClick triggered by keyboard");
                    PerformCaptureClick();
                }
                else if (vkCode == VK_PRIOR)
                {
                    if (recordingStarted)
                    {
                        if (_instance.buttonRecStop.Text == "Hentikan Rekam")
                        {
                            _instance.buttonRecStop.PerformClick();
                            recordingStarted = false; // Reset recording status after stopping recording
                            Debug.WriteLine("Recording stopped");
                        }
                    }
                    else
                    {
                        _instance.buttonRecSave.PerformClick();
                        recordingStarted = true; // Set recording status to true when starting recording
                        Debug.WriteLine("Recording started");
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName); 

        // Override CreateParams to enable double buffering for the form
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
  
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update data dari database
            UpdateDataFromDatabase();
        }


        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string dir = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Image\";
            string dir1 = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Video\";

            string[] imageExtensions = { ".png", ".jpg" };
            string[] videoExtensions = { ".mp4", ".avi" };

            // Periksa ekstensi file
            string fileExtension = Path.GetExtension(e.FullPath).ToLower();

            try
            {
                // Jika file adalah gambar, pindahkan ke direktori gambar
                if (imageExtensions.Contains(fileExtension))
                {
                    string destinationFile = Path.Combine(dir, Path.GetFileName(e.FullPath));
                    MoveImageFile(e.FullPath, destinationFile);
                }
                // Jika file adalah video, panggil metode baru untuk pemindahan video
                else if (videoExtensions.Contains(fileExtension))
                {
                    OnNewFile(sender, e);
                }
                else
                {
                    // Debugging output for unsupported file type
                    MessageBox.Show($"Unsupported file type: {fileExtension}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error moving file: {ex.Message}");
            }
        }

        private void MoveImageFile(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            }

            while (IsFileLockedCustom(new FileInfo(sourcePath))) // Use custom method name
            {
                System.Threading.Thread.Sleep(500);
            }

            File.Move(sourcePath, destinationPath);
        }

        private bool IsFileLockedCustom(FileInfo file) // Use custom method name
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        private void OnNewFile(object sender, FileSystemEventArgs e)
        {
            // Memastikan file sudah selesai ditulis dengan menunggu beberapa waktu
            SystemTask.Run(async () =>
            {
                await WaitForFile(e.FullPath);
                MoveVideoFile(e.FullPath);
            });
        }

        private async SystemTask WaitForFile(string filePath)
        {
            const int delay = 1000; // 1 detik
            bool fileIsAccessible = false;

            while (!fileIsAccessible)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        fileIsAccessible = true;
                    }
                }
                catch (IOException)
                {
                    // Jika file masih diakses, tunggu sebelum mencoba lagi
                    await SystemTask.Delay(delay);
                }
            }
        }

        private void MoveVideoFile(string sourcePath)
        {
            try
            {
                string dir1 = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Video\";
                string fileName = Path.GetFileName(sourcePath);
                string targetPath = Path.Combine(dir1, fileName);

                // Pastikan folder tujuan ada
                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                }

                // Pindahkan file
                File.Move(sourcePath, targetPath);
                //MessageBox.Show($"File moved to {targetPath}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error moving file: {ex.Message}");
            }
        } 

        private void UpdateDataFromDatabase()
        {
            tanggal = DateTime.Now.ToString("ddMMyyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";
             
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Read(); // Skip header record

                while (csv.Read())
                {
                    // Read data from the CSV
                    var noRM = csv.GetField<string>("Rm")?.Trim();
                    var name = csv.GetField<string>("Nama")?.Trim();
                    var action = csv.GetField<string>("Jenis Pemeriksaan")?.Trim();

                    // Generate directory paths based on the extracted data
                    string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image";
                    string dir1 = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video";

                    // Create directories if they don't exist
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (!Directory.Exists(dir1))
                    {
                        Directory.CreateDirectory(dir1);
                    }

                    // Update UI elements
                    lblCode.Text = noRM;
                    richTextBox1.Text = name;
                }
            } 
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (txt_Form.Text != "")
            {
                string message = "Tutup halaman terlebih dahulu";
                string title = "Peringatan";
                MessageBox.Show(message, title);
            }
            else
            {
                // Mendapatkan daftar perangkat video yang tersedia
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (VideoCaptureDevices.Count == 0)
                {
                    MessageBox.Show("Tidak ada perangkat video ditemukan.");
                    return;
                }

                // Mencari perangkat video dengan nama "USB Video"
                foreach (FilterInfo device in VideoCaptureDevices)
                {
                    if (device.Name.Contains("ezcap LIVE GAMER RAW"))
                    {
                        FinalVideo = new VideoCaptureDevice(device.MonikerString);
                        break;
                    }
                    else if (device.Name.Contains("ezcap Game Link RAW"))
                    {
                        FinalVideo = new VideoCaptureDevice(device.MonikerString);
                        break;
                    }
                    else if (device.Name.Contains("Elgato 4K Pro"))
                    {
                        FinalVideo = new VideoCaptureDevice(device.MonikerString);
                        break;
                    }
                    else if (device.Name.Contains("USB Video"))
                    {
                        FinalVideo = new VideoCaptureDevice(device.MonikerString);
                        break;
                    }
                    
                    else if (device.Name.Contains("Integrated Camera"))
                    {
                        FinalVideo = new VideoCaptureDevice(device.MonikerString);
                        break;
                    }

                }

                // Jika perangkat tidak ditemukan, tampilkan pesan kesalahan
                if (FinalVideo == null)
                {
                    MessageBox.Show("Perangkat 'USB Video' tidak ditemukan.");
                    return;
                }

                // Memastikan resolusi video diatur
                if (FinalVideo.VideoCapabilities.Length > 0)
                {
                    FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[0];
                }
                else
                {
                    MessageBox.Show("Perangkat video tidak memiliki kemampuan resolusi yang tersedia.");
                    return;
                }

                // Memulai sumber video
                OpenVideoSource(FinalVideo);
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                FinalVideo.Start();

                int close = 2;
                textBox1.Text = close.ToString();
                videoSourcePlayer.Visible = true;
                panelKanan.Visible = true;
                panelKiri.Visible = true;
                panelBawah.Visible = true;
                buttonRecStart.Enabled = false;
                btn_Capture.Enabled = true;
                buttonRecSave.Enabled = true;
                btn_Record_OBS.Enabled = true;
                btn_patient.Enabled = true;
                txtFoot.Enabled = true;
                buttonRecStart.BackColor = Color.FromArgb(0, 85, 119);
                btn_Record_OBS.BackColor = Color.FromArgb(0, 107, 150);
                buttonRecSave.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
                vCamera = true;
                buttonRecStop.Enabled = true; 
                _hookID = SetHook(_proc);
            }

            btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
        }

        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset stop watch
            stopWatch = null;

            // start timer
            timer1.Start();
            this.Cursor = Cursors.Default;
        }

        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (videoSourcePlayer.IsRunning)
                {
                    videoSourcePlayer.Stop();
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //try
            //{
            //    if (buttonRecStop.Text == "Hentikan Rekam")
            //    {
            //        using (Bitmap videoFrame = (Bitmap)eventArgs.Frame.Clone())
            //        {
            //            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

            //            if (FileWriter != null)
            //            {
            //                // Crop the frame based on the user-defined area
            //                using (Bitmap croppedFrame = CropFrame(videoFrame, cropRectangle))
            //                {
            //                    // Write the cropped frame to the video file
            //                    FileWriter.WriteVideoFrame(croppedFrame);
            //                }
            //            }
            //        }
            //    }
            //    else //Stop
            //    {
            //        using (Bitmap videoFrame = (Bitmap)eventArgs.Frame.Clone())
            //        {
            //            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            //        }
            //    }
            //}
            //catch (System.AccessViolationException ex)
            //{
            //    // Tangani pengecualian dengan mencetak pesan kesalahan atau log
            //    Console.WriteLine("Terjadi kesalahan Access Violation:");
            //    Console.WriteLine(ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    // Tangani pengecualian umum lainnya di sini
            //    Console.WriteLine("Terjadi kesalahan lain saat menangani frame video:");
            //    Console.WriteLine(ex.Message);
            //}

            try
            {
                if (buttonRecStop.Text == "Hentikan Rekam")
                {
                    video = (Bitmap)eventArgs.Frame.Clone();
                    pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

                    if (FileWriter != null && video != null)
                    {
                        FileWriter.WriteVideoFrame(video);
                    }
                }
                else //Stop
                {
                    video = (Bitmap)eventArgs.Frame.Clone();
                    pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                }
            }
            catch (System.AccessViolationException ex)
            {
                // Tangani pengecualian dengan mencetak pesan kesalahan atau log
                Console.WriteLine("Terjadi kesalahan Access Violation:");
                Console.WriteLine(ex.Message);
                // Tambahkan langkah-langkah penanganan tambahan jika diperlukan
            }
            catch (Exception ex)
            {
                // Tangani pengecualian umum lainnya di sini
                Console.WriteLine("Terjadi kesalahan lain saat menangani frame video:");
                Console.WriteLine(ex.Message);
                // Tambahkan langkah-langkah penanganan tambahan jika diperlukan
            }


        } 

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            s1 = new Stopwatch();
            t1 = new System.Windows.Forms.Timer();
            t1.Interval = 10;
            t1.Tick += T1_Tick;
            t1.Start();
            buttonRecSave.Enabled = false;
            txtFoot.Enabled = false;
            videoCaptureDevice = new VideoCaptureDevice();
            var ellipseRadius = new System.Drawing.Drawing2D.GraphicsPath();
            ellipseRadius.StartFigure();
            ellipseRadius.AddArc(new Rectangle(0, 0, 10, 10), 180, 90);
            ellipseRadius.AddLine(10, 0, buttonRecSave.Width - 20, 0);
            ellipseRadius.AddArc(new Rectangle(buttonRecSave.Width - 10, 0, 10, 10), -90, 90);
            ellipseRadius.AddLine(buttonRecSave.Width, 20, buttonRecSave.Width, buttonRecSave.Height - 10);
            ellipseRadius.AddArc(new Rectangle(buttonRecSave.Width - 10, buttonRecSave.Height - 10, 10, 10), 0, 90);
            ellipseRadius.AddLine(buttonRecSave.Width - 10, buttonRecSave.Height, 20, buttonRecSave.Height);
            ellipseRadius.AddArc(new Rectangle(0, buttonRecSave.Height - 10, 10, 10), 90, 90);
            ellipseRadius.CloseAllFigures();
            buttonRecSave.Region = new Region(ellipseRadius); 
            buttonRecStop.Enabled = false;
            btn_Capture.Enabled = false;
            buttonRecSave.Enabled = false; 
            videoSourcePlayer.Visible = false; 
            panelBawah.Visible = false;
            panelKiri.Visible = false;
            panelKanan.Visible = false;
            FormUser newMDIChild = new FormUser();
            newMDIChild.MdiParent = this;
            newMDIChild.StartPosition = FormStartPosition.Manual;
            newMDIChild.Left = 0;
            newMDIChild.Top = 0;
            newMDIChild.TransfEvent += frm_TransfEvent;
            newMDIChild.Show();
            btn_patient.Enabled = false;
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = SystemColors.ButtonHighlight;
            lblRec1.Visible = false;
            picRec1.Visible = false;
            var ellipseRadius2 = new System.Drawing.Drawing2D.GraphicsPath();
            ellipseRadius2.StartFigure();
            ellipseRadius2.AddArc(new Rectangle(0, 0, 10, 10), 180, 90);
            ellipseRadius2.AddLine(10, 0, panelPatientData.Width - 20, 0);
            ellipseRadius2.AddArc(new Rectangle(panelPatientData.Width - 10, 0, 10, 10), -90, 90);
            ellipseRadius2.AddLine(panelPatientData.Width, 20, panelPatientData.Width, panelPatientData.Height - 10);
            ellipseRadius2.AddArc(new Rectangle(panelPatientData.Width - 10, panelPatientData.Height - 10, 10, 10), 0, 90);
            ellipseRadius2.AddLine(panelPatientData.Width - 10, panelPatientData.Height, 20, panelPatientData.Height);
            ellipseRadius2.AddArc(new Rectangle(0, panelPatientData.Height - 10, 10, 10), 90, 90);
            ellipseRadius2.CloseAllFigures();
            panelPatientData.Region = new Region(ellipseRadius2);

        }

        private void T1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = s1.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            lblRec1.Text = elapsedTime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int test = 1;
            textBox2.Text = test.ToString();
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

            if (videoSource != null)
            {
                int framesReceived = videoSource.FramesReceived;

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();
                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }

            jam = DateTime.Now.ToString("hhmmss");
            tanggal = DateTime.Now.ToString("ddMMyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];
            tanggalHari = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss dddd");
            getPatient();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void btn_patient_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    FormUser newMDIChild = new FormUser();
                    newMDIChild.MdiParent = this;
                    newMDIChild.StartPosition = FormStartPosition.Manual;
                    newMDIChild.Left = 0;
                    newMDIChild.Top = 0;
                    newMDIChild.TransfEvent += frm_TransfEvent;
                    newMDIChild.textBox4.Text = "formPasien";
                    newMDIChild.Show();
                    videoSourcePlayer.Visible = false; 
                    panelBawah.Visible = false;
                    panelKiri.Visible = false;
                    panelKanan.Visible = false;
                    btn_patient.Enabled = false;
                    btn_patient.BackColor = Color.FromArgb(0, 85, 119);
                    int Fuser = 1;
                    txt_Form.Text = Fuser.ToString();
                    string kirim = "kirim";
                    newMDIChild.textBox3.Text = kirim;
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Hentikan Kamera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Hentikan Rekam terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox1.Text == "2")
            {
                FinalVideo.Stop();
                Environment.Exit(0);
            }

        }

        private void btn_Record_Click_1(object sender, EventArgs e)
        { 
            if (buttonRecStop.Text == "Hentikan Kamera")
            {
                string dirr = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video\";

                if (!Directory.Exists(dirr))
                {
                    Directory.CreateDirectory(dirr);
                }

                saveAvi = new SaveFileDialog();
                saveAvi.Filter = "Avi Files (*.avi)|*.avi";
                saveAvi.FileName = dirr + jam + ".avi";

                // Pastikan FinalVideo dan VideoResolution tidak null sebelum mengakses FrameSize
                if (FinalVideo != null && FinalVideo.VideoResolution != null)
                {
                    int h = FinalVideo.VideoResolution.FrameSize.Height;
                    int w = FinalVideo.VideoResolution.FrameSize.Width;

                    FileWriter.Open(saveAvi.FileName, w, h, 30, VideoCodec.Default, 50000000);
                    FileWriter.WriteVideoFrame(video);
                }
                else
                {
                    MessageBox.Show("Resolusi video tidak valid atau tidak diatur dengan benar.");
                    return;
                }

                buttonRecStop.Text = "Hentikan Rekam";
                s1.Start();
                lblRec1.Visible = true;
                picRec1.Visible = true;
                txtFoot.Enabled = true;
                txtFoot.Focus();
                buttonRecSave.BackColor = Color.FromArgb(0, 85, 119);
                vRecord = true;
            }


        } 

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(900);
            SendKeys.SendWait("{Enter}");//or Esc
        }

        private bool isButton1Turn = true; // Variabel untuk melacak giliran tombol

        private void txtFoot_TextChanged(object sender, EventArgs e)
        {
            string foot = "1";
            if (txtFoot.Text == foot.ToString())
            {
                // Jangan panggil PerformCaptureClick lagi di sini
                SaveImageWithText();
            }
        }

        private void SaveImageWithText()
        {
            textBox2.Clear();
            txtFoot.Focus();
            pictureBox2.Image = pictureBox1.Image;

            string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string imageFilePath = Path.Combine(dir, $"{jam}.bmp");
            string imageFilePathJPG = Path.Combine(dir, $"{jam}.jpg");

            try
            {
                if (pictureBox1.Image == null)
                {
                    throw new Exception("pictureBox1 does not contain an image.");
                }

                // Tambahkan delay sebelum menyimpan gambar
                System.Threading.Thread.Sleep(200); // Delay 500ms

                // Save the image as BMP
                pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);

                // Define the target resolution (e.g., same as video resolution)
                int targetWidth = 1058; // Replace with the actual width of your video
                int targetHeight = 797; // Replace with the actual height of your video

                using (var originalImage = System.Drawing.Image.FromFile(imageFilePath))
                {
                    // Ensure crop rectangle is within the bounds of the original image
                    Rectangle validCropRectangle = new Rectangle(
                        Math.Max(0, cropRectangle.X),
                        Math.Max(0, cropRectangle.Y),
                        Math.Min(cropRectangle.Width, originalImage.Width - cropRectangle.X),
                        Math.Min(cropRectangle.Height, originalImage.Height - cropRectangle.Y)
                    );

                    using (var croppedBitmap = new Bitmap(validCropRectangle.Width, validCropRectangle.Height))
                    using (Graphics graphics = Graphics.FromImage(croppedBitmap))
                    {
                        graphics.DrawImage(originalImage, new Rectangle(0, 0, validCropRectangle.Width, validCropRectangle.Height),
                                            validCropRectangle, GraphicsUnit.Pixel);

                        // Calculate the cropping rectangle to fit the target resolution
                        float ratioX = (float)targetWidth / croppedBitmap.Width;
                        float ratioY = (float)targetHeight / croppedBitmap.Height;
                        float ratio = Math.Max(ratioX, ratioY); // Choose the larger ratio to fill the target size

                        int newWidth = (int)(croppedBitmap.Width * ratio);
                        int newHeight = (int)(croppedBitmap.Height * ratio);

                        int xOffset = (newWidth - targetWidth) / 2;
                        int yOffset = (newHeight - targetHeight) / 2;

                        using (var resizedBitmap = new Bitmap(targetWidth, targetHeight))
                        using (Graphics resizeGraphics = Graphics.FromImage(resizedBitmap))
                        {
                            resizeGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            resizeGraphics.DrawImage(croppedBitmap, -xOffset, -yOffset, newWidth, newHeight);

                            // Optionally add text to the resized image
                            using (Font arialFont = new Font("Arial", 15))
                            {
                                //resizeGraphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
                                //resizeGraphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
                                //resizeGraphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));
                            }

                            // Save the resized image as JPG
                            resizedBitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
                        }
                    }
                }
                // Hapus file BMP setelah menyimpan JPEG
                File.Delete(imageFilePath);

                AddImageToFlowLayoutPanel(imageFilePathJPG);
                btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"General error: {ex.Message}");
            }
            finally
            {
                txtFoot.Clear();
            }
        }
         
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    this.ActiveControl = label2;
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }
                else
                {
                    videoSourcePlayer.Visible = false; 
                    panelBawah.Visible = false;
                    panelKiri.Visible = false;
                    panelKanan.Visible = false;
                    createFolder();
                    FormPrint newMDIChilddddd = new FormPrint();
                    newMDIChilddddd.MdiParent = this;
                    newMDIChilddddd.StartPosition = FormStartPosition.Manual;
                    newMDIChilddddd.Left = 0;
                    newMDIChilddddd.Top = 0;
                    newMDIChilddddd.TransfEventtttt += frm_TransfEvent4;
                    newMDIChilddddd.TransfEventPrint1 += frm_TransfEventPrint1;
                    newMDIChilddddd.Show();
                    int Fone = 2;
                    txt_Form.Text = Fone.ToString();
                    button3.Enabled = false;
                    button3.BackColor = Color.FromArgb(0, 85, 119);
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Hentikan Kamera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Hentikan Rekam terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } 

        private void txtFoot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '1')
            {
                e.Handled = true; // Abaikan input yang bukan angka '1'
            }
        }

        private async void btn_Capture_Click(object sender, EventArgs e)
        {
            btn_Capture.BackColor = Color.FromArgb(0, 85, 119);

            textBox2.Clear();
            txtFoot.Focus();
            pictureBox2.Image = pictureBox1.Image;

            string cleanedName = Name.Trim();
            string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string imageFilePath = Path.Combine(dir, $"{timestamp}.bmp");
            string imageFilePathJPG = Path.Combine(dir, $"{timestamp}.jpg");

            try
            {
                if (pictureBox1.Image == null)
                {
                    throw new Exception("pictureBox1 does not contain an image.");
                }

                // Tambahkan delay sebelum menyimpan gambar
                System.Threading.Thread.Sleep(200); // Delay 500ms

                // Save the image as BMP
                pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);

                // Define the target resolution (e.g., same as video resolution)
                int targetWidth = 1058; // Replace with the actual width of your video
                int targetHeight = 797; // Replace with the actual height of your video

                using (var originalImage = System.Drawing.Image.FromFile(imageFilePath))
                {
                    // Ensure crop rectangle is within the bounds of the original image
                    Rectangle validCropRectangle = new Rectangle(
                        Math.Max(0, cropRectangle.X),
                        Math.Max(0, cropRectangle.Y),
                        Math.Min(cropRectangle.Width, originalImage.Width - cropRectangle.X),
                        Math.Min(cropRectangle.Height, originalImage.Height - cropRectangle.Y)
                    );

                    using (var croppedBitmap = new Bitmap(validCropRectangle.Width, validCropRectangle.Height))
                    using (Graphics graphics = Graphics.FromImage(croppedBitmap))
                    {
                        graphics.DrawImage(originalImage, new Rectangle(0, 0, validCropRectangle.Width, validCropRectangle.Height),
                                            validCropRectangle, GraphicsUnit.Pixel);

                        // Calculate the cropping rectangle to fit the target resolution
                        float ratioX = (float)targetWidth / croppedBitmap.Width;
                        float ratioY = (float)targetHeight / croppedBitmap.Height;
                        float ratio = Math.Max(ratioX, ratioY); // Choose the larger ratio to fill the target size

                        int newWidth = (int)(croppedBitmap.Width * ratio);
                        int newHeight = (int)(croppedBitmap.Height * ratio);

                        int xOffset = (newWidth - targetWidth) / 2;
                        int yOffset = (newHeight - targetHeight) / 2;

                        using (var resizedBitmap = new Bitmap(targetWidth, targetHeight))
                        using (Graphics resizeGraphics = Graphics.FromImage(resizedBitmap))
                        {
                            resizeGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            resizeGraphics.DrawImage(croppedBitmap, -xOffset, -yOffset, newWidth, newHeight);

                            // Optionally add text to the resized image
                            using (Font arialFont = new Font("Arial", 15))
                            {
                                //resizeGraphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
                                //resizeGraphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
                                //resizeGraphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));
                            }

                            // Save the resized image as JPG
                            resizedBitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
                        }
                    }
                }
                // Hapus file BMP setelah menyimpan JPEG
                File.Delete(imageFilePath);

                AddImageToFlowLayoutPanel(imageFilePathJPG);
                btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Out of memory error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Argument exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddImageToFlowLayoutPanel(string imagePath)
        {
            if (panelBawah.Controls.Count >= 100)
            {
                MessageBox.Show("Silahkan hapus beberapa gambar", "Batas Maksimal 100 Gambar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Buat panel baru untuk menampung gambar dan tombol hapus
            Panel imagePanel = new Panel();
            imagePanel.Size = new Size(319, 191);
            // Ukuran panel lebih kecil
            imagePanel.BorderStyle = BorderStyle.FixedSingle;

            // Buat PictureBox untuk menampilkan gambar
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = AForge.Imaging.Image.FromFile(imagePath);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Fill;
            imagePanel.BorderStyle = BorderStyle.None; // Tidak menampilkan borders

            // Buat tombol hapus
            Button deleteButton = new Button();
            deleteButton.Text = "X";
            deleteButton.Size = new Size(20, 20); // Ukuran tombol lebih kecil
            deleteButton.BackColor = Color.FromArgb(255, 69, 58); // Warna merah elegan
            deleteButton.ForeColor = Color.White; // Warna teks putih
            deleteButton.FlatStyle = FlatStyle.Flat; // Gaya tombol datar
            deleteButton.FlatAppearance.BorderSize = 0; // Tanpa batas
            deleteButton.Cursor = Cursors.Hand; // Ubah kursor menjadi tangan saat di hover
            deleteButton.Font = new Font("Arial", 8, FontStyle.Bold); // Font lebih kecil
            deleteButton.Location = new Point(imagePanel.Width - deleteButton.Width - 5, 5); // Posisi di kanan atas

            // Tambahkan event handler untuk tombol hapus
            deleteButton.Click += (sender, e) => DeleteImage(imagePanel, imagePath);

            // Tambahkan PictureBox dan tombol hapus ke panel
            imagePanel.Controls.Add(pictureBox);
            imagePanel.Controls.Add(deleteButton);

            // Atur posisi tombol hapus di atas gambar
            deleteButton.BringToFront();
            
            // Tambahkan panel ke flowLayoutPanel1
            panelBawah.Controls.Add(imagePanel);
            panelBawah.Controls.SetChildIndex(imagePanel, 0);

            //AdjustAllPanelSizes();
            AdjustAllPanelSizes();
        }


        private void AdjustAllPanelSizes()
        {
            int imageCount = panelBawah.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<PictureBox>()).Count();
        }


        private void DeleteImage(Panel imagePanel, string imagePath)
        {
            var result = MessageBox.Show("Apakah Anda yakin ingin menghapus gambar ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Pastikan PictureBox tidak lagi menggunakan gambar
                PictureBox pictureBox = imagePanel.Controls.OfType<PictureBox>().FirstOrDefault();
                if (pictureBox != null)
                {
                    ClearImageFromPictureBox(pictureBox);
                } 

                // Hapus panel gambar dari flowLayoutPanel
                panelBawah.Controls.Remove(imagePanel);
                imagePanel.Dispose();

                try
                {
                    File.Delete(imagePath);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Gagal menghapus file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
                panelBawah.AutoScrollPosition = new Point(0, 0);
            }
        }  

        private void ClearImageFromPictureBox(PictureBox pictureBox)
        {
            if (pictureBox != null && pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        private void textBoxPrint_TextChanged(object sender, EventArgs e)
        { 
            int format3Print = 8;
            int format4Print = 9;

            if (textBoxPrint.Text == format3Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop(); 
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form310Gambar form310 = new Form310Gambar();
                form310.MdiParent = this;
                form310.StartPosition = FormStartPosition.Manual;
                form310.Left = 0;
                form310.Top = 0;
                form310.TransfEventPrint310 += frm_TransfEventPrint310;
                form310.TEClose10Gambar += frm_TEClose10Gambar;
                form310.TransfEventPrint310Print += frm_TransfEventPrint310Print;
                string kirim = "kirim";
                form310.textBox2.Text = kirim;
                form310.Show();
                int Fsix = 7;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }
            else if (textBoxPrint.Text == format4Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop(); 
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form48Gambar form48 = new Form48Gambar();
                form48.MdiParent = this;
                form48.StartPosition = FormStartPosition.Manual;
                form48.Left = 0;
                form48.Top = 0; 
                form48.TEClose48Gambar += frm_TEClose48Gambar; 
                string kirim = "kirim";
                form48.textBox2.Text = kirim;
                form48.Show();
                int Fsix = 7;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }
        }

        private void frm_TEClose48Gambar(string value)
        {
            txt_kondisi.Text = value;
        } 

        private void frm_TransfEventPrint310Print(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose10Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEventPrint310(string value)
        {
            textBoxPrint.Text = value;
        } 

        private void txt_kondisi_TextChanged(object sender, EventArgs e)
        {
            int fUser = 1; 
            int fPrint = 6;  
            int fDokter = 7; 
            int FS310Gambar = 17; 
            int FS48Gambar = 21; 

            if (txt_kondisi.Text == fUser.ToString())
            {
                videoSourcePlayer.Visible = true; 
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                btn_patient.Enabled = true;
                btn_patient.BackColor = Color.FromArgb(0, 107, 150);
                txt_Form.Clear();
                txtFoot.Focus();
            }
             
            else if (txt_kondisi.Text == fPrint.ToString())
            {
                videoSourcePlayer.Visible = true; 
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            
            else if (txt_kondisi.Text == fDokter.ToString())
            {
                videoSourcePlayer.Visible = true; 
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                buttonDokter.Enabled = true;
                buttonDokter.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            
            else if (txt_kondisi.Text == FS310Gambar.ToString())
            {
                panel2.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                videoSourcePlayer.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
            }
            else if (txt_kondisi.Text == FS48Gambar.ToString())
            { 
                panel2.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                videoSourcePlayer.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
            }
        }

        private void createFolder()
        {
            pictureBox2.Image = pictureBox1.Image;
            string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        void frm_TransfEvent(string value)
        {
            txt_kondisi.Text = value;
        }

        private void buttonDokter_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {
                    FormDokter formDokter = new FormDokter();
                    formDokter.MdiParent = this;
                    formDokter.StartPosition = FormStartPosition.Manual;
                    formDokter.Left = 0;
                    formDokter.Top = 0;
                    formDokter.TransfEventDokter += frm_TransfEventDokter;
                    formDokter.Show();
                    videoSourcePlayer.Visible = false; 
                    panelBawah.Visible = false;
                    panelKiri.Visible = false;
                    panelKanan.Visible = false;
                    buttonDokter.BackColor = Color.FromArgb(0, 85, 119);
                    buttonDokter.Enabled = false;
                    int Fuser = 8;
                    txt_Form.Text = Fuser.ToString();
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Hentikan Kamera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Hentikan Rekam terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void frm_TransfEventDokter(string value)
        {
            txt_kondisi.Text = value;
        }

        private void buttonRecStop_Click(object sender, EventArgs e)
        {
            if (buttonRecStop.Text == "Hentikan Rekam")
            {
                buttonRecStop.Text = "Hentikan Kamera";
                if (FinalVideo == null)
                { return; }
                if (FinalVideo.IsRunning)
                { 
                    FileWriter.Close(); 
                }

                s1.Stop();
                s1.Reset();
                lblRec1.Visible = false;
                picRec1.Visible = false; 
                txtFoot.Enabled = true;
                txtFoot.Focus();
                buttonRecSave.BackColor = Color.FromArgb(0, 107, 150);
                vRecord = false; 
                MessageBox.Show("Video disimpan di folder", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            else
            {
                vCamera = false;
                this.FinalVideo.SignalToStop();
                FileWriter.Close(); 
                pictureBox1.Image = null;
                buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                btn_Capture.Enabled = false;
                buttonRecSave.Enabled = false;
                buttonRecStop.Enabled = false;
                buttonRecStart.Enabled = true; 
                ClearFlowLayoutPanel(); 
                UnhookWindowsHookEx(_hookID);
            }
        }

        private void ClearFlowLayoutPanel()
        {
            // Iterasi melalui semua kontrol di dalam FlowLayoutPanel dan hapus PictureBox
            foreach (Control control in panelBawah.Controls.OfType<Panel>().ToList())
            {
                // Dispose semua PictureBox dan panel yang mengandungnya
                foreach (var pictureBox in control.Controls.OfType<PictureBox>().ToList())
                {
                    control.Controls.Remove(pictureBox);
                    pictureBox.Dispose();
                }
                // Hapus panel
                panelBawah.Controls.Remove(control);
                control.Dispose();
            }

            // Perbarui UI jika diperlukan
            panelBawah.Refresh();
        }

        private void btn_Record_OBS_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                { 
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = "C:/Program Files/obs-studio/bin/64bit"; // like cd path command
                    startInfo.FileName = "obs64.exe";
                    Process.Start(startInfo); 
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Hentikan Kamera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Hentikan Rekam terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } 

        private void frm_TransfEvent4(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEventPrint1(string value)
        {
            textBoxPrint.Text = value;
        } 

        private void ReadDataFromCSV(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    // Baca record dari file CSV satu per satu
                    while (csv.Read())
                    {
                        var noRM = csv.GetField<string>("Rm");
                        var name = csv.GetField<string>("Nama");
                        var action = csv.GetField<string>("Jenis Pemeriksaan");
                        var date = csv.GetField<string>("Tanggal Kunjungan");
                        gabung = noRM + "-" + name;
                        Name = name;
                        action1 = action;

                        // Tampilkan data di Label dan RichTextBox
                        lblCode.Text = noRM;
                        richTextBox1.Text = name;
                        label7.Visible = false;

                        //MessageBox.Show($"Nilai name: {name}\nNilai action: {action}", "Nilai Name dan Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak ada data yang tersedia. Mohon isi data Pasien terlebih dahulu.", "Informasi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        } 

        private void getPatient()
        {
            // Specify the path for the CSV file
            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";

            // Check if the CSV file exists
            if (File.Exists(csvFilePath))
            {
                // Call the method to read data from the CSV file
                ReadDataFromCSV(csvFilePath);
            }
            else
            {
                // Handle the case where the file does not exist
                //Console.WriteLine("CSV file does not exist.");
            }
        }
    }
}
