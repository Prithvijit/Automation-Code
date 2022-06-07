using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using System.IO;


namespace DoMeasurement
{
    public partial class frmIpParameters : Form
    {
        string myProgramDataPath; //Base Program Data Dir
        string myIpParamFile;

        IpParameters myIpParameters = new IpParameters();

        frmAutoTransfect myParent;

        Mat dtOut;
        bool optimize = false;


        //Image analysis parameters
        Image<Bgr, byte> imgInput;
        Image<Bgr, byte> imgAnalysis;
        Image<Bgr, byte> imgOutput;
        Image<Gray, byte> imgMask;
        Image<Bgr, byte> foranalysis;

        //performance parameters
        int Ncells;
        int NCellsOut;
        int TP;
        int FP;
        double sensitivity;
        double precision;

        //for ROI selction
        bool ROIselect = false;
        List<Point> PolyPt = new List<Point>();

        //Input for watershed methods
        Emgu.CV.Util.VectorOfPoint watershedInput = new Emgu.CV.Util.VectorOfPoint();

        //centers of cells
        List<PointF> centers;        

        public frmIpParameters()
        {
            InitializeComponent();
            
        }

        public frmIpParameters(Image<Bgr, byte> img, frmAutoTransfect _myParent)
        {
            InitializeComponent();
            foranalysis = img;
            myParent = _myParent;
            pctBoxAnalysis.Image = new Bitmap(img.Bitmap);

            myProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NFPE"); //Base Program Data Dir
            myIpParamFile = Path.Combine(myProgramDataPath, "IpParameters.json");

            if (!Directory.Exists(myProgramDataPath))
            {
                Directory.CreateDirectory(myProgramDataPath);
            }
        }

        private void frmIpParameters_Load(object sender, EventArgs e)
        {
            if (!File.Exists(myIpParamFile))
            {
                // Create default Parameter file and store default values
                myIpParameters.SetIpDefaults();
                myIpParameters.ToJSONFile(myIpParamFile);
            }
            else
            {
                // loading last updated Image Processing Parameter values from file to display in text box
                myIpParameters = myIpParameters.FromJSONFile(myIpParamFile);
            }

            nudMaxFeret.Value = (decimal)myIpParameters.maxFeret;
            nudMinArea.Value = (decimal)myIpParameters.minArea;
            nudMaxFeret2.Value = (decimal)myIpParameters.maxFeret2;
            nudMinArea2.Value = (decimal)myIpParameters.minArea2;
            nudMaxArea.Value = (decimal)myIpParameters.maxArea;

        }


        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            btnOptimize.Enabled = false;
            btnAnalyze.Enabled = false;
            await Task.Run(() => EllipseFitAlgorithm());
            btnOptimize.Enabled = true;
            btnAnalyze.Enabled = true;
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            optimize = true;
            pnlFilters.Visible = true;
            pctBoxBinary.Image = new Bitmap(dtOut.Bitmap);
            pctBoxBinary.SizeMode = PictureBoxSizeMode.Zoom;
            btnAnalyze.Enabled = false;
            btnOptimize.Enabled = false;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            myIpParameters.SetIpDefaults();
            nudMaxFeret.Value = (decimal)myIpParameters.maxFeret;
            nudMinArea.Value = (decimal)myIpParameters.minArea;
            nudMaxFeret2.Value = (decimal)myIpParameters.maxFeret2;
            nudMinArea2.Value = (decimal)myIpParameters.minArea2;
            nudMaxArea.Value = (decimal)myIpParameters.maxArea;
        }

        private async void btnApply_Click(object sender, EventArgs e)
        {
            btnApply.Enabled = false;
            myIpParameters.maxFeret = (double)nudMaxFeret.Value;
            myIpParameters.minArea = (double)nudMinArea.Value;
            myIpParameters.maxFeret2 = (double)nudMaxFeret2.Value;
            myIpParameters.minArea2 = (double)nudMinArea2.Value;
            myIpParameters.maxArea = (double)nudMaxArea.Value;
            await Task.Run(() => EllipseFitAlgorithm());
            btnApply.Enabled = true;
        }

        

