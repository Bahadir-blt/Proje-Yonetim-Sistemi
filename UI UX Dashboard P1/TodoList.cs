using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UI_UX_Dashboard_P1
{
    public partial class TodoList : Form
    {
        public TodoList()
        {
            InitializeComponent();
        }
        private string Path = Application.StartupPath + @"\data.xml";
        private List<ToDoItem> Gorevlerim = new List<ToDoItem>();
        private MyXmlSerializer Serializer = new MyXmlSerializer();

        private void ListeleriDoldur()
        {
            this.lstTamamlananlarListesi.Items.Clear();
            this.clbYapilacaklarListesi.Items.Clear();

            foreach (ToDoItem gorev in Gorevlerim)
            {
                if (gorev.Tamamlandi == false)
                    this.clbYapilacaklarListesi.Items.Add(gorev);
                else
                   this.lstTamamlananlarListesi.Items.Add(gorev);
            }
        }
        private void YapilacaklarListesiKaydet()
        {
            Serializer.Serialize<List<ToDoItem>>(Path, this.Gorevlerim);
        }
        private void YapilacaklarListesiOku()
        {
            this.Gorevlerim = Serializer.Deserialize<List<ToDoItem>>(Path);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void TodoList_Load(object sender, EventArgs e)
        {
            txtYeniGorev.Text = string.Empty;

            if (System.IO.File.Exists(Path))
            {
                this.YapilacaklarListesiOku();
            }

            this.ListeleriDoldur();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            ToDoItem yeniGorev = new ToDoItem()
            {
                Id = Guid.NewGuid(),
                GorevMetni = txtYeniGorev.Text,
                Tamamlandi = false,
            };
            this.Gorevlerim.Add(yeniGorev);
            this.ListeleriDoldur();
            this.YapilacaklarListesiKaydet();
            //this.clbYapilacaklarListesi.Items.Add(yeniGorev);
        }

        private void btnDuzelt_Click(object sender, EventArgs e)
        {
            if (clbYapilacaklarListesi.SelectedIndex == -1)
            {
               MessageBox.Show("Lütfen düzeltme işlemi için bir görev seçiniz.");
               return;
            }

            ToDoItem gorev = (ToDoItem)clbYapilacaklarListesi.SelectedItem;
            gorev.GorevMetni = txtYeniGorev.Text;
            this.ListeleriDoldur();
            this.YapilacaklarListesiKaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (clbYapilacaklarListesi.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen silme işlemi için bir görev seçiniz.");
                return;
            }

            this.Gorevlerim.Remove((ToDoItem)clbYapilacaklarListesi.SelectedItem);
            //clbYapilacaklarListesi.Items.Remove(clbYapilacaklarListesi.SelectedItem);

            this.ListeleriDoldur();
            this.YapilacaklarListesiKaydet();
        }

        private void clbYapilacaklarListesi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clbYapilacaklarListesi.SelectedIndex == -1)
            return;

            ToDoItem gorev = (ToDoItem)clbYapilacaklarListesi.SelectedItem;
            txtYeniGorev.Text = gorev.GorevMetni;
        }

        private void btnKes_Click(object sender, EventArgs e)
        {
            txtYeniGorev.Cut();
        }

        private void btnKopyala_Click(object sender, EventArgs e)
        {
            txtYeniGorev.Copy();
        }

        private void btnYapistir_Click(object sender, EventArgs e)
        {
            txtYeniGorev.Paste();
        }

        private void clbYapilacaklarListesi_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
              ToDoItem gorev = (ToDoItem)clbYapilacaklarListesi.SelectedItem;
                gorev.Tamamlandi = true;
                gorev.TamamlanmaTarihi = DateTime.Now;
                this.YapilacaklarListesiKaydet();
            }
        }

        private void clbYapilacaklarListesi_MouseUp(object sender, MouseEventArgs e)
        {
            if (clbYapilacaklarListesi.CheckedItems.Count > 0)
                this.ListeleriDoldur();
        }

        private void mnuCikis_Click(object sender, EventArgs e)
        {
            Form1 login1 = new Form1(); // Login ekranını yeniden oluştur
            login1.Show();                      // Göster
            this.Close();
        }

        private void Logout_Click(object sender, EventArgs e)
        {

        }
    }
}
