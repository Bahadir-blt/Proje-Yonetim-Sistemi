using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace UI_UX_Dashboard_P1
{
    public partial class Projects : Form
    {
        private string connectionString = "Server=.\\SQLEXPRESS;Database=ProjeYonetimSistemi;Trusted_Connection=True;";

        private TextBox txtAra;
        private ComboBox cmbDurum;
        private ComboBox cmbCalisan;
        private Button btnEkle;
        private Button btnSil;
        private Button btnAnaSayfa;
        private DataGridView dgvProjeler;

        // 🔸 Ana sayfaya dön için event
        public event EventHandler GeriDonuldu;

        public Projects()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // 🔹 Üst Panel
            Panel topPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.WhiteSmoke
            };

            txtAra = new TextBox()
            {
                Text = "Ara...",
                ForeColor = Color.Gray,
                Width = 180,
                Location = new Point(10, 12)
            };

            // Placeholder davranışı
            txtAra.GotFocus += (s, e) =>
            {
                if (txtAra.Text == "Ara...")
                {
                    txtAra.Text = "";
                    txtAra.ForeColor = Color.Black;
                }
            };

            txtAra.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAra.Text))
                {
                    txtAra.Text = "Ara...";
                    txtAra.ForeColor = Color.Gray;
                }
            };


            Button btnAra = new Button()
            {
                Text = "🔍",
                Location = new Point(200, 10),
                Width = 35
            };
            btnAra.Click += (s, e) => ProjeYukle();

            cmbDurum = new ComboBox()
            {
                Location = new Point(250, 12),
                Width = 150
            };
            cmbDurum.Items.AddRange(new[] { "Durum (Tümü)", "Beklemede", "Devam Ediyor", "Tamamlandı" });
            cmbDurum.SelectedIndex = 0;

            cmbCalisan = new ComboBox()
            {
                Location = new Point(410, 12),
                Width = 150
            };
            cmbCalisan.Items.AddRange(new[] { "Çalışan (Tümü)", "Ahmet Aslan", "Diğer" });
            cmbCalisan.SelectedIndex = 0;

            btnSil = new Button()
            {
                Text = "SİL",
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                Location = new Point(580, 10),
                Width = 90 ,
                Height = 40
            };
            btnSil.Click += BtnSil_Click;

            btnEkle = new Button()
            {
                Text = "EKLE",
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                Location = new Point(680, 10),
                Width = 90 ,
                Height = 40
            };
            btnEkle.Click += BtnEkle_Click;

            btnAnaSayfa = new Button()
            {
                Text = "ANA SAYFAYA DÖN",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Location = new Point(780, 10),
                Width = 150 ,
                 Height = 40
            };
            btnAnaSayfa.Click += BtnAnaSayfa_Click;

            topPanel.Controls.AddRange(new Control[]
            {
                txtAra, btnAra, cmbDurum, cmbCalisan, btnSil, btnEkle, btnAnaSayfa
            });

            // 🔹 DataGridView
            dgvProjeler = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvProjeler.Columns.Add("Id", "ID");
            dgvProjeler.Columns.Add("ProjectName", "Proje Adı");
            dgvProjeler.Columns.Add("Owner", "Sahibi");
            dgvProjeler.Columns.Add("Status", "Durum");
            dgvProjeler.Columns.Add("Document", "Döküman");
            dgvProjeler.Columns.Add("StartDate", "Başlangıç Tarihi");
            dgvProjeler.Columns.Add("CompletionDate", "Bitiş Tarihi");
            dgvProjeler.Columns.Add("Days", "Gün Sayısı");

            this.Controls.Add(dgvProjeler);
            this.Controls.Add(topPanel);

            this.Load += Projects_Load;
        }

        private void Projects_Load(object sender, EventArgs e)
        {
            ProjeYukle();
        }

        private void ProjeYukle()
        {
            dgvProjeler.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Projects";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                // ... kodun devamı
            

            while (rdr.Read())
                {
                    dgvProjeler.Rows.Add(
                        rdr["Id"],
                        rdr["ProjectName"],
                        rdr["Owner"],
                        rdr["Status"],
                        rdr["Document"],
                        ((DateTime)rdr["StartDate"]).ToShortDateString(),
                        ((DateTime)rdr["CompletionDate"]).ToShortDateString(),
                        rdr["Days"]
                    );
                }
            }
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            AddProjectForm frmEkle = new AddProjectForm(connectionString);
            frmEkle.ProjectSaved += (s, ev) => ProjeYukle();
            frmEkle.ShowDialog();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvProjeler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek projeyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvProjeler.SelectedRows[0].Cells["Id"].Value);

            if (MessageBox.Show("Bu projeyi silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Projects WHERE Id=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Proje başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProjeYukle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
        }

        private void BtnAnaSayfa_Click(object sender, EventArgs e)
        {
            // 🔹 Eğer ana sayfa bir üst formda (örneğin MainForm içinde) açılıyorsa:
            GeriDonuldu?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