        private void EllipseFitAlgorithm()
        {
            /*Ellipse Fitting Algorithm: algorithm consists of detecting internal features of cells(i.e nucleolus)
             * by applying the following steps
             * 1. Gaussian smoothing 
             * 2. Sobel gradient in x and y components
             * 3. Otsu binarization 
             * 4. Maximum Feret diameter filter
             * 5. Image addition
             * 6. Mathematical Morphology operations
             *      -dilation (3x3 kernel)
             *      -small particle removal
             *      -dilation (3x3)
             *      -erosion
             *      -hole filling
             * 7. Separate clumped objects
             * 8. Ellipse fitting
             * 9. k-means clustering callibration
             */

            //1. Gaussian Filter------------------------------------------------------------------------------------------
            //load the image from pictureBox.Text            


            if (imgInput == null)
            {
                imgInput = new Image<Bgr, byte>("C:\\Users\\pmn310\\Documents\\ph6.tiff");
                //imgInput = foranalysis;
            }

            imgAnalysis = null;
            imgAnalysis = imgInput;
            imgInput = null;

            //convert to greyscale
            Mat imGrey = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            Mat imGauss = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            Size ksize = new Size(5, 5);     //kernel size

            try
            {                                           //exception handling
                CvInvoke.CvtColor(imgAnalysis, imGrey, ColorConversion.Bgr2Gray);
                //CvInvoke.EqualizeHist(imGrey, imGrey);
                CvInvoke.GaussianBlur(imGrey, imGauss, ksize, 0);
            }
            catch (Exception entry)
            {
                MessageBox.Show(entry.Message);
            }

            //display image
            //pictureBox1.Image = imGauss.Bitmap;

            //2. Sobel gradient x and y----------------------------------------------------------------------------------
            //Sobel gradient
            Mat sobelx = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            Mat sobely = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            int ksizeSobel = 3;     //kernel size
            try
            {
                CvInvoke.Sobel(imGauss, sobelx, DepthType.Cv8U, 1, 0, ksizeSobel);
                CvInvoke.Sobel(imGauss, sobely, DepthType.Cv8U, 0, 1, ksizeSobel);
            }
            catch (Exception entry)
            {
                MessageBox.Show(entry.Message);
            }

            //Display image
            //CvInvoke.Imshow("sobelX", sobelx);
            //pictureBox1.Image = sobelx.Bitmap;

            //3. Otsu Binarization------------------------------------------------------------------------------------------
            Mat otsuX = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            Mat otsuY = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);

            try
            {
                //otsu threshold 
                CvInvoke.Threshold(sobelx, otsuX, 10, 300, ThresholdType.Otsu);
                CvInvoke.Threshold(sobely, otsuY, 10, 300, ThresholdType.Otsu);

            }
            catch (Exception entry)
            {
                MessageBox.Show(entry.Message);
            }

            //Display image
            // pictureBox1.Image = otsuX.Bitmap;

            //4. Maximum Feret Diameter filter-------------------------------------------------------------------------------
            Emgu.CV.Util.VectorOfVectorOfPoint contoursX = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Emgu.CV.Util.VectorOfVectorOfPoint contoursY = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            var cntX = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            var cntY = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            double wX;          //width bounding rect x
            double hX;          //height bounding rect x
            double wY;
            double hY;
            double maxFeret = myIpParameters.maxFeret;


