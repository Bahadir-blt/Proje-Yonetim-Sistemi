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
    public partial class FrmPersonelEkle : Form
    {
        string connStr = @"Server=.\SQLEXPRESS;Database=ProjeYonetimSistemi;Integrated Security=true;";
        public FrmPersonelEkle()
        {
            InitializeComponent();
        }

        private void btnFotoYukle_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyası | *.jpg; *.jpeg; *.png; *.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            {
                string ad = txtAd.Text.Trim();
                string soyad = txtSoyad.Text.Trim();
                string statu = Statü.Text.Trim();
                byte[] foto = pictureBox1.Image != null ? ImageToByteArray(pictureBox1.Image) : null;

                if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad))
                {
                    MessageBox.Show("Ad ve Soyad zorunludur.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Personel (Ad, Soyad, Statu, Foto) VALUES (@ad, @soyad, @statu, @foto)", conn);
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@statu", string.IsNullOrEmpty(statu) ? (object)DBNull.Value : statu);
                    if (foto != null)
                        cmd.Parameters.AddWithValue("@foto", foto);
                    else
                        cmd.Parameters.AddWithValue("@foto", DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Personel kaydedildi.");
                this.Close(); // ekleme tamamlandığında formu kapat
            }

        }
        public byte[] ImageToByteArray(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
