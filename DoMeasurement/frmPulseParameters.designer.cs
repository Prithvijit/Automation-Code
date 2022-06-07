namespace DoMeasurement
{
    partial class frmPulseParameters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPulseParameters));
            this.CmbPulseType = new System.Windows.Forms.ComboBox();
            this.btnSetPulse = new System.Windows.Forms.Button();
            this.btnCancelPulse = new System.Windows.Forms.Button();
            this.labelVoltage1 = new System.Windows.Forms.Label();
            this.labelVoltage2 = new System.Windows.Forms.Label();
            this.labelPulseType = new System.Windows.Forms.Label();
            this.labelTime1 = new System.Windows.Forms.Label();
            this.labelTime2 = new System.Windows.Forms.Label();
            this.labelPulsesInTrain = new System.Windows.Forms.Label();
            this.labelPulseFrequency = new System.Windows.Forms.Label();
            this.labelRestingTime = new System.Windows.Forms.Label();
            this.labelNumberOfTrains = new System.Windows.Forms.Label();
            this.btnDefaultParameters = new System.Windows.Forms.Button();
            this.nudVoltage1 = new System.Windows.Forms.NumericUpDown();
            this.nudVoltage2 = new System.Windows.Forms.NumericUpDown();
            this.nudTime1 = new System.Windows.Forms.NumericUpDown();
            this.nudTime2 = new System.Windows.Forms.NumericUpDown();
            this.nudPulsesInTrain = new System.Windows.Forms.NumericUpDown();
            this.nudPulseFrequency = new System.Windows.Forms.NumericUpDown();
            this.nudRestingTime = new System.Windows.Forms.NumericUpDown();
            this.nudNumberOfTrains = new System.Windows.Forms.NumericUpDown();
            this.pnlV1T1 = new System.Windows.Forms.Panel();
            this.pnlV2T2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulsesInTrain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulseFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfTrains)).BeginInit();
            this.pnlV1T1.SuspendLayout();
            this.pnlV2T2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbPulseType
            // 
            this.CmbPulseType.FormattingEnabled = true;
            this.CmbPulseType.Items.AddRange(new object[] {
            "Bilevel",
            "Square",
            "Exponential"});
            this.CmbPulseType.Location = new System.Drawing.Point(205, 15);
            this.CmbPulseType.Margin = new System.Windows.Forms.Padding(4);
            this.CmbPulseType.Name = "CmbPulseType";
            this.CmbPulseType.Size = new System.Drawing.Size(132, 24);
            this.CmbPulseType.TabIndex = 10;
            this.CmbPulseType.SelectedIndexChanged += new System.EventHandler(this.CmbPulseType_SelectedIndexChanged);
            // 
            // btnSetPulse
            // 
            this.btnSetPulse.Location = new System.Drawing.Point(16, 481);
            this.btnSetPulse.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetPulse.Name = "btnSetPulse";
            this.btnSetPulse.Size = new System.Drawing.Size(100, 28);
            this.btnSetPulse.TabIndex = 12;
            this.btnSetPulse.Text = "Set";
            this.btnSetPulse.UseVisualStyleBackColor = true;
            this.btnSetPulse.Click += new System.EventHandler(this.btnSetPulse_Click);
            // 
            // btnCancelPulse
            // 
            this.btnCancelPulse.Location = new System.Drawing.Point(264, 481);
            this.btnCancelPulse.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelPulse.Name = "btnCancelPulse";
            this.btnCancelPulse.Size = new System.Drawing.Size(100, 28);
            this.btnCancelPulse.TabIndex = 13;
            this.btnCancelPulse.Text = "Exit";
            this.btnCancelPulse.UseVisualStyleBackColor = true;
            this.btnCancelPulse.Click += new System.EventHandler(this.btnCancelPulse_Click);
            // 
            // labelVoltage1
            // 
            this.labelVoltage1.AutoSize = true;
            this.labelVoltage1.Location = new System.Drawing.Point(37, 10);
            this.labelVoltage1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVoltage1.Name = "labelVoltage1";
            this.labelVoltage1.Size = new System.Drawing.Size(62, 16);
            this.labelVoltage1.TabIndex = 15;
            this.labelVoltage1.Text = "Voltage1";
            // 
            // labelVoltage2
            // 
            this.labelVoltage2.AutoSize = true;
            this.labelVoltage2.Location = new System.Drawing.Point(37, 10);
            this.labelVoltage2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVoltage2.Name = "labelVoltage2";
            this.labelVoltage2.Size = new System.Drawing.Size(62, 16);
            this.labelVoltage2.TabIndex = 16;
            this.labelVoltage2.Text = "Voltage2";
            // 
            // labelPulseType
            // 
            this.labelPulseType.AutoSize = true;
            this.labelPulseType.Location = new System.Drawing.Point(108, 18);
            this.labelPulseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPulseType.Name = "labelPulseType";
            this.labelPulseType.Size = new System.Drawing.Size(77, 16);
            this.labelPulseType.TabIndex = 17;
            this.labelPulseType.Text = "Pulse Type";
            // 
            // labelTime1
            // 
            this.labelTime1.AutoSize = true;
            this.labelTime1.Location = new System.Drawing.Point(53, 59);
            this.labelTime1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTime1.Name = "labelTime1";
            this.labelTime1.Size = new System.Drawing.Size(46, 16);
            this.labelTime1.TabIndex = 18;
            this.labelTime1.Text = "Time1";
            // 
            // labelTime2
            // 
            this.labelTime2.AutoSize = true;
            this.labelTime2.Location = new System.Drawing.Point(53, 59);
            this.labelTime2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTime2.Name = "labelTime2";
            this.labelTime2.Size = new System.Drawing.Size(46, 16);
            this.labelTime2.TabIndex = 19;
            this.labelTime2.Text = "Time2";
            // 
            // labelPulsesInTrain
            // 
            this.labelPulsesInTrain.AutoSize = true;
            this.labelPulsesInTrain.Location = new System.Drawing.Point(93, 264);
            this.labelPulsesInTrain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPulsesInTrain.Name = "labelPulsesInTrain";
            this.labelPulsesInTrain.Size = new System.Drawing.Size(96, 16);
            this.labelPulsesInTrain.TabIndex = 20;
            this.labelPulsesInTrain.Text = "Pulses in Train";
            // 
            // labelPulseFrequency
            // 
            this.labelPulseFrequency.AutoSize = true;
            this.labelPulseFrequency.Location = new System.Drawing.Point(80, 313);
            this.labelPulseFrequency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPulseFrequency.Name = "labelPulseFrequency";
            this.labelPulseFrequency.Size = new System.Drawing.Size(109, 16);
            this.labelPulseFrequency.TabIndex = 21;
            this.labelPulseFrequency.Text = "Pulse Frequency";
            // 
            // labelRestingTime
            // 
            this.labelRestingTime.AutoSize = true;
            this.labelRestingTime.Location = new System.Drawing.Point(97, 362);
            this.labelRestingTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRestingTime.Name = "labelRestingTime";
            this.labelRestingTime.Size = new System.Drawing.Size(88, 16);
            this.labelRestingTime.TabIndex = 22;
            this.labelRestingTime.Text = "Resting Time";
            // 
            // labelNumberOfTrains
            // 
            this.labelNumberOfTrains.AutoSize = true;
            this.labelNumberOfTrains.Location = new System.Drawing.Point(74, 409);
            this.labelNumberOfTrains.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumberOfTrains.Name = "labelNumberOfTrains";
            this.labelNumberOfTrains.Size = new System.Drawing.Size(111, 16);
            this.labelNumberOfTrains.TabIndex = 23;
            this.labelNumberOfTrains.Text = "Number of Trains";
            // 
            // btnDefaultParameters
            // 
            this.btnDefaultParameters.Location = new System.Drawing.Point(140, 481);
            this.btnDefaultParameters.Margin = new System.Windows.Forms.Padding(4);
            this.btnDefaultParameters.Name = "btnDefaultParameters";
            this.btnDefaultParameters.Size = new System.Drawing.Size(100, 28);
            this.btnDefaultParameters.TabIndex = 24;
            this.btnDefaultParameters.Text = "Default";
            this.btnDefaultParameters.UseVisualStyleBackColor = true;
            this.btnDefaultParameters.Click += new System.EventHandler(this.btnDefaultParameters_Click);
            // 
            // nudVoltage1
            // 
            this.nudVoltage1.DecimalPlaces = 1;
            this.nudVoltage1.Location = new System.Drawing.Point(119, 8);
            this.nudVoltage1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudVoltage1.Name = "nudVoltage1";
            this.nudVoltage1.Size = new System.Drawing.Size(132, 22);
            this.nudVoltage1.TabIndex = 25;
            this.nudVoltage1.ValueChanged += new System.EventHandler(this.nudVoltage1_ValueChanged);
            // 
            // nudVoltage2
            // 
            this.nudVoltage2.DecimalPlaces = 1;
            this.nudVoltage2.Location = new System.Drawing.Point(119, 8);
            this.nudVoltage2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudVoltage2.Name = "nudVoltage2";
            this.nudVoltage2.Size = new System.Drawing.Size(132, 22);
            this.nudVoltage2.TabIndex = 26;
            this.nudVoltage2.ValueChanged += new System.EventHandler(this.nudVoltage2_ValueChanged);
            // 
            // nudTime1
            // 
            this.nudTime1.DecimalPlaces = 1;
            this.nudTime1.Location = new System.Drawing.Point(119, 57);
            this.nudTime1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTime1.Name = "nudTime1";
            this.nudTime1.Size = new System.Drawing.Size(132, 22);
            this.nudTime1.TabIndex = 27;
            this.nudTime1.ValueChanged += new System.EventHandler(this.nudTime1_ValueChanged);
            // 
            // nudTime2
            // 
            this.nudTime2.DecimalPlaces = 1;
            this.nudTime2.Location = new System.Drawing.Point(119, 57);
            this.nudTime2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTime2.Name = "nudTime2";
            this.nudTime2.Size = new System.Drawing.Size(132, 22);
            this.nudTime2.TabIndex = 28;
            this.nudTime2.ValueChanged += new System.EventHandler(this.nudTime2_ValueChanged);
            // 
            // nudPulsesInTrain
            // 
            this.nudPulsesInTrain.Location = new System.Drawing.Point(205, 262);
            this.nudPulsesInTrain.Name = "nudPulsesInTrain";
            this.nudPulsesInTrain.Size = new System.Drawing.Size(132, 22);
            this.nudPulsesInTrain.TabIndex = 29;
            this.nudPulsesInTrain.ValueChanged += new System.EventHandler(this.nudPulsesInTrain_ValueChanged);
            // 
            // nudPulseFrequency
            // 
            this.nudPulseFrequency.Location = new System.Drawing.Point(205, 311);
            this.nudPulseFrequency.Maximum = new decimal(new int[] {
            285,
            0,
            0,
            0});
            this.nudPulseFrequency.Name = "nudPulseFrequency";
            this.nudPulseFrequency.Size = new System.Drawing.Size(132, 22);
            this.nudPulseFrequency.TabIndex = 30;
            this.nudPulseFrequency.ValueChanged += new System.EventHandler(this.nudPulseFrequency_ValueChanged);
            // 
            // nudRestingTime
            // 
            this.nudRestingTime.DecimalPlaces = 1;
            this.nudRestingTime.Location = new System.Drawing.Point(205, 360);
            this.nudRestingTime.Name = "nudRestingTime";
            this.nudRestingTime.Size = new System.Drawing.Size(132, 22);
            this.nudRestingTime.TabIndex = 31;
            this.nudRestingTime.ValueChanged += new System.EventHandler(this.nudRestingTime_ValueChanged);
            // 
            // nudNumberOfTrains
            // 
            this.nudNumberOfTrains.Location = new System.Drawing.Point(205, 409);
            this.nudNumberOfTrains.Name = "nudNumberOfTrains";
            this.nudNumberOfTrains.Size = new System.Drawing.Size(132, 22);
            this.nudNumberOfTrains.TabIndex = 32;
            this.nudNumberOfTrains.ValueChanged += new System.EventHandler(this.nudNumberOfTrains_ValueChanged);
            // 
            // pnlV1T1
            // 
            this.pnlV1T1.Controls.Add(this.labelTime1);
            this.pnlV1T1.Controls.Add(this.nudTime1);
            this.pnlV1T1.Controls.Add(this.labelVoltage1);
            this.pnlV1T1.Controls.Add(this.nudVoltage1);
            this.pnlV1T1.Location = new System.Drawing.Point(86, 59);
            this.pnlV1T1.Name = "pnlV1T1";
            this.pnlV1T1.Size = new System.Drawing.Size(282, 86);
            this.pnlV1T1.TabIndex = 33;
            // 
            // pnlV2T2
            // 
            this.pnlV2T2.Controls.Add(this.nudVoltage2);
            this.pnlV2T2.Controls.Add(this.labelVoltage2);
            this.pnlV2T2.Controls.Add(this.labelTime2);
            this.pnlV2T2.Controls.Add(this.nudTime2);
            this.pnlV2T2.Location = new System.Drawing.Point(86, 157);
            this.pnlV2T2.Name = "pnlV2T2";
            this.pnlV2T2.Size = new System.Drawing.Size(282, 86);
            this.pnlV2T2.TabIndex = 34;
            // 
            // frmPulseParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 524);
            this.Controls.Add(this.pnlV2T2);
            this.Controls.Add(this.pnlV1T1);
            this.Controls.Add(this.nudNumberOfTrains);
            this.Controls.Add(this.nudRestingTime);
            this.Controls.Add(this.nudPulseFrequency);
            this.Controls.Add(this.nudPulsesInTrain);
            this.Controls.Add(this.btnDefaultParameters);
            this.Controls.Add(this.labelNumberOfTrains);
            this.Controls.Add(this.labelRestingTime);
            this.Controls.Add(this.labelPulseFrequency);
            this.Controls.Add(this.labelPulsesInTrain);
            this.Controls.Add(this.labelPulseType);
            this.Controls.Add(this.btnCancelPulse);
            this.Controls.Add(this.btnSetPulse);
            this.Controls.Add(this.CmbPulseType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPulseParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pulse Parameters";
            this.Load += new System.EventHandler(this.Pulse_Parameters_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulsesInTrain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulseFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfTrains)).EndInit();
            this.pnlV1T1.ResumeLayout(false);
            this.pnlV1T1.PerformLayout();
            this.pnlV2T2.ResumeLayout(false);
            this.pnlV2T2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CmbPulseType;
        private System.Windows.Forms.Button btnSetPulse;
        private System.Windows.Forms.Button btnCancelPulse;
        private System.Windows.Forms.Label labelVoltage1;
        private System.Windows.Forms.Label labelVoltage2;
        private System.Windows.Forms.Label labelPulseType;
        private System.Windows.Forms.Label labelTime1;
        private System.Windows.Forms.Label labelTime2;
        private System.Windows.Forms.Label labelPulsesInTrain;
        private System.Windows.Forms.Label labelPulseFrequency;
        private System.Windows.Forms.Label labelRestingTime;
        private System.Windows.Forms.Label labelNumberOfTrains;
        private System.Windows.Forms.Button btnDefaultParameters;
        private System.Windows.Forms.NumericUpDown nudVoltage1;
        private System.Windows.Forms.NumericUpDown nudVoltage2;
        private System.Windows.Forms.NumericUpDown nudTime1;
        private System.Windows.Forms.NumericUpDown nudTime2;
        private System.Windows.Forms.NumericUpDown nudPulsesInTrain;
        private System.Windows.Forms.NumericUpDown nudPulseFrequency;
        private System.Windows.Forms.NumericUpDown nudRestingTime;
        private System.Windows.Forms.NumericUpDown nudNumberOfTrains;
        private System.Windows.Forms.Panel pnlV1T1;
        private System.Windows.Forms.Panel pnlV2T2;
    }
}