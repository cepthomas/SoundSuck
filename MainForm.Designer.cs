namespace SoundSuck
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            toolTip = new System.Windows.Forms.ToolTip(components);
            btnRecord = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            sldVolume = new Ephemera.NBagOfUis.ToolStripSlider();
            meterLevel = new Ephemera.NBagOfUis.ToolStripMeter();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            btnSettings = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            ToolStrip = new System.Windows.Forms.ToolStrip();
            tvInfo = new Ephemera.NBagOfUis.TextViewer();
            ToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(243, 6);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(243, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(243, 6);
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(228, 6);
            // 
            // btnRecord
            // 
            btnRecord.AutoSize = false;
            btnRecord.CheckOnClick = true;
            btnRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnRecord.Image = Properties.Resources.glyphicons_170_record;
            btnRecord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new System.Drawing.Size(40, 40);
            btnRecord.Text = "toolStripButton1";
            btnRecord.ToolTipText = "Record";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 43);
            // 
            // sldVolume
            // 
            sldVolume.AccessibleName = "sldVolume";
            sldVolume.AutoSize = false;
            sldVolume.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sldVolume.DrawColor = System.Drawing.Color.White;
            sldVolume.Label = "Volume";
            sldVolume.Maximum = 2D;
            sldVolume.Minimum = 0D;
            sldVolume.Name = "sldVolume";
            sldVolume.Resolution = 0.05D;
            sldVolume.Size = new System.Drawing.Size(150, 38);
            sldVolume.Text = "vol";
            sldVolume.ToolTipText = "Master volume";
            sldVolume.Value = 1D;
            // 
            // meterLevel
            // 
            meterLevel.AutoSize = false;
            meterLevel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            meterLevel.DrawColor = System.Drawing.Color.White;
            meterLevel.Label = "";
            meterLevel.Maximum = 100D;
            meterLevel.MeterType = Ephemera.NBagOfUis.MeterType.Linear;
            meterLevel.Minimum = 0D;
            meterLevel.Name = "meterLevel";
            meterLevel.Size = new System.Drawing.Size(150, 30);
            meterLevel.Text = "toolStripMeter1";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 43);
            // 
            // btnSettings
            // 
            btnSettings.AutoSize = false;
            btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnSettings.Image = Properties.Resources.glyphicons_137_cogwheel;
            btnSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new System.Drawing.Size(40, 40);
            btnSettings.Text = "toolStripSplitButton1";
            btnSettings.ToolTipText = "Settings";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 43);
            // 
            // ToolStrip
            // 
            ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnRecord, toolStripSeparator6, meterLevel, toolStripSeparator5, btnSettings, toolStripSeparator4, sldVolume });
            ToolStrip.Location = new System.Drawing.Point(0, 0);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Size = new System.Drawing.Size(587, 43);
            ToolStrip.TabIndex = 1;
            ToolStrip.Text = "toolStrip";
            // 
            // tvInfo
            // 
            tvInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tvInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tvInfo.Location = new System.Drawing.Point(0, 46);
            tvInfo.MatchUseBackground = true;
            tvInfo.MaxText = 5000;
            tvInfo.Name = "tvInfo";
            tvInfo.Prompt = "> ";
            tvInfo.Size = new System.Drawing.Size(587, 203);
            tvInfo.TabIndex = 92;
            tvInfo.TabStop = false;
            tvInfo.WordWrap = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(587, 251);
            Controls.Add(ToolStrip);
            Controls.Add(tvInfo);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "MainForm";
            Text = "Sound Suck";
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnRecord;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private Ephemera.NBagOfUis.ToolStripSlider sldVolume;
        private Ephemera.NBagOfUis.ToolStripMeter meterLevel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private Ephemera.NBagOfUis.TextViewer tvInfo;
    }
}