            try
            {
                //find contours
                CvInvoke.FindContours(otsuX, contoursX, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                CvInvoke.FindContours(otsuY, contoursY, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                //Feret diameter filter
                for (int i = 0; i < contoursX.Size; i++)
                {
                    var rectX = CvInvoke.MinAreaRect(contoursX[i]);
                    wX = rectX.Size.Width;
                    hX = rectX.Size.Height;

                    //draw contours that satisfy Feret diameter condition
                    if (wX < maxFeret && hX < maxFeret)
                    {
                        CvInvoke.DrawContours(cntX, contoursX, i, new MCvScalar(255, 0, 0), -1);
                    }
                }
                for (int j = 0; j < contoursY.Size; j++)
                {
                    var rectY = CvInvoke.MinAreaRect(contoursY[j]);
                    wY = rectY.Size.Width;
                    hY = rectY.Size.Height;

                    if (wY < maxFeret && hY < maxFeret)
                    {
                        CvInvoke.DrawContours(cntY, contoursY, j, new MCvScalar(255, 0, 0), -1);
                    }
                }
            }
            catch (Exception entry)
            {
                Console.WriteLine(entry.Message);
            }

            //Display image
            //pictureBox1.Image = cntY.Bitmap;

            //5. Contour addition---------------------------------------------------------------------------------------------
            var cnt = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            CvInvoke.Add(cntX, cntY, cnt);
            //Display image
            //pictureBox1.Image = cnt.Bitmap;

            //6. Mathematical Morphology-------------------------------------------------------------------------------------


            Mat dilate = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);                     //dilate output
            Mat dilate2 = new Mat();
            Mat fill = new Mat();
            Mat erode = new Mat();
            Mat element = new Mat();                    //structuring element
            Point anchor = new Point(-1, -1);
            Size ksizeMM = new Size(3, 3);               //structuring element size
            Emgu.CV.Util.VectorOfVectorOfPoint contoursA = new Emgu.CV.Util.VectorOfVectorOfPoint();        //contours for area filter
            Emgu.CV.Util.VectorOfVectorOfPoint contoursErode = new Emgu.CV.Util.VectorOfVectorOfPoint();        //contours for fill
            Mat hierarchy2 = new Mat();
            double min_area = myIpParameters.minArea;
            //double min_area = Convert.ToDouble(textBoxMinA1.Text);
            //Mat cntA = new Mat();
            var cntA = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            var cntE = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);


            try
            {

                //dilate
                element = CvInvoke.GetStructuringElement(ElementShape.Ellipse, ksizeMM, anchor);
                CvInvoke.Dilate(cnt, dilate, element, anchor, 1, BorderType.Constant, new MCvScalar(0));        //border value must be 0

                //remove small object
                CvInvoke.FindContours(dilate, contoursA, hierarchy2, RetrType.External, ChainApproxMethod.ChainApproxSimple);


                for (int k = 0; k < contoursA.Size; k++)
                {
                    double area = CvInvoke.ContourArea(contoursA[k]);           //measure area

                    if (area > min_area)
                    {
                        CvInvoke.DrawContours(cntA, contoursA, k, new MCvScalar(255, 0, 0), -1);
                    }

                }
                //textBox1.Text = Convert.ToString(contoursA.Size);

                //dilate
                CvInvoke.Dilate(cntA, dilate2, element, anchor, 1, BorderType.Constant, new MCvScalar(0));

                //hole filling
                CvInvoke.MorphologyEx(dilate2, fill, MorphOp.Close, element, anchor, 7, BorderType.Constant, new MCvScalar(0));

                //erosion
                CvInvoke.Erode(fill, erode, element, anchor, 1, BorderType.Constant, new MCvScalar(0));
                //fill holes in image
                CvInvoke.FindContours(erode, contoursErode, hierarchy2, RetrType.External, ChainApproxMethod.ChainApproxNone);
                CvInvoke.DrawContours(cntE, contoursErode, -1, new MCvScalar(255), -1);
            }
            catch (Exception entry)
            {
                Console.WriteLine(entry.Message);
            }
            //output contour area statistics



            //display image
            //pictureBox1.Image = cntE.Bitmap;
            //7.Separate clumped objects--------------------------------------------------------------------------------------------
            //apply distance transform to separate 
            Mat dt = new Mat(erode.Size, DepthType.Cv8U, 1);
            Mat labels = new Mat();
            dtOut = new Mat();



            try
            {
                CvInvoke.DistanceTransform(cntE, dt, labels, DistType.L1, 3);
                dt.ConvertTo(dt, DepthType.Cv8U);
                //spread histogram
                CvInvoke.EqualizeHist(dt, dt);
                CvInvoke.Threshold(dt, dtOut, 0.7 * 255, 255, ThresholdType.Binary);

            }
            catch (Exception entry)
            {
                Console.WriteLine(entry.Message);
            }

            if (optimize == true)
            {
                updatePictureBoxBinary();
            }

            //7. Feret Diameter Filter----------------------------------------------------------------------------------------------
            Emgu.CV.Util.VectorOfVectorOfPoint contoursFeret = new Emgu.CV.Util.VectorOfVectorOfPoint();

