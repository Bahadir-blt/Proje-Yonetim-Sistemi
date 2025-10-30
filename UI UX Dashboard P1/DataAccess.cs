using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms; // Hata mesajları için

public class DataAccess
{
    // LÜTFEN BURAYI KENDİ SQL BAĞLANTI DİZİNİZ İLE DEĞİŞTİRİN
    private readonly string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=ProjeYonetimSistemiI;User ID=KULLANICI_ADI;Password=SIFRE;";

    // Veri tabanından tüm projeleri çeker
    public DataTable ProjeleriYukle()
    {
        string query = "SELECT ProjeID, ProjeAdi, Sahip, Durum, BaslangicTarihi, BitisTarihi, TamamlanmaYuzdesi FROM Projeler ORDER BY ProjeID DESC";
        return ExecuteQuery(query, null);
    }

    // Proje Adına veya Sahip Adına göre arama yapar
    public DataTable ProjeAra(string aramaMetni)
    {
        string query = "SELECT * FROM Projeler WHERE ProjeAdi LIKE @AramaMetni OR Sahip LIKE @AramaMetni";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@AramaMetni", SqlDbType.NVarChar) { Value = "%" + aramaMetni + "%" }
        };

        return ExecuteQuery(query, parameters);
    }

    // Yeni proje ekler (CREATE)
    public bool ProjeEkle(string projeAdi, string sahip, string durum, DateTime baslangic, DateTime bitis, decimal yuzde)
    {
        string query = @"INSERT INTO Projeler (ProjeAdi, Sahip, Durum, BaslangicTarihi, BitisTarihi, TamamlanmaYuzdesi) 
                         VALUES (@ProjeAdi, @Sahip, @Durum, @BaslangicTarihi, @BitisTarihi, @TamamlanmaYuzdesi)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ProjeAdi", projeAdi),
            new SqlParameter("@Sahip", sahip),
            new SqlParameter("@Durum", durum),
            new SqlParameter("@BaslangicTarihi", baslangic),
            new SqlParameter("@BitisTarihi", bitis),
            new SqlParameter("@TamamlanmaYuzdesi", yuzde)
        };

        return ExecuteNonQuery(query, parameters);
    }

    // Seçilen projeyi siler (DELETE)
    public bool ProjeSil(int projeId)
    {
        string query = "DELETE FROM Projeler WHERE ProjeID = @ProjeID";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ProjeID", projeId)
        };

        return ExecuteNonQuery(query, parameters);
    }

    // --------------------------------------------------------------------------------
    // YARDIMCI METOTLAR: Tekrarlanan kodları sadeleştirmek için kullanılır
    // --------------------------------------------------------------------------------

    // SELECT sorguları için (veri okuma)
    private DataTable ExecuteQuery(string query, SqlParameter[] parameters)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Veritabanı okuma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return dt;
    }

    // INSERT/UPDATE/DELETE sorguları için (veri değiştirme)
    private bool ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Veritabanı yazma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }
}