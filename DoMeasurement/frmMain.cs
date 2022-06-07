using SerialPortLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using Gma.System.MouseKeyHook;
using System.IO;
using System.Xml.Serialization;
using System.Timers;

namespace DoMeasurement
{
    public partial class frmMain : Form
    {
        const double MOVEUP = 0.03;             // Retract tip this amount in mm after contact
        const int MAXCELLSELECTS = 1000;        // Maximum amount of cells to select
        const bool XYZSTAGE = true;             // Define whether an XYZ stage should be connected
        const bool THUMBAR = false;             // This enables experimental code
        const bool DEMO = false;                // This enables demo version
        const bool CORRECTION = false;          // This enables experimental code
        const bool CHART = true;                // This enables a chart of the resistance for a single customer only
        const bool NOSERIAL = false;            // Set as "true", this says that there is no serial communication
        const double E_PI = Math.PI;
        const int MEAS_RATE = 150;              // Resistance measurement rate in Hz
        //const double CYCLE_DELAY_MS = 2.0;    // Add delay between resistance measurements, no longer used
        //const double PULSE_WIDTH = 5.0;       // Add delay between resistance measurements, no longer used
        const double CRASH_DISTANCE_MM = 0.003;
        const bool CRASH_PROTECTION = false;
        const bool CRASH_PROTECTION2 = true;
        // Filter1 and filter2 tau values
        const double TAU1 = 5;
        const double TAU2 = 0.5;
        const double MS_TO_S = 0.001;
        const double THRESHOLD = 2.4;
        const double FRACVAR = 0.012;
        const double LAMBDA = 0.0115;
        // The below code sets the number of points that will be included in two rolling means
        int FILTER_LENGTH = (int)Math.Round(0.4 / (1 / (double)MEAS_RATE)); //60
        int FILTER_LENGTH2 = (int)Math.Round(1.6 / (1 / (double)MEAS_RATE)); //240
        // This is the minimum amount of measurements before a red or green light is displayed
        int NUM_MEASUREMENTS = ((int)Math.Round(8 / (1 / (double)MEAS_RATE))); //1200
        int NUMPOINT_DIFF = (int) Math.Round(0.4 / (1 / (double) MEAS_RATE)); //60
        int STDNUM = (int)Math.Round(2.4 / (1 / (double)MEAS_RATE)); //360
        double LastMean;
        double ShortMean;
        double LongMean;
        double LastFilterValueZ = 0;

        //bool goingDown = false;
        bool joystickOn = true;        
        public string OldSpeed = "Medium";
        int detected = 0;
        bool detect = false;
        bool alarm = true;
        int PauseTime = 1000;

        SimpleMovingAverage ShortMeanMovingAvg;
        SimpleMovingAverage LongMeanMovingAvg;

        bool StartStop = false;
        bool timeInit = false;
        Stopwatch timer = Stopwatch.StartNew();
        bool AutoContact = false;
        bool AutoMode = false;
        List<long> LastElapsedTime;
        double StopTime = 0;
        double IntervalTime = 0;
        double LastOutOfRange = 0;
        int MeasurementCount = 0;
        List<double> xvals1;
        List<double> yvals1;
        double LastRunTimeQuartSeconds = 0;
        List<double> ResistanceListForPlot;
        public bool Light;

        int measTarget1 = 50;
        int NumPlotPoints = 60;

        public int cellcount = 0;
        bool pulsing = false;
        public bool Transfect { get; set; } = false;
        public bool Next { get; set; } = true;
        public List<PointF> ptsTransfection = new List<PointF>();
        double alpha1;
        double alpha2;
        double beta1;
        double beta2;
        public double[,] calibConst = new double[2, 2]; 

        EBoxController myEBox = new EBoxController();
        string myEboxPort;

        public StageController myStageController = new StageController();
        bool isReplayMode = false; //should data come from data read from file?

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            myEboxPort = myEBox.GetFirstComPort(); //e.g. "COM23" -- this is the first FTDI port found

            //LastElapsedTime = new double[NUMPOINT_DIFF];

            InitializeSiApp(this.Handle); // set up joystick communication

