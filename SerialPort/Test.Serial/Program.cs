using System;
using System.Threading;

using SerialPortLib;
//using NLog;

namespace Test.Serial
{
    class MainClass
    {
        private static string defaultPort = "/dev/ttyUSB0";
        private static SerialPortInput serialPort;

        public static void Main(string[] args)
        {
            //TestCavro();
            TestLaird();
        }

        private static void TestLaird()
        {
            LairdController myLaird = new LairdController();
            myLaird.Connect("COM9", 115200);
            string myReply = myLaird.Reset();
            Console.WriteLine("     " + myReply + "      <=============== RESET REPLY");
            System.Threading.Thread.Sleep(3000);

            myReply = myLaird.GetVersion();
            Console.WriteLine("     " + myReply + "      <=============== VERSION REPLY");

            double TCgain = 1.0;
            double TCoffset = -45.5;
            double TCcoeffA = 1.52786595e-03;
            double TCcoeffB = 2.43706279e-04;
            double TCcoeffC = -3.56562509e-07;
            myLaird.SetTcCoefficients(TCgain, TCoffset, TCcoeffA, TCcoeffB, TCcoeffC);

            double PidPval = 14.0; //
            double PidIval = 100.0; //
            double PidDval = 700.0; //
            double PidTRval = 1.0;
            double PidTEval = 1.0;
            double PidLimit = 100.0;
            myLaird.SetPidValues(PidPval, PidIval, PidDval, PidTRval, PidTEval, PidLimit);

            double PwrDeadBand = 1.5;
            double PwrMax = 25.0;
            double PwrCoolGain = 1.0;
            double PwrHeatGain = 1.0;
            double PwrDecay = 0.1;
            myLaird.SetPowerValues(PwrDeadBand, PwrMax, PwrCoolGain, PwrHeatGain, PwrDecay);

            double FanTemp = 20.0;
            double FanDeadBand = 8.0;
            double FanLSHyst = 4.0;
            double FanHSHyst = 2.0;
            double FanLSVolts = 12.0;
            double FanHSVolts = 12.0;
            myLaird.SetFanValues(FanTemp, FanDeadBand, FanLSHyst, FanHSHyst, FanLSVolts, FanHSVolts);

            int RegulatorMode = 6; //PID Control
            myLaird.SetRegulatorMode(RegulatorMode);

            double TargetTemp = 37.0; //Target Temperature to control to (Regulator)
            myLaird.SetRegulatorTargetTemperature(TargetTemp);

            double SampleRate = 0.04; //Sampling rate (Regulator) -- defult is 0.05sec
            myLaird.SetRegulatorSampleRate(SampleRate);

            //int FanMode = 1; //on
            //myLaird.SetFanMode(FanMode);

            myLaird.RunRegulator(); //starts fan by default

            double myTemp;
            for (int i = 0; i < 120; i++)
            {
                myTemp = myLaird.GetTemperature();
                Console.WriteLine("     " + myTemp.ToString() + "      <=============== TEMP REPLY");
                System.Threading.Thread.Sleep(1000);
            }
                       
            //System.Threading.Thread.Sleep(5000);

            //RegulatorMode = 0; //Control Off
            //myLaird.SetRegulatorMode(RegulatorMode);

            //FanMode = 0; //off
            //myLaird.SetFanMode(FanMode);

            myLaird.StopRegulator(); //stops fan by default


            myLaird.Disconnect();

            return;

            CavroPump myCavro = new CavroPump();
            myCavro.Connect("COM4", 9600, "1");
            //myCavro.Reset(false);
            for (int i = 1; i < 11; i++)
            {
                string reply = myCavro.SendCmdGetResponse("j2o1540m5h1L10V1200P400R");
                //Thread.Sleep(50);
                myCavro.AwaitPumpReady(5000);
                //Thread.Sleep(100);

                reply = myCavro.SendCmdGetResponse("j2o1540m5h1L10V3800D400R");
                //Thread.Sleep(50);
                myCavro.AwaitPumpReady(5000);
                //Thread.Sleep(100);

                if (i % 2 == 0)
                    myCavro.SetDeviceAddress("1");
                else
                    myCavro.SetDeviceAddress("3");
            }
            myCavro.Disconnect();
        }
        private static void TestCavro()
        {
            CavroPump myCavro = new CavroPump();
            myCavro.Connect("COM4", 9600, "1");
            //myCavro.Reset(false);
            for (int i = 1; i < 11; i++)
            {
                string reply = myCavro.SendCmdGetResponse("j2o1540m5h1L10V1200P400R");
                //Thread.Sleep(50);
                myCavro.AwaitPumpReady(5000);
                //Thread.Sleep(100);

                reply = myCavro.SendCmdGetResponse("j2o1540m5h1L10V3800D400R");
                //Thread.Sleep(50);
                myCavro.AwaitPumpReady(5000);
                //Thread.Sleep(100);

                if (i % 2 == 0)
                    myCavro.SetDeviceAddress("1");
                else
                    myCavro.SetDeviceAddress("3");
            }
            myCavro.Disconnect();
        }

        static void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            Console.WriteLine("Serial port connection status = {0}", args.Connected);
        }
    }
}
