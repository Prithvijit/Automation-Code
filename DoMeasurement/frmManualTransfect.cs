using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.IO;

namespace DoMeasurement
{
    public partial class frmManualTransfect : Form
    {
        //for calibration
        int clickCt;
        const double CALIBRATION_DISTANCE_MM = 0.15;
        double[,] pixel = new double[5, 2];
        double sign;
        bool resMeas = false;
        String speed = "Slow";        

        //Base Program Data Dir
        string myProgramDataPath;
        string mySingleCalibrationFile;

        //form objects
        frmMain myParent;
        CalibrationSingle cs = new CalibrationSingle();

        //to track mouse and keyboard events
        IKeyboardMouseEvents m_GlobalHook;
        List<Transparent_Form> tf = new List<Transparent_Form>();

        //for transfection
        int count;
        List<Point> pts = new List<Point>();

        public frmManualTransfect()
        {
            InitializeComponent();
            
        }

        public frmManualTransfect(frmMain _myParent)
        {
            InitializeComponent();
            myParent = _myParent;
            m_GlobalHook = Hook.GlobalEvents();

            
            myProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NFPE"); //Base Program Data Dir
            mySingleCalibrationFile = Path.Combine(myProgramDataPath, "CalibrationManual.json");

            if (!Directory.Exists(myProgramDataPath))
            {
                Directory.CreateDirectory(myProgramDataPath);
            }
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            myParent.myStageController.speed = speed;
            clickCt = 0;
            this.Hide();

            myParent.CloseSiApp();
            timerCalib.Start();
        }


        // calibration timer
        private void timerCalibration_Tick(object sender, EventArgs e)
        {
            timerCalib.Stop();
            m_GlobalHook.KeyPress += GlobalHookKeyPressCalib;
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExtCalib;
        }

        //handles keyboard input during calibration
        private void GlobalHookKeyPressCalib(object sender, KeyPressEventArgs e)
        {
            timerCalib.Stop();

            if (e.KeyChar == (char)Keys.Escape)
            {               
                clickCt = 0;
                myParent.myStageController.JogWithVelocity(0, 0, 0);
                myParent.myStageController.MoveRelative(0, 0, -0.03);                
                m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtCalib;
                myParent.myStageController.speed = myParent.OldSpeed;
                myParent.InitializeSiApp(myParent.Handle);
                this.Show();
            }
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtCalib;
                timerCalib.Start();
            }

        }

