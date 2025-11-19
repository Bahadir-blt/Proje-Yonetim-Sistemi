using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace UI_UX_Dashboard_P1
{
    public partial class RaporlarForm : Form
    {
        private string connStr = @"Server=.\SQLEXPRESS;Database=ProjeYonetimSistemi;Integrated Security=true;";

        // Yazdırma değişkenleri
        private PrintDocument printDocument = new PrintDocument();
        private DataTable printTable;
        private int currentRow = 0;

        public RaporlarForm()
        {
            InitializeComponent();

            this.Load += RaporlarForm_Load;

            // Yazıcı işlemleri
            printDocument.PrintPage += PrintDocument_PrintPage;
            btnPrint.Click += BtnCiktiAl_Click;

            // PDF butonu
            exportPdf.Click += btnPdfAktar_Click;
        }

        private void RaporlarForm_Load(object sender, EventArgs e)
        {
            cmbRaporTuru.Items.Add("Tüm Projeler");
            cmbRaporTuru.Items.Add("Aktif Projeler");
            cmbRaporTuru.Items.Add("Tamamlanan Projeler");
            cmbRaporTuru.Items.Add("Geciken Projeler");
            cmbRaporTuru.Items.Add("Teslim Tarihine Kalan Günler");

            cmbRaporTuru.SelectedIndex = 0;
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            string secim = cmbRaporTuru.SelectedItem.ToString();

            switch (secim)
            {
                case "Tüm Projeler": TumProjeleriGetir(); break;
                case "Aktif Projeler": AktifProjeleriGetir(); break;
                case "Tamamlanan Projeler": TamamlananProjeleriGetir(); break;
                case "Geciken Projeler": GecikenProjeleriGetir(); break;
                case "Teslim Tarihine Kalan Günler": KalanGunRaporu(); break;
            }
        }

        private void TumProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " proje bulundu.";
            }
        }

        private void AktifProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects WHERE Status='Aktif'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " aktif proje bulundu.";
            }
        }

        private void TamamlananProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects WHERE Status='Tamamlandı'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " tamamlanan proje bulundu.";
            }
        }

        private void GecikenProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                SELECT *, 
                DATEDIFF(day, CompletionDate, GETDATE()) AS GecikmeGun
                FROM Projects
                WHERE CompletionDate < GETDATE()
                AND Status!='Tamamlandı'";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " gecikmiş proje bulundu.";
            }
        }

        private void KalanGunRaporu()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                SELECT *,
                DATEDIFF(day, GETDATE(), CompletionDate) AS KalanGun
                FROM Projects
                ORDER BY KalanGun ASC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgRapor.DataSource = dt;
                lblDurum.Text = "Projeler kalan güne göre sıralandı.";
            }
        }


        // ============================================================
        //                       EXCEL AKTARMA
        // ============================================================
        private void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgRapor.DataSource == null)
                {
                    MessageBox.Show("Önce raporu listeleyin.");
                    return;
                }

                DataTable dt = dgRapor.DataSource as DataTable;
                if (dt == null) return;

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Dosyası | *.xlsx",
                    FileName = "Rapor.xlsx"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Rapor");
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Excel başarıyla oluşturuldu.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        // ============================================================
        //                      PDF AKTARMA
        // ============================================================
        private void btnPdfAktar_Click(object sender, EventArgs e)
        {
            if (dgRapor.DataSource == null)
            {
                MessageBox.Show("Önce raporu listeleyin.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Dosyası | *.pdf",
                FileName = "Rapor.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                doc.Open();

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font baslikFont = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font metinFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                // Başlık
                Paragraph baslik = new Paragraph("Güncel Rapor\n\n", baslikFont);
                baslik.Alignment = Element.ALIGN_CENTER;
                doc.Add(baslik);

                PdfPTable table = new PdfPTable(dgRapor.Columns.Count);
                table.WidthPercentage = 100;

                // Sütun Başlıkları
                foreach (DataGridViewColumn col in dgRapor.Columns)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(col.HeaderText, metinFont));
                    headerCell.BackgroundColor = new BaseColor(220, 220, 220);
                    table.AddCell(headerCell);
                }

                // Satırlar
                foreach (DataGridViewRow row in dgRapor.Rows)
                {
                    if (row.IsNewRow) continue;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        table.AddCell(new Phrase(cell.Value?.ToString(), metinFont));
                    }
                }

                doc.Add(table);
                doc.Close();

                MessageBox.Show("PDF başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF oluşturulurken hata: " + ex.Message);
            }
        }



        // ============================================================
        //                   YAZDIR (PRINTER)
        // ============================================================
        private void BtnCiktiAl_Click(object sender, EventArgs e)
        {
            if (dgRapor.DataSource == null)
            {
                MessageBox.Show("Önce raporu listeleyin.");
                return;
            }

            printTable = dgRapor.DataSource as DataTable;
            currentRow = 0;

            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument;

            if (pd.ShowDialog() == DialogResult.OK)
                printDocument.Print();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
            int x = 40;
            int y = 40;
            int lineHeight = 25;

            // Başlık
            foreach (DataColumn col in printTable.Columns)
            {
                e.Graphics.DrawString(col.ColumnName, new System.Drawing.Font("Arial", 9, FontStyle.Bold), Brushes.Black, x, y);
                x += 150;
            }

            y += lineHeight;
            x = 40;

            // Satırlar
            while (currentRow < printTable.Rows.Count)
            {
                foreach (DataColumn col in printTable.Columns)
                {
                    e.Graphics.DrawString(printTable.Rows[currentRow][col].ToString(), font, Brushes.Black, x, y);
                    x += 150;
                }

                x = 40;
                y += lineHeight;
                currentRow++;

                if (y > e.MarginBounds.Bottom - lineHeight)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }
    }
}

