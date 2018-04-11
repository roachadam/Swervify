namespace Swervify.UI
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.cbBanner = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbUAC = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVideos = new System.Windows.Forms.CheckBox();
            this.cbStartup = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lTrack = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lArtist = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUAC)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbBanner
            // 
            this.cbBanner.AutoCheck = false;
            this.cbBanner.AutoSize = true;
            this.cbBanner.Location = new System.Drawing.Point(25, 46);
            this.cbBanner.Name = "cbBanner";
            this.cbBanner.Size = new System.Drawing.Size(169, 17);
            this.cbBanner.TabIndex = 0;
            this.cbBanner.Text = "Block Spotify banner ads";
            this.toolTip.SetToolTip(this.cbBanner, "Enabling this feature will block the banner advertisements displayed in the Spoti" +
        "fy application.\r\nRequires Administrator");
            this.cbBanner.UseVisualStyleBackColor = true;
            this.cbBanner.CheckedChanged += new System.EventHandler(this.cbBanner_CheckedChanged);
            this.cbBanner.Click += new System.EventHandler(this.cbBanner_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbUAC);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbVideos);
            this.groupBox1.Controls.Add(this.cbStartup);
            this.groupBox1.Controls.Add(this.cbBanner);
            this.groupBox1.Location = new System.Drawing.Point(12, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 108);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // pbUAC
            // 
            this.pbUAC.Location = new System.Drawing.Point(189, 45);
            this.pbUAC.Name = "pbUAC";
            this.pbUAC.Size = new System.Drawing.Size(18, 18);
            this.pbUAC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbUAC.TabIndex = 4;
            this.pbUAC.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(180, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Note: Hover over each option for details";
            // 
            // cbVideos
            // 
            this.cbVideos.AutoSize = true;
            this.cbVideos.Location = new System.Drawing.Point(25, 69);
            this.cbVideos.Name = "cbVideos";
            this.cbVideos.Size = new System.Drawing.Size(144, 17);
            this.cbVideos.TabIndex = 2;
            this.cbVideos.Text = "Skip spotlight videos";
            this.toolTip.SetToolTip(this.cbVideos, "Enabling this feature will block all \'spotlight\' and music videos.");
            this.cbVideos.UseVisualStyleBackColor = true;
            this.cbVideos.CheckedChanged += new System.EventHandler(this.cbVideos_CheckedChanged);
            // 
            // cbStartup
            // 
            this.cbStartup.AutoSize = true;
            this.cbStartup.Location = new System.Drawing.Point(25, 23);
            this.cbStartup.Name = "cbStartup";
            this.cbStartup.Size = new System.Drawing.Size(226, 17);
            this.cbStartup.TabIndex = 1;
            this.cbStartup.Text = "Run Swervify when Windows starts";
            this.toolTip.SetToolTip(this.cbStartup, "Enabling this feature will automatically start Swervify when your computer boots " +
        "up.");
            this.cbStartup.UseVisualStyleBackColor = true;
            this.cbStartup.CheckedChanged += new System.EventHandler(this.cbStartup_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current Track:";
            // 
            // lTrack
            // 
            this.lTrack.AutoEllipsis = true;
            this.lTrack.Location = new System.Drawing.Point(119, 25);
            this.lTrack.Name = "lTrack";
            this.lTrack.Size = new System.Drawing.Size(294, 13);
            this.lTrack.TabIndex = 3;
            this.lTrack.Text = "None";
            this.lTrack.UseMnemonic = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Artist:";
            // 
            // lArtist
            // 
            this.lArtist.AutoEllipsis = true;
            this.lArtist.AutoSize = true;
            this.lArtist.Location = new System.Drawing.Point(119, 48);
            this.lArtist.Name = "lArtist";
            this.lArtist.Size = new System.Drawing.Size(36, 13);
            this.lArtist.TabIndex = 5;
            this.lArtist.Text = "None";
            this.lArtist.UseMnemonic = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lStatus);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lArtist);
            this.groupBox2.Controls.Add(this.lTrack);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 100);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information";
            // 
            // lStatus
            // 
            this.lStatus.AutoEllipsis = true;
            this.lStatus.AutoSize = true;
            this.lStatus.Location = new System.Drawing.Point(119, 71);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(107, 13);
            this.lStatus.TabIndex = 7;
            this.lStatus.Text = "Spotify not active";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Status:";
            // 
            // toolTip
            // 
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Details";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 236);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Swervify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUAC)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbBanner;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbStartup;
        private System.Windows.Forms.CheckBox cbVideos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lTrack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lArtist;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbUAC;
    }
}

