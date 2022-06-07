using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace DoMeasurement
{
    class ManagedGlobals
    {
        public static string portNameGlobal;

        //public static SerialPort serialPortGlobal;

        public static string manPortNameGlobal;

        //public static SerialPort manPortGlobal;

        public static string calNameGlobal;

        public static bool continueMeasurement;

        public static bool paramsInitialized;

        public static bool mainFormInitialized;

        public static DataTable resistanceSet;

        public static DataTable parameterSet;

        public static DataTable calibrations;

        public static int NumFile;

        public static double X;

        public static double Y;

        public static double Z;

        public static string dataFromBox;

        public static bool goingDown;


        static ManagedGlobals()
        {
            ManagedGlobals.calNameGlobal = "previous";
            ManagedGlobals.continueMeasurement = false;
            ManagedGlobals.paramsInitialized = false;
            ManagedGlobals.mainFormInitialized = false;
            ManagedGlobals.resistanceSet = new DataTable("Measurements");
            ManagedGlobals.parameterSet = new DataTable("Parameters");
            ManagedGlobals.calibrations = new DataTable("Calibrations");
            ManagedGlobals.NumFile = 1;
            ManagedGlobals.X = 0;
            ManagedGlobals.Y = 0;
            ManagedGlobals.Z = 0;
            ManagedGlobals.dataFromBox = "";
            ManagedGlobals.goingDown = false;
        }

        public ManagedGlobals()
        {
        }
    }
}
