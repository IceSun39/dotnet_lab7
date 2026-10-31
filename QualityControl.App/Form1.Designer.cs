namespace QualityControl.App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblSize = new Label();
            nudSize = new NumericUpDown();
            btnScan = new Button();
            btnReset = new Button();
            lstLog = new ListBox();
            lblStats = new Label();
            ((System.ComponentModel.ISupportInitialize)nudSize).BeginInit();
            SuspendLayout();
            lblSize.AutoSize = true;
            lblSize.Location = new Point(12, 15);
            lblSize.Name = "lblSize";
            lblSize.Text = "Розмір (номінал 50, допуск ±0.1):";
            nudSize.DecimalPlaces = 2;
            nudSize.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            nudSize.Location = new Point(230, 12);
            nudSize.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSize.Name = "nudSize";
            nudSize.Size = new Size(80, 23);
            nudSize.Value = new decimal(new int[] { 50, 0, 0, 0 });
            btnScan.Location = new Point(325, 10);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(110, 27);
            btnScan.Text = "Сканувати";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            btnReset.Location = new Point(445, 10);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(90, 27);
            btnReset.Text = "Скинути";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            lstLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.FormattingEnabled = true;
            lstLog.HorizontalScrollbar = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 50);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(523, 244);
            lblStats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStats.AutoSize = true;
            lblStats.Location = new Point(12, 310);
            lblStats.Name = "lblStats";
            lblStats.Text = "lblStats";
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(547, 340);
            Controls.Add(lblSize);
            Controls.Add(nudSize);
            Controls.Add(btnScan);
            Controls.Add(btnReset);
            Controls.Add(lstLog);
            Controls.Add(lblStats);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Контроль якості";
            ((System.ComponentModel.ISupportInitialize)nudSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblSize;
        private NumericUpDown nudSize;
        private Button btnScan;
        private Button btnReset;
        private ListBox lstLog;
        private Label lblStats;
    }
}
