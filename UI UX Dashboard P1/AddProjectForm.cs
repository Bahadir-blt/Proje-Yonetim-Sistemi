using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UI_UX_Dashboard_P1
{
    public partial class AddProjectForm : Form
    {
        private string connectionString;

        public event EventHandler ProjectSaved;

        public AddProjectForm(string connStr)
        {
            InitializeComponent();
            connectionString = connStr;
            BuildForm();
        }

        private TextBox txtName;
        private TextBox txtOwner;
        private ComboBox cmbStatus;
        private TextBox txtDoc;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;

        private void BuildForm()
        {
            this.Text = "Yeni Proje Ekle";
            this.Width = 400;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label() { Text = "Proje Adı:", Left = 20, Top = 30 };
            txtName = new TextBox() { Left = 150, Top = 25, Width = 200 };

            Label lblOwner = new Label() { Text = "Sahibi:", Left = 20, Top = 70 };
            txtOwner = new TextBox() { Left = 150, Top = 65, Width = 200 };

            Label lblStatus = new Label() { Text = "Durum:", Left = 20, Top = 110 };
            cmbStatus = new ComboBox() { Left = 150, Top = 105, Width = 200 };
            cmbStatus.Items.AddRange(new[] { "Beklemede", "Devam ediyor", "Tamamlandı" });
            cmbStatus.SelectedIndex = 1;

            Label lblDoc = new Label() { Text = "Döküman (URL):", Left = 20, Top = 150 };
            txtDoc = new TextBox() { Left = 150, Top = 145, Width = 200 };

            Label lblStart = new Label() { Text = "Başlangıç Tarihi:", Left = 20, Top = 190 };
            dtpStart = new DateTimePicker() { Left = 150, Top = 185, Width = 200, Format = DateTimePickerFormat.Short };

            Label lblEnd = new Label() { Text = "Bitiş Tarihi:", Left = 20, Top = 230 };
            dtpEnd = new DateTimePicker() { Left = 150, Top = 225, Width = 200, Format = DateTimePickerFormat.Short };

            Button btnSave = new Button() { Text = "Kaydet", Left = 150, Top = 280, Width = 100 };
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button() { Text = "İptal", Left = 260, Top = 280, Width = 100 };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblName, txtName,
                lblOwner, txtOwner,
                lblStatus, cmbStatus,
                lblDoc, txtDoc,
                lblStart, dtpStart,
                lblEnd, dtpEnd,
                btnSave, btnCancel
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Proje adı zorunludur!");
                return;
            }

            int days = (int)(dtpEnd.Value - dtpStart.Value).TotalDays;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Projects (ProjectName, Owner, Status, Document, StartDate, CompletionDate, Days)
                        VALUES (@name, @owner, @status, @doc, @start, @end, @days)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@owner", txtOwner.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@doc", txtDoc.Text);
                    cmd.Parameters.AddWithValue("@start", dtpStart.Value);
                    cmd.Parameters.AddWithValue("@end", dtpEnd.Value);
                    cmd.Parameters.AddWithValue("@days", days);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Proje başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProjectSaved?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
