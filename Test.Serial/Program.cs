using System;
using System.Threading;

using SerialPortLib;
//using NLog;

namespace Test.Serial
{
    class MainClass
    {
        //private static string defaultPort = "/dev/ttyUSB0";
        //private static SerialPortInput serialPort;

        public static void Main(string[] args)
        {
            TestEBox();
        }

        static  EBoxController myEBox = new EBoxController();

        private static void TestEBox()
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(80, 80);

            string myPort = myEBox.GetFirstComPort(); //e.g. "COM23" -- this is the first FTDI port found

            myEBox.Connect(myPort, 115200);

            if (!myEBox.IsConnected())
            {
                Console.WriteLine("Could not connect!");
            }
            else
            {
                myEBox.StopMeasurement();
                Thread.Sleep(250);
                //myEBox.Disconnect(); //reset it
                //myEBox.Connect("COM23", 115200);
                myEBox.ClearBuffer();

                myEBox.msTimeOut = 50; //timeout in ms

                Console.WriteLine("Press x to start, y to stop");
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(100);
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.X);

                string myVersion = "";
                for (int i = 0; i < 9; i++)
                {
                    myVersion = myEBox.GetVersion();
                    if (myVersion == "Version 1.1")
                        break; //To Do: stopt or warn if wrong version
                    Thread.Sleep(100);
                }
                Console.WriteLine("     " + myVersion + "      <=============== VERSION REPLY");

                myEBox.GetStreamedMeasurements(MeasurementReceived);

                myEBox.StopMeasurement();
                //myReply = myEBox.GetVersion();
                myEBox.Disconnect();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }

        }

        public static void MeasurementReceived(double value)
        {
            bool stop = false;

            //Get values until double.Nan is received, then quit
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                stop = true;
            }

            if (stop || double.IsNaN(value))
            {
                myEBox.CancelMeasurements = true;
            }
            else
                Console.WriteLine("     " + value + "      <=============== MEASUREMENT REPLY");

        }

        static void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            Console.WriteLine("Serial port connection status = {0}", args.Connected);
        }
    }
}