            Mat hierarchy3 = new Mat();
            var cntF = new Mat(imgAnalysis.Size, DepthType.Cv8U, 1);
            //Image<Gray, byte> cntF;
            double wXF;          //width bounding rect x
            double hXF;          //height bounding rect x
            double max_area = myIpParameters.maxArea;
            double min_area2 = myIpParameters.minArea2;
            double maxFeret2 = myIpParameters.maxFeret2;




            try
            {
                //find contours
                CvInvoke.FindContours(dtOut, contoursFeret, hierarchy3, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                centers = new List<PointF>();//[contoursFeret.Size];                     //create array of floating points 


                //Feret diameter filter
                for (int ii = 0; ii < contoursFeret.Size; ii++)
                {
                    var rectF = CvInvoke.MinAreaRect(contoursFeret[ii]);
                    var areaF = CvInvoke.ContourArea(contoursFeret[ii]);
                    wXF = rectF.Size.Width;
                    hXF = rectF.Size.Height;

                    /*if (wXF > maxFeret2 || hXF > maxFeret2 ) //declump
                    {
                        Rectangle boundRect = CvInvoke.BoundingRectangle(contoursFeret[ii]);                             //bounding rectangle used to find ROI
                        Mat ROI = new Mat(cntF, boundRect);
                        CvInvoke.Erode(ROI, cntF, element, anchor, 1, BorderType.Constant, new MCvScalar(0));
                    }
                    */
                    //draw contours that satisfy Feret diameter condition
                    if (wXF < maxFeret2 && hXF < maxFeret2 && areaF < max_area && areaF > min_area2)
                    {

                        CvInvoke.DrawContours(cntF, contoursFeret, ii, new MCvScalar(255, 0, 0), -1);
                        //var ellFit = CvInvoke.FitEllipse(contoursFeret[ii]);
                        RotatedRect ellFit = CvInvoke.MinAreaRect(contoursFeret[ii]);
                        ellFit.Angle = ellFit.Angle - 90;  //rotate angle, for some reason it shifts 
                        //CvInvoke.Ellipse(imgInput, ellFit, new MCvScalar(255, 255,255 ),2,LineType.EightConnected);
                        //draw centroid
                        var mu = CvInvoke.Moments(contoursFeret[ii], true);                 //moments of contour
                        int x = (int)(mu.M10 / mu.M00);                               //x_centroid
                        int y = (int)(mu.M01 / mu.M00);
                        PointF z = new PointF((float)x, (float)y);                    //centroid (x,y)
                        CvInvoke.Circle(imgAnalysis, new Point(x, y), 8, new MCvScalar(0, 255, 0), -1);
                        //append 

                        centers.Add(z);

                    }

                }
                NCellsOut = centers.Count();                
                myParent.pts = centers;


            }
            catch (Exception entry)
            {
                MessageBox.Show(entry.Message);
            }

            //Display image
            updatePictureBoxAnalysis();
            

        }

        private void updatePictureBoxAnalysis()
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    if (pctBoxAnalysis.Image != null)
                    {
                        pctBoxAnalysis.Image.Dispose();
                    }
                    pctBoxAnalysis.Image = imgAnalysis.Bitmap;
                    pctBoxAnalysis.SizeMode = PictureBoxSizeMode.Zoom;
                };

                this.Invoke(del);

            }
            else
            {
                if (pctBoxAnalysis.Image != null)
                {
                    pctBoxAnalysis.Image.Dispose();
                }
                pctBoxAnalysis.Image = imgAnalysis.Bitmap;
                pctBoxAnalysis.SizeMode = PictureBoxSizeMode.Zoom;
            }


        }

        private void updatePictureBoxBinary()
        {
            if (InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    if (pctBoxBinary.Image != null)
                    {
                        pctBoxBinary.Image.Dispose();
                    }
                    pctBoxBinary.Image = new Bitmap(dtOut.Bitmap);
                    pctBoxBinary.SizeMode = PictureBoxSizeMode.Zoom;
                };

                this.Invoke(del);

            }
            else
            {
                if (pctBoxBinary.Image != null)
                {
                    pctBoxBinary.Image.Dispose();
                }
                pctBoxBinary.Image = new Bitmap(dtOut.Bitmap);
                pctBoxBinary.SizeMode = PictureBoxSizeMode.Zoom;
            }


        }

        
    }
}
