using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data; 

namespace UI_UX_Dashboard_P1
{
    public partial class Form1 : Form
    {
     
        private const string ConnectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=ProjeYonetimSistemi; Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDashboardData();

        }
        private void LoadDashboardData()
        {
            string query = @"
                SELECT
                    ToplamSiparisSayisi, YeniSiparisSayisi,
                    ToplamZiyaretciSayisi, OtlumCikisOrani,
                    ToplamKullaniciSayisi, YeniKullaniciSayisi,
                    ToplamAlertSayisi, YeniAlertSayisi
                FROM DashboardIstatistikleri
                WHERE ID = 1;
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
       
                            label5.Text = reader["ToplamSiparisSayisi"].ToString();
                            label6.Text = reader["YeniSiparisSayisi"].ToString() + " YENİ SİPARİŞ";

             
                            label8.Text = reader["ToplamZiyaretciSayisi"].ToString();

                            label7.Text = "Öne Çıkma Oranı %" + Convert.ToDecimal(reader["OtlumCikisOrani"]).ToString("N2");

                   
                            label11.Text = reader["ToplamKullaniciSayisi"].ToString();
                            label10.Text = reader["YeniKullaniciSayisi"].ToString() + " YENİ KULLANICI";


                            label14.Text = reader["ToplamAlertSayisi"].ToString();
                            label13.Text = reader["YeniAlertSayisi"].ToString() + " YENİ BİLDİRİM";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken bir hata oluştu. Bağlantı dizesini ve SQL Server durumunu kontrol edin.\n\nHata: " + ex.Message, "SQL Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e) { /* ... */ }
        private void panel6_Paint(object sender, PaintEventArgs e) { /* ... */ }
        private void panel8_Paint(object sender, PaintEventArgs e) { /* ... */ }
        private void label5_Click(object sender, EventArgs e) { /* ... */ }
        private void label9_Click(object sender, EventArgs e) { /* ... */ }
        private void label12_Click(object sender, EventArgs e) { /* ... */ }

        private void button6_Click(object sender, EventArgs e)
        {
           Projects frm2gcs = new Projects();
            frm2gcs.Show();
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 frm1gcs = new Form1();
            frm1gcs.Show();

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            LoginPage login = new LoginPage(); // Login ekranını yeniden oluştur
            login.Show();                      // Göster
            this.Close();                      // Bu Form1'i kapat
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
           TodoList frm5gcs = new TodoList();
            frm5gcs.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
          Ekip frm3gcs = new Ekip();
            frm3gcs.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            RaporlarForm frm4gcs = new RaporlarForm();
            frm4gcs.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
        
        }
    }
}