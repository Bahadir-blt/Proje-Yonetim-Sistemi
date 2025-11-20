using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace UI_UX_Dashboard_P1
{
    public partial class LoginPage : Form
    {
        SqlConnection con;
        SqlDataReader dr;
        SqlCommand com;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string password = textBox2.Text;
            con = new SqlConnection(@"Server=.\SQLEXPRESS; Database=ProjeYonetimSistemi; Integrated Security=True; TrustServerCertificate=True;");

            com = new SqlCommand();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Login WHERE kullanici_adi = @user AND sifre = @pass";
            com.Parameters.AddWithValue("@user", textBox1.Text);
            com.Parameters.AddWithValue("@pass", textBox2.Text);


            dr = com.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Giriş Başarılı");
                Form1 gecis = new Form1();
                gecis.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı Adı Veya Şifre");
            }
            con.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
