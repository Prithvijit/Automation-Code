namespace DoMeasurement
{
    partial class frmManualTransfect
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
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.timerCalib = new System.Windows.Forms.Timer(this.components);
            this.timerTransfect = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalibrate.Location = new System.Drawing.Point(87, 12);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(114, 38);
            this.btnCalibrate.TabIndex = 0;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(87, 76);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(114, 38);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timerCalib
            // 
            this.timerCalib.Tick += new System.EventHandler(this.timerCalibration_Tick);
            // 
            // timerTransfect
            // 
            this.timerTransfect.Tick += new System.EventHandler(this.timerTransfect_Tick);
            // 
            // frmManualTransfect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 135);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCalibrate);
            this.Name = "frmManualTransfect";
            this.Text = "frmManualTransfect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timerCalib;
        private System.Windows.Forms.Timer timerTransfect;
    }
}