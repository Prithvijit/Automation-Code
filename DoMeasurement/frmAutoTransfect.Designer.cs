namespace DoMeasurement
{
    partial class frmAutoTransfect
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
            this.tableLayoutPanelCamera = new System.Windows.Forms.TableLayoutPanel();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnLive = new System.Windows.Forms.Button();
            this.tableLayoutPanelTransfect = new System.Windows.Forms.TableLayoutPanel();
            this.btnTransfect = new System.Windows.Forms.Button();
            this.tableLayoutPanelSave = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanelLive = new System.Windows.Forms.TableLayoutPanel();
            this.pctBoxCalibrate = new Emgu.CV.UI.PanAndZoomPictureBox();
            this.pctBoxLive = new Emgu.CV.UI.PanAndZoomPictureBox();
            this.timerCalib = new System.Windows.Forms.Timer(this.components);
            this.timerTransfect = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanelCamera.SuspendLayout();
            this.tableLayoutPanelTransfect.SuspendLayout();
            this.tableLayoutPanelSave.SuspendLayout();
            this.tableLayoutPanelLive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxCalibrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxLive)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelCamera
            // 
            this.tableLayoutPanelCamera.ColumnCount = 1;
            this.tableLayoutPanelCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCamera.Controls.Add(this.btnCapture, 0, 3);
            this.tableLayoutPanelCamera.Controls.Add(this.btnCalibrate, 0, 2);
            this.tableLayoutPanelCamera.Controls.Add(this.btnStop, 0, 1);
            this.tableLayoutPanelCamera.Controls.Add(this.btnLive, 0, 0);
            this.tableLayoutPanelCamera.Location = new System.Drawing.Point(12, 40);
            this.tableLayoutPanelCamera.Name = "tableLayoutPanelCamera";
            this.tableLayoutPanelCamera.RowCount = 4;
            this.tableLayoutPanelCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCamera.Size = new System.Drawing.Size(110, 253);
            this.tableLayoutPanelCamera.TabIndex = 0;
            // 
            // btnCapture
            // 
            this.btnCapture.Enabled = false;
            this.btnCapture.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapture.Location = new System.Drawing.Point(3, 192);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(86, 30);
            this.btnCapture.TabIndex = 4;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Enabled = false;
            this.btnCalibrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalibrate.Location = new System.Drawing.Point(3, 129);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(86, 30);
            this.btnCalibrate.TabIndex = 3;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(3, 66);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(86, 30);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnLive
            // 
            this.btnLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLive.Location = new System.Drawing.Point(3, 3);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(86, 30);
            this.btnLive.TabIndex = 1;
            this.btnLive.Text = "Live";
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // tableLayoutPanelTransfect
            // 
            this.tableLayoutPanelTransfect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanelTransfect.ColumnCount = 1;
            this.tableLayoutPanelTransfect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTransfect.Controls.Add(this.btnTransfect, 0, 0);
            this.tableLayoutPanelTransfect.Location = new System.Drawing.Point(12, 387);
            this.tableLayoutPanelTransfect.Name = "tableLayoutPanelTransfect";
            this.tableLayoutPanelTransfect.RowCount = 1;
            this.tableLayoutPanelTransfect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTransfect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanelTransfect.Size = new System.Drawing.Size(98, 39);
            this.tableLayoutPanelTransfect.TabIndex = 1;
            // 
            // btnTransfect
            // 
            this.btnTransfect.Enabled = false;
            this.btnTransfect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfect.Location = new System.Drawing.Point(3, 3);
            this.btnTransfect.Name = "btnTransfect";
            this.btnTransfect.Size = new System.Drawing.Size(86, 30);
            this.btnTransfect.TabIndex = 5;
            this.btnTransfect.Text = "Transfect";
            this.btnTransfect.UseVisualStyleBackColor = true;
            this.btnTransfect.Click += new System.EventHandler(this.btnTransfect_Click);
            // 
            // tableLayoutPanelSave
            // 
            this.tableLayoutPanelSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSave.ColumnCount = 2;
            this.tableLayoutPanelSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.Controls.Add(this.btnClose, 0, 0);
            this.tableLayoutPanelSave.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanelSave.Location = new System.Drawing.Point(880, 387);
            this.tableLayoutPanelSave.Name = "tableLayoutPanelSave";
            this.tableLayoutPanelSave.RowCount = 1;
            this.tableLayoutPanelSave.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSave.Size = new System.Drawing.Size(200, 39);
            this.tableLayoutPanelSave.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(103, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelLive
            // 
            this.tableLayoutPanelLive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLive.ColumnCount = 2;
            this.tableLayoutPanelLive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLive.Controls.Add(this.pctBoxCalibrate, 0, 0);
            this.tableLayoutPanelLive.Controls.Add(this.pctBoxLive, 0, 0);
            this.tableLayoutPanelLive.Location = new System.Drawing.Point(142, 21);
            this.tableLayoutPanelLive.Name = "tableLayoutPanelLive";
            this.tableLayoutPanelLive.RowCount = 1;
            this.tableLayoutPanelLive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLive.Size = new System.Drawing.Size(946, 356);
            this.tableLayoutPanelLive.TabIndex = 3;
            // 
            // pctBoxCalibrate
            // 
            this.pctBoxCalibrate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctBoxCalibrate.Location = new System.Drawing.Point(476, 3);
            this.pctBoxCalibrate.Name = "pctBoxCalibrate";
            this.pctBoxCalibrate.Size = new System.Drawing.Size(467, 350);
            this.pctBoxCalibrate.TabIndex = 1;
            this.pctBoxCalibrate.TabStop = false;
            // 
            // pctBoxLive
            // 
            this.pctBoxLive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctBoxLive.Location = new System.Drawing.Point(3, 3);
            this.pctBoxLive.Name = "pctBoxLive";
            this.pctBoxLive.Size = new System.Drawing.Size(467, 350);
            this.pctBoxLive.TabIndex = 0;
            this.pctBoxLive.TabStop = false;
            // 
            // timerCalib
            // 
            this.timerCalib.Tick += new System.EventHandler(this.timerCalib_Tick);
            // 
            // timerTransfect
            // 
            this.timerTransfect.Tick += new System.EventHandler(this.timerTransfect_Tick);
            // 
            // frmAutoTransfect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 438);
            this.Controls.Add(this.tableLayoutPanelLive);
            this.Controls.Add(this.tableLayoutPanelSave);
            this.Controls.Add(this.tableLayoutPanelTransfect);
            this.Controls.Add(this.tableLayoutPanelCamera);
            this.Name = "frmAutoTransfect";
            this.Text = "frmAutoTransfect";
            this.tableLayoutPanelCamera.ResumeLayout(false);
            this.tableLayoutPanelTransfect.ResumeLayout(false);
            this.tableLayoutPanelSave.ResumeLayout(false);
            this.tableLayoutPanelLive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxCalibrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBoxLive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCamera;
        private System.Windows.Forms.Button btnLive;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransfect;
        private System.Windows.Forms.Button btnTransfect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLive;
        private Emgu.CV.UI.PanAndZoomPictureBox pctBoxLive;
        private Emgu.CV.UI.PanAndZoomPictureBox pctBoxCalibrate;
        private System.Windows.Forms.Timer timerCalib;
        private System.Windows.Forms.Timer timerTransfect;
    }
}