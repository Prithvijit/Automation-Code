namespace DoMeasurement
{
    partial class frmMain
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.chartMeas1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMeasPlotX1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblAvgResistance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblShortMean = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbStageSpeed = new System.Windows.Forms.ComboBox();
            this.btnPulseParameters = new System.Windows.Forms.Button();
            this.btnTransfect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTransfectMode = new System.Windows.Forms.ComboBox();
            this.chkReplayData = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartMeas1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartMeas1
            // 
            this.chartMeas1.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea1.AxisX.MaximumAutoSize = 90F;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.chartMeas1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Enabled = false;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chartMeas1.Legends.Add(legend1);
            this.chartMeas1.Location = new System.Drawing.Point(53, 144);
            this.chartMeas1.Name = "chartMeas1";
            this.chartMeas1.Size = new System.Drawing.Size(800, 460);
            this.chartMeas1.TabIndex = 9;
            this.chartMeas1.Text = "Meas (Resistance)";
            // 
            // lblMeasPlotX1
            // 
            this.lblMeasPlotX1.AutoSize = true;
            this.lblMeasPlotX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeasPlotX1.Location = new System.Drawing.Point(26, 348);
            this.lblMeasPlotX1.Name = "lblMeasPlotX1";
            this.lblMeasPlotX1.Size = new System.Drawing.Size(54, 16);
            this.lblMeasPlotX1.TabIndex = 65;
            this.lblMeasPlotX1.Text = "MOhms";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(423, 583);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 66;
            this.label1.Text = "Time (sec)";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(103, 28);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(83, 40);
            this.btnStartStop.TabIndex = 67;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(210, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 40);
            this.btnSave.TabIndex = 69;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblAvgResistance
            // 
            this.lblAvgResistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAvgResistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgResistance.Location = new System.Drawing.Point(445, 26);
            this.lblAvgResistance.Name = "lblAvgResistance";
            this.lblAvgResistance.Size = new System.Drawing.Size(118, 40);
            this.lblAvgResistance.TabIndex = 70;
            this.lblAvgResistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 20);
            this.label2.TabIndex = 71;
            this.label2.Text = "Resistance (MOhm)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(582, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 20);
            this.label3.TabIndex = 73;
            this.label3.Text = "Short Mean (MOhm)";
            // 
            // lblShortMean
            // 
            this.lblShortMean.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShortMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShortMean.Location = new System.Drawing.Point(738, 26);
            this.lblShortMean.Name = "lblShortMean";
            this.lblShortMean.Size = new System.Drawing.Size(118, 40);
            this.lblShortMean.TabIndex = 72;
            this.lblShortMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(629, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 20);
            this.label4.TabIndex = 74;
            this.label4.Text = "Stage Speed";
            // 
            // cmbStageSpeed
            // 
            this.cmbStageSpeed.FormattingEnabled = true;
            this.cmbStageSpeed.Items.AddRange(new object[] {
            "Slow",
            "Medium",
            "Fast"});
            this.cmbStageSpeed.Location = new System.Drawing.Point(738, 79);
            this.cmbStageSpeed.Name = "cmbStageSpeed";
            this.cmbStageSpeed.Size = new System.Drawing.Size(118, 28);
            this.cmbStageSpeed.TabIndex = 75;
            this.cmbStageSpeed.Text = "Medium";
            this.cmbStageSpeed.SelectedIndexChanged += new System.EventHandler(this.cmbStageSpeed_SelectedIndexChanged);
            // 
            // btnPulseParameters
            // 
            this.btnPulseParameters.Location = new System.Drawing.Point(20, 79);
            this.btnPulseParameters.Name = "btnPulseParameters";
            this.btnPulseParameters.Size = new System.Drawing.Size(147, 40);
            this.btnPulseParameters.TabIndex = 77;
            this.btnPulseParameters.Text = "Pulse Parameters";
            this.btnPulseParameters.UseVisualStyleBackColor = true;
            this.btnPulseParameters.Click += new System.EventHandler(this.btnPulseParameters_Click);
            // 
            // btnTransfect
            // 
            this.btnTransfect.Location = new System.Drawing.Point(173, 79);
            this.btnTransfect.Name = "btnTransfect";
            this.btnTransfect.Size = new System.Drawing.Size(147, 40);
            this.btnTransfect.TabIndex = 78;
            this.btnTransfect.Text = "Transfect";
            this.btnTransfect.UseVisualStyleBackColor = true;
            this.btnTransfect.Click += new System.EventHandler(this.btnTransfect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(341, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 20);
            this.label5.TabIndex = 79;
            this.label5.Text = "Transfection Mode";
            // 
            // cmbTransfectMode
            // 
            this.cmbTransfectMode.FormattingEnabled = true;
            this.cmbTransfectMode.Items.AddRange(new object[] {
            "Manual",
            "Auto",
            "Array"});
            this.cmbTransfectMode.Location = new System.Drawing.Point(488, 79);
            this.cmbTransfectMode.Name = "cmbTransfectMode";
            this.cmbTransfectMode.Size = new System.Drawing.Size(118, 28);
            this.cmbTransfectMode.TabIndex = 80;
            this.cmbTransfectMode.Text = "Manual";
            this.cmbTransfectMode.SelectedIndexChanged += new System.EventHandler(this.cmbTransfectMode_SelectedIndexChanged);
            // 
            // chkReplayData
            // 
            this.chkReplayData.AutoSize = true;
            this.chkReplayData.Location = new System.Drawing.Point(22, 35);
            this.chkReplayData.Name = "chkReplayData";
            this.chkReplayData.Size = new System.Drawing.Size(77, 24);
            this.chkReplayData.TabIndex = 81;
            this.chkReplayData.Text = "Replay";
            this.chkReplayData.UseVisualStyleBackColor = true;
            this.chkReplayData.CheckedChanged += new System.EventHandler(this.chkReplayData_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(913, 628);
            this.Controls.Add(this.chkReplayData);
            this.Controls.Add(this.cmbTransfectMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTransfect);
            this.Controls.Add(this.btnPulseParameters);
            this.Controls.Add(this.cmbStageSpeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblShortMean);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAvgResistance);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMeasPlotX1);
            this.Controls.Add(this.chartMeas1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Do Measurement";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartMeas1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartMeas1;
        private System.Windows.Forms.Label lblMeasPlotX1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblAvgResistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblShortMean;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbStageSpeed;
        private System.Windows.Forms.Button btnPulseParameters;
        private System.Windows.Forms.Button btnTransfect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbTransfectMode;
        private System.Windows.Forms.CheckBox chkReplayData;
    }
}