            ShortMeanMovingAvg = new SimpleMovingAverage(FILTER_LENGTH);
            LongMeanMovingAvg = new SimpleMovingAverage(FILTER_LENGTH2);

            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    ConfigureChart(chartMeas1);

                };

                this.Invoke(del);

            }
            else
            {
                ConfigureChart(chartMeas1);
            }

            

            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("time")); //Set up the columns of the data dump spreadsheet
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("raw"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("Z filter"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("ShortMean"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("LongMean"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("STD*Thresh"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("HPF"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("LPF"));
            ManagedGlobals.resistanceSet.Columns.Add(new DataColumn("Detects"));

            myStageController.portName = myStageController.GetFirstComPort(); //<================================ Set This!
            if (!myStageController.Connect())
            {
                MessageBox.Show("Could not connect to Stage Controller", "Com Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
        }

        public enum TransfectionMode
        {
            Manual,
            Auto,
            Array
        }

        public TransfectionMode Mode { get; set; }

        public bool Start {
            get
            {
                return StartStop;
            }
            set
            {
                StartStop = value;
                if (StartStop == true)
                {
                    //Start
                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            ConfigureChart(chartMeas1);

                        };
                        this.Invoke(del);
                    }
                    else
                    {
                        ConfigureChart(chartMeas1);
                    }

                    xvals1 = new List<double>();
                    yvals1 = new List<double>();
                    LastRunTimeQuartSeconds = 0;
                    ResistanceListForPlot = new List<double>();
                    timeInit = false;

                    LastElapsedTime = new List<long>(); //in milliseconds

                    this.Invalidate();

                    if (timeInit == false) { timer.Reset(); timeInit = true; }
                    if(Transfect && !pulsing)
                    {
                        AutoContact = true;
                    }

                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            btnStartStop.Text = "Stop";

                        };
                        this.Invoke(del);
                    }
                    else
                    {
                        btnStartStop.Text = "Stop";
                    }

                    
                    if (!AutoMode && !ManagedGlobals.continueMeasurement)
                    {
                        // Start the measurement when this button is pressed
                        startMeas();
                        timer.Start();
                    }

                    //detectAndButtonSwitch();
                }
                else if (StartStop == false)
                {
                    //MainWindow::MainForm::Light = false;
                    stopMeas();

                    if(Transfect && !pulsing)
                    {
                        AutoContact = false;
                        Transfect = false;
                        ptsTransfection.RemoveRange(0, cellcount + 1);
                        Next = true;
                        InitializeSiApp(this.Handle);
                    }

                    if (InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            btnStartStop.Text = "Start";

                        };

                        this.Invoke(del);

                    }
                    else
                    {
                        btnStartStop.Text = "Start";
                    }

                    
                    this.Invalidate();
                    //this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &MainForm::MainForm_Paint2);
                    //detectAndButtonSwitch();
                }
            }
        }

        public void MeasurementReceived(double value)
        {
            MeasurementReceived(value, timer.ElapsedMilliseconds);
        }

        public void MeasurementReceived(double value, long myElapsedTime)
        {
            //This is the main callback function

            bool stop = false;

            if (stop || double.IsNaN(value))
            {
                if (!isReplayMode)
                    myEBox.CancelMeasurements = true;
            }
            else
            {

                Console.WriteLine("     " + value + "      <=============== MEASUREMENT REPLY");

                //Add it to measure data table and to plot

                if (processMeasurement(value, myElapsedTime))
                {
                    //detection occurred
                    SystemSounds.Beep.Play();

                }

            }

        }

        private void startMeas()
        {
            if (!ManagedGlobals.continueMeasurement)
            {
                ManagedGlobals.continueMeasurement = true;
                if (!this.Start)
                {
                    this.Start = true;
                }

                arrayFilterValueZ = new double[NUMPOINT_DIFF];

                if (isReplayMode && !InvokeRequired)
                {
                    //Data will come from a saved file instead of the instrument
                    //Ask for the file to read:

                    var fileContent = string.Empty;
                    var filePath = string.Empty;

                    string dataFilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NFPE", "Data");
                    if (!Directory.Exists(dataFilesPath))
                        Directory.CreateDirectory(dataFilesPath);


                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = dataFilesPath;
                        openFileDialog.Filter = "Data files (*.xml)|*.xml|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Get the path of specified file
                            filePath = openFileDialog.FileName;
                            //DataReader.MeasurementSet myMeasurementSet = myDataReader.GetMeasurementFile(filePath);
                            //if (myMeasurementSet == null)
                            //    return;
                        }
                    }
                    StreamData(filePath, 1, this); //expects a method named MeasurementReceived for callback

                }

                else
                 if (!isReplayMode)
                 {

                    if (!myEBox.IsConnected())
                        myEBox.Connect(myEboxPort, 115200);

                    if (!myEBox.IsConnected())
                    {
                        Console.WriteLine("Could not connect!");
                    }
                    else
                    {
                        //myEBox.StopMeasurement();
                        //Thread.Sleep(250);
                        myEBox.ClearBuffer();
                        myEBox.msTimeOut = 50; //timeout in ms

                        //Reinit the chart:



                        myEBox.StartMeasurement();

                        //myEBox.GetStreamedMeasurements(MeasurementReceived); //set up a callback for received data
                        myEBox.StartStreamedMeasurements(MeasurementReceived); //set up a callback for received data

                    }
                }
            }
        }

        private void stopMeas()
        {
            if (isReplayMode)
            {
                StopDataStreaming();
            }
            else
            {
                myEBox.StopStreamedMeasurements(); //stop callbacks on received measurement data
            }
            if (ManagedGlobals.continueMeasurement)
            {
                if (this.Start)
                {
                    this.Start = false;
                }

                ManagedGlobals.continueMeasurement = false;
                MeasurementCount = 0;

                if (!isReplayMode)
                {
                    if (myEBox.IsConnected())
                    {
                        myEBox.StopMeasurement();
                        myEBox.Disconnect();
                    }
                }
            }
            this.timer.Stop();
            //elapsedMilliseconds = this;
            StopTime = (double)timer.ElapsedMilliseconds;
            //MainForm measurementCount = this;
            //measurementCount.Stopped = (double)measurementCount.MeasurementCount;
        }

        double FilterValueX = 0;
        double FilterValueY = 0;
        double FilterValueZ = 0;
        double[] arrayFilterValueZ;

        private bool processMeasurement(double resistance, long ElapsedTime)
        {
            //return true on detect

            if (ResistanceListForPlot == null)
                return false;

            ResistanceListForPlot.Add(resistance);

            //Plot the raw resistance data and ShortMean <==============================================================

            //Now update rolling resistance plot:
            //long ElapsedTime = timer.ElapsedMilliseconds;
            LastElapsedTime.Add(ElapsedTime);
            double  RunTimeQuartSeconds = timer.ElapsedMilliseconds / 250;

            //return true;

            // Calculate ancillary values & store to workRow and test for detection ==================================================

            //double resistance = double.NaN;
            int num;
            //string resistanceStr = null;
            DataRow workRow = null;
            workRow = ManagedGlobals.resistanceSet.NewRow();
            //double FilterValueX = 0;
            //double FilterValueY = 0;
            //double FilterValueZ = 0;

            LastMean = ShortMean;
            LongMeanMovingAvg.AddSample(resistance); //add to the buffer
            ShortMeanMovingAvg.AddSample(resistance); //add to the buffer
            LongMean = LongMeanMovingAvg.Average;
            ShortMean = ShortMeanMovingAvg.Average;

            //Update the plot
            if (RunTimeQuartSeconds > LastRunTimeQuartSeconds)
            {
                //plot the average of the last N measurements:
                double AvgResistance = ResistanceListForPlot.Average();

                xvals1.Add(RunTimeQuartSeconds / 4);
                yvals1.Add(AvgResistance / 1000000);

                MethodInvoker del = delegate { UpdateChart(1); };
                this.Invoke(del);

                LastRunTimeQuartSeconds = RunTimeQuartSeconds;
                ResistanceListForPlot.Clear();
            }

            //---------------------------------------------------------------------------------------------------------

            if (this.MeasurementCount <= FILTER_LENGTH2)
            {
                FilterValueY = 0;
                FilterValueX = 0;
                workRow[2] = 0;
            }
            else
            {
                //AlgorithmVars.CalculateFilterValue(MeasurementCount, ShortMeanMovingAvg.Average, LastMean, ElapsedTime, LastElapsedTime, LongMeanMovingAvg.Average);
                //Calculate Filter Values----------------------------
                double _FilterValueY = FilterValueY;
                double _FilterValueX = FilterValueX;



                //FilterValueY = TAU1 / (TAU1 + ElapsedTime * MS_TO_S - LastElapsedTime[MeasurementCount % NUMPOINT_DIFF] * MS_TO_S) * (_FilterValueY + ShortMean - LastMean);
                //FilterValueX = _FilterValueX + (ElapsedTime * MS_TO_S - LastElapsedTime[MeasurementCount % NUMPOINT_DIFF] * MS_TO_S) / (TAU2 + ElapsedTime * MS_TO_S - LastElapsedTime[(MeasurementCount) % NUMPOINT_DIFF] * MS_TO_S) * (FilterValueY - FilterValueX);
                double myTimeInterval = (LastElapsedTime[MeasurementCount] - LastElapsedTime[MeasurementCount - 1]) * MS_TO_S;
                FilterValueY = 5 / (5 + myTimeInterval) * (_FilterValueY + ShortMean - LastMean);
                FilterValueX = _FilterValueX + (myTimeInterval / (0.5 + myTimeInterval)) * (FilterValueY - _FilterValueX);

                FilterValueZ = FilterValueY - FilterValueX;

                //double dRdt = (FilterValueZ - LastFilterValueZ) / (ElapsedTime * MS_TO_S - LastElapsedTime[(MeasurementCount + 2) % NUMPOINT_DIFF] * MS_TO_S) / LongMean;

                arrayFilterValueZ[MeasurementCount % NUMPOINT_DIFF] = FilterValueZ;
                double dRdt = ((arrayFilterValueZ[MeasurementCount % NUMPOINT_DIFF] - arrayFilterValueZ[(MeasurementCount + 1) % NUMPOINT_DIFF]) / (ElapsedTime * MS_TO_S - LastElapsedTime[(MeasurementCount + 2) % NUMPOINT_DIFF] * MS_TO_S)) / LongMean;

                AlgorithmVars.UpdateFilterValues(MeasurementCount, FilterValueY, FilterValueX, FilterValueZ, dRdt);

                //--------------------------------------------------
                workRow[2] = AlgorithmVars.FilterValueZ[MeasurementCount % NUMPOINT_DIFF];
                //if (MeasurementCount > 360)
                //    workRow[2] = FilterValueZ; // % FILTER_LENGTH2;
                //else
                //    workRow[2] = 0;

                if (!isReplayMode && Transfect)
                {
                    if (detected >= cellcount)
                    {
                        detected = 0;
                        cellcount = 0;
                        ptsTransfection.RemoveRange(0, cellcount + 1);                        
                        Start = !Start;
                    }

                    if (Next)
                    {
                        //move to the next cell
                        Next = false;
                        moveToNextCell();
                    }

                }


                if (isReplayMode)
                    AutoContact = true;

                if (AutoContact)
                {
                    int cond1 = FilterValueZ <= THRESHOLD * AlgorithmVars.STD ? 0 : 1;
                    int cond2 = FilterValueZ <= FRACVAR * LongMean ? 0 : 1;
                    int cond3 = dRdt <= LAMBDA ? 0 : 1;
                    int cond4 = ElapsedTime <= this.StopTime + 1000 ? 0 : 1;
                    num = (0 != 0 || ManagedGlobals.goingDown && 1 != 0 ? 1 : 0);
                    int cond5 = num;
                    if (isReplayMode)
                        cond5 = 1;
                    if (cond1 + cond2 + cond3 >= 2 && cond4 != 0 && cond5 != 0)
                    {
                        if (!isReplayMode)
                        {

                            myStageController.JogWithVelocity(0, 0, 0); //<=============================================================================================
                            if (joystickOn)
                            {
                                Thread.Sleep(500);
                            }
                            ManagedGlobals.goingDown = false;
                            this.timer.Stop();

                            SystemSounds.Beep.Play();
                            if (!isReplayMode)
                                myEBox.StopMeasurement();
                            StopTime = (double)timer.ElapsedMilliseconds;
                            //MeasurementCount = this;
                            //measurementCount.Stopped = (double)measurementCount.MeasurementCount;
                            ManagedGlobals.continueMeasurement = false;
                        }
                        detect = true;
                    }
                }
            }

            //---------------------------------------------------------------------------------------------------------

            /*
            if (ShortMeanMovingAvg.Count == FILTER_LENGTH)
            {
                ShortMeanMovingAvg.ClearSamples();
            }
            if (LongMeanMovingAvg.Count == FILTER_LENGTH2)
            {
                LongMeanMovingAvg.ClearSamples();
            }
            */

//            this.resistanceChart.Series["Resistance"].Points.AddXY(ElapsedTime * 0.001, resistance);
//            this.resistanceChart.Series["Filter1"].Points.AddXY(ElapsedTime * 0.001, ShortMean);
            //MainForm elapsedMilliseconds1 = this;
            IntervalTime = (double)timer.ElapsedMilliseconds * 0.001 / (double)(this.MeasurementCount + 1);
            if (0.00666666666666667 > this.IntervalTime)
            {
                Thread.Sleep((int)((0.0133333333333333 - this.IntervalTime) / 0.001));
            }
            workRow[0] = ElapsedTime * 0.001;
            workRow[1] = resistance;
            workRow[3] = ShortMean;
            workRow[4] = LongMean;
            workRow[5] = AlgorithmVars.STD * THRESHOLD;
            workRow[6] = FilterValueY;
            workRow[7] = FilterValueX;
            workRow[8] = detect ? 1 : 0;
            ManagedGlobals.resistanceSet.Rows.Add(workRow.ItemArray);
            AlgorithmVars.CalculateStandardDeviation(this.MeasurementCount);
            if (this.MeasurementCount >= NUM_MEASUREMENTS)
            {
                if (AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] < -(LongMean * 0.02) || AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] > LongMean * 0.02 || ShortMean > 180000000)
                {
                    //MainForm measurementCount1 = this;
                    LastOutOfRange = (double)MeasurementCount;
                }
                if ((AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] < -(LongMean * 0.02) || AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] > LongMean * 0.02 || ShortMean > 180000000) && this.Light)
                {
                    this.Light = false;
                }
                else if (AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] > -(LongMean * 0.02) && AlgorithmVars.FilterValueZ[this.MeasurementCount % NUMPOINT_DIFF] < LongMean * 0.02 && ShortMean < 180000000 && (double)this.MeasurementCount > this.LastOutOfRange + (double)(FILTER_LENGTH2) && !this.Light)
                {
                    this.Light = true;
                }
            }
            else
                LastFilterValueZ = FilterValueZ;
