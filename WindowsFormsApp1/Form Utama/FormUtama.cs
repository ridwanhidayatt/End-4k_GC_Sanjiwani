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





        //private Panel fullScreenOverlay;
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

            // Mulai memantau
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

            // Set default crop area (example values)
            SetCropRectangle(0, 0, 1460, 1080); // Set your custom values here 
        }

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

        //private static bool recordingStarted = false;

        //private static bool isButtonCapturing = false;

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









        //private void InitializeOverlay()
        //{
        //    fullScreenOverlay = new Panel
        //    {
        //        BackColor = Color.FromArgb(128, 0, 0, 0), // Semi-transparent black
        //        Visible = false,
        //        Parent = this,
        //    };

        //    // Set parent control to the form
        //    fullScreenOverlay.Parent = this;
        //    fullScreenOverlay.BringToFront(); // Ensure overlay is always on top

        //    overlayTimer = new System.Windows.Forms.Timer();
        //    overlayTimer.Interval = 200; // Show overlay for 200 milliseconds
        //    overlayTimer.Tick += OverlayTimer_Tick;

        //    SetLayered(fullScreenOverlay, 128); // Set initial transparency

        //    // Add size and location change event handlers for videoSourcePlayer
        //    videoSourcePlayer.SizeChanged += VideoSourcePlayer_SizeChanged;
        //    videoSourcePlayer.LocationChanged += VideoSourcePlayer_LocationChanged;

        //    // Initialize overlay size and position
        //    UpdateOverlaySizeAndPosition();
        //}

        private void UpdateOverlaySizeAndPosition()
        {
            // Update size and location of overlay to match videoSourcePlayer
            //fullScreenOverlay.Size = videoSourcePlayer.Size;
            //fullScreenOverlay.Location = new Point(99, 0); // Convert to screen coordinates

            //fullScreenOverlay.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //fullScreenOverlay.Location = new Point(-videoSourcePlayer.Location.X, -videoSourcePlayer.Location.Y);
        }

        private void VideoSourcePlayer_SizeChanged(object sender, EventArgs e)
        {
            // Update overlay size and location when videoSourcePlayer's size changes
            UpdateOverlaySizeAndPosition();
        }

        private void VideoSourcePlayer_LocationChanged(object sender, EventArgs e)
        {
            // Update overlay location when videoSourcePlayer's location changes
            UpdateOverlaySizeAndPosition();
        }

        private void OverlayTimer_Tick(object sender, EventArgs e)
        {
            //fullScreenOverlay.Visible = false;
            overlayTimer.Stop();
        }

        public static class Win32Helper
        {
            // Win32 API constants
            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_LAYERED = 0x80000;
            public const int LWA_ALPHA = 0x2;

            // Win32 API methods
            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, int dwFlags);
        }

        private void SetLayered(Control control, byte alpha)
        {
            IntPtr handle = control.Handle;
            int style = Win32Helper.GetWindowLong(handle, Win32Helper.GWL_EXSTYLE);
            style |= Win32Helper.WS_EX_LAYERED;
            Win32Helper.SetWindowLong(handle, Win32Helper.GWL_EXSTYLE, style);
            Win32Helper.SetLayeredWindowAttributes(handle, 0, alpha, Win32Helper.LWA_ALPHA);

            control.Size = new Size(1430, 801);  // Atur ukuran sesuai dengan videoSourcePlayer
            control.Location = new Point(79, 2);
        }

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

        private void ShowOverlay()
        {
            UpdateOverlaySizeAndPosition();
            //fullScreenOverlay.Visible = true;
            //fullScreenOverlay.BringToFront();
            overlayTimer.Start(); // Start the timer to hide the overlay after interval
        }


        //END AKHIR DARI OVERLAY












        private void invisibleCard()
        {
            panelBawah.Visible = false;
            //hScrollBar1.Visible = false;
        }

        private void visibleCard()
        {
            panelBawah.Visible = true;
            //hScrollBar1.Visible = true;
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



        //nepi dieu 




        //private void OnFileCreated(object sender, FileSystemEventArgs e)
        //{
        //    string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image\";
        //    string dir1 = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video\";

        //    string[] imageExtensions = { ".png", ".jpg" };
        //    string[] videoExtensions = { ".mp4", ".avi" };

        //    // Periksa ekstensi file
        //    string fileExtension = Path.GetExtension(e.FullPath).ToLower();

        //    // Jika file adalah gambar, pindahkan ke direktori gambar
        //    if (imageExtensions.Contains(fileExtension))
        //    {
        //        string destinationFile = Path.Combine(dir, Path.GetFileName(e.FullPath));

        //        //MessageBox.Show(dir);
        //        if (!Directory.Exists(dir))
        //        {
        //            Directory.CreateDirectory(dir);
        //        }

        //        // Tunggu hingga file selesai ditulis oleh sistem
        //        while (IsFileLocked(new FileInfo(e.FullPath)))
        //        {
        //            System.Threading.Thread.Sleep(500);
        //        }

        //        // Pindahkan file
        //        File.Move(e.FullPath, destinationFile);

        //        // Perbarui nilai TextBox secara langsung
        //        //textBox3.Text = dir;
        //    }
        //    // Jika file adalah video, pindahkan ke direktori video
        //    else if (videoExtensions.Contains(fileExtension))
        //    {
        //        string destinationFile = Path.Combine(dir1, Path.GetFileName(e.FullPath));

        //        //MessageBox.Show(dir);
        //        if (!Directory.Exists(dir1))
        //        {
        //            Directory.CreateDirectory(dir1);
        //        }

        //        // Tunggu hingga file selesai ditulis oleh sistem
        //        while (IsFileLocked(new FileInfo(e.FullPath)))
        //        {
        //            System.Threading.Thread.Sleep(500);
        //        }



        //        // Pindahkan file
        //        File.Move(e.FullPath, destinationFile);

        //        // Perbarui nilai TextBox secara langsung
        //        //textBox3.Text = dir;
        //    }
        //}

        //private bool IsFileLocked(FileInfo file)
        //{
        //    FileStream stream = null;

        //    try
        //    {
        //        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        //    }
        //    catch (IOException)
        //    {
        //        // File sedang terkunci
        //        return true;
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //            stream.Close();
        //    }

        //    // File tidak terkunci
        //    return false;
        //}

        private void UpdateDataFromDatabase()
        {
            tanggal = DateTime.Now.ToString("ddMMyyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";

            try
            {
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
            catch (Exception ex)
            {
                // Handle any exceptions
                // MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    if (buttonRecSave.Text == "Stop Rekam")
        //    {
        //        video = (Bitmap)eventArgs.Frame.Clone();
        //        pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        //        //AVIwriter.Quality = 0;
        //        FileWriter.WriteVideoFrame(video);
        //        //AVIwriter.AddFrame(video);
        //    }
        //    else //Stop
        //    {
        //        video = (Bitmap)eventArgs.Frame.Clone();
        //        pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        //    }
        //} 

        private Bitmap CropFrame(Bitmap originalFrame, Rectangle cropArea)
        {
            Bitmap croppedFrame = new Bitmap(cropArea.Width, cropArea.Height);

            using (Graphics g = Graphics.FromImage(croppedFrame))
            {
                g.DrawImage(originalFrame, new Rectangle(0, 0, cropArea.Width, cropArea.Height), cropArea, GraphicsUnit.Pixel);
            }

            return croppedFrame;
        }


        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (buttonRecStop.Text == "Hentikan Rekam")
                {
                    using (Bitmap videoFrame = (Bitmap)eventArgs.Frame.Clone())
                    {
                        pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

                        if (FileWriter != null)
                        {
                            // Crop the frame based on the user-defined area
                            using (Bitmap croppedFrame = CropFrame(videoFrame, cropRectangle))
                            {
                                // Write the cropped frame to the video file
                                FileWriter.WriteVideoFrame(croppedFrame);
                            }
                        }
                    }
                }
                else //Stop
                {
                    using (Bitmap videoFrame = (Bitmap)eventArgs.Frame.Clone())
                    {
                        pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                    }
                }
            }
            catch (System.AccessViolationException ex)
            {
                // Tangani pengecualian dengan mencetak pesan kesalahan atau log
                Console.WriteLine("Terjadi kesalahan Access Violation:");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // Tangani pengecualian umum lainnya di sini
                Console.WriteLine("Terjadi kesalahan lain saat menangani frame video:");
                Console.WriteLine(ex.Message);
            }
        }

        //13/08/2024
        //void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    try
        //    {
        //        if (buttonRecStop.Text == "Hentikan Rekam")
        //        {
        //            video = (Bitmap)eventArgs.Frame.Clone();
        //            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

        //            if (FileWriter != null && video != null)
        //            {
        //                FileWriter.WriteVideoFrame(video);
        //            }
        //        }
        //        else //Stop
        //        {
        //            video = (Bitmap)eventArgs.Frame.Clone();
        //            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        //        }
        //    }
        //    catch (System.AccessViolationException ex)
        //    {
        //        // Tangani pengecualian dengan mencetak pesan kesalahan atau log
        //        Console.WriteLine("Terjadi kesalahan Access Violation:");
        //        Console.WriteLine(ex.Message);
        //        // Tambahkan langkah-langkah penanganan tambahan jika diperlukan
        //    }
        //    catch (Exception ex)
        //    {
        //        // Tangani pengecualian umum lainnya di sini
        //        Console.WriteLine("Terjadi kesalahan lain saat menangani frame video:");
        //        Console.WriteLine(ex.Message);
        //        // Tambahkan langkah-langkah penanganan tambahan jika diperlukan
        //    }
        //}




        private void stopCamera()
        {
            //close 
            //this.FinalVideo.Stop();
            this.FinalVideo.SignalToStop();
            this.FinalVideo = null;
            FileWriter.Close();
            //this.AVIwriter.Close();
            pictureBox1.Image = null;
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
            //btn_Record_OBS.Region = new Region(ellipseRadius); 
            buttonRecStop.Enabled = false;
            btn_Capture.Enabled = false;
            buttonRecSave.Enabled = false;
            //btn_Record_OBS.Enabled = false; 
            videoSourcePlayer.Visible = false;
            //panelAtas.Visible = false;
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
                    //panelAtas.Visible = false;
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
                    int h = cropRectangle.Height;
                    int w = cropRectangle.Width;

                    FileWriter = new VideoFileWriter();
                    FileWriter.Open(saveAvi.FileName, w, h, 30, VideoCodec.Default, 50000000);
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
        //private void btn_Record_Click_1(object sender, EventArgs e)
        //{
        //    if (buttonRecStop.Text == "Hentikan Kamera")
        //    {
        //        string dirr = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video\";

        //        if (!Directory.Exists(dirr))
        //        {
        //            Directory.CreateDirectory(dirr);
        //        }

        //        saveAvi = new SaveFileDialog();
        //        saveAvi.Filter = "Avi Files (*.avi)|*.avi";
        //        saveAvi.FileName = dirr + jam + ".avi";

        //        // Pastikan FinalVideo dan VideoResolution tidak null sebelum mengakses FrameSize
        //        if (FinalVideo != null && FinalVideo.VideoResolution != null)
        //        {
        //            int h = FinalVideo.VideoResolution.FrameSize.Height;
        //            int w = FinalVideo.VideoResolution.FrameSize.Width;

        //            FileWriter.Open(saveAvi.FileName, w, h, 30, VideoCodec.Default, 50000000);
        //            FileWriter.WriteVideoFrame(video);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Resolusi video tidak valid atau tidak diatur dengan benar.");
        //            return;
        //        }

        //        buttonRecStop.Text = "Hentikan Rekam";
        //        s1.Start();
        //        lblRec1.Visible = true;
        //        picRec1.Visible = true;
        //        txtFoot.Enabled = true;
        //        txtFoot.Focus();
        //        buttonRecSave.BackColor = Color.FromArgb(0, 85, 119);
        //        vRecord = true;
        //    }
        //}

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
                int targetWidth = 1920; // Replace with the actual width of your video
                int targetHeight = 1080; // Replace with the actual height of your video

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


        //private void txtFoot_TextChanged(object sender, EventArgs e)
        //{
        //    string foot = "1";
        //    if (txtFoot.Text == foot.ToString())
        //    {
        //        textBox2.Clear();
        //        txtFoot.Focus();
        //        pictureBox2.Image = pictureBox1.Image;

        //        string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
        //        if (!Directory.Exists(dir))
        //        {
        //            Directory.CreateDirectory(dir);
        //        }

        //        string imageFilePath = Path.Combine(dir, $"{jam}.bmp");
        //        string imageFilePathJPG = Path.Combine(dir, $"{jam}.jpg");

        //        try
        //        {
        //            // Memeriksa apakah pictureBox1 mengandung gambar
        //            if (pictureBox1.Image == null)
        //            {
        //                throw new Exception("pictureBox1 does not contain an image.");
        //            }

        //            // Menyimpan gambar sebagai BMP
        //            pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);
        //            Debug.WriteLine($"BMP file saved at: {imageFilePath}");

        //            // Menambahkan teks ke gambar dan menyimpannya sebagai JPEG
        //            using (var bitmap = new Bitmap(imageFilePath))
        //            {
        //                //using (Graphics graphics = Graphics.FromImage(bitmap))
        //                using (Font arialFont = new Font("Arial", 15))
        //                {
        //                    //graphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
        //                    //graphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
        //                    //graphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));
        //                }

        //                bitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
        //                Debug.WriteLine($"JPEG file saved at: {imageFilePathJPG}");
        //            }

        //            // Menambahkan gambar JPEG ke FlowLayoutPanel
        //            AddImageToFlowLayoutPanel(imageFilePathJPG);
        //            Debug.WriteLine($"JPEG file added to FlowLayoutPanel: {imageFilePathJPG}");

        //            // Mencoba menghapus file BMP
        //            if (File.Exists(imageFilePath))
        //            {
        //                File.Delete(imageFilePath);
        //                if (File.Exists(imageFilePath))
        //                {
        //                    Debug.WriteLine($"Failed to delete BMP file: {imageFilePath}");
        //                    MessageBox.Show("BMP file was not deleted.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                }
        //                else
        //                {
        //                    Debug.WriteLine($"BMP file successfully deleted: {imageFilePath}");
        //                }
        //            }
        //        }
        //        catch (OutOfMemoryException ex)
        //        {
        //            MessageBox.Show("Out of memory error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            Debug.WriteLine($"Out of memory error: {ex.Message}");
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            MessageBox.Show("Argument exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            Debug.WriteLine($"Argument exception: {ex.Message}");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            Debug.WriteLine($"General error: {ex.Message}");
        //        }
        //        finally
        //        {
        //            txtFoot.Clear();
        //        }
        //    }
        //}

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
                    //panelAtas.Visible = false;
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

        // Method untuk mendapatkan folder penyimpanan gambar berdasarkan pengaturan default yang baru
        private string GetDefaultFolder()
        {
            // Tentukan path folder berdasarkan pengaturan default yang baru
            string defaultFolder = "";

            // Implementasi untuk mendapatkan folder berdasarkan pengaturan default yang baru
            // Anda harus menyesuaikan kode ini sesuai dengan logika dan struktur aplikasi Anda

            return defaultFolder;
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
                int targetWidth = 1920; // Replace with the actual width of your video
                int targetHeight = 1080; // Replace with the actual height of your video

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

        //private void btn_Capture_Click(object sender, EventArgs e)
        //{
        //    btn_Capture.BackColor = Color.FromArgb(0, 85, 119);

        //    textBox2.Clear();
        //    txtFoot.Focus();
        //    pictureBox2.Image = pictureBox1.Image;

        //    string cleanedName = Name.Trim();
        //    string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    string imageFilePath = Path.Combine(dir, $"{jam}.bmp");
        //    string imageFilePathJPG = Path.Combine(dir, $"{jam}.jpg");

        //    try
        //    {
        //        if (pictureBox1.Image == null)
        //        {
        //            throw new Exception("pictureBox1 does not contain an image.");
        //        }

        //        // Tambahkan delay sebelum menyimpan gambar
        //        await Task.Delay(500); // Delay 500ms

        //        // Save the image as BMP
        //        pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);

        //        // Define the target resolution (e.g., same as video resolution)
        //        int targetWidth = 1920; // Replace with the actual width of your video
        //        int targetHeight = 1080; // Replace with the actual height of your video

        //        using (var originalImage = System.Drawing.Image.FromFile(imageFilePath))
        //        {
        //            // Ensure crop rectangle is within the bounds of the original image
        //            Rectangle validCropRectangle = new Rectangle(
        //                Math.Max(0, cropRectangle.X),
        //                Math.Max(0, cropRectangle.Y),
        //                Math.Min(cropRectangle.Width, originalImage.Width - cropRectangle.X),
        //                Math.Min(cropRectangle.Height, originalImage.Height - cropRectangle.Y)
        //            );

        //            using (var croppedBitmap = new Bitmap(validCropRectangle.Width, validCropRectangle.Height))
        //            using (Graphics graphics = Graphics.FromImage(croppedBitmap))
        //            {
        //                graphics.DrawImage(originalImage, new Rectangle(0, 0, validCropRectangle.Width, validCropRectangle.Height),
        //                                    validCropRectangle, GraphicsUnit.Pixel);

        //                // Calculate the cropping rectangle to fit the target resolution
        //                float ratioX = (float)targetWidth / croppedBitmap.Width;
        //                float ratioY = (float)targetHeight / croppedBitmap.Height;
        //                float ratio = Math.Max(ratioX, ratioY); // Choose the larger ratio to fill the target size

        //                int newWidth = (int)(croppedBitmap.Width * ratio);
        //                int newHeight = (int)(croppedBitmap.Height * ratio);

        //                int xOffset = (newWidth - targetWidth) / 2;
        //                int yOffset = (newHeight - targetHeight) / 2;

        //                using (var resizedBitmap = new Bitmap(targetWidth, targetHeight))
        //                using (Graphics resizeGraphics = Graphics.FromImage(resizedBitmap))
        //                {
        //                    resizeGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //                    //resizeGraphics.Clear(Color.White); // Optional: Clear background with white color or any other color
        //                    resizeGraphics.DrawImage(croppedBitmap, -xOffset, -yOffset, newWidth, newHeight);

        //                    // Optionally add text to the resized image
        //                    using (Font arialFont = new Font("Arial", 15))
        //                    {
        //                        //resizeGraphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
        //                        //resizeGraphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
        //                        //resizeGraphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));
        //                    }

        //                    // Save the resized image as JPG
        //                    resizedBitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
        //                }
        //            }
        //        }
        //        // Hapus file BMP setelah menyimpan JPEG
        //        File.Delete(imageFilePath);

        //        AddImageToFlowLayoutPanel(imageFilePathJPG);
        //        btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
        //    }
        //    catch (OutOfMemoryException ex)
        //    {
        //        MessageBox.Show("Out of memory error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        MessageBox.Show("Argument exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private void btn_Capture_Click(object sender, EventArgs e)
        //{
        //    btn_Capture.BackColor = Color.FromArgb(0, 85, 119);

        //    textBox2.Clear();
        //    txtFoot.Focus();
        //    pictureBox2.Image = pictureBox1.Image;

        //    string cleanedName = Name.Trim();

        //    string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    string imageFilePath = Path.Combine(dir, $"{jam}.bmp");
        //    string imageFilePathJPG = Path.Combine(dir, $"{jam}.jpg");

        //    try
        //    {
        //        if (pictureBox1.Image == null)
        //        {
        //            throw new Exception("pictureBox1 does not contain an image.");
        //        }

        //        pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);

        //        using (var originalImage = System.Drawing.Image.FromFile(imageFilePath))
        //        using (var bitmap = new Bitmap(originalImage))
        //        //using (Graphics graphics = Graphics.FromImage(bitmap))
        //        using (Font arialFont = new Font("Arial", 15))
        //        {
        //            //graphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
        //            //graphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
        //            //graphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));

        //            bitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
        //        }

        //        // Hapus file BMP setelah menyimpan JPEG
        //        File.Delete(imageFilePath);

        //        AddImageToFlowLayoutPanel(imageFilePathJPG);
        //        btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
        //    }
        //    catch (OutOfMemoryException ex)
        //    {
        //        MessageBox.Show("Out of memory error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        MessageBox.Show("Argument exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        // Clean up resources if necessary
        //    }


        //}

        //private void btn_Capture_Click(object sender, EventArgs e)
        //{
        //    btn_Capture.BackColor = Color.FromArgb(0, 85, 119);

        //    textBox2.Clear();
        //    txtFoot.Focus();
        //    pictureBox2.Image = pictureBox1.Image;

        //    string cleanedName = Name.Trim();

        //    string dir = Path.Combine(@"D:\GLEndoscope\", splitTahun, splitBulan, tanggal, gabung, "Image");
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    string imageFilePath = Path.Combine(dir, $"{jam}.bmp");
        //    string imageFilePathJPG = Path.Combine(dir, $"{jam}.jpg");

        //    try
        //    {
        //        if (pictureBox1.Image == null)
        //        {
        //            throw new Exception("pictureBox1 does not contain an image.");
        //        }

        //        pictureBox1.Image.Save(imageFilePath, ImageFormat.Bmp);

        //        using (var originalImage = System.Drawing.Image.FromFile(imageFilePath))
        //        using (var bitmap = new Bitmap(originalImage))
        //        using (Graphics graphics = Graphics.FromImage(bitmap))
        //        using (Font arialFont = new Font("Arial", 15))
        //        {
        //            graphics.DrawString(tanggalHari, arialFont, Brushes.White, new PointF(30f, 25f));
        //            graphics.DrawString(Name, arialFont, Brushes.White, new PointF(1550f, 25f));
        //            graphics.DrawString(action1, arialFont, Brushes.White, new PointF(1550f, 50f));

        //            bitmap.Save(imageFilePathJPG, ImageFormat.Jpeg);
        //        }

        //        AddImageToFlowLayoutPanel(imageFilePathJPG);
        //        btn_Capture.BackColor = Color.FromArgb(0, 107, 150);
        //    }
        //    catch (OutOfMemoryException ex)
        //    {
        //        MessageBox.Show("Out of memory error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        MessageBox.Show("Argument exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        // Clean up resources if necessary
        //    }

        //} 

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

            // Ubah ukuran panel berdasarkan jumlah gambar yang ditampilkan
            //AdjustPanelSize(imagePanel);

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

                // Simpan indeks gambar yang akan dihapus
                //int removedIndex = panelBawah.Controls.GetChildIndex(imagePanel);

                // Hapus panel gambar dari flowLayoutPanel
                panelBawah.Controls.Remove(imagePanel);
                imagePanel.Dispose();

                // Coba hapus file gambar dari sistem
                try
                {
                    File.Delete(imagePath);
                }
                catch (IOException ex)
                {
                    // Tangani pengecualian jika file tidak dapat dihapus
                    MessageBox.Show("Gagal menghapus file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Update jumlah gambar setelah menghapus gambar
                //UpdateImageCount();

                // Gulir ke foto terbaru setelah menghapus
                //ScrollToNewestImage(removedIndex);



                panelBawah.AutoScrollPosition = new Point(0, 0);
            }
        }

        //private void ScrollToNewestImage(int removedIndex)
        //{
        //    // Jika masih ada gambar tersisa, gulir ke gambar terbaru
        //    if (panelBawah.Controls.Count > 0)
        //    {
        //        // Ambil indeks foto terbaru setelah penghapusan
        //        int newIndex = Math.Min(removedIndex, panelBawah.Controls.Count - 1);
        //        Control newControl = panelBawah.Controls[newIndex];
        //        panelBawah.ScrollControlIntoView(newControl);
        //    }
        //}



        private bool IsDirectoryEmpty(string path)
        {
            return Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0;
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
            int print1Picture = 1;
            int print4Picture = 2;
            int print6Picture = 3;
            int format2Print = 4;
            int print2CaptureCirebon = 5;
            int print4CaptureCirebon = 6;
            int print6CaptureCirebon = 7;
            int format3Print = 8;
            int format4Print = 9;

            if (textBoxPrint.Text == print1Picture.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form1Print newMDIChildd = new Form1Print();
                newMDIChildd.MdiParent = this;
                newMDIChildd.StartPosition = FormStartPosition.Manual;
                newMDIChildd.Left = 0;
                newMDIChildd.Top = 0;
                newMDIChildd.TransfEventt += frm_TransfEvent1;
                newMDIChildd.TransfEvenPrint4 += frm_TransfEventPrint4;
                newMDIChildd.TransfEvenPrint6 += frm_TransfEventPrint6;
                newMDIChildd.TransfEventPrint1G += frm_TransfEventPrint1G;
                newMDIChildd.Show();
                int Fone = 2;
                txt_Form.Text = Fone.ToString();
                string test = "kirim";
                newMDIChildd.textBox3.Text = test;
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print4Picture.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form4Print newMDIChilddd = new Form4Print();
                newMDIChilddd.MdiParent = this;
                newMDIChilddd.StartPosition = FormStartPosition.Manual;
                newMDIChilddd.Left = 0;
                newMDIChilddd.Top = 0;
                newMDIChilddd.TransfEventtt += frm_TransfEvent2;
                newMDIChilddd.TransfEventPrint1 += frm_TransfEventPrint14;
                newMDIChilddd.TransfEventPrint6 += frm_TransfEventPrint16;
                newMDIChilddd.TransfEvenPrint4G += frm_TransfEvenPrint4G;
                newMDIChilddd.Show();
                int Ffour = 3;
                txt_Form.Text = Ffour.ToString();
                string kirim = "kirim";
                newMDIChilddd.textBox3.Text = kirim;
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print6Picture.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form6Print newMDIChildddd = new Form6Print();
                newMDIChildddd.MdiParent = this;
                newMDIChildddd.StartPosition = FormStartPosition.Manual;
                newMDIChildddd.Left = 0;
                newMDIChildddd.Top = 0;
                newMDIChildddd.TransfEventttt += frm_TransfEvent3;
                newMDIChildddd.TransfEventPrint1 += frm_TransfEvent61;
                newMDIChildddd.TransfEventPrint6 += frm_TransfEvent64;
                newMDIChildddd.TransfEventPrint6G += frm_TransfEventPrint6G;
                newMDIChildddd.Show();
                int Fsix = 4;
                txt_Form.Text = Fsix.ToString();
                string kirim = "kirim";
                newMDIChildddd.textBox3.Text = kirim;
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == format2Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form21Gambar form21 = new Form21Gambar();
                form21.MdiParent = this;
                form21.StartPosition = FormStartPosition.Manual;
                form21.Left = 0;
                form21.Top = 0;
                form21.TEViewC6Gambar += frm_TEViewC6Gambar;
                form21.TEFormat2 += frm_TransfEventFormat2;
                form21.TEViewC2Gambar += frm_TEViewC2Gambar;
                form21.TEViewC4Gambar += frm_TEViewC4Gambar;
                form21.TEViewC21G += frm_TEViewC21G;
                form21.Show();
                int Fsix = 4;
                txt_Form.Text = Fsix.ToString();
                string kirim = "kirim";
                form21.textBox2.Text = kirim;
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print2CaptureCirebon.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form22Gambar form22 = new Form22Gambar();
                form22.MdiParent = this;
                form22.StartPosition = FormStartPosition.Manual;
                form22.Left = 0;
                form22.Top = 0;

                form22.TEViewC21Gambar += frm_TEViewC21Gambar;
                form22.TEViewC24Gambar += frm_TEViewC24Gambar;
                form22.TEViewC26Gambar += frm_TEViewC26Gambar;
                form22.TEClose2Gambar += frm_TEClose2Gambar;
                form22.TEViewC2 += frm_TEViewC2;

                form22.Show();
                int Fsix = 5;
                txt_Form.Text = Fsix.ToString();
                string kirim = "kirim";
                form22.textBox2.Text = kirim;
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print4CaptureCirebon.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form24Gambar form24 = new Form24Gambar();
                form24.MdiParent = this;
                form24.StartPosition = FormStartPosition.Manual;
                form24.Left = 0;
                form24.Top = 0;
                form24.TEViewC41Gambar += frm_TEViewC41Gambar;
                form24.TEViewC42Gambar += frm_TEViewC42Gambar;
                form24.TEViewC46Gambar += frm_TEViewC46Gambar;
                form24.TEClose4Gambar += frm_TEClose4Gambar;
                form24.TEViewC24G += frm_TEViewC24G;
                string kirim = "kirim";
                form24.textBox2.Text = kirim;
                form24.Show();
                int Fsix = 6;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print6CaptureCirebon.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelBawah.Visible = false;
                panelKanan.Visible = false;
                panelKiri.Visible = false;
                createFolder();
                Form26Gambar form26 = new Form26Gambar();
                form26.MdiParent = this;
                form26.StartPosition = FormStartPosition.Manual;
                form26.Left = 0;
                form26.Top = 0;
                //formCirebon6Gambar.TEViewC41Gambar += frm_TEViewC41Gambar;
                //formCirebon6Gambar.TEViewC42Gambar += frm_TEViewC42Gambar;
                form26.TEClose6Gambar += frm_TEClose6Gambar;
                form26.TEViewC64Gambar += frm_TEViewC64Gambar;
                form26.TEViewC62Gambar += frm_TEViewC62Gambar;
                form26.TEViewC61Gambar += frm_TEViewC61Gambar;
                form26.TEViewC46G += frm_TEViewC46G;
                string kirim = "kirim";
                form26.textBox2.Text = kirim;
                form26.Show();
                int Fsix = 7;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == format3Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Capture.Enabled = false;
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
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Capture.Enabled = false;
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
                //form48.TransfEventPrint310 += frm_TransfEventPrint310;
                form48.TEClose48Gambar += frm_TEClose48Gambar;
                //form48.TransfEventPrint310Print += frm_TransfEventPrint310Print;
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

        //private void textBoxPrint_TextChanged(object sender, EventArgs e)
        //{
        //    int print1Picture = 1;
        //    int print4Picture = 2;
        //    int print6Picture = 3;
        //    int format2Print = 4; 
        //    int print2CaptureCirebon = 5; 
        //    int print4CaptureCirebon = 6;  
        //    int print6CaptureCirebon = 7;  

        //    if (textBoxPrint.Text == print1Picture.ToString())
        //    { 
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        } 

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder(); 
        //        Form1Print newMDIChildd = new Form1Print();
        //        newMDIChildd.MdiParent = this;
        //        newMDIChildd.StartPosition = FormStartPosition.Manual;
        //        newMDIChildd.Left = 0;
        //        newMDIChildd.Top = 0; 
        //        newMDIChildd.TransfEventt += frm_TransfEvent1;
        //        newMDIChildd.TransfEvenPrint4 += frm_TransfEventPrint4;
        //        newMDIChildd.TransfEvenPrint6 += frm_TransfEventPrint6;
        //        newMDIChildd.TransfEventPrint1G += frm_TransfEventPrint1G;
        //        newMDIChildd.Show();
        //        int Fone = 2;
        //        txt_Form.Text = Fone.ToString(); 
        //        string test = "kirim"; 
        //        newMDIChildd.textBox3.Text = test; 
        //        textBoxPrint.Clear();
        //    }

        //    else if (textBoxPrint.Text == print4Picture.ToString())
        //    {
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        } 

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form4Print newMDIChilddd = new Form4Print();
        //        newMDIChilddd.MdiParent = this;
        //        newMDIChilddd.StartPosition = FormStartPosition.Manual;
        //        newMDIChilddd.Left = 0;
        //        newMDIChilddd.Top = 0; 
        //        newMDIChilddd.TransfEventtt += frm_TransfEvent2;
        //        newMDIChilddd.TransfEventPrint1 += frm_TransfEventPrint14;
        //        newMDIChilddd.TransfEventPrint6 += frm_TransfEventPrint16;
        //        newMDIChilddd.TransfEvenPrint4G += frm_TransfEvenPrint4G;
        //        newMDIChilddd.Show();
        //        int Ffour = 3;
        //        txt_Form.Text = Ffour.ToString(); 
        //        string kirim = "kirim";
        //        newMDIChilddd.textBox3.Text = kirim; 
        //        textBoxPrint.Clear(); 
        //    }

        //    else if (textBoxPrint.Text == print6Picture.ToString())
        //    {
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        }

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form6Print newMDIChildddd = new Form6Print();
        //        newMDIChildddd.MdiParent = this;
        //        newMDIChildddd.StartPosition = FormStartPosition.Manual;
        //        newMDIChildddd.Left = 0;
        //        newMDIChildddd.Top = 0; 
        //        newMDIChildddd.TransfEventttt += frm_TransfEvent3;
        //        newMDIChildddd.TransfEventPrint1 += frm_TransfEvent61;
        //        newMDIChildddd.TransfEventPrint6 += frm_TransfEvent64;
        //        newMDIChildddd.TransfEventPrint6G += frm_TransfEventPrint6G;
        //        newMDIChildddd.Show();
        //        int Fsix = 4;
        //        txt_Form.Text = Fsix.ToString(); 
        //        string kirim = "kirim";
        //        newMDIChildddd.textBox3.Text = kirim; 
        //        textBoxPrint.Clear();  
        //    }

        //    else if (textBoxPrint.Text == format2Print.ToString())
        //    {
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        }

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form21Gambar form21 = new Form21Gambar();
        //        form21.MdiParent = this;
        //        form21.StartPosition = FormStartPosition.Manual;
        //        form21.Left = 0;
        //        form21.Top = 0;
        //        form21.TEViewC6Gambar += frm_TEViewC6Gambar;
        //        form21.TEFormat2 += frm_TransfEventFormat2;
        //        form21.TEViewC2Gambar += frm_TEViewC2Gambar;
        //        form21.TEViewC4Gambar += frm_TEViewC4Gambar;
        //        form21.TEViewC21G += frm_TEViewC21G;
        //        form21.Show();
        //        int Fsix = 4;
        //        txt_Form.Text = Fsix.ToString(); 
        //        string kirim = "kirim";
        //        form21.textBox2.Text = kirim; 
        //        textBoxPrint.Clear();
        //    } 

        //    else if (textBoxPrint.Text == print2CaptureCirebon.ToString())
        //    { 
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        }

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form22Gambar form22 = new Form22Gambar();
        //        form22.MdiParent = this;
        //        form22.StartPosition = FormStartPosition.Manual;
        //        form22.Left = 0;
        //        form22.Top = 0;

        //        form22.TEViewC21Gambar += frm_TEViewC21Gambar;
        //        form22.TEViewC24Gambar += frm_TEViewC24Gambar;
        //        form22.TEViewC26Gambar += frm_TEViewC26Gambar;
        //        form22.TEClose2Gambar += frm_TEClose2Gambar;
        //        form22.TEViewC2 += frm_TEViewC2;

        //        form22.Show();
        //        int Fsix = 5;
        //        txt_Form.Text = Fsix.ToString(); 
        //        string kirim = "kirim";
        //        form22.textBox2.Text = kirim; 
        //        textBoxPrint.Clear(); 
        //    }

        //    else if (textBoxPrint.Text == print4CaptureCirebon.ToString())
        //    { 
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true; 
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
        //            btn_Capture.Enabled = false;
        //        }

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form24Gambar form24 = new Form24Gambar();
        //        form24.MdiParent = this;
        //        form24.StartPosition = FormStartPosition.Manual;
        //        form24.Left = 0;
        //        form24.Top = 0;
        //        form24.TEViewC41Gambar += frm_TEViewC41Gambar;
        //        form24.TEViewC42Gambar += frm_TEViewC42Gambar;
        //        form24.TEViewC46Gambar += frm_TEViewC46Gambar;
        //        form24.TEClose4Gambar += frm_TEClose4Gambar; 
        //        form24.TEViewC24G += frm_TEViewC24G; 
        //        string kirim = "kirim";
        //        form24.textBox2.Text = kirim;
        //        form24.Show();
        //        int Fsix = 6;
        //        txt_Form.Text = Fsix.ToString(); 
        //        textBoxPrint.Clear(); 
        //    }

        //    else if (textBoxPrint.Text == print6CaptureCirebon.ToString())
        //    {
        //        if (textBox1.Text == "2")
        //        {
        //            FinalVideo.Stop();
        //            buttonRecStart.Enabled = true;
        //            buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
        //            btn_Capture.Enabled = false;
        //        }

        //        videoSourcePlayer.Visible = false;
        //        //panelAtas.Visible = false;
        //        panelBawah.Visible = false;
        //        panelKiri.Visible = false;
        //        panelKanan.Visible = false;
        //        createFolder();
        //        Form26Gambar form26 = new Form26Gambar();
        //        form26.MdiParent = this;
        //        form26.StartPosition = FormStartPosition.Manual;
        //        form26.Left = 0;
        //        form26.Top = 0;
        //        //formCirebon6Gambar.TEViewC41Gambar += frm_TEViewC41Gambar;
        //        //formCirebon6Gambar.TEViewC42Gambar += frm_TEViewC42Gambar;
        //        form26.TEClose6Gambar += frm_TEClose6Gambar;
        //        form26.TEViewC64Gambar += frm_TEViewC64Gambar;
        //        form26.TEViewC62Gambar += frm_TEViewC62Gambar;
        //        form26.TEViewC61Gambar += frm_TEViewC61Gambar;
        //        form26.TEViewC46G += frm_TEViewC46G;
        //        string kirim = "kirim";
        //        form26.textBox2.Text = kirim;
        //        form26.Show();
        //        int Fsix = 7;
        //        txt_Form.Text = Fsix.ToString();
        //        textBoxPrint.Clear();
        //    }
        //}

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

        private void frm_TransfEventPrint1G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvenPrint4G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint6G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC21G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC2(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC24G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC46G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC46Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC26Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC61Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC62Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC64Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose6Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEViewC6Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose4Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEClose2Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEViewC42Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC41Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC24Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC21Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC4Gambar(string value)
        {
            textBoxPrint.Text = value;
        }
        private void frm_TEViewC2Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void txt_kondisi_TextChanged(object sender, EventArgs e)
        {
            int nn = 1;
            int mm = 2;
            int ss = 3;
            int kk = 4;
            int dd = 5;
            int aa = 6;
            int fDokter = 7;
            int FormatClose = 9;
            int FKlikF4 = 10;
            int FKlikF146 = 11;
            int FKlikF141 = 13;
            int FKlikF16 = 12;
            int FKlikF161 = 14;
            int FKlikF164 = 15;
            int FS24Gambar = 16;
            int FS310Gambar = 17;
            int FS48Gambar = 21;

            if (txt_kondisi.Text == nn.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                btn_patient.Enabled = true;
                btn_patient.BackColor = Color.FromArgb(0, 107, 150);
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == ss.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
            }
            else if (txt_kondisi.Text == kk.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == dd.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == aa.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
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
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                buttonDokter.Enabled = true;
                buttonDokter.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FormatClose.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF4.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF146.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }

            else if (txt_kondisi.Text == FKlikF16.ToString())
            {
                videoSourcePlayer.Visible = true;
                ////panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF141.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }

            else if (txt_kondisi.Text == FKlikF161.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }


            else if (txt_kondisi.Text == FKlikF164.ToString())
            {
                videoSourcePlayer.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FS24Gambar.ToString())
            {
                panel2.Visible = true;
                //panelAtas.Visible = true;
                panelBawah.Visible = true;
                panelKiri.Visible = true;
                panelKanan.Visible = true;
                videoSourcePlayer.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
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
                //MessageBox.Show("1");
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

        private void frm_TransfEvent1(string value)
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
                    //panelAtas.Visible = false;
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

        private void frm_TransfEvent2(string value)
        {
            txt_kondisi.Text = value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (FinalVideo == null)
                    {

                    }
                    else
                    {
                        FinalVideo.SignalToStop();
                    }

                    buttonRecStart.Enabled = true;
                    buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    createFolder();
                    panel2.Visible = false;
                    //panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    panelKiri.Visible = false;
                    panelKanan.Visible = false;
                    videoSourcePlayer.Visible = false;
                    FormSwitcing2Gambar formSwitcing2Gambar = new FormSwitcing2Gambar();
                    formSwitcing2Gambar.MdiParent = this;
                    formSwitcing2Gambar.StartPosition = FormStartPosition.Manual;
                    formSwitcing2Gambar.Left = 0;
                    formSwitcing2Gambar.Top = 0;
                    formSwitcing2Gambar.TEFS2Gambar += frm_TEFS2Gambar;
                    formSwitcing2Gambar.textBox1.Text = "1";
                    formSwitcing2Gambar.Show();
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

        private void frm_TEFS2Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    if (FinalVideo == null)
                    {

                    }
                    else
                    {
                        FinalVideo.SignalToStop();
                    }
                    buttonRecStart.Enabled = true;
                    buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //FinalVideo.SignalToStop();
                    createFolder();
                    panel2.Visible = false;
                    //panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    panelKiri.Visible = false;
                    panelKanan.Visible = false;
                    videoSourcePlayer.Visible = false;
                    FormSwitcing4Gambar formSwitcing4Gambar = new FormSwitcing4Gambar();
                    formSwitcing4Gambar.MdiParent = this;
                    formSwitcing4Gambar.StartPosition = FormStartPosition.Manual;
                    formSwitcing4Gambar.Left = 0;
                    formSwitcing4Gambar.Top = 0;
                    formSwitcing4Gambar.TEFS4Gambar += frm_TEFS4Gambar;
                    formSwitcing4Gambar.textBox1.Text = "1";
                    formSwitcing4Gambar.Show();
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

        private void frm_TEFS4Gambar(string value)
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
                    //this.FinalVideo.Stop();
                    FileWriter.Close();
                    //this.AVIwriter.Close();
                    //pic.Image = null;
                }

                s1.Stop();
                s1.Reset();
                lblRec1.Visible = false;
                picRec1.Visible = false;
                //btn_Record.Text = "Rekam Internal";

                txtFoot.Enabled = true;
                txtFoot.Focus();
                buttonRecSave.BackColor = Color.FromArgb(0, 107, 150);
                vRecord = false;
                //panelBawah.Visible = false;
                //hScrollBar1.Visible = false;
                //ToggleImageVisibility();
                MessageBox.Show("Video disimpan di folder", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);



            }
            else
            {
                vCamera = false;
                this.FinalVideo.SignalToStop();
                FileWriter.Close();
                //this.AVIwriter.Close();
                pictureBox1.Image = null;
                buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                btn_Capture.Enabled = false;
                buttonRecSave.Enabled = false;
                buttonRecStop.Enabled = false;
                buttonRecStart.Enabled = true;

                // Bersihkan semua PictureBox dari FlowLayoutPanel
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
                    //FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Record_OBS.BackColor = Color.FromArgb(0, 85, 119);
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = "C:/Program Files/obs-studio/bin/64bit"; // like cd path command
                    startInfo.FileName = "obs64.exe";
                    Process.Start(startInfo);
                    //btn_Record_OBS.Enabled = false;
                    //buttonRecSave.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            panelBawah.AutoScrollPosition = new Point(0, 0);
        }

        private void frm_TransfEvent3(string value)
        {
            txt_kondisi.Text = value;
        }


        private void frm_TransfEvent4(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEventPrint1(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint4(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint6(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint14(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint16(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvent61(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvent64(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventFormat2(string value)
        {
            txt_kondisi.Text = value;
        }



        private void disableButtonRecord()
        {
            btn_Capture.Enabled = false;
            btn_patient.Enabled = false;
            btn_Record_OBS.Enabled = false;
        }

        private void enableButtonRecord()
        {
            btn_Capture.Enabled = true;
            btn_patient.Enabled = true;
            btn_Record_OBS.Enabled = true;
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
