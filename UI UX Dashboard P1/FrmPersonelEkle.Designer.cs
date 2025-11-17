namespace UI_UX_Dashboard_P1
{
    partial class FrmPersonelEkle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAd = new System.Windows.Forms.TextBox();
            this.txtSoyad = new System.Windows.Forms.TextBox();
            this.Statü = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnFotoYukle = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAd
            // 
            this.txtAd.Location = new System.Drawing.Point(46, 49);
            this.txtAd.Multiline = true;
            this.txtAd.Name = "txtAd";
            this.txtAd.Size = new System.Drawing.Size(151, 52);
            this.txtAd.TabIndex = 0;
            this.txtAd.Text = "Ad";
            // 
            // txtSoyad
            // 
            this.txtSoyad.Location = new System.Drawing.Point(46, 138);
            this.txtSoyad.Multiline = true;
            this.txtSoyad.Name = "txtSoyad";
            this.txtSoyad.Size = new System.Drawing.Size(151, 52);
            this.txtSoyad.TabIndex = 1;
            this.txtSoyad.Text = "Soyad";
            // 
            // Statü
            // 
            this.Statü.Location = new System.Drawing.Point(46, 226);
            this.Statü.Multiline = true;
            this.Statü.Name = "Statü";
            this.Statü.Size = new System.Drawing.Size(151, 52);
            this.Statü.TabIndex = 2;
            this.Statü.Text = "Statü";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(612, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(146, 86);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnFotoYukle
            // 
            this.btnFotoYukle.Location = new System.Drawing.Point(612, 171);
            this.btnFotoYukle.Name = "btnFotoYukle";
            this.btnFotoYukle.Size = new System.Drawing.Size(146, 76);
            this.btnFotoYukle.TabIndex = 4;
            this.btnFotoYukle.Text = "Fotoğraf Yükle";
            this.btnFotoYukle.UseVisualStyleBackColor = true;
            this.btnFotoYukle.Click += new System.EventHandler(this.btnFotoYukle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(89, 342);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(154, 60);
            this.btnKaydet.TabIndex = 5;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(288, 342);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(154, 60);
            this.btnIptal.TabIndex = 6;
            this.btnIptal.Text = "Çıkış";
            this.btnIptal.UseVisualStyleBackColor = true;
            // 
            // FrmPersonelEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.btnFotoYukle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Statü);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.txtAd);
            this.Name = "FrmPersonelEkle";
            this.Text = "FrmPersonelEkle";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAd;
        private System.Windows.Forms.TextBox txtSoyad;
        private System.Windows.Forms.TextBox Statü;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnFotoYukle;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
    }
}