using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMeasurement
{
    internal class AlgorithmVars
    {
        public double[] LastN = new double[240];

        public int index;

        public int MeasNum = 0;

        public static int stdCount;

        public double MeanValue = 0;

        public static double[] stdArray;

        public static double stdMean;

        public static double STD;

        public static double[] FilterValueX;

        public static double[] FilterValueY;

        public static double[] FilterValueZ;

        public static double dRdt;

        public static double[] LastElapsedTime;

        public double @value
        {
            get
            {
                return this.LastN[this.index];
            }
            set
            {
                this.LastN[this.index] = value;
            }
        }

        static AlgorithmVars()
        {
            AlgorithmVars.stdCount = 0;
            AlgorithmVars.stdArray = new double[360];
            AlgorithmVars.FilterValueX = new double[2];
            AlgorithmVars.FilterValueY = new double[2];
            AlgorithmVars.FilterValueZ = new double[60];
            AlgorithmVars.dRdt = 0;
            AlgorithmVars.LastElapsedTime = new double[60];
        }

        public AlgorithmVars()
        {
        }

        public static void CalculateFilterValue(int MeasurementCount, double Mean, double LastMean, double time, List<long> LastTime, double LongMean)
        {
            AlgorithmVars.FilterValueY[0] = AlgorithmVars.FilterValueY[1];
            AlgorithmVars.FilterValueX[0] = AlgorithmVars.FilterValueX[1];

            AlgorithmVars.FilterValueY[1] = 5 / (5 + time * 0.001 - LastTime[MeasurementCount % 60] * 0.001) * (AlgorithmVars.FilterValueY[0] + Mean - LastMean);
            AlgorithmVars.FilterValueX[1] = AlgorithmVars.FilterValueX[0] + (time * 0.001 - LastTime[MeasurementCount % 60] * 0.001) / (0.5 + time * 0.001 - LastTime[MeasurementCount % 60] * 0.001) * (AlgorithmVars.FilterValueY[1] - AlgorithmVars.FilterValueX[0]);
            AlgorithmVars.FilterValueZ[MeasurementCount % 60] = AlgorithmVars.FilterValueY[1] - AlgorithmVars.FilterValueX[1];
            AlgorithmVars.dRdt = (AlgorithmVars.FilterValueZ[MeasurementCount % 60] - AlgorithmVars.FilterValueZ[(MeasurementCount + 1) % 60]) / (time * 0.001 - LastTime[(MeasurementCount + 2) % 60] * 0.001) / LongMean;
        }
        public static void UpdateFilterValues(int MeasurementCount, double _FilterY, double _FilterX, double _FilterZ, double _dRdt)
        {
            AlgorithmVars.FilterValueY[0] = AlgorithmVars.FilterValueY[1];
            AlgorithmVars.FilterValueX[0] = AlgorithmVars.FilterValueX[1];

            AlgorithmVars.FilterValueY[1] = _FilterY;
            AlgorithmVars.FilterValueX[1] = _FilterX;
            AlgorithmVars.FilterValueZ[MeasurementCount % 60] = _FilterZ;
            AlgorithmVars.dRdt = _dRdt;
        }

        public void CalculateRollingMean(double latestValue, int MeasurementCount, int filterLength)
        {
            this.index = MeasurementCount % filterLength;
            this.@value = latestValue;
            AlgorithmVars measNum = this;
            measNum.MeasNum = measNum.MeasNum + 1;
            this.MeanValue = 0;
            if (MeasurementCount < filterLength)
            {
                for (int index = 0; index < this.MeasNum; index++)
                {
                    AlgorithmVars meanValue = this;
                    meanValue.MeanValue = meanValue.MeanValue + this.@value;
                }
                AlgorithmVars algorithmVar = this;
                algorithmVar.MeanValue = algorithmVar.MeanValue / (double)this.MeasNum;
            }
            else
            {
                this.index = 0;
                while (this.index < filterLength)
                {
                    AlgorithmVars meanValue1 = this;
                    meanValue1.MeanValue = meanValue1.MeanValue + this.@value;
                    AlgorithmVars algorithmVar1 = this;
                    algorithmVar1.index = algorithmVar1.index + 1;
                }
                AlgorithmVars meanValue2 = this;
                meanValue2.MeanValue = meanValue2.MeanValue / (double)filterLength;
            }
        }

        public static void CalculateStandardDeviation(int MeasurementCount)
        {
            if (AlgorithmVars.stdCount < 360)
            {
                AlgorithmVars.stdArray[AlgorithmVars.stdCount] = AlgorithmVars.FilterValueZ[MeasurementCount % 60];
                AlgorithmVars.stdCount++;
            }
            else
            {
                AlgorithmVars.stdArray[AlgorithmVars.stdCount % 360] = AlgorithmVars.FilterValueZ[MeasurementCount % 60];
                for (int i = 0; i < 360; i++)
                {
                    AlgorithmVars.stdMean += AlgorithmVars.stdArray[i];
                }
                AlgorithmVars.stdMean /= (double)(360);
                for (int i = 0; i < 360; i++)
                {
                    AlgorithmVars.STD += (AlgorithmVars.stdArray[i] - AlgorithmVars.stdMean) * (AlgorithmVars.stdArray[i] - AlgorithmVars.stdMean) / 360.0;
                }
                AlgorithmVars.STD = Math.Sqrt(AlgorithmVars.STD);
                AlgorithmVars.stdCount++;
            }
        }
    }
}
