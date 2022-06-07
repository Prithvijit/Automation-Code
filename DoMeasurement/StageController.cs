using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialPortLib;

namespace DoMeasurement
{
    public class StageController
    {
        //The following properties must be set by calling code
        public string speed { get; set; }
        public string portName { get; set; }

        public string LastReceivedData = "";

        public StageController()
        {
            speed = "Medium"; //set initial speed
            portName = "COM17";
        }

        // Jog with velocity given in X, Y, Z. X, Y, and Z velocities are given as a percent of the max velocity, and further scaled by the scale factor.
        public void JogWithVelocity(double X_v, double Y_v, double Z_v)
        {
            double scale = 3;
            if (speed == "Slow")
            {
                //scale = 3;
                scale = 0.46;// 0.195;REAL
            }
            else if (speed == "Medium")
            {
                //scale = 20;
                scale = 3;// 1.953;REAL
            }
            else if (speed == "Fast")
            {
                scale = 20;// 39.063;REAL
            }

            //bool moveZonly = ((Z_v > X_v) && (Z_v > Y_v));
            double scaledSpeedX = Math.Round((scale * X_v / 100/*/ 351*/), 3);
            double scaledSpeedY = Math.Round((scale * Y_v / 100/*/ 351*/), 3);
            double scaledSpeedZ;
            if (speed == "Slow")
            {
                //scaledSpeedZ = Math::Round(Math::Sign(Z_v) * scale, 3);
                scaledSpeedZ = Math.Round((scale * Z_v / 100/*/ 351*/), 3);
            }
            else
            {
                scaledSpeedZ = Math.Round((scale * Z_v / 100/*/ 351*/), 3);
            }

            string toTXx;
            string toTXy;
            string toTXz;
            string toTX;

            string toTXold = "";

            //string portName = ManagedGlobals::manPortNameGlobal;

            //String^ toTX = (toTX + "\n\r");
            if (Math.Abs(X_v) <= 14)// || moveZonly)
            {
                toTXx = "2STP";
            }
            else
            {
                toTXx = "2JOG" + scaledSpeedX.ToString();
            }
            if (Math.Abs(Y_v) <= 14)// || moveZonly)
            {
                toTXy = "1STP";
            }
            else
            {
                toTXy = "1JOG" + scaledSpeedY.ToString();
            }
            if (Math.Abs(Z_v) <= 14)
            {
                toTXz = "3STP";
                ManagedGlobals.goingDown = false;
            }
            else
            {
                toTXz = "3JOG" + scaledSpeedZ.ToString();
                if (scaledSpeedZ > 0)
                {
                    ManagedGlobals.goingDown = true;
                }
                else
                {
                    ManagedGlobals.goingDown = false;
                }
            }
            toTX = toTXx + ";" + toTXy + ";" + toTXz + "\n\r";
            if (!string.Equals(toTX, toTXold))
            {
                //sendManCommand(toTX);
                ManipulatorSendCommand(toTX);
            }
            toTXold = toTX;
            //measurePosition();        }
        }
        // Move a given amount in X,Y, and Z. These amounts are given in millimeters.
        public void MoveRelative(double X_d, double Y_d, double Z_d)
        {
            double movX = Math.Round(X_d, 6);
            double movY = Math.Round(Y_d, 6);
            double movZ = Math.Round(Z_d, 6);
            //double movX = Math.Round(X_d, 3);
            //double movY = Math.Round(Y_d, 3);
            //double movZ = Math.Round(Z_d, 3);
            String moveXYZ;
            moveXYZ = "2MSR" + movY.ToString() + ";1MSR" + movX.ToString() + ";3MSR" + movZ.ToString() + ";\n\r" + "0RUN;" + "\n\r";
            //moveXYZ = "2MVR" + movY.ToString() + ";1MVR" + movX.ToString() + ";3MVR" + movZ.ToString() + ";\n\r";
            ManipulatorSendCommand(moveXYZ);
        }
        // Set the current position to the zero position.
        public void zeroXYZ()
        {
            ManipulatorSendCommand("1ZRO;2ZRO;3ZRO;\n\r");
        }
        // Send the stage back to the physical zero position, and also zero it.
        public void homeXYZ()
        {
            ManipulatorSendCommand("1MLP;2MLP;3MLN;\n\r");
            System.Threading.Thread.Sleep(10000);
            ManipulatorSendCommand("1MSR-14.75;2MSR-14.75;3MSR14.75;0RUN;\n\r");
            System.Threading.Thread.Sleep(10000);
            ManipulatorSendCommand("1ZRO;2ZRO;3ZRO;\n\r");
        }
        public double readPosition(int axis)
        {
            double position = 0;
            _serialPort.DiscardInBuffer();
            LastReceivedData = "";
            System.Threading.Thread.Sleep(100);
            ManipulatorSendCommand(axis.ToString() + "POS?;");
            System.Threading.Thread.Sleep(100);
            if (!string.IsNullOrWhiteSpace(LastReceivedData))
            {
                try
                {
                    position = double.Parse(LastReceivedData);
                }
                catch (Exception)
                {
                }
            }

            //position = Double::Parse(readString);
            return position;
        }

        //===========================================================================================
        //Simple Serial Port Code
        SerialPort _serialPort;
        SerialPortInput serialPortInput;
        public bool Connect()
        {
            //return true on success
            if(portName==null)
            {
                return false;
            }
            else
            {
                _serialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
                _serialPort.Handshake = Handshake.None;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                try
                {
                    if (!_serialPort.IsOpen)
                        _serialPort.Open();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening/writing to serial port :: " + ex.Message, "Error!");
                    return false;
                }
                return true;
            }
            
        }

        public void Disconnect()
        {
            _serialPort.DataReceived -= new SerialDataReceivedEventHandler(sp_DataReceived);

            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

        void ManipulatorSendCommand(string str)
        {
            _serialPort.Write(str);
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            LastReceivedData = _serialPort.ReadLine();
        }


        public string GetFirstComPort()
        {
            serialPortInput = new SerialPortInput();
            String manPort = serialPortInput.GetFirstPort("0403", "6001");
            return manPort;
        }
    }
}
