using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMeasurement
{
    internal class ManagedParameters
    {
        public static string V1;

        public static string V2;

        public static string T1;

        public static string T2;

        public static string TC;

        public static string PW;

        public static string expPW;

        public static string amp;

        public static string numPulse;

        public static string pulseFreq;

        public static string restTime;

        public static string numTrains;

        public static string pulseType;

        public static string calibration;

        static ManagedParameters()
        {
            ManagedParameters.V1 = "15.0";
            ManagedParameters.V2 = "10.0";
            ManagedParameters.T1 = "0.5";
            ManagedParameters.T2 = "2.5";
            ManagedParameters.TC = "5.0";
            ManagedParameters.PW = "4.0";
            ManagedParameters.expPW = "10.0";
            ManagedParameters.amp = "15";
            ManagedParameters.numPulse = "50";
            ManagedParameters.pulseFreq = "200";
            ManagedParameters.restTime = "0";
            ManagedParameters.numTrains = "1";
            ManagedParameters.pulseType = "0";
            ManagedParameters.calibration = "previous";
        }

        public ManagedParameters()
        {
        }
    }
}
