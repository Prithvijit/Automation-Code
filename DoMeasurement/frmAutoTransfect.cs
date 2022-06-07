using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SpinnakerNET;
using SpinnakerNET.GenApi;
using Emgu;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using System.IO;
using Gma.System.MouseKeyHook;

namespace DoMeasurement
{
    public partial class frmAutoTransfect : Form
    {
        //for calibration
        int Ct;
        const double CALIBRATION_DISTANCE_MM = 0.15;
        double[,] pixel = new double[5, 2];
        double sign;
        String speed = "Slow";
        CalibrationAuto cs = new CalibrationAuto();

        //Base Program Data Dir
        string myProgramDataPath;
        string mySingleCalibrationFile;

        // Camera Acquisition parameters
        bool capture = false;
        bool analysis = false;
        bool calibrate = false;
        Image<Bgr, byte> foranalysis;
        Image<Bgr, byte> forcalibration;
        Image<Bgr, byte> ImgOutput;
        Image<Bgr, byte> ImgROI; 
        string locationROI;
        CancellationTokenSource cts;

        //for pipette calibration
        double minVal;
        double maxVal;
        Point minLoc;
        Point maxLoc;

        //form objects
        frmMain myParent;

        //for keyboard key press
        IKeyboardMouseEvents m_GlobalHook;

        //for transfection        
        public List<PointF> pts = new List<PointF>();

        public frmAutoTransfect()
        {
            InitializeComponent();            
        }

        public frmAutoTransfect(frmMain _myParent)
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            myParent = _myParent;
            m_GlobalHook = Hook.GlobalEvents();

            myProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NFPE"); //Base Program Data Dir
            mySingleCalibrationFile = Path.Combine(myProgramDataPath, "CalibrationAuto.json");
            locationROI = Path.Combine(myProgramDataPath, "25.png");
            ImgROI = new Image<Bgr, byte>(locationROI);

            if (!Directory.Exists(myProgramDataPath))
            {
                Directory.CreateDirectory(myProgramDataPath);
            }
        }

        private async void btnLive_Click(object sender, EventArgs e)
        {
            capture = true;
            btnLive.Enabled = false;

            var result = await startAcquisition();

            capture = false;
            btnLive.Enabled = true;
            btnStop.Enabled = false;
            btnCapture.Enabled = false;
            btnCalibrate.Enabled = false;
            if (pctBoxLive.Image != null)
            {
                pctBoxLive.Image.Dispose();
            }

            pctBoxLive.Image = null;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            capture = false;
            btnLive.Enabled = true;
            btnStop.Enabled = false;
            btnCapture.Enabled = false;
            btnCalibrate.Enabled = false;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            analysis = true;
        }