        //handles mouse input during calibration
        private void GlobalHookMouseDownExtCalib(object sender, MouseEventExtArgs e)
        {
            timerCalib.Stop();

            if (e.Button == MouseButtons.Left)
            {
                pixel[clickCt, 0] = e.X;
                pixel[clickCt, 1] = e.Y;

                if (clickCt < 4)
                {
                    if (clickCt < 2)
                    {
                        sign = Math.Pow(-1, clickCt);
                    }

                    else
                    {
                        sign = -Math.Pow(-1, clickCt);
                    }


                    if (clickCt % 2 == 0)
                    {
                        myParent.myStageController.MoveRelative(sign * CALIBRATION_DISTANCE_MM / (clickCt + 1), 0, 0);
                    }
                    else
                    {
                        myParent.myStageController.MoveRelative(0, -sign * CALIBRATION_DISTANCE_MM / clickCt, 0);
                    }

                    clickCt++;
                    m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;
                    m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtCalib;
                    timerCalib.Start();
                }
                else
                {
                    myParent.myStageController.JogWithVelocity(0, 0, 0);
                    myParent.myStageController.MoveRelative(0, 0, -0.03);
                    CalibrationSave();
                    clickCt = 0;
                    m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;
                    m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtCalib;
                    myParent.myStageController.speed = myParent.OldSpeed;
                    myParent.InitializeSiApp(myParent.Handle);
                    this.Show();
                }
            }
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtCalib;
                timerCalib.Start();
            }

        }

        //saves the pipette calibration parameters
        private void CalibrationSave()
        {
            cs.XX = 0;
            cs.XY = 0;
            cs.YX = 0;
            cs.YY = 0;

            for (int i = 0, j = 0; i < 3; i += 2, j++)
            {
                double alpha1 = pixel[i + 1, 0] - pixel[i, 0];
                double alpha2 = pixel[i + 1, 1] - pixel[i, 1];
                double alpha3 = pixel[i + 2, 0] - pixel[i + 1, 0];
                double alpha4 = pixel[i + 2, 1] - pixel[i + 1, 1];
                double cst = Math.Pow(-1, j) * CALIBRATION_DISTANCE_MM / ((i + 1) * (alpha4 * alpha1 - alpha2 * alpha3));
                cs.XX += cst * alpha4;
                cs.XY -= cst * alpha2;
                cs.YX -= cst * alpha3;
                cs.YY += cst * alpha1;
            }

            cs.XX /= 2;
            cs.XY /= 2;
            cs.YX /= 2;
            cs.YY /= 2;

            cs.ToJSONFile(mySingleCalibrationFile);
        }

        //calibration class
        private class CalibrationSingle
        {
            public double XX { get; set; }
            public double XY { get; set; }
            public double YX { get; set; }
            public double YY { get; set; }

            public string ToJSON()
            {
                return JsonConvert.SerializeObject(this); //no indentation
            }
            public CalibrationSingle FromJSON(string myJSON)
            {
                return (CalibrationSingle)JsonConvert.DeserializeObject(myJSON, typeof(CalibrationSingle));
            }
            public void ToJSONFile(string myFilePath)
            {
                System.IO.File.WriteAllText(myFilePath, ToJSON());
            }
            public CalibrationSingle FromJSONFile(string myFilePath)
            {
                return FromJSON(System.IO.File.ReadAllText(myFilePath));
            }
        }

        
        

        private void btnStart_Click(object sender, EventArgs e)
        {
            myParent.myStageController.speed = speed;
            count = 0;
            this.Hide();           
            
            myParent.CloseSiApp();
            timerTransfect.Start();            
            
        }
              

        //timer for transfection
        private void timerTransfect_Tick(object sender, EventArgs e)
        {
            timerTransfect.Stop();
            m_GlobalHook.KeyPress += GlobalHookKeyPressTransfect;
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExtTransfect;

        }

        //handles keyboard input during transfection
        private async void GlobalHookKeyPressTransfect(object sender, KeyPressEventArgs e)
        {
            timerTransfect.Stop();

            if (e.KeyChar == (char)Keys.Escape)
            {
                if (myParent.Transfect)
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            StopTransfection();

                        };

                        this.Invoke(del);

                    }
                    else
                    {
                        StopTransfection();
                    }

                    
                }

                for (int i = 0; i < count; i++)
                {
                    tf[i].Dispose();
                }
                tf.RemoveRange(0, count);
                pts.RemoveRange(0, count);
                count = 0;
                          
                myParent.myStageController.JogWithVelocity(0, 0, 0);
                myParent.myStageController.MoveRelative(0, 0, -0.03);
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtTransfect;
                myParent.myStageController.speed = myParent.OldSpeed;
                myParent.InitializeSiApp(myParent.Handle);
                this.Show();
            }
            else if (e.KeyChar == (char)Keys.Return)
            {
                if(!myParent.Transfect)
                {
                    for (int i = 0; i < count; i++)
                    {
                        tf[i].Dispose();
                        myParent.ptsTransfection.Add(pts[i]);
                    }

                    tf.RemoveRange(0, count);
                    pts.RemoveRange(0, count);
                    myParent.cellcount = count - 1;
                    count = 0;                 

                    Thread.Sleep(500);
                    await manualTransfect();
                    

                }

                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtTransfect;
                timerTransfect.Start();
            }
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtTransfect;
                timerTransfect.Start();
            }

        }

        //handles mouse input during transfection
        private void GlobalHookMouseDownExtTransfect(object sender, MouseEventExtArgs e)
        {
            timerTransfect.Stop();

            if (e.Button == MouseButtons.Left)
            {
                if (!myParent.Transfect)
                {
                    pts.Add(new Point(e.X, e.Y));
                    tf.Add(new Transparent_Form(count));
                    tf[count].Show();
                    tf[count].TopMost = true;
                    tf[count].Location = e.Location;
                    count++;
                }
                
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtTransfect;
                timerTransfect.Start();
            }
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExtTransfect;
                timerTransfect.Start();
            }

        }

        //routine for selecting cells and transfecting them manually
        private async Task manualTransfect()
        {
            if (File.Exists(mySingleCalibrationFile))
            {
                cs = cs.FromJSONFile(mySingleCalibrationFile);
                myParent.calibConst[0, 0] = cs.XX;
                myParent.calibConst[0, 1] = cs.XY;
                myParent.calibConst[1, 0] = cs.YX;
                myParent.calibConst[1, 1] = cs.YY;

                if (myParent.cellcount > 0)
                {
                    await Task.Run(() => StartTransfection());
                }

            }

            else
            {
                MessageBox.Show("Pipette Calibration Required!");
            }


        }

        //starts the transfection process
        private int StartTransfection()
        {
            myParent.Transfect = true;
            myParent.Start = true;
            return 1;
        }

        //stops the transfection process
        private int StopTransfection()
        {
            myParent.Start = false;
            myParent.myStageController.speed = myParent.OldSpeed;
            return 1;
        }
    }
}
