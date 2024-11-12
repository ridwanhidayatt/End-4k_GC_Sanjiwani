using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace WindowsFormsApp1.Form_Utama
{
    public partial class FormDokter : Form
    {
        public delegate void TransfDelegate(String value);
        public event TransfDelegate TransfEventDokter;

        // Deklarasikan DataTable di tingkat kelas
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        private bool isEditing = false;
        // Define event untuk memberi tahu bahwa Form2 ditutup
        public event Action FormClosedEvent;

        private List<Dokter> dataList = new List<Dokter>();

        // Buat sumber data BindingList<Dokter>
        BindingList<Dokter> dataSource = new BindingList<Dokter>();
        private BindingSource bindingSource = new BindingSource();

        private BindingList<Dokter> bindingList = new BindingList<Dokter>();


        public class Dokter
        {
            public int No { get; set; }
            public string NIP { get; set; }
            public string Nama { get; set; }
            [DisplayName("Jenis Kelamin")]
            public string JenisKelamin { get; set; }
            public string Alamat { get; set; }
        }

        public sealed class DokterMap : CsvClassMap<Dokter>
        {
            public DokterMap()
            {

                Map(m => m.No).Index(0);
                Map(m => m.NIP).Index(1);
                Map(m => m.Nama).Index(2);
                Map(m => m.JenisKelamin).Index(3);
                Map(m => m.Alamat).Index(4);
            }
        }


        // Fungsi untuk memuat data dari file CSV ke dalam list of Dokter
        public List<Dokter> MuatDataDariCSV()
        {
            string directoryPath = @"D:\GLEndoscope\Database\dataDokter";
            string fileName = "namaDokter.csv";
            string filePath4 = Path.Combine(directoryPath, fileName);

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Prepare the list to store dokter data
            List<Dokter> dokters = new List<Dokter>();

            // Check if the file exists before attempting to read it
            if (File.Exists(filePath4))
            {
                // Open the CSV file and read the data
                using (var reader = new StreamReader(filePath4))
                using (var csv = new CsvReader(reader))
                {
                    // Read all rows of data from the CSV file
                    while (csv.Read())
                    {
                        // Map the current row's record to a Dokter object
                        Dokter dokter = new Dokter
                        {
                            No = csv.GetField<int>(0),
                            NIP = csv.GetField<string>(1),
                            Nama = csv.GetField<string>(2),
                            JenisKelamin = csv.GetField<string>(3),
                            Alamat = csv.GetField<string>(4)
                        };

                        // Add the Dokter object to the list
                        dokters.Add(dokter);
                    }
                }
            }
            else
            {
                // If the file doesn't exist, handle it accordingly
                // For example, you can log an error or return an empty list
                Console.WriteLine($"File not found: {filePath4}");
            }

            return dokters;
        }





        public FormDokter()
        {
            InitializeComponent();

            // Initialize columns for dt
            //dt.Columns.Add("No", typeof(int));
            //dt.Columns.Add("NIP", typeof(string));
            //dt.Columns.Add("Nama", typeof(string));
            //dt.Columns.Add("JenisKelamin", typeof(string));
            //dt.Columns.Add("Alamat", typeof(string));

            //// Set DataGridView1 to use dt as its data source
            //dataGridView1.DataSource = dt; 

            // Mengatur event handler untuk double-click pada sel DataGridView
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;


            txt_NipDokter.TextChanged += new EventHandler(AnyFieldChanged);
            txt_NamaDokter.TextChanged += new EventHandler(AnyFieldChanged);
            txt_AlamatDokter.TextChanged += new EventHandler(AnyFieldChanged);
            radioButtonPria.CheckedChanged += new EventHandler(AnyFieldChanged);
            radioButtonWanita.CheckedChanged += new EventHandler(AnyFieldChanged);

            btn_Save.Enabled = false;
            btn_Cancel.Enabled = false;

            //// Menetapkan event handler ke setiap TextBox dan RadioButton
            //txt_NipDokter.TextChanged += textBox_TextChanged;
            //txt_NamaDokter.TextChanged += textBox_TextChanged;
            //txt_AlamatDokter.TextChanged += textBox_TextChanged;
            //radioButtonPria.CheckedChanged += radioButton_CheckedChanged;
            //radioButtonWanita.CheckedChanged += radioButton_CheckedChanged;

            //// Inisialisasi status tombol "Save"
            //UpdateSaveButtonStatus();



            RefreshDataGridView();

            // Bind DataGridView ke sumber data BindingList
            dataGridView1.DataSource = dataSource;
        }

        private void AnyFieldChanged(object sender, EventArgs e)
        {
            CheckAllFieldsFilled();
        }

        private void CheckAllFieldsFilled()
        {
            if (isEditing)
            {
                // Jika sedang mengedit, tetap nonaktifkan tombol Save
                btn_Save.Enabled = false;
                return;
            }
            // Periksa apakah semua kontrol telah diisi
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txt_NipDokter.Text) &&
                                   !string.IsNullOrWhiteSpace(txt_NamaDokter.Text) &&
                                   !string.IsNullOrWhiteSpace(txt_AlamatDokter.Text) &&
                                   (radioButtonPria.Checked || radioButtonWanita.Checked);

            // Aktifkan atau nonaktifkan tombol berdasarkan hasil pengecekan
            btn_Save.Enabled = allFieldsFilled;
            btn_Cancel.Enabled = allFieldsFilled;
        }
        //baru

        private void LoadLastSelectedResolution()
        {
            // Membaca nilai lebar dan tinggi terakhir dari file teks
            string filePath = Path.Combine(Application.StartupPath, "last_selected_resolution.txt");
            if (File.Exists(filePath))
            {
                int width, height; // Deklarasi variabel sebelum blok if
                string[] resolution = File.ReadAllText(filePath).Split(',');
                if (resolution.Length == 2 && int.TryParse(resolution[0], out width) && int.TryParse(resolution[1], out height))
                {
                    SetFormResolution(width, height);
                    AdjustControlsLayout(width, height);
                }
            }
        }

        private void AdjustControlsLayout(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void SetFormResolution(int lastSelectedWidth, int lastSelectedHeight)
        {
            this.Width = lastSelectedWidth;
            this.Height = lastSelectedHeight;
        }

        // Event handler untuk memperbarui status tombol "Save" setiap kali isi dari TextBox atau RadioButton berubah
        //private void textBox_TextChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void radioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void UpdateSaveButtonStatus()
        //{
        //    // Periksa apakah semua TextBox telah diisi
        //    bool allTextBoxesFilled = !String.IsNullOrEmpty(txt_NipDokter.Text) &&
        //                     !String.IsNullOrEmpty(txt_NamaDokter.Text) &&
        //                     !String.IsNullOrEmpty(txt_AlamatDokter.Text);

        //    // Periksa apakah RadioButton telah dipilih
        //    bool radioButtonSelected = radioButtonPria.Checked || radioButtonWanita.Checked;

        //    // Aktifkan tombol "Save" hanya jika semua TextBox telah diisi dan RadioButton telah dipilih
        //    btn_Save.Enabled = allTextBoxesFilled && radioButtonSelected;
        //    btn_Cancel.Enabled = btn_Save.Enabled;
        //}

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan baris yang diklik tidak di luar batas indeks
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                // Ambil data dari baris yang diklik
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];


                // Transfer data ke textbox
                txt_NipDokter.Text = selectedRow.Cells["NIP"].Value.ToString();
                txt_NamaDokter.Text = selectedRow.Cells["Nama"].Value.ToString();
                txt_AlamatDokter.Text = selectedRow.Cells["Alamat"].Value.ToString();

                // Tentu saja, Anda perlu menangani jenis kelamin sesuai kebutuhan aplikasi Anda.
                // Dalam contoh ini, saya asumsikan data jenis kelamin adalah string dan ditampilkan di radio button.

                string jenisKelamin = selectedRow.Cells["JenisKelamin"].Value.ToString();
                if (jenisKelamin == "Laki - laki")
                {
                    radioButtonPria.Checked = true;
                    radioButtonWanita.Checked = false;
                }
                else if (jenisKelamin == "Perempuan")
                {
                    radioButtonPria.Checked = false;
                    radioButtonWanita.Checked = true;
                }
                isEditing = true;
                txt_NipDokter.Enabled = false;
                textBoxAwal.Text = txt_NamaDokter.Text;
                //btn_Save.Enabled = false;
                EnableButtons();
                btn_Save.Enabled = false;
            }
        }

        private void FormDokter_Load(object sender, EventArgs e)
        {




            this.ActiveControl = label1;
            DisableButtons();
            // Isi BindingList dengan data dari CSV atau sumber data lainnya
            bindingList = LoadDataFromCSV1();

            // Hapus header dari bindingList jika file CSV tidak memiliki header
            if (!bindingList.Any())
            {
                dataGridView1.DataSource = bindingList;
                return;
            }
            else
            {
                // Hapus header dari bindingList
                bindingList.RemoveAt(0);
            }

            // Atur DataGridView untuk menggunakan BindingList sebagai sumber data
            dataGridView1.DataSource = bindingList;

            RefreshDataGridView();


        }

        private void DisableButtons()
        {
            btn_Cancel.Enabled = false;
            button5.Enabled = false;
            btn_DeleteForm.Enabled = false;
            btn_Save.Enabled = false;
        }

        private void EnableButtons()
        {
            btn_Cancel.Enabled = true;
            button5.Enabled = true;
            btn_DeleteForm.Enabled = true;
            btn_Save.Enabled = true;
        }


        private BindingList<Dokter> LoadDataFromCSV1()
        {
            //string directoryPath7 = @"D:\";
            //string filePath6 = Path.Combine(directoryPath7, "dokter.csv");

            string directoryPath = @"D:\GLEndoscope\Database\dataDokter";
            string fileName = "namaDokter.csv";
            string filePath6 = Path.Combine(directoryPath, fileName);


            // Cek apakah file CSV ada
            if (!File.Exists(filePath6))
            {
                //MessageBox.Show("File CSV tidak ditemukan.");
                return new BindingList<Dokter>(); // Kembalikan BindingList kosong jika file tidak ditemukan
            }

            // Membaca data dari file CSV
            BindingList<Dokter> data = new BindingList<Dokter>();
            using (var reader = new StreamReader(filePath6))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false; // Atur ke false jika file CSV tidak memiliki header
                while (csv.Read())
                {
                    data.Add(new Dokter
                    {
                        NIP = csv.GetField<string>(1),
                        Nama = csv.GetField<string>(2),
                        JenisKelamin = csv.GetField<string>(3),
                        Alamat = csv.GetField<string>(4)
                    });
                }
            }

            // Jika data kosong, tampilkan pesan
            //if (data.Count == 0)
            //{
            //    MessageBox.Show("Tidak ada data yang tersedia. Silakan tambahkan data baru.");
            //}

            return data; // Kembalikan BindingList yang berisi data dari CSV
        }




        private void RefreshDataGridView()
        {
            var dataList = MuatDataDariCSV();
            bindingSource.DataSource = new BindingList<Dokter>(dataList);

            dataGridView1.DataSource = bindingSource;
        }







        private DataTable dataTable;
        List<string> hasilPencarian = new List<string>();
        private bool IsNullOrWhiteSpace(string value)
        {
            if (value == null)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        //private bool IsDataValid(string nip, string nama, string jenisKelamin, string alamat)
        //{
        //    // Validasi bahwa semua data telah diisi
        //    if (IsNullOrWhiteSpace(nip) ||
        //        IsNullOrWhiteSpace(nama) ||
        //        IsNullOrWhiteSpace(jenisKelamin) ||
        //        IsNullOrWhiteSpace(alamat))
        //    {
        //        MessageBox.Show("Harap isi semua data sebelum menyimpan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return false; // Data tidak valid, hentikan proses penyimpanan
        //    }
        //    return true; // Data valid, lanjutkan proses penyimpanan
        //}

        private bool IsNIPAlreadyExistsSimpan(string nip)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["NIP"].Value != null && row.Cells["NIP"].Value.ToString() == nip)
                {
                    return true;
                }
            }
            return false;
        }


        static DataTable ReadDataFromCSV(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string[] headers = sr.ReadLine().Split(',');

                    foreach (string header in headers)
                    {
                        // Assuming all columns are of type string for simplicity
                        dataTable.Columns.Add(header.Trim(), typeof(string));
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');

                        DataRow dr = dataTable.NewRow();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }

                        dataTable.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
            }

            return dataTable;
        }

        //public void TampilkanDataKeDataGridView()
        //{
        //    //// Memuat data dari file CSV
        //    //List<Dokter> data = MuatDataDariCSV();

        //    //// Mengatur sumber data DataGridView
        //    //dataGridView1.DataSource = data;

        //    string directoryPath = @"D:\GLEndoscope\Database\dataDokter";
        //    string fileName = "namaDokter.csv";
        //    string filePath = Path.Combine(directoryPath, fileName);


        //    // Cek apakah file CSV ada
        //    if (!File.Exists(filePath))
        //    {
        //        //MessageBox.Show("File CSV tidak ditemukan.");
        //        return;
        //    }

        //    // Membaca data dari file CSV
        //    List<Dokter> data = new List<Dokter>();
        //    using (var reader = new StreamReader(filePath))
        //    using (var csv = new CsvReader(reader))
        //    {
        //        csv.Configuration.HasHeaderRecord = false; // Atur ke false jika file CSV tidak memiliki header
        //        while (csv.Read())
        //        {
        //            data.Add(new Dokter
        //            {
        //                NIP = csv.GetField<string>(0),
        //                Nama = csv.GetField<string>(1),
        //                JenisKelamin = csv.GetField<string>(2),
        //                Alamat = csv.GetField<string>(3)
        //            });
        //        }
        //    }

        //    // Menetapkan data ke DataGridView
        //    dataGridView1.DataSource = data;

        //    // Jika data kosong, tampilkan pesan
        //    //if (data.Count == 0)
        //    //{
        //    //    MessageBox.Show("Tidak ada data yang tersedia. Silakan tambahkan data baru.");
        //    //}
        //}


        public void TampilkanDataKeDataGridView()
        {
            string directoryPath = @"D:\GLEndoscope\Database\dataDokter";
            string fileName = "namaDokter.csv";
            string filePath = Path.Combine(directoryPath, fileName);

            // Cek apakah file CSV ada
            if (!File.Exists(filePath))
            {
                //MessageBox.Show("File CSV tidak ditemukan.");
                return;
            }

            // Membaca data dari file CSV
            List<Dokter> data = new List<Dokter>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true; // Set true jika file CSV memiliki header
                while (csv.Read())
                {
                    data.Add(new Dokter
                    {
                        No = csv.GetField<int>("No"),
                        NIP = csv.GetField<string>("NIP"),
                        Nama = csv.GetField<string>("Nama"),
                        JenisKelamin = csv.GetField<string>("JenisKelamin"),
                        Alamat = csv.GetField<string>("Alamat")
                    });
                }
            }

            // Menetapkan data ke DataGridView
            dataGridView1.DataSource = data;
        }

        //private void SimpanDataKeCSV(string nip, string nama, string jenisKelamin, string alamat)
        //{




        //    string directoryPath = @"D:\GLEndoscope\Database\dataDokter";
        //    string fileName = "namaDokter.csv";
        //    string filePath = Path.Combine(directoryPath, fileName);

        //    // Buat objek Dokter untuk menampung data
        //    var dokter = new Dokter
        //    {
        //        NIP = nip,
        //        Nama = nama,
        //        JenisKelamin = jenisKelamin,
        //        Alamat = alamat
        //    };

        //    bool isHeaderExist = File.Exists(filePath);

        //    // Menambahkan data ke file CSV
        //    using (var writer = new StreamWriter(filePath, true))
        //    using (var csv = new CsvWriter(writer))
        //    {
        //        if (!isHeaderExist)
        //        {
        //            // Menulis header jika tidak ada
        //            csv.WriteField("NIP");
        //            csv.WriteField("Nama");
        //            csv.WriteField("JenisKelamin");
        //            csv.WriteField("Alamat");
        //            csv.NextRecord();
        //        }

        //        // Menulis baris data ke file CSV
        //        csv.WriteRecord(dokter);
        //    }
        //}



        private void ResetFormMode()
        {
            // Bersihkan textbox dan radio button untuk entri berikutnya
            txt_NipDokter.Clear();
            txt_NamaDokter.Clear();
            txt_AlamatDokter.Clear();
            radioButtonPria.Checked = false;
            radioButtonWanita.Checked = false;
            btn_Save.Enabled = true;
            txt_NipDokter.Enabled = true;
        }



        private void UpdateNoValues()
        {
            // Iterate through the dataList and update the 'No' values
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].No = i + 1;
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            int kondisi = 7;
            TransfEventDokter(kondisi.ToString());
            this.Close();
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "Laki - laki")
            {
                radioButtonPria.Checked = true;
                textBox1.Clear();
            }
            else if (textBox1.Text == "Perempuan")
            {
                radioButtonWanita.Checked = true;
                textBox1.Clear();
            }
        }


        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ResetFormMode();
            txt_NipDokter.Enabled = true;
            DisableButtons();
            isEditing = false;
        }

        private void btn_Search1_Click(object sender, EventArgs e)
        {
            ResetFormMode();
            DisableButtons();
            ClearTextBoxes();
            isEditing = false;
            string keyword = txt_Search1.Text.Trim().ToLower();

            var csvConfig = new CsvConfiguration(); // CsvConfiguration tanpa argumen

            using (var reader = new StreamReader(@"D:\GLEndoscope\Database\\dataDokter\namaDokter.csv"))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                var records = csv.GetRecords<Dokter>().ToList();

                var searchResult = records.Where(data =>
                    data.NIP.ToLower().Contains(keyword) ||
                    data.Nama.ToLower().Contains(keyword) ||
                    data.JenisKelamin.ToLower().Contains(keyword) ||
                    data.Alamat.ToLower().Contains(keyword)
                ).ToList();

                if (searchResult.Any())
                {
                    DataTable searchTable = ToDataTable(searchResult);
                    dataGridView1.DataSource = searchTable;
                }
                else
                {
                    MessageBox.Show("Tidak ada hasil yang ditemukan.", "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ClearTextBoxes()
        {
            txt_NipDokter.Clear();
            txt_NamaDokter.Clear();
            radioButtonPria.Checked = false;
            radioButtonWanita.Checked = false;
            txt_AlamatDokter.Clear();

            // Reset the flag and button text
            //isEditing = false;
            //btn_Save.Text = "Simpan";
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            System.Reflection.PropertyInfo[] props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            isEditing = false;
            ResetFormMode();
            DisableButtons();
            RefreshDataGridView();
            txt_Search1.Clear();
            ClearTextBoxes();
        }

        private void btn_DeleteForm_Click(object sender, EventArgs e)
        {
            isEditing = false;
            // Ambil NIP yang akan dihapus
            string nipToDelete = txt_NipDokter.Text.Trim();

            // Konfirmasi pengguna sebelum menghapus
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data dengan NIP: " + nipToDelete + " ?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Cari item yang sesuai dengan NIP yang ingin dihapus
                Dokter dokterToDelete = bindingList.FirstOrDefault(d => d.NIP == nipToDelete);

                if (dokterToDelete != null)
                {
                    // Hapus item dari BindingList
                    bindingList.Remove(dokterToDelete);

                    // Simpan data ke file CSV setelah penghapusan selesai
                    List<Dokter> dataList = bindingList.ToList(); // Konversi BindingList ke List
                    SimpanDataKeCSV(dataList, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

                    // Perbarui tampilan DataGridView
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = bindingList;
                }
                else
                {
                    MessageBox.Show("Tidak ada data dengan NIP yang dimasukkan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ResetFormMode();
                UpdateNoValues();
            }
            else if (result == DialogResult.No)
            {
                // Set the editing flag to true
                isEditing = false;
                txt_NipDokter.Enabled = true;
                // Clear the TextBoxes after deletion
                ClearTextBoxes();
            }
            btn_Save.Enabled = true;
            DisableButtons();
            // Clear the TextBoxes after deletion
            ClearTextBoxes();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Ambil NIP baru dari TextBox
            string newNIP = txt_NipDokter.Text;

            // Iterasi melalui setiap baris dalam DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Ambil NIP dari baris saat ini
                string currentNIP = row.Cells["NIP"].Value.ToString();

                // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
                if (currentNIP == newNIP)
                {
                    // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
                    row.Cells["Nama"].Value = txt_NamaDokter.Text;
                    row.Cells["Alamat"].Value = txt_AlamatDokter.Text;

                    // Perbarui nilai jenis kelamin berdasarkan radio button
                    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
                    row.Cells["JenisKelamin"].Value = jenisKelamin;

                    ResetFormMode();

                    // Tampilkan pesan keberhasilan
                    MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Simpan data ke CSV setelah perubahan
                    List<Dokter> data = GetDataFromDataGridView(dataGridView1);
                    SimpanDataKeCSV(data, @"D:\dokter.csv");

                    // Keluar dari iterasi setelah menemukan NIP yang cocok
                    return;

                }
            }

            // Jika tidak ada baris dengan NIP yang sesuai
            MessageBox.Show("Tidak ada data dengan NIP yang dimasukkan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        private void FormDokter_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent?.Invoke();
        }

        private void txt_NamaDokter_TextChanged(object sender, EventArgs e)
        {
            int maxLength = 45;

            if (txt_NamaDokter.Text.Length > maxLength)
            {
                lbl_error.Visible = true;  // Tampilkan label
                lbl_error.Text = "*Maksimal 33 karakter!";  // Ubah teks label
            }
            else
            {
                lbl_error.Visible = false;  // Sembunyikan label jika tidak ada error
            }
        }


        private List<Dokter> GetDataFromDataGridView(DataGridView dataGridView)
        {
            List<Dokter> data = new List<Dokter>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string nip = row.Cells["NIP"].Value.ToString();
                string nama = row.Cells["Nama"].Value.ToString();
                string jenisKelamin = row.Cells["JenisKelamin"].Value.ToString();
                string alamat = row.Cells["Alamat"].Value.ToString();

                Dokter dokter = new Dokter
                {
                    NIP = nip,
                    Nama = nama,
                    JenisKelamin = jenisKelamin,
                    Alamat = alamat
                };

                data.Add(dokter);
            }

            return data;
        }

        private void SimpanDataKeCSV(List<Dokter> data, string filePath)
        {
            // Perbarui nomor urut (No) untuk setiap item dalam list
            for (int i = 0; i < data.Count; i++)
            {
                data[i].No = i + 1; // Atur nilai No sesuai dengan urutan dalam list
            }

            // Tulis data ke file CSV menggunakan CsvWriter
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                // Tulis header jika perlu (untuk versi CsvHelper yang lebih lama)
                csv.WriteHeader<Dokter>();

                // Tulis data dari objek List<Dokter> ke file CSV
                csv.WriteRecords(data);
            }
        }


        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    isEditing = false;

        //    // Ambil NIP baru dari TextBox
        //    string newNIP = txt_NipDokter.Text.Trim();

        //    // Validasi setiap entri data
        //    if (string.IsNullOrEmpty(newNIP))
        //    {
        //        MessageBox.Show("NIP harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string nama = RemoveExtraSpaces(txt_NamaDokter.Text);
        //    if (string.IsNullOrEmpty(nama))
        //    {
        //        MessageBox.Show("Nama harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string alamat = RemoveExtraSpaces(txt_AlamatDokter.Text);
        //    if (string.IsNullOrEmpty(alamat))
        //    {
        //        MessageBox.Show("Alamat harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //    if (!radioButtonPria.Checked && !radioButtonWanita.Checked)
        //    {
        //        MessageBox.Show("Jenis Kelamin harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Iterasi melalui setiap baris dalam DataGridView
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        // Ambil NIP dari baris saat ini
        //        string currentNIP = row.Cells["NIP"].Value.ToString();

        //        // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
        //        if (currentNIP == newNIP)
        //        {
        //            // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
        //            row.Cells["Nama"].Value = nama;
        //            row.Cells["Alamat"].Value = alamat;
        //            row.Cells["JenisKelamin"].Value = jenisKelamin;

        //            ResetFormMode();
        //            DisableButtons();
        //            // Tampilkan pesan keberhasilan
        //            MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Simpan data ke CSV setelah perubahan
        //            List<Dokter> data = GetDataFromDataGridView(dataGridView1);
        //            SimpanDataKeCSV(data, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

        //            // Keluar dari iterasi setelah menemukan NIP yang cocok
        //            return;
        //        }
        //    }

        //    // Jika tidak ditemukan NIP yang cocok, tampilkan pesan
        //    MessageBox.Show("NIP tidak ditemukan dalam data. Silakan periksa kembali NIP yang Anda masukkan.", "NIP Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //}

        private string RemoveExtraSpaces(string input)
        {
            return string.Join(" ", input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            // Validasi setiap entri data
            string nip = txt_NipDokter.Text.Trim();
            if (string.IsNullOrEmpty(nip))
            {
                MessageBox.Show("NIP harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string namaDokter = RemoveExtraSpaces(txt_NamaDokter.Text);
            if (string.IsNullOrEmpty(namaDokter))
            {
                MessageBox.Show("Nama Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string alamatDokter = RemoveExtraSpaces(txt_AlamatDokter.Text);
            if (string.IsNullOrEmpty(alamatDokter))
            {
                MessageBox.Show("Alamat Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : (radioButtonWanita.Checked ? "Perempuan" : "");
            if (string.IsNullOrEmpty(jenisKelamin))
            {
                MessageBox.Show("Jenis Kelamin harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = false;

            // Iterasi melalui setiap baris dalam DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Ambil NIP dari baris saat ini
                string currentNIP = row.Cells["NIP"].Value.ToString();

                // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
                if (currentNIP == nip)
                {
                    // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
                    row.Cells["Nama"].Value = namaDokter;
                    row.Cells["Alamat"].Value = alamatDokter;
                    row.Cells["JenisKelamin"].Value = jenisKelamin;

                    // Perbarui nilai di BindingList
                    Dokter dokterToUpdate = bindingList.FirstOrDefault(d => d.NIP == nip);
                    if (dokterToUpdate != null)
                    {
                        dokterToUpdate.Nama = namaDokter;
                        dokterToUpdate.Alamat = alamatDokter;
                        dokterToUpdate.JenisKelamin = jenisKelamin;
                    }

                    // Simpan data ke CSV setelah perubahan
                    List<Dokter> data = bindingList.ToList();
                    SimpanDataKeCSV(data, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

                    // Tampilkan pesan keberhasilan
                    MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetFormMode();
                    DisableButtons();

                    // Keluar dari iterasi setelah menemukan NIP yang cocok
                    return;
                }
            }

            // Jika tidak ada NIP yang cocok, tampilkan pesan kesalahan
            MessageBox.Show("NIP tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    // Validasi setiap entri data
        //    string nip = txt_NipDokter.Text.Trim();
        //    if (string.IsNullOrEmpty(nip))
        //    {
        //        MessageBox.Show("NIP harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string namaDokter = RemoveExtraSpaces(txt_NamaDokter.Text);
        //    if (string.IsNullOrEmpty(namaDokter))
        //    {
        //        MessageBox.Show("Nama Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string alamatDokter = RemoveExtraSpaces(txt_AlamatDokter.Text);
        //    if (string.IsNullOrEmpty(alamatDokter))
        //    {
        //        MessageBox.Show("Alamat Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : (radioButtonWanita.Checked ? "Perempuan" : "");
        //    if (string.IsNullOrEmpty(jenisKelamin))
        //    {
        //        MessageBox.Show("Jenis Kelamin harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    isEditing = false;

        //    // Iterasi melalui setiap baris dalam DataGridView
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        // Ambil NIP dari baris saat ini
        //        string currentNIP = row.Cells["NIP"].Value.ToString();

        //        // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
        //        if (currentNIP == nip)
        //        {
        //            // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
        //            row.Cells["Nama"].Value = namaDokter;
        //            row.Cells["Alamat"].Value = alamatDokter;
        //            row.Cells["JenisKelamin"].Value = jenisKelamin;

        //            ResetFormMode();
        //            DisableButtons();

        //            // Tampilkan pesan keberhasilan
        //            MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Simpan data ke CSV setelah perubahan
        //            List<Dokter> data = GetDataFromDataGridView(dataGridView1);
        //            SimpanDataKeCSV(data, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

        //            // Keluar dari iterasi setelah menemukan NIP yang cocok
        //            return;
        //        }
        //    }

        //    // Jika tidak ada NIP yang cocok, tampilkan pesan kesalahan
        //    MessageBox.Show("NIP tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}


        //tanpa required
        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    isEditing = false;

        //    // Ambil NIP baru dari TextBox
        //    string newNIP = txt_NipDokter.Text.Trim();

        //    // Iterasi melalui setiap baris dalam DataGridView
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        // Ambil NIP dari baris saat ini
        //        string currentNIP = row.Cells["NIP"].Value.ToString();

        //        // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
        //        if (currentNIP == newNIP)
        //        {
        //            // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
        //            row.Cells["Nama"].Value = RemoveExtraSpaces(txt_NamaDokter.Text);
        //            row.Cells["Alamat"].Value = RemoveExtraSpaces(txt_AlamatDokter.Text);

        //            // Perbarui nilai jenis kelamin berdasarkan radio button
        //            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //            row.Cells["JenisKelamin"].Value = jenisKelamin;

        //            ResetFormMode();
        //            DisableButtons();
        //            // Tampilkan pesan keberhasilan
        //            MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Simpan data ke CSV setelah perubahan
        //            List<Dokter> data = GetDataFromDataGridView(dataGridView1);
        //            SimpanDataKeCSV(data, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

        //            // Keluar dari iterasi setelah menemukan NIP yang cocok
        //            return;
        //        }
        //    }
        //}


        //tanpa menggunakan Trim
        //private void button5_Click_1(object sender, EventArgs e)
        //{

        //    isEditing = false;
        //    // Ambil NIP baru dari TextBox
        //    string newNIP = txt_NipDokter.Text;

        //    // Iterasi melalui setiap baris dalam DataGridView
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        // Ambil NIP dari baris saat ini
        //        string currentNIP = row.Cells["NIP"].Value.ToString();

        //        // Jika NIP di baris saat ini cocok dengan NIP yang baru dimasukkan pengguna
        //        if (currentNIP == newNIP)
        //        {
        //            // Perbarui nilai di DataGridView berdasarkan nilai dalam TextBox dan RadioButton
        //            row.Cells["Nama"].Value = txt_NamaDokter.Text;
        //            row.Cells["Alamat"].Value = txt_AlamatDokter.Text;

        //            // Perbarui nilai jenis kelamin berdasarkan radio button
        //            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //            row.Cells["JenisKelamin"].Value = jenisKelamin;

        //            ResetFormMode();
        //            DisableButtons();
        //            // Tampilkan pesan keberhasilan
        //            MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Simpan data ke CSV setelah perubahan
        //            List<Dokter> data = GetDataFromDataGridView(dataGridView1);
        //            SimpanDataKeCSV(data, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");

        //            // Keluar dari iterasi setelah menemukan NIP yang cocok

        //            return;
        //        }
        //    }



        //}

        private void btn_Save_Click(object sender, EventArgs e)
        {
            // Ambil data dari kontrol UI dengan Trim dan RemoveExtraSpaces
            string nip = txt_NipDokter.Text.Trim();
            string nama = RemoveExtraSpaces(txt_NamaDokter.Text);
            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : (radioButtonWanita.Checked ? "Perempuan" : "");
            string alamat = RemoveExtraSpaces(txt_AlamatDokter.Text);

            // Periksa apakah semua data telah diisi
            if (!IsDataValid(nip, nama, jenisKelamin, alamat))
            {
                MessageBox.Show("Harap lengkapi semua data sebelum menyimpan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Hentikan proses penyimpanan jika data tidak valid
            }

            // Periksa apakah NIP sudah ada di DataGridView
            if (IsNIPAlreadyExistsSimpan(nip))
            {
                MessageBox.Show("NIP sudah ada di dalam DataGridView. Harap masukkan NIP yang berbeda.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hentikan proses penyimpanan jika NIP sudah ada
            }

            // Simpan data ke file CSV


            bindingList.Add(new Dokter
            {

                No = bindingList.Count + 1,
                NIP = nip,
                Nama = nama,
                JenisKelamin = jenisKelamin,
                Alamat = alamat
            });

            // Simpan data ke file CSV setelah penghapusan selesai
            List<Dokter> dataList = bindingList.ToList(); // Konversi BindingList ke List
            SimpanDataKeCSV(dataList, @"D:\GLEndoscope\Database\dataDokter\namaDokter.csv");


            //SimpanDataKeCSV(nip, nama, jenisKelamin, alamat);

            // Tampilkan data di DataGridView setelah simpan
            TampilkanDataKeDataGridView();

            // Bersihkan kontrol setelah menyimpan
            txt_NipDokter.Clear();
            txt_NamaDokter.Clear();
            radioButtonPria.Checked = false;
            radioButtonWanita.Checked = false;
            txt_AlamatDokter.Clear();

            // Clear the DataTable (assuming it's the data source)
            if (dataTable != null)
            {
                dataTable.Rows.Clear();
            }

            // Call the event to notify Form1 that Form2 is closed
            FormClosedEvent?.Invoke();

            // Kembalikan kontrol NIPDokter ke keadaan aktif
            txt_NipDokter.Enabled = true;
        }

        private bool IsDataValid(string nip, string nama, string jenisKelamin, string alamat)
        {
            if (string.IsNullOrEmpty(nip))
            {
                MessageBox.Show("NIP harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(nama))
            {
                MessageBox.Show("Nama Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(jenisKelamin))
            {
                MessageBox.Show("Jenis Kelamin harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(alamat))
            {
                MessageBox.Show("Alamat Dokter harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        //tanpa remove spaces
        //private void btn_Save_Click(object sender, EventArgs e)
        //    {
        //        // Ambil data dari kontrol UI
        //        string nip = txt_NipDokter.Text;
        //        string nama = txt_NamaDokter.Text;
        //        string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //        string alamat = txt_AlamatDokter.Text;

        //        // Periksa apakah semua data telah diisi
        //        if (!IsDataValid(nip, nama, jenisKelamin, alamat))
        //        {
        //            return; // Hentikan proses penyimpanan jika data tidak valid
        //        }

        //        // Periksa apakah NIP sudah ada di DataGridView
        //        if (IsNIPAlreadyExistsSimpan(nip))
        //        {
        //            MessageBox.Show("NIP sudah ada di dalam DataGridView. Harap masukkan NIP yang berbeda.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return; // Hentikan proses penyimpanan jika NIP sudah ada
        //        }

        //        // Simpan data ke file CSV
        //        SimpanDataKeCSV(nip, nama, jenisKelamin, alamat);

        //        bindingList.Add(new Dokter { NIP = nip, Nama = nama, JenisKelamin = jenisKelamin, Alamat = alamat });

        //        // Tampilkan data di DataGridView setelah simpan
        //        TampilkanDataKeDataGridView();

        //        // Bersihkan kontrol setelah menyimpan
        //        txt_NipDokter.Clear();
        //        txt_NamaDokter.Clear();
        //        radioButtonPria.Checked = false;
        //        radioButtonWanita.Checked = false;
        //        txt_AlamatDokter.Clear();

        //        // Clear the DataTable (assuming it's the data source)
        //        if (dataTable != null)
        //        {
        //            dataTable.Rows.Clear();
        //        }

        //        // Call the event to notify Form1 that Form2 is closed
        //        FormClosedEvent?.Invoke();

        //        // Kembalikan kontrol NIPDokter ke keadaan aktif
        //        txt_NipDokter.Enabled = true;
        //    }


        //tanpa menggunakan Trim
        //private void btn_Save_Click(object sender, EventArgs e)
        //{
        //    // Ambil data dari kontrol UI
        //    string nip = txt_NipDokter.Text;
        //    string nama = txt_NamaDokter.Text;
        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //    string alamat = txt_AlamatDokter.Text;

        //    // Periksa apakah semua data telah diisi
        //    if (!IsDataValid(nip, nama, jenisKelamin, alamat))
        //    {
        //        return; // Hentikan proses penyimpanan jika data tidak valid
        //    }

        //    // Periksa apakah NIP sudah ada di DataGridView
        //    if (IsNIPAlreadyExistsSimpan(nip))
        //    {
        //        MessageBox.Show("NIP sudah ada di dalam DataGridView. Harap masukkan NIP yang berbeda.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return; // Hentikan proses penyimpanan jika NIP sudah ada
        //    }

        //    // Simpan data ke file CSV
        //    SimpanDataKeCSV(nip, nama, jenisKelamin, alamat);

        //    bindingList.Add(new Dokter { NIP = nip, Nama = nama, JenisKelamin = jenisKelamin, Alamat = alamat });

        //    // Tampilkan data di DataGridView setelah simpan
        //    TampilkanDataKeDataGridView();

        //    // Bersihkan kontrol setelah menyimpan
        //    txt_NipDokter.Clear();
        //    txt_NamaDokter.Clear();
        //    radioButtonPria.Checked = false;
        //    radioButtonWanita.Checked = false;
        //    txt_AlamatDokter.Clear();

        //    // Clear the DataTable (assuming it's the data source)
        //    if (dataTable != null)
        //    {
        //        dataTable.Rows.Clear();
        //    }

        //    // Call the event to notify Form1 that Form2 is closed
        //    FormClosedEvent?.Invoke();

        //    // Kembalikan kontrol NIPDokter ke keadaan aktif
        //    txt_NipDokter.Enabled = true;
        //}
    }
}