        private async Task<int> startAcquisition()
        {
            // Retrieve singleton reference to system object
            ManagedSystem system = new ManagedSystem();

            // Retrieve list of cameras from the system
            IList<IManagedCamera> camList = system.GetCameras();

            // Finish if there are no cameras
            if (camList.Count == 0)
            {
                // Clear camera list before releasing system
                camList.Clear();

                // Release system
                system.Dispose();

                MessageBox.Show("Camera Absent!");

                return -1;
            }

            foreach (IManagedCamera managedCamera in camList)
                using (managedCamera)
                {
                    try
                    {
                        // Run example
                        await RunSingleCamera(managedCamera);
                    }
                    catch (SpinnakerException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

            // Clear camera list before releasing system
            camList.Clear();

            // Release system
            system.Dispose();

            return 0;

        }

        private async Task RunSingleCamera(IManagedCamera cam)
        {
            try
            {
                // Retrieve TL device nodemap and print device information
                INodeMap nodeMapTLDevice = cam.GetTLDeviceNodeMap();

                //result = PrintDeviceInfo(nodeMapTLDevice);

                // Initialize camera
                cam.Init();

                // Retrieve GenICam nodemap
                INodeMap nodeMap = cam.GetNodeMap();

                // Acquire images
                var result = await Task.Run(() => AcquireImages(cam, nodeMap, nodeMapTLDevice));

                // Deinitialize camera
                cam.DeInit();

            }
            catch (SpinnakerException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async Task<int> AcquireImages(IManagedCamera cam, INodeMap nodeMap, INodeMap nodeMapTLDevice)
        {

            try
            {
                // Set acquisition mode to continuous              
                // Retrieve enumeration node from nodemap
                IEnum iAcquisitionMode = nodeMap.GetNode<IEnum>("AcquisitionMode");
                if (iAcquisitionMode == null || !iAcquisitionMode.IsWritable)
                {
                    MessageBox.Show("Unable to set acquisition mode to continuous (node retrieval). Aborting...\n");
                    return -1;
                }

                // Retrieve entry node from enumeration node
                IEnumEntry iAcquisitionModeContinuous = iAcquisitionMode.GetEntryByName("Continuous");
                if (iAcquisitionModeContinuous == null || !iAcquisitionMode.IsReadable)
                {
                    MessageBox.Show("Unable to set acquisition mode to continuous (enum entry retrieval). Aborting...\n");
                    return -1;
                }

                // Set symbolic from entry node as new value for enumeration node
                iAcquisitionMode.Value = iAcquisitionModeContinuous.Symbolic;

                // Begin acquiring images               
                cam.BeginAcquisition();

                //enable the other buttons once acquisition starts
                enableButtons();

                do
                {
                    try
                    {
                        // Retrieve next received image                       

                        using (IManagedImage rawImage = cam.GetNextImage())
                        {
                            // Ensure image completion                           

                            if (rawImage.IsIncomplete)
                            {
                                MessageBox.Show("Image incomplete");
                            }
                            else
                            {
                                // Convert image to mono 8

                                using (IManagedImage convertedImage = rawImage.Convert(PixelFormatEnums.Mono8))
                                {
                                    // display image 

                                    updatePictureBoxLive(convertedImage, cts.Token);

                                    if (analysis == true)
                                    {
                                        analysis = false;
                                        foranalysis = new Image<Bgr, byte>(convertedImage.bitmap);
                                        createAnalysisForm();

                                    }

                                    if(calibrate == true)
                                    {
                                        
                                        forcalibration = new Image<Bgr, byte>(convertedImage.bitmap);
                                        await Task.Run(() => CalibratePipette());
                                        
                                       
                                    }




                                }
                            }
                        }
                    }
                    catch (SpinnakerException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                } while (capture != false);


            }
            catch (SpinnakerException ex)
            {
                MessageBox.Show(ex.Message);
            }

            //end acquisition
            cam.EndAcquisition();

            return 0;
        }

        private void updatePictureBoxLive(IManagedImage img, CancellationToken token)
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    if (pctBoxLive.Image != null)
                    {
                        pctBoxLive.Image.Dispose();
                    }

                    pctBoxLive.Image = new Bitmap(img.bitmap);
                    pctBoxLive.SizeMode = PictureBoxSizeMode.Zoom;

                };

                if (token.IsCancellationRequested)
                {
                    capture = false;
                }
                else
                {
                    try
                    {
                        this.Invoke(del);
                    }
                    catch (ObjectDisposedException)
                    {
                        capture = false;
                    }
                }


            }
            else
            {
                if (pctBoxLive.Image != null)
                {
                    pctBoxLive.Image.Dispose();

                }
                pctBoxLive.Image = new Bitmap(img.bitmap);
                pctBoxLive.SizeMode = PictureBoxSizeMode.Zoom;
            }


        }

        private void createAnalysisForm()
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    frmIpParameters frmIp = new frmIpParameters(foranalysis, this);
                    frmIp.Show();

                };

                this.Invoke(del);

            }
            else
            {
                frmIpParameters frmIp = new frmIpParameters(foranalysis, this);
                frmIp.Show();
            }


        }

        private void enableButtons()
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    btnStop.Enabled = true;
                    btnCapture.Enabled = true;
                    btnCalibrate.Enabled = true;

                };

                this.Invoke(del);

            }
            else
            {
                btnStop.Enabled = true;
                btnCapture.Enabled = true;
                btnCalibrate.Enabled = true;
            }


        }

        private void frmCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (capture == true)
            {
                capture = false;
                cts.Cancel();
            }

        }


        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            myParent.myStageController.speed = speed;
            Ct = 0;
            calibrate = true;
            timerCalib.Start();
        }

        private int CalibratePipette()
        {
            if (true)
            {
                Image<Bgr, byte> temp = forcalibration.CopyBlank();

                //Image<Bgr, byte> temp2 = imgInput.CopyBlank();
                Image<Bgr, byte> tempROI = ImgROI.CopyBlank();
                forcalibration.CopyTo(temp);
                //instantiate output
                ImgOutput = new Image<Bgr, byte>(forcalibration.Size);
                forcalibration.CopyTo(ImgOutput);
                //imgInput.CopyTo(temp2);
                ImgROI.CopyTo(tempROI);

                //normalize image
                Emgu.CV.Util.VectorOfDouble mean = new Emgu.CV.Util.VectorOfDouble();
                Emgu.CV.Util.VectorOfDouble std = new Emgu.CV.Util.VectorOfDouble();
                Emgu.CV.Util.VectorOfDouble meanROI = new Emgu.CV.Util.VectorOfDouble();
                Emgu.CV.Util.VectorOfDouble stdROI = new Emgu.CV.Util.VectorOfDouble();

                CvInvoke.MeanStdDev(temp, mean, std);
                CvInvoke.MeanStdDev(tempROI, meanROI, stdROI);

                //temp = (temp - mean[0]) / std[0];
                //ROIimg = (ROIimg - meanROI[0]) / stdROI[0];

                CvInvoke.Normalize(temp, temp, 255, 0, NormType.MinMax);
                CvInvoke.Normalize(tempROI, ImgROI, 255, 0, NormType.MinMax);
                //pre-process images with gradient filters
                CvInvoke.Sobel(temp, temp, DepthType.Cv8U, 1, 0, 3);
                CvInvoke.Sobel(tempROI, tempROI, DepthType.Cv8U, 1, 0, 3);

                //template matching of pippette

                CvInvoke.MatchTemplate(temp, tempROI, temp, TemplateMatchingType.Ccoeff);
                //draw output


                CvInvoke.MinMaxLoc(temp, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                //textBox_Nmatch.Text = Convert.ToString(maxVal / maxVal_ROI);      //normalized value
                //textBox_match.Text = Convert.ToString(maxVal);                  //max value output

                Point TopLeft = new Point(maxLoc.X, maxLoc.Y);
                Point BotRight = new Point(maxLoc.X + ImgROI.Cols, maxLoc.Y + ImgROI.Rows);
                Point Pippete = new Point(maxLoc.X + ImgROI.Cols / 2, maxLoc.Y + ImgROI.Rows);
                pixel[Ct, 0] = Pippete.X;
                pixel[Ct, 1] = Pippete.Y;

                if (Ct < 4)
                {
                    if (Ct < 2)
                    {
                        sign = Math.Pow(-1, Ct);
                    }

                    else
                    {
                        sign = -Math.Pow(-1, Ct);
                    }


                    if (Ct % 2 == 0)
                    {
                        myParent.myStageController.MoveRelative(sign * CALIBRATION_DISTANCE_MM / (Ct + 1), 0, 0);
                    }
                    else
                    {
                        myParent.myStageController.MoveRelative(0, -sign * CALIBRATION_DISTANCE_MM / Ct, 0);
                    }

                    Ct++;                    
                }
                else
                {
                    myParent.myStageController.JogWithVelocity(0, 0, 0);
                    myParent.myStageController.MoveRelative(0, 0, -0.03);
                    myParent.ptsTransfection = pts;
                    myParent.cellcount = pts.Count;
                    myParent.ptsTransfection.Insert(0, new PointF((float)pixel[4, 0], (float)pixel[4, 1]));
                    CalibrationSave();
                    Ct = 0;                    
                    myParent.myStageController.speed = myParent.OldSpeed;
                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            myParent.InitializeSiApp(myParent.Handle);

                        };

                        this.Invoke(del);

                    }
                    else
                    {
                        myParent.InitializeSiApp(myParent.Handle);
                    }
                    
                    calibrate = false;
                }

                Rectangle MatchRect = new Rectangle(maxLoc.X, maxLoc.Y, ImgROI.Cols, ImgROI.Rows);
                CvInvoke.Rectangle(ImgOutput, MatchRect, new MCvScalar(255, 255, 255), 1);
                CvInvoke.Circle(ImgOutput, Pippete, 3, new MCvScalar(255, 0, 0), -1);

                updatePictureBoxCalibrate(ImgOutput);

                Thread.Sleep(1000);
                //ROI for the template match
               // forcalibration.ROI = MatchRect;
                //Image<Bgr, byte> temp2 = forcalibration.CopyBlank();
                //forcalibration.CopyTo(temp2);
                //forcalibration.ROI = Rectangle.Empty;
                //ROIimg2 = temp2;
                //temp2.ROI = MatchRect;

               // pictureBox2.Image = ROIimg2.Bitmap;
               // pictureBox1.Image = imgOutput.Bitmap;

            }
            return 1;
        }

        private void updatePictureBoxCalibrate(Image<Bgr, byte> img)
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    if (pctBoxCalibrate.Image != null)
                    {
                        pctBoxCalibrate.Image.Dispose();
                    }

                    pctBoxCalibrate.Image = new Bitmap(img.Bitmap);
                    pctBoxCalibrate.SizeMode = PictureBoxSizeMode.Zoom;

                };

                this.Invoke(del);

            }
            else
            {
                if (pctBoxCalibrate.Image != null)
                {
                    pctBoxCalibrate.Image.Dispose();

                }
                pctBoxCalibrate.Image = new Bitmap(img.Bitmap);
                pctBoxCalibrate.SizeMode = PictureBoxSizeMode.Zoom;
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
        private class CalibrationAuto
        {
            public double XX { get; set; }
            public double XY { get; set; }
            public double YX { get; set; }
            public double YY { get; set; }

            public string ToJSON()
            {
                return JsonConvert.SerializeObject(this); //no indentation
            }
            public CalibrationAuto FromJSON(string myJSON)
            {
                return (CalibrationAuto)JsonConvert.DeserializeObject(myJSON, typeof(CalibrationAuto));
            }
            public void ToJSONFile(string myFilePath)
            {
                System.IO.File.WriteAllText(myFilePath, ToJSON());
            }
            public CalibrationAuto FromJSONFile(string myFilePath)
            {
                return FromJSON(System.IO.File.ReadAllText(myFilePath));
            }
        }

        //routine for starting cell transfection
        private async Task autoTransfect()
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

        private void btnTransfect_Click(object sender, EventArgs e)
        {
            StartTransfection();
            timerTransfect.Start();
        }        

        private void timerCalib_Tick(object sender, EventArgs e)
        {
            timerCalib.Stop();
            m_GlobalHook.KeyPress += GlobalHookKeyPressCalib;
        }

        //handles keyboard input during calibration
        private void GlobalHookKeyPressCalib(object sender, KeyPressEventArgs e)
        {
            timerCalib.Stop();

            if (e.KeyChar == (char)Keys.Escape)
            {
                Ct = 0;
                calibrate = false;
                myParent.myStageController.JogWithVelocity(0, 0, 0);
                myParent.myStageController.MoveRelative(0, 0, -0.03);
                m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;                
                myParent.myStageController.speed = myParent.OldSpeed;
                myParent.InitializeSiApp(myParent.Handle);                
            }
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressCalib;                
                timerCalib.Start();
            }

        }

        private void timerTransfect_Tick(object sender, EventArgs e)
        {
            timerTransfect.Stop();
            m_GlobalHook.KeyPress += GlobalHookKeyPressTransfect;
        }

        //handles keyboard input during transfection
        private void GlobalHookKeyPressTransfect(object sender, KeyPressEventArgs e)
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
                               
                pts.RemoveRange(0, pts.Count);                

                myParent.myStageController.JogWithVelocity(0, 0, 0);
                myParent.myStageController.MoveRelative(0, 0, -0.03);
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;                
                myParent.myStageController.speed = myParent.OldSpeed;
                myParent.InitializeSiApp(myParent.Handle);                
            }
            
            else
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPressTransfect;                
                timerTransfect.Start();
            }

        }
    }
}
