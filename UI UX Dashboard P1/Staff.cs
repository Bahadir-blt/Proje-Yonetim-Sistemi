using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_UX_Dashboard_P1
{
    public partial class Staff : Form
    {
        string connStr = @"Server=.\SQLEXPRESS;Database=ProjeYonetimSistemi;Integrated Security=true;";
        public Staff()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            PersonelListele();
        }
        private void SetupGrid()
        {
            dataGridView1.Columns.Clear();

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.HeaderText = "Fotoğraf";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns.Add(imgCol);

            dataGridView1.Columns.Add("Ad", "Ad");
            dataGridView1.Columns.Add("Soyad", "Soyad");
            dataGridView1.Columns.Add("Statu", "Statü");

            dataGridView1.RowTemplate.Height = 70;
            dataGridView1.AllowUserToAddRows = false;
        }

        public void PersonelListele()
        {
            dataGridView1.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT PersonelID, Ad, Soyad, Statu, Foto FROM Personel", conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Image img = null;
                    if (row["Foto"] != DBNull.Value)
                    {
                        byte[] bytes = (byte[])row["Foto"];
                        img = ByteArrayToImage(bytes);
                    }

                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = img;
                    dataGridView1.Rows[index].Cells[1].Value = row["Ad"].ToString();
                    dataGridView1.Rows[index].Cells[2].Value = row["Soyad"].ToString();
                    dataGridView1.Rows[index].Cells[3].Value = row["Statu"].ToString();
                }
            }
        }

        public Image ByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void btnPersonelEkle_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnPersonelEkle_Click_1(object sender, EventArgs e)
        {
            FrmPersonelEkle ekle = new FrmPersonelEkle();
            ekle.FormClosed += (s, args) => { PersonelListele(); }; // ekleme sonrasında yeniden listele
            ekle.ShowDialog();
        }
    }
}
