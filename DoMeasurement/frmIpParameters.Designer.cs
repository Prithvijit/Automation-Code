namespace DoMeasurement
{
    partial class frmIpParameters
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
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnOptimize = new System.Windows.Forms.Button();
            this.lblMaxFeret = new System.Windows.Forms.Label();
            this.lblMinArea = new System.Windows.Forms.Label();
            this.lblMaxFeret2 = new System.Windows.Forms.Label();
            this.lblMinArea2 = new System.Windows.Forms.Label();
            this.lblMaxArea = new System.Windows.Forms.Label();
            this.nudMaxFeret = new System.Windows.Forms.NumericUpDown();
            this.nudMinArea = new System.Windows.Forms.NumericUpDown();
            this.nudMaxFeret2 = new System.Windows.Forms.NumericUpDown();
            this.nudMinArea2 = new System.Windows.Forms.NumericUpDown();
            this.nudMaxArea = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.tableLayoutPanelImageAnalysis = new System.Windows.Forms.TableLayoutPanel();
            this.pctBoxBinary = new Emgu.CV.UI.PanAndZoomPictureBox();
            this.pctBoxAnalysis = new Emgu.CV.UI.PanAndZoomPictureBox();
            this.tableLayoutPanelSave = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFeret)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFeret2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinArea2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxArea)).BeginInit();
            this.pnlFilters.SuspendLayout();
            this.tableLayoutPanelImageAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxBinary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxAnalysis)).BeginInit();
            this.tableLayoutPanelSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnalyze.Location = new System.Drawing.Point(75, 23);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(80, 33);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnOptimize
            // 
            this.btnOptimize.Enabled = false;
            this.btnOptimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptimize.Location = new System.Drawing.Point(75, 72);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(80, 33);
            this.btnOptimize.TabIndex = 1;
            this.btnOptimize.Text = "Optimize";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // lblMaxFeret
            // 
            this.lblMaxFeret.AutoSize = true;
            this.lblMaxFeret.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxFeret.Location = new System.Drawing.Point(1, 6);
            this.lblMaxFeret.Name = "lblMaxFeret";
            this.lblMaxFeret.Size = new System.Drawing.Size(80, 20);
            this.lblMaxFeret.TabIndex = 2;
            this.lblMaxFeret.Text = "Max Feret";
            // 
            // lblMinArea
            // 
            this.lblMinArea.AutoSize = true;
            this.lblMinArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinArea.Location = new System.Drawing.Point(1, 49);
            this.lblMinArea.Name = "lblMinArea";
            this.lblMinArea.Size = new System.Drawing.Size(72, 20);
            this.lblMinArea.TabIndex = 3;
            this.lblMinArea.Text = "Min Area";
            // 
            // lblMaxFeret2
            // 
            this.lblMaxFeret2.AutoSize = true;
            this.lblMaxFeret2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxFeret2.Location = new System.Drawing.Point(1, 92);
            this.lblMaxFeret2.Name = "lblMaxFeret2";
            this.lblMaxFeret2.Size = new System.Drawing.Size(93, 20);
            this.lblMaxFeret2.TabIndex = 4;
            this.lblMaxFeret2.Text = "Max Feret 2";
            // 
            // lblMinArea2
            // 
            this.lblMinArea2.AutoSize = true;
            this.lblMinArea2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinArea2.Location = new System.Drawing.Point(1, 135);
            this.lblMinArea2.Name = "lblMinArea2";
            this.lblMinArea2.Size = new System.Drawing.Size(85, 20);
            this.lblMinArea2.TabIndex = 5;
            this.lblMinArea2.Text = "Min Area 2";
            // 
            // lblMaxArea
            // 
            this.lblMaxArea.AutoSize = true;
            this.lblMaxArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxArea.Location = new System.Drawing.Point(1, 178);
            this.lblMaxArea.Name = "lblMaxArea";
            this.lblMaxArea.Size = new System.Drawing.Size(76, 20);
            this.lblMaxArea.TabIndex = 6;
            this.lblMaxArea.Text = "Max Area";
            // 
            // nudMaxFeret
            // 
            this.nudMaxFeret.DecimalPlaces = 1;
            this.nudMaxFeret.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxFeret.Location = new System.Drawing.Point(117, 6);
            this.nudMaxFeret.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMaxFeret.Name = "nudMaxFeret";
            this.nudMaxFeret.Size = new System.Drawing.Size(120, 26);
            this.nudMaxFeret.TabIndex = 7;
            // 
            // nudMinArea
            // 
            this.nudMinArea.DecimalPlaces = 1;
            this.nudMinArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinArea.Location = new System.Drawing.Point(117, 49);
            this.nudMinArea.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMinArea.Name = "nudMinArea";
            this.nudMinArea.Size = new System.Drawing.Size(120, 26);
            this.nudMinArea.TabIndex = 8;
            // 
            // nudMaxFeret2
            // 
            this.nudMaxFeret2.DecimalPlaces = 1;
            this.nudMaxFeret2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxFeret2.Location = new System.Drawing.Point(117, 92);
            this.nudMaxFeret2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMaxFeret2.Name = "nudMaxFeret2";
            this.nudMaxFeret2.Size = new System.Drawing.Size(120, 26);
            this.nudMaxFeret2.TabIndex = 9;
            // 
            // nudMinArea2
            // 
            this.nudMinArea2.DecimalPlaces = 1;
            this.nudMinArea2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinArea2.Location = new System.Drawing.Point(117, 135);
            this.nudMinArea2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMinArea2.Name = "nudMinArea2";
            this.nudMinArea2.Size = new System.Drawing.Size(120, 26);
            this.nudMinArea2.TabIndex = 10;
            // 
            // nudMaxArea
            // 
            this.nudMaxArea.DecimalPlaces = 1;
            this.nudMaxArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxArea.Location = new System.Drawing.Point(117, 178);
            this.nudMaxArea.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMaxArea.Name = "nudMaxArea";
            this.nudMaxArea.Size = new System.Drawing.Size(120, 26);
            this.nudMaxArea.TabIndex = 11;
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(24, 224);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 33);
            this.btnApply.TabIndex = 12;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefault.Location = new System.Drawing.Point(117, 224);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(80, 33);
            this.btnDefault.TabIndex = 13;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.btnDefault);
            this.pnlFilters.Controls.Add(this.lblMaxFeret);
            this.pnlFilters.Controls.Add(this.btnApply);
            this.pnlFilters.Controls.Add(this.lblMinArea);
            this.pnlFilters.Controls.Add(this.nudMaxArea);
            this.pnlFilters.Controls.Add(this.lblMaxFeret2);
            this.pnlFilters.Controls.Add(this.nudMinArea2);
            this.pnlFilters.Controls.Add(this.lblMinArea2);
            this.pnlFilters.Controls.Add(this.nudMaxFeret2);
            this.pnlFilters.Controls.Add(this.lblMaxArea);
            this.pnlFilters.Controls.Add(this.nudMinArea);
            this.pnlFilters.Controls.Add(this.nudMaxFeret);
            this.pnlFilters.Location = new System.Drawing.Point(12, 135);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(245, 270);
            this.pnlFilters.TabIndex = 14;
            this.pnlFilters.Visible = false;
            // 
            // tableLayoutPanelImageAnalysis
            // 
            this.tableLayoutPanelImageAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelImageAnalysis.ColumnCount = 2;
            this.tableLayoutPanelImageAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImageAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImageAnalysis.Controls.Add(this.pctBoxBinary, 1, 0);
            this.tableLayoutPanelImageAnalysis.Controls.Add(this.pctBoxAnalysis, 0, 0);
            this.tableLayoutPanelImageAnalysis.Location = new System.Drawing.Point(267, 23);
            this.tableLayoutPanelImageAnalysis.Name = "tableLayoutPanelImageAnalysis";
            this.tableLayoutPanelImageAnalysis.RowCount = 1;
            this.tableLayoutPanelImageAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImageAnalysis.Size = new System.Drawing.Size(946, 356);
            this.tableLayoutPanelImageAnalysis.TabIndex = 15;
            // 
            // pctBoxBinary
            // 
            this.pctBoxBinary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctBoxBinary.Location = new System.Drawing.Point(476, 3);
            this.pctBoxBinary.Name = "pctBoxBinary";
            this.pctBoxBinary.Size = new System.Drawing.Size(467, 350);
            this.pctBoxBinary.TabIndex = 1;
            this.pctBoxBinary.TabStop = false;
            // 
            // pctBoxAnalysis
            // 
            this.pctBoxAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctBoxAnalysis.Location = new System.Drawing.Point(3, 3);
            this.pctBoxAnalysis.Name = "pctBoxAnalysis";
            this.pctBoxAnalysis.Size = new System.Drawing.Size(467, 350);
            this.pctBoxAnalysis.TabIndex = 0;
            this.pctBoxAnalysis.TabStop = false;
            // 
            // tableLayoutPanelSave
            // 
            this.tableLayoutPanelSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSave.ColumnCount = 2;
            this.tableLayoutPanelSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.Controls.Add(this.btnClose, 0, 0);
            this.tableLayoutPanelSave.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanelSave.Location = new System.Drawing.Point(998, 411);
            this.tableLayoutPanelSave.Name = "tableLayoutPanelSave";
            this.tableLayoutPanelSave.RowCount = 1;
            this.tableLayoutPanelSave.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.Size = new System.Drawing.Size(200, 37);
            this.tableLayoutPanelSave.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(103, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 31);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 31);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // frmIpParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 460);
            this.Controls.Add(this.tableLayoutPanelSave);
            this.Controls.Add(this.tableLayoutPanelImageAnalysis);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.btnOptimize);
            this.Controls.Add(this.btnAnalyze);
            this.Name = "frmIpParameters";
            this.Text = "frmIpParameters";
            this.Load += new System.EventHandler(this.frmIpParameters_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFeret)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFeret2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinArea2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxArea)).EndInit();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.tableLayoutPanelImageAnalysis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxBinary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxAnalysis)).EndInit();
            this.tableLayoutPanelSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnOptimize;
        private System.Windows.Forms.Label lblMaxFeret;
        private System.Windows.Forms.Label lblMinArea;
        private System.Windows.Forms.Label lblMaxFeret2;
        private System.Windows.Forms.Label lblMinArea2;
        private System.Windows.Forms.Label lblMaxArea;
        private System.Windows.Forms.NumericUpDown nudMaxFeret;
        private System.Windows.Forms.NumericUpDown nudMinArea;
        private System.Windows.Forms.NumericUpDown nudMaxFeret2;
        private System.Windows.Forms.NumericUpDown nudMinArea2;
        private System.Windows.Forms.NumericUpDown nudMaxArea;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImageAnalysis;
        private Emgu.CV.UI.PanAndZoomPictureBox pctBoxBinary;
        private Emgu.CV.UI.PanAndZoomPictureBox pctBoxAnalysis;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}