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
            cmbRaporTuru.Items.Add("Beklemede");
            cmbRaporTuru.Items.Add("Devam Ediyor");
            cmbRaporTuru.Items.Add("Tamamlandı");

            cmbRaporTuru.SelectedIndex = 0;


        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            string secim = cmbRaporTuru.SelectedItem.ToString();

            switch (secim)
            {
                case "Tüm Projeler":
                    TumProjeleriGetir();
                    break;

                case "Beklemede":
                    BeklemedeProjeleriGetir();
                    break;

                case "Devam Ediyor":
                    DevamEdenProjeleriGetir();
                    break;

                case "Tamamlandı":
                    TamamlananProjeleriGetir();
                    break;
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

        private void BeklemedeProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects WHERE Status='Beklemede'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " bekleyen proje bulundu.";
            }
        }

        private void DevamEdenProjeleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects WHERE Status='Devam Ediyor'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgRapor.DataSource = dt;
                lblDurum.Text = dt.Rows.Count + " devam eden proje bulundu.";
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

        private void RaporlarForm_Load_1(object sender, EventArgs e)
        {

        }

        private void cmbRaporTuru_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