//            if (this.MeasurementCount > NUM_MEASUREMENTS)
//            {
//                this.resistanceChart.Series["Resistance"].Points.RemoveAt(0);
//                this.resistanceChart.Series["Filter1"].Points.RemoveAt(0);
//                this.resistanceChart.ResetAutoValues();
//            }
            //MainForm mainForm1 = this;
            //mainForm1.MeasurementCount = mainForm1.MeasurementCount + 1;
            MeasurementCount += 1;

            if (this.detect)
            {
                //MainForm mainForm2 = this;
                //mainForm2.detected = mainForm2.detected + 1;
                detected += 1;                
                int num1 = this.detected;
                //this.label1.Text = num1.ToString();
                this.detect = false;
                if (!isReplayMode)
                    this.sendPulse();
                Next = true;

                if (this.AutoMode)
                {
                    return false;
                }
                if (this.alarm)
                {
                    return false;
                }
                return true;
            }

            return false;

        }

        private void moveToNextCell()
        {
            alpha1 = ptsTransfection[detected + 1].X - ptsTransfection[detected].X;
            alpha2 = ptsTransfection[detected + 1].Y - ptsTransfection[detected].Y;

            beta1 = calibConst[0, 0] * alpha1 + calibConst[0, 1] * alpha2;
            beta2 = calibConst[1, 0] * alpha1 + calibConst[1, 1] * alpha2;

            myStageController.MoveRelative(beta1, beta2, 0);
            Thread.Sleep(1300);
            myStageController.JogWithVelocity(0, 0, 75);
        }


        private void sendPulse()
        {
            bool prevSetting = ManagedGlobals.continueMeasurement;

            if (!isReplayMode)
                myEBox.StopMeasurement();
            pulsing = true;
            if (prevSetting)
            {
                this.Start = (this.Start ? false : true);
            }
            this.timer.Stop();
            StopTime = (double)timer.ElapsedMilliseconds;
            //measurementCount.Stopped = (double)measurementCount.MeasurementCount;
            double timeDuringPulse = (double.Parse(ManagedParameters.numPulse) + double.Parse(ManagedParameters.restTime)) * 1000 * double.Parse(ManagedParameters.numTrains) / double.Parse(ManagedParameters.pulseFreq);

            if (!isReplayMode)
                myEBox.SendPulse(ManagedParameters.pulseType);
            Thread.Sleep((timeDuringPulse >= 500 ? (int)Math.Round(timeDuringPulse) : 500) + this.PauseTime);

            SystemSounds.Beep.Play();
            pulsing = false;
            if (prevSetting)
            {
                this.Start = (this.Start ? false : true);
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            Start = !Start;
        }


        // Charting =============================================================================================================

        private void UpdateChart(int chartNum)
        {
            double LastAvgReading = Math.Round(yvals1.LastOrDefault(), 2);
            lblAvgResistance.Text = LastAvgReading.ToString("0.00");

            lblShortMean.Text = (ShortMean/1000000).ToString("0.00");

            Chart chart;
            int measTarget;
            List<double> xvals;
            List<double> yvals;

            chart = chartMeas1;
            measTarget = measTarget1;
            xvals = xvals1;
            yvals = yvals1;
            if (xvals.Count < 1) return; //need at least 1 data point

            if (xvals.Count > NumPlotPoints)
            {
                chart.ChartAreas[0].AxisX.Maximum = xvals[xvals.Count - 1];
                chart.ChartAreas[0].AxisX.Minimum = xvals[(xvals.Count - NumPlotPoints)];
            }
            else
            {
                chart.ChartAreas[0].AxisX.Maximum = 15;
                chart.ChartAreas[0].AxisX.Minimum = 0.25;
            }

            // bind the datapoints
            chart.Series["Series1"].Points.DataBindXY(xvals, yvals);

            // draw!
            chart.Invalidate();
        }
        private void ConfigureChart(Chart myChart)
        {
            // set up some data
            List<int> xvals = new List<int> { 0 };
            List<int> yvals = new List<int> { 0 };

            // create the chart
            var chart = myChart; // new Chart();
            //chart.Size = new Size(800, 900);

            var chartArea = chart.ChartAreas[0];
            //var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = "F0";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            //chart.ChartAreas.Add(chartArea);

            chart.ChartAreas[0].AxisX.Maximum = 15;
            chart.ChartAreas[0].AxisX.Minimum = 0.25;

            chart.ChartAreas[0].AxisX.IntervalOffset = -0.25;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0; //do not show
            chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart.ChartAreas[0].AxisX.MinorTickMark.Enabled = true;
            chart.ChartAreas[0].AxisX.MinorTickMark.Interval = 1;
            chart.ChartAreas[0].AxisX.MinorTickMark.IntervalOffset = -0.25;
            chart.ChartAreas[0].AxisX.MajorTickMark.LineWidth = 1;
            chart.ChartAreas[0].AxisX.MajorTickMark.TickMarkStyle = TickMarkStyle.OutsideArea;
            chart.ChartAreas[0].AxisX.MajorTickMark.Size = 2;
            chart.ChartAreas[0].AxisY.Interval = 5;
            chart.ChartAreas[0].AxisY.Minimum = 5; //840
            chart.ChartAreas[0].AxisY.Maximum = 100; //855

            var series = new Series();
            series.Name = "Series1";
            series.ChartType = SeriesChartType.Line;
            //series.XValueType = ChartValueType.Int32;
            series.XValueType = ChartValueType.Double;

            chart.Series.Clear();

            chart.Series.Add(series);
            chart.Series["Series1"].BorderWidth = 3;
            //chart.Series["Series1"].Color = Color.Black;
            chart.Series["Series1"].MarkerColor = Color.Black;
            chart.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
            chart.Series["Series1"].MarkerSize = 7;

            // bind the datapoints
            chart.Series["Series1"].Points.DataBindXY(xvals, yvals);

            //Control Line
            //================================
            // Find point with maximum Y value 
            DataPoint maxValuePoint = chart.Series[0].Points.FindMaxByValue();
            chart.ChartAreas[0].AxisY.StripLines.Add(
                new StripLine()
                {
                    BorderColor = Color.Red,
                    BorderWidth = 2,
                    //IntervalOffset = maxValuePoint.YValues[0],
                    IntervalOffset = measTarget1 + 40, //maxValuePoint.YValues[0],
                    //Text = "Max Value"
                });

            // Find point with minimum Y value 
            DataPoint minValuePoint = chart.Series[0].Points.FindMinByValue();
            chart.ChartAreas[0].AxisY.StripLines.Add(
                new StripLine()
                {
                    BorderColor = Color.Red,
                    BorderWidth = 2,
                    //IntervalOffset = minValuePoint.YValues[0],
                    IntervalOffset = measTarget1 - 40, //minValuePoint.YValues[0],
                    //Text = "Min Value",
                    TextLineAlignment = StringAlignment.Far
                });

            // draw!
            chart.Invalidate();

            // write out a file
            //chart.SaveImage("chart.png", ChartImageFormat.Png);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String fileName = "data" + ManagedGlobals.NumFile.ToString() + ".xml"; // Save the data dump when this is selected

            ManagedGlobals.resistanceSet.WriteXml(fileName);

            ManagedGlobals.NumFile++;
        }

        // Charting =============================================================================================================

        private void cmbStageSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            myStageController.speed = cmbStageSpeed.Text;
            OldSpeed = cmbStageSpeed.Text; 
        }




        // Joystick Test ========================================================================================================

        private void JogWithVelocity(double X_v, double Y_v, double Z_v)
        {
            Print(String.Format("X:{0}, Y:{1}, Z:{2}", X_v, Y_v, Z_v));
        }


        // Joystick Test ========================================================================================================



        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseSiApp(); //close joystick support objects
            //myStageController.Disconnect(); =================== Need more methods in StageController Class            
        }

        private void btnPulseParameters_Click(object sender, EventArgs e)
        {
            frmPulseParameters tf = new frmPulseParameters();
            tf.Show();
        }

        private void cmbTransfectMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = (TransfectionMode)cmbTransfectMode.SelectedIndex;
        }

        private void btnTransfect_Click(object sender, EventArgs e)
        {
            if(Mode == TransfectionMode.Manual)
            {
                frmManualTransfect manualTransfect = new frmManualTransfect(this);

                try
                {
                    manualTransfect.Show();
                }
                catch(ObjectDisposedException ex)
                {

                }
                      
            }
            else if(Mode == TransfectionMode.Auto)
            {
                frmAutoTransfect autoTransfect = new frmAutoTransfect(this);

                try
                {
                    autoTransfect.Show();
                }
                catch(ObjectDisposedException ex)
                {

                }
            }
            else
            {

            }
        }

        private void chkReplayData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReplayData.Checked)
                isReplayMode = true;
            else
                isReplayMode = false;
        }



        //=========================================================================================
        public MeasurementSet GetMeasurementFile(string measFilePath)
        {
            MeasurementSet myMeasurementSet = new MeasurementSet();

            if (File.Exists(measFilePath))
                return myMeasurementSet.FromXMLFile(measFilePath);
            else
                return null;
        }

        public void SaveMeasurementFile(MeasurementSet myMeasurementSet, string measFilePath)
        {
            myMeasurementSet.ToXMLFile(measFilePath);
        }

        private MeasurementSet readMeasurementSet; // read from file
        bool isReplayDone = false;
        public void StreamData(string measFilePath, int interval, object myObj)
        {
            //Call back to a function in the caller to send each data point.

            //Read in the data file:
            readMeasurementSet = GetMeasurementFile(measFilePath);
            if (readMeasurementSet == null)
                return;

            CallLongRunningMethod1("");
        }
        public void StopDataStreaming()
        {
            isReplayDone = true;
        }

        private async void CallLongRunningMethod1(string xx)
        {
            isReplayDone = false;
            string result = await LongRunningMethodAsync1(xx);
            //long running method has completed
            isReplayDone = true;
        }
        private Task<string> LongRunningMethodAsync1(string xx)
        {
            return Task.Run<string>(() => LongRunningMethod1(xx));
        }
        private string LongRunningMethod1(string xx)
        {
            for (int i = 0; i < readMeasurementSet.MeasurementList.Count; i++)
            {
                if (isReplayDone) break;
                MeasurementReceived(readMeasurementSet.MeasurementList[i].raw, (long)(readMeasurementSet.MeasurementList[i].time * 1000));
                System.Threading.Thread.Sleep(6);
                if (timer.ElapsedMilliseconds / 1000 < readMeasurementSet.MeasurementList[i].time)
                {
                    System.Threading.Thread.Sleep((int)((readMeasurementSet.MeasurementList[i].time) - (timer.ElapsedMilliseconds / 1000)));
                }
                //while (timer.ElapsedMilliseconds / 1000 < readMeasurementSet.MeasurementList[i].time)
                //{
                //    System.Threading.Thread.Sleep(1);
                //}
            }
            Start = false; //done
            return "Done";
        }

    }
    //================================================================================================
    public class Measurements
    {
        public double time { get; set; }

        public double raw { get; set; }

        [XmlElement(ElementName = "Z\x0020filter")]
        public double Z_x0020_filter { get; set; }

        public double ShortMean { get; set; }

        public double LongMean { get; set; }

        [XmlElement(ElementName = "STD\x002AThresh")]
        public double STD_x002A_Thresh { get; set; }

        public double HPF { get; set; }

        public double LPF { get; set; }

        public int Detects { get; set; }

    }
    public class MeasurementSet
    {
        [XmlElement("Measurements")] //suppress MeasurementList level in xml
        public List<Measurements> MeasurementList { get; set; }

        public MeasurementSet()
        {
            MeasurementList = new List<Measurements>();
        }

        public void ToXMLFile(string xmlFilename)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "urn:NFP-E-MeasurementData");
            // Add lib namespace with empty prefix

            //System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(MeasurementSet));
            //Rename root object:
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(MeasurementSet), new XmlRootAttribute("DocumentElement"));

            System.IO.FileStream fs1 = new System.IO.FileStream(xmlFilename, System.IO.FileMode.Create);
            System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(fs1, System.Text.Encoding.UTF8);
            xmlTextWriter.WriteProcessingInstruction("xml", "version=\"1.0\" standalone=\"yes\"");
            xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
            Serializer.Serialize(xmlTextWriter, this, ns);
            fs1.Close();
        }

        public MeasurementSet FromXMLFile(string xmlFilename)
        {
            //System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(MeasurementSet)); //
            //Rename root object:
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(MeasurementSet), new XmlRootAttribute("DocumentElement")); //
            System.IO.FileStream fs = new System.IO.FileStream(xmlFilename, System.IO.FileMode.Open);
            MeasurementSet myMeasurementSet;
            try
            {
                myMeasurementSet = (MeasurementSet)Serializer.Deserialize(fs);
            }
            catch
            {
                myMeasurementSet = new MeasurementSet();
            }
            fs.Close();
            return myMeasurementSet;
        }

    }
}
