using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace SerialPortLib
{

    public class EBoxController
    {

        List<byte> leftOverBytes = new List<byte>();
        int replyLineCount = 0; //lines received in response to last cmd

        string lastReceivedFullMessage;
        byte[] _lineStartOut = new byte[] { 0x24 }; //$
        byte[] _lineEndOut = new byte[] {  };
        //byte[] _lineStartIn = new byte[] { 0x24 }; //$
        byte[] _lineStartIn = new byte[] { 0x0D, 0x0A }; //cr
        //byte[] _lineEndIn = new byte[] { 0x0D, 0x3E, 0x0D }; //>cr
        byte[] _lineEndIn = new byte[] { 0x0D }; //>cr

        private SerialPortInput _serialPort = null;

        public long msTimeOut { get; set; }

        public EBoxController()
        {
            //_serialPort = new SerialPortInput();
            //_serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            //_serialPort.MessageReceived += SerialPort_MessageReceived;

            //IsEBoxInitialized = true;
            //lastReceivedFullMessage = null;

            //msTimeOut = 300;

        }


        public string GetFirstComPort()
        {
            _serialPort = new SerialPortInput();
            String eboxPort = _serialPort.GetFirstPort("0403", "6015");
            return eboxPort;
        }


        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            byte[] newData = args.Data;
            byte[] allBytes = leftOverBytes.Concat(newData).ToArray();
            leftOverBytes.Clear();

            int lineCountThisMssg = 0;

            //find first instance of end line sequence:
            IEnumerable<int> lineEndLocations = allBytes.StartingIndex(_lineEndIn);
            //find first instance of start line sequence:
            IEnumerable<int> lineStartLocations = allBytes.StartingIndex(_lineStartIn);

            int headPtr = 0;


            foreach (int i in lineEndLocations)
            {
                lineCountThisMssg++;
                //get the line from the allBytes array and print it to console:
                List<byte> extractedLineByteList = new List<byte>();

                //if a corresponding start location does not exist, discard this line:
                if (lineStartLocations.Count() == 0 || lineStartLocations.Count() > i)
                {
                    if (allBytes.Count() > i + _lineEndIn.Count() - 1)
                    {
                        for (int j = (i + _lineEndIn.Count()); j < (allBytes.Count() - 1); j++)
                        {
                            leftOverBytes.Add(allBytes[j]);
                            return;
                        }
                    }
                }

                for (int j = headPtr; j < (_lineEndIn.Count() + i); j++)
                {
                    byte myByte = allBytes.ElementAt(j);
                    if (myByte != 0) extractedLineByteList.Add(myByte); //do not include null bytes
                    //if extractdline ends in termination string, break:
                    if (extractedLineByteList.Count == (i - headPtr))
                    {
                        replyLineCount++;
                        int byteCnt = extractedLineByteList.Count;
                        int byteIndex = 0;
                        if (extractedLineByteList[0] == 10)
                        {
                            byteCnt -= 1;
                            byteIndex = 1;
                        }
                        var extractedLineString = System.Text.Encoding.Default.GetString(extractedLineByteList.ToArray(), byteIndex, byteCnt);
                        lastReceivedFullMessage = extractedLineString;
                        ProcessLineAndIssueCallback(lastReceivedFullMessage); //<=============================================
//                        Console.WriteLine(extractedLineString + "<===========");
//                        System.Threading.Thread.Sleep(10);
                        break;
                    }
                }
                headPtr = i + _lineEndIn.Count(); //skip the line end chars
            }
            if (headPtr < allBytes.Count())
            {
                for (int i = headPtr; i < allBytes.Count(); i++)
                {
                    byte myByte = allBytes.ElementAt(i);
                    if (myByte > 0) leftOverBytes.Add(myByte); //do not include null values received
                }

            }

        }

        void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            Console.WriteLine("Serial port connection status = {0}", args.Connected);
        }

        public void ClearBuffer()
        {
            _serialPort.ClearBuffer();
        }

        public bool IsConnected()
        {
            return _serialPort.IsConnected;
        }

        public bool Connect(string port, int baud)
        {
            _serialPort = new SerialPortInput();
            _serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            _serialPort.MessageReceived += SerialPort_MessageReceived;

            IsEBoxInitialized = true;
            lastReceivedFullMessage = null;

            msTimeOut = 300;

            bool isConnected = true;

            //_serialPort.SetPort(port, baud);
            _serialPort.SetPort(port);
            _serialPort.Connect();
            _serialPort.SetNewLine("\r\n");

            Console.WriteLine("Waiting for serial port connection on {0}.", port);
            if (!_serialPort.IsConnected) isConnected = false;
            return isConnected;
        }

        public void Disconnect()
        {
            _serialPort.Disconnect();

            _serialPort.ConnectionStatusChanged -= SerialPort_ConnectionStatusChanged;
            _serialPort.MessageReceived -= SerialPort_MessageReceived;
        }

        public string GetVersion()
        {
            //string version = sendAndCheckEBoxCmd(CMD_GET_VERSION, "get version", CMD_GET_VERSION);

            ClearBuffer();
            string version = "";

            version = sendAndCheckEBoxCmd("", "get version", CMD_GET_VERSION);
            if (string.IsNullOrEmpty(version))
            {
                version = sendAndCheckEBoxCmd("", "get version", CMD_GET_VERSION);
            }

            return version;
        }

        private double ValidatedMeasurementReading(string myStrReading)
        {
            double myDblReading;
            bool isDouble = Double.TryParse(myStrReading, out myDblReading);
            if (isDouble)
            {
                return myDblReading;
            }
            else
                return double.NaN;
        }

        public void StartMeasurement()
        {
            //string reply = sendAndCheckEBoxCmd(START_MEASUREMENT, "start measurement", START_MEASUREMENT);

            string reply = "";
            int cnt = 0;
            while (cnt < 6)
            {
                cnt++;
                reply = sendAndCheckEBoxCmd("", "start measurement", START_MEASUREMENT);
                if (string.IsNullOrEmpty(reply))
                    System.Threading.Thread.Sleep(25);
                else
                    break;
            }
        }

        public void StopMeasurement()
        {
            //string reply = sendAndCheckEBoxCmd(STOP_MEASUREMENT, "stop measurement", STOP_MEASUREMENT);
            string reply = sendAndCheckEBoxCmd("", "stop measurement", STOP_MEASUREMENT);
        }

        public void SendPulse(string pulseType)
        {
            string reply = sendAndCheckEBoxCmd("", "send pulse", SEND_PULSE);
            SendCmdGetResponse(pulseType);
        }

        Action<double> streamedMeasurementCallback = null;
        public bool CancelMeasurements { get; set; }
        public void StopStreamedMeasurements()
        {
            streamedMeasurementCallback = null;
        }
        public void StartStreamedMeasurements(Action<double> callback)
        {
            streamedMeasurementCallback = callback;
        }
        private void ProcessLineAndIssueCallback(string dataLine)
        {
            if (streamedMeasurementCallback != null && !string.IsNullOrEmpty(dataLine))
            {
                double myDblReading = ValidatedMeasurementReading(dataLine);
                if (double.IsNaN(myDblReading))
                {
                    //streamedMeasurementCallback(double.NaN);
                }
                else
                {
                    streamedMeasurementCallback(myDblReading);
                }
            }
        }
        //Following is a blocking call
        public void GetStreamedMeasurements(Action<double> callback)
        {
            //Intercept measurement stream and issue callback with each value until a non-numeric value is received

            string reply = null;
            bool continueStream = true;
            double myDblReading = double.NaN;

            while (continueStream)
            {
                if (CancelMeasurements)
                    break;

                reply = getUnsolicitedMessage();
                if (string.IsNullOrEmpty(reply))
                {
                    StartMeasurement();
                    continue;
                    callback(double.NaN);
                    continueStream = false;
                }
                else
                {
                    myDblReading = ValidatedMeasurementReading(reply);
                    if (double.IsNaN(myDblReading))
                    {
                        callback(double.NaN);
                        continueStream = false;
                    }
                    else
                        callback(myDblReading);
                }
            }
        }

        private string getUnsolicitedMessage()
        {
            lastReceivedFullMessage = null;

            lock (_serialPort)
            {
                int myTimeOut = (int)(msTimeOut);

                lastReceivedFullMessage = null;
                //Wait up to myTimeOut for a full response (reply):
                for (int i = 0; i < myTimeOut; i++)
                {
                    if (lastReceivedFullMessage != null) break;
                    Thread.Sleep(1);
                }
            }
            return lastReceivedFullMessage;
        }

        private byte[] constructCommandEBoxProtocol(string cmd)
        {
            Debug.Assert(string.IsNullOrEmpty(cmd) == false, "Command is null or empty");

            /*
             * Construct the command block using DT communication protocol
             */

            int cmdBlkIdx = 0;

            byte[] cmdBlk = new byte[cmd.Length + 1];
            cmdBlk[0] = _lineStartOut[0];
            cmdBlkIdx = 1;

            foreach(char ch in cmd)
                cmdBlk[cmdBlkIdx++] = (byte)ch;         // insert command

            //cmdBlk[cmdBlkIdx] = (byte)0x0d;             // Carriage Return

            return cmdBlk;
        }

        private EBoxResponse sendEBoxCmd(string expectReply, string cmd)
        {
            Debug.Assert(string.IsNullOrEmpty(cmd) == false, "Command is null or empty");

            EBoxResponse EBoxResponse = null;

            try
            {
                byte[] cmdBlk = constructCommandEBoxProtocol(cmd);

                string rawResponse = sendEBoxCmd(expectReply, cmdBlk);

                EBoxResponse = new EBoxResponse(rawResponse);
            }
            catch (TimeoutException)
            {
                throw new Exception(string.Format("Timeout waiting for response for command '{0}'", cmd));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error sending command '{0}' - {1}", cmd, ex.Message));
            }

            return EBoxResponse;
        }

        private string sendEBoxCmd(string expectReply, byte[] cmdBlk)
        {
            lastReceivedFullMessage = null;

            lock (_serialPort)
            {
                _serialPort.SendMessage(cmdBlk);
                //_serialPort.Handle.Write(cmdBlk, 0, cmdBlk.Length);

                //ToDo: set ErrorCode if 1st timeout to ECHO_TIMEOUT
                //      set ErrorCode if 2nd timeout to CMD_TIMEOUT

                int myTimeOut = (int)(msTimeOut);

                //Wait up to myTimeOut for a full response (echo):
                if (!string.IsNullOrEmpty(expectReply))
                {
                    for (int i = 0; i < myTimeOut; i++)
                    {
                        System.Threading.Thread.Sleep(1);
                        if (lastReceivedFullMessage != null && lastReceivedFullMessage.Contains(expectReply))
                            break;
                    }
                }


                lastReceivedFullMessage = null;
                //Wait up to myTimeOut for a full response (reply):
                for (int i = 0; i < myTimeOut; i++)
                {
                    if (lastReceivedFullMessage != null) break;
                    System.Threading.Thread.Sleep(1);
                }

                //response = _serialPort.Handle.ReadLine();
            }
            return lastReceivedFullMessage;
        }

        public string SendCmdGetResponse(string cmd)
        {
            Debug.Assert(string.IsNullOrEmpty(cmd) == false, "Command is null or empty");

            string rawResponse = "";

            EBoxResponse EBoxResponse = null;

            try
            {
                byte[] cmdBlk = constructCommandEBoxProtocol(cmd);

                rawResponse = sendEBoxCmd(cmd, cmdBlk); //expect the command to be echoed

                EBoxResponse = new EBoxResponse(rawResponse);
            }
            catch (TimeoutException)
            {
                throw new Exception(string.Format("Timeout waiting for response for command '{0}'", cmd));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error sending command '{0}' - {1}", cmd, ex.Message));
            }

            return rawResponse;
        }

        private string sendAndCheckEBoxCmd(string expectReply, string cmdDesc, string cmdPrefix, params object[] cmdParams)
        {
            StringBuilder sb = new StringBuilder(cmdPrefix);

            int nParams = cmdParams.Length;

            for (int idx = 0; idx < nParams; ++idx)
            {
                string sep = (idx < (nParams - 1)) ? "," : "";

                sb.AppendFormat("{0}{1}", cmdParams[idx], sep);
            }

            //sb.Append(CMD_EXECUTE);
            Console.WriteLine("sending '{0}' cmd: '{1}'", cmdDesc, sb);

            string cmd = sb.ToString();

            EBoxResponse EBoxResponse = sendEBoxCmd(expectReply, cmd);

            //if (EBoxResponse.ErrorCode == ErrorCode.DeviceNotInitialized) throw new EBoxNotInitializedException("EBox Controller");

            //if (EBoxResponse.IsValid == false)
            //{
            //    throw new Exception(string.Format("Error sending '{0}' command ('{1}'): Invalid response '{2}'", cmdDesc, cmd, EBoxResponse.RawResponse));
            //}

            //if (EBoxResponse.ErrorCode != ErrorCode.NoError)
            //{
            //    throw new Exception(string.Format("Error sending '{0}' command ('{1}'): {2}", cmdDesc, cmd, EBoxResponse.ErrorCode));
            //}

            return EBoxResponse.Data;
        }

        private bool IsEBoxInitialized = false;

        private const int DEFAULT_CMD_TIMEOUT_ms = 1000;    // 1 sec command timeout

        private const string CMD_GET_VERSION = "v";
        private const string START_MEASUREMENT = "x";
        private const string STOP_MEASUREMENT = "y";
        private const string SEND_PULSE = "o";
        private const string CHANGE_PARAMETER = "t";
        private const string T1_TIME = "0:";
        private const string T2_TIME = "1:";
        private const string V1_LEVEL = "2:";
        private const string V2_LEVEL = "3:";
        private const string EXP_TC = "5:";
        private const string SQUARE_PULSE_LENGTH = "6:";
        private const string NUM_PULSE = "7:";
        private const string PULSE_FREQ = "8:";
        private const string REST_TIME = "9:";
        private const string NUM_TRAINS = "10:";

        public enum ErrorCode
        {
            NoError = 0,
            ECHO_TIMEOUT = 1,
            COMMAND_TIMEOUT = 2,
        }

        private class EBoxResponse
        {
            public bool IsValid { get; private set; }

            public ErrorCode ErrorCode  { get; private set; }

            public string Data { get; private set; }

            public string RawResponse { get; private set; }

            private bool IsEmulated;

            private static ASCIIEncoding _ASCII = new ASCIIEncoding();

            public EBoxResponse(string rawResponse)
            {
                RawResponse = rawResponse;
                ErrorCode = ErrorCode.NoError;

                if (string.IsNullOrEmpty(rawResponse))
                    IsValid = false;
                else
                    parseResponse();
            }

            public override string ToString()
            {
                if (IsValid)
                {
                    return String.Format("Err: {0}  EBox Controller Status: {1}  Data: {2}", ErrorCode, Data, RawResponse);
                }
                else
                {
                    return String.Format("INVALID: '{0}'", RawResponse);
                }
            }

            private void parseResponse()
            {
                if (IsEmulated == true) { return; }

                try
                {
                    byte[] buffer = _ASCII.GetBytes(RawResponse);

                    int dataStartIdx = 0;

                    int dataLen = buffer.Length - dataStartIdx - 0;

                    if (dataLen > 0)
                    {
                        Data = RawResponse.Substring(dataStartIdx, dataLen);
                    }

                    IsValid = true;
                }
                catch
                {
                    IsValid = false;
                }
            }

        }

        private class EBoxNotInitializedException : Exception
        {
            public EBoxNotInitializedException(string name)
                : base(String.Format("EBox Controller '{0}' is not initialized!", name))
            {
            }
        }
    }
    static class ArrayExtensions
    {
        public static IEnumerable<int> StartingIndex(this byte[] x, byte[] y)
        {
            IEnumerable<int> index = Enumerable.Range(0, x.Length - y.Length + 1);
            for (int i = 0; i < y.Length; i++)
            {
                index = index.Where(n => x[n + i] == y[i]).ToArray();
            }
            return index;
        }
    }

}
