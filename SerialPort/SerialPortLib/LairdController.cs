using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace SerialPortLib
{
    public enum RegulatorModeValue
    {
        None,
        Power,
        OnOff,
        P,
        PI,
        PD,
        PID
    }
    public enum FanModeValue
    {
        Off,
        On,
        Cool,
        Heat,
        CoolHeat,
        Auto
    }

    public class LairdController
    {

        List<byte> leftOverBytes = new List<byte>(); //<=====rlm 10-11-16
        int replyLineCount = 0; //lines received in response to last cmd

        private int _operationTimeout_ms;

        string lastReceivedFullMessage;
        byte[] _lineStartOut = new byte[] { 0x24 }; //$
        byte[] _lineEndOut = new byte[] {  };
        //byte[] _lineStartIn = new byte[] { 0x24 }; //$
        byte[] _lineStartIn = new byte[] { 0x0D, 0x0A }; //cr
        //byte[] _lineEndIn = new byte[] { 0x0D, 0x3E, 0x0D }; //>cr
        byte[] _lineEndIn = new byte[] { 0x0D }; //>cr

        private SerialPortInput _serialPort = null;

        public LairdController()
        {
            _serialPort = new SerialPortInput();
            _serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            _serialPort.MessageReceived += SerialPort_MessageReceived;

            IsLairdInitialized = true;
            lastReceivedFullMessage = null;

        }

        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            byte[] newData = args.Data;
            byte[] allBytes = leftOverBytes.Concat(newData).ToArray();
            leftOverBytes.Clear();

            //-----------------------<=====rlm 10-11-16
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
                        Console.WriteLine(extractedLineString + "<==========================");
                        System.Threading.Thread.Sleep(25);
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

        public bool Connect(string port, int baud)
        {
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

        public string Reset()
        {
            string expectReply = "Booting..";
            string myCmd = CMD_INIT;
            //myCmd = CMD_RPT_ABS_TEMPERATURE;
            string reply = sendAndCheckLairdCmd(expectReply, "reset", myCmd);

            System.Threading.Thread.Sleep(500); //give time to reset

            IsLairdInitialized = true;

            return reply;
        }

        public string GetVersion()
        {
            string version = sendAndCheckLairdCmd("$" + CMD_GET_VERSION, "get version", CMD_GET_VERSION);

            return version;
        }

        public double GetTemperature()
        {
            string actualTemp = sendAndCheckLairdCmd("$" + CMD_RPT_ABS_TEMPERATURE, "get temperature", CMD_RPT_ABS_TEMPERATURE);

            return double.Parse(actualTemp);
        }

        public void SetTcCoefficients(double gain, double offset, double coeffA, double coeffB, double coeffC)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_TC_GAIN, gain);
            reply = sendAndCheckLairdCmd("$" + cmd, "set gain", cmd);

            cmd = string.Format(CMD_SET_TC_OFFSET, offset);
            reply = sendAndCheckLairdCmd("$" + cmd, "set offset", cmd);

            cmd = string.Format(CMD_SET_TC_COEFFA, coeffA);
            reply = sendAndCheckLairdCmd("$" + cmd, "set coeffA", cmd);

            cmd = string.Format(CMD_SET_TC_COEFFB, coeffB);
            reply = sendAndCheckLairdCmd("$" + cmd, "set coeffB", cmd);

            cmd = string.Format(CMD_SET_TC_COEFFC, coeffC);
            reply = sendAndCheckLairdCmd("$" + cmd, "set coeffC", cmd);

            System.Threading.Thread.Sleep(250); //give time to set
        }

        public void SetPidValues(double Pval, double Ival, double Dval, double TRval, double TEval, double Limitval)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_PID_P, Pval);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID P value", cmd);

            cmd = string.Format(CMD_SET_PID_I, Ival);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID I value", cmd);

            cmd = string.Format(CMD_SET_PID_D, Dval);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID D value", cmd);

            cmd = string.Format(CMD_SET_PID_FILTER_TR, TRval);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID Tr value", cmd);

            cmd = string.Format(CMD_SET_PID_FILTER_TE, TEval);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID Te value", cmd);

            cmd = string.Format(CMD_SET_PID_INTEG_LIMIT, Limitval);
            reply = sendAndCheckLairdCmd("$" + cmd, "set PID Limit value", cmd);

            System.Threading.Thread.Sleep(250); //give time to set
        }

        public void RunRegulator(bool FanOn = true)
        {
            if (FanOn)
                SetFanMode(1);

            string cmd;
            string reply;

            cmd = CMD_RUN_REGULATOR;
            reply = sendAndCheckLairdCmd("$" + cmd, "Run Regulator", cmd);

            System.Threading.Thread.Sleep(250);
        }

        public void StopRegulator(bool FanOff = true)
        {
            if (FanOff)
                SetFanMode(0);

            string cmd;
            string reply;

            cmd = CMD_STOP_REGULATOR;
            reply = sendAndCheckLairdCmd("$" + cmd, "Stop Regulator", cmd);

            System.Threading.Thread.Sleep(250);
        }

        public void SetPowerValues(double DeadBand, double Max, double CoolGain, double HeatGain, double Decay)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_PWR_DEADBAND, DeadBand);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Power DeadBand value", cmd);

            cmd = string.Format(CMD_SET_PWR_MAX, Max);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Power Max value", cmd);

            cmd = string.Format(CMD_SET_PWR_COOL_GAIN, CoolGain);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Power CoolGain value", cmd);

            cmd = string.Format(CMD_SET_PWR_HEAT_GAIN, HeatGain);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Power HeatGain value", cmd);

            cmd = string.Format(CMD_SET_PWR_DECAY, Decay);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Power Decay value", cmd);

            System.Threading.Thread.Sleep(250); //give time to set
        }

        public void SetRegulatorMode(int Mode)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_REGULATOR_MODE, Mode);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Regulator Mode value", cmd);

            System.Threading.Thread.Sleep(250);
        }

        public void SetRegulatorTargetTemperature(double Temp)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_REGULATOR_TARGET_TEMP, Temp);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Regulator Target Temperature", cmd);

            System.Threading.Thread.Sleep(250);
        }

        public void SetRegulatorSampleRate(double Rate)
        {
            //Rate is in seconds (default is 0.05)
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_REGULATOR_SAMPLE_RATE, Rate);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Sample Rate", cmd);

            System.Threading.Thread.Sleep(250);
        }

        public void SetFanValues(double Temp, double DeadBand, double LSHyst, double HSHyst, double LSVolts, double HSVolts)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_FAN_TEMP, Temp);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan Temp value", cmd);

            cmd = string.Format(CMD_SET_FAN_DEADBAND, DeadBand);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan DeadBand value", cmd);

            cmd = string.Format(CMD_SET_FAN_LOW_SPEED_HYST, LSHyst);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan LSHyst value", cmd);

            cmd = string.Format(CMD_SET_FAN_HIGH_SPEED_HYST, HSHyst);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan HSHyst value", cmd);

            cmd = string.Format(CMD_SET_FAN_LOW_SPEED_VOLTAGE, LSVolts);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan LSVolts value", cmd);

            cmd = string.Format(CMD_SET_FAN_HIGH_SPEED_VOLTAGE, HSVolts);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan HSVolts value", cmd);


            System.Threading.Thread.Sleep(250); //give time to set
        }

        public void SetFanMode(int Mode)
        {
            string cmd;
            string reply;

            cmd = string.Format(CMD_SET_FAN_MODE, Mode);
            reply = sendAndCheckLairdCmd("$" + cmd, "set Fan Mode value", cmd);

            System.Threading.Thread.Sleep(250);
        }


        private byte[] constructCommandLairdProtocol(string cmd)
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

        private LairdResponse sendLairdCmd(string expectReply, string cmd)
        {
            Debug.Assert(string.IsNullOrEmpty(cmd) == false, "Command is null or empty");

            LairdResponse LairdResponse = null;

            try
            {
                byte[] cmdBlk = constructCommandLairdProtocol(cmd);

                string rawResponse = sendLairdCmd(expectReply, cmdBlk);

                LairdResponse = new LairdResponse(rawResponse);
            }
            catch (TimeoutException)
            {
                throw new Exception(string.Format("Timeout waiting for response for command '{0}'", cmd));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error sending command '{0}' - {1}", cmd, ex.Message));
            }

            return LairdResponse;
        }

        private string sendLairdCmd(string expectReply, byte[] cmdBlk)
        {
            lastReceivedFullMessage = null;

            lock (_serialPort)
            {
                _serialPort.SendMessage(cmdBlk);
                //_serialPort.Handle.Write(cmdBlk, 0, cmdBlk.Length);

                //ToDo: set ErrorCode if 1st timeout to ECHO_TIMEOUT
                //      set ErrorCode if 2nd timeout to CMD_TIMEOUT

                //Wait up to 3000ms for a full response (echo):
                for (int i = 0; i < 300; i++)
                {
                    System.Threading.Thread.Sleep(10);
                    if (lastReceivedFullMessage != null && lastReceivedFullMessage.Contains(expectReply))
                        break;
                }


                lastReceivedFullMessage = null;
                //Wait up to 3000ms for a full response (reply):
                for (int i = 0; i < 300; i++)
                {
                    if (lastReceivedFullMessage != null) break;
                    System.Threading.Thread.Sleep(10);
                }

                //response = _serialPort.Handle.ReadLine();
            }
            return lastReceivedFullMessage;
        }

        public string SendCmdGetResponse(string cmd)
        {
            Debug.Assert(string.IsNullOrEmpty(cmd) == false, "Command is null or empty");

            string rawResponse = "";

            LairdResponse LairdResponse = null;

            try
            {
                byte[] cmdBlk = constructCommandLairdProtocol(cmd);

                rawResponse = sendLairdCmd("$" + cmd, cmdBlk); //expect the command to be echoed

                LairdResponse = new LairdResponse(rawResponse);
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

        private string sendAndCheckLairdCmd(string expectReply, string cmdDesc, string cmdPrefix, params object[] cmdParams)
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

            LairdResponse LairdResponse = sendLairdCmd(expectReply, cmd);

            //if (LairdResponse.ErrorCode == ErrorCode.DeviceNotInitialized) throw new LairdNotInitializedException("Laird Controller");

            //if (LairdResponse.IsValid == false)
            //{
            //    throw new Exception(string.Format("Error sending '{0}' command ('{1}'): Invalid response '{2}'", cmdDesc, cmd, LairdResponse.RawResponse));
            //}

            //if (LairdResponse.ErrorCode != ErrorCode.NoError)
            //{
            //    throw new Exception(string.Format("Error sending '{0}' command ('{1}'): {2}", cmdDesc, cmd, LairdResponse.ErrorCode));
            //}

            return LairdResponse.Data;
        }

        private bool IsLairdInitialized = false;

        private const int DEFAULT_CMD_TIMEOUT_ms = 1000;    // 1 sec command timeout

        private const string CMD_INIT                       /**/ = "BC";
        private const string CMD_GET_VERSION                /**/ = "v";
        private const string CMD_RPT_ABS_TEMPERATURE        /**/ = "R100?";

        private const string CMD_RUN_REGULATOR              /**/ = "W";
        private const string CMD_STOP_REGULATOR              /**/ = "Q";

        private const string CMD_SET_TC_GAIN                /**/ = "R35={0}";
        private const string CMD_SET_TC_OFFSET              /**/ = "R36={0}";
        private const string CMD_SET_TC_COEFFA              /**/ = "R59={0}";
        private const string CMD_SET_TC_COEFFB              /**/ = "R60={0}";
        private const string CMD_SET_TC_COEFFC              /**/ = "R61={0}";

        private const string CMD_SET_REGULATOR_MODE         /**/ = "R13={0}";
        private const string CMD_SET_REGULATOR_TARGET_TEMP  /**/ = "R0={0}";
        private const string CMD_SET_REGULATOR_SAMPLE_RATE  /**/ = "R9={0}";

        private const string CMD_SET_PID_P                  /**/ = "R1={0}";
        private const string CMD_SET_PID_I                  /**/ = "R2={0}";
        private const string CMD_SET_PID_D                  /**/ = "R3={0}";
        private const string CMD_SET_PID_FILTER_TR          /**/ = "R4={0}";
        private const string CMD_SET_PID_FILTER_TE          /**/ = "R5={0}";
        private const string CMD_SET_PID_INTEG_LIMIT        /**/ = "R6={0}";

        private const string CMD_SET_PWR_DEADBAND           /**/ = "R7={0}";
        private const string CMD_SET_PWR_MAX                /**/ = "R8={0}";
        private const string CMD_SET_PWR_COOL_GAIN          /**/ = "R10={0}";
        private const string CMD_SET_PWR_HEAT_GAIN          /**/ = "R11={0}";
        private const string CMD_SET_PWR_DECAY              /**/ = "R12={0}";

        private const string CMD_SET_FAN_MODE               /**/ = "R16={0}";

        private const string CMD_SET_FAN_TEMP               /**/ = "R17={0}";
        private const string CMD_SET_FAN_DEADBAND           /**/ = "R18={0}";
        private const string CMD_SET_FAN_LOW_SPEED_HYST     /**/ = "R19={0}";
        private const string CMD_SET_FAN_HIGH_SPEED_HYST    /**/ = "R20={0}";
        private const string CMD_SET_FAN_LOW_SPEED_VOLTAGE  /**/ = "R21={0}";
        private const string CMD_SET_FAN_HIGH_SPEED_VOLTAGE /**/ = "R22={0}";

        public enum ErrorCode
        {
            NoError = -1,

            STARTUP_DELAY = 0,
            DOWNLOAD_ERROR = 1,
            C_ERROR = 2,
            R_ERROR = 3,
            HIGH_VOLT = 4,
            LOW_VOLT = 5,
            HIGH_12V = 6,
            LOW_12V = 7,
            CURRENT_HIGH = 8,
            CURRENT_LOW = 9,
            FAN1_HIGH = 10,
            FAN1_LOW = 11,
            FAN2_HIGH = 12,
            FAN2_LOW = 13,
            TEMP_SENSOR_ALARM_1 = 14,
            TEMP_SENSOR_ALARM_2 = 15,

            ECHO_TIMEOUT = 16,
            COMMAND_TIMEOUT = 17,
        }

        private class LairdResponse
        {
            public bool IsValid { get; private set; }

            public ErrorCode ErrorCode  { get; private set; }

            public string Data { get; private set; }

            public string RawResponse { get; private set; }

            private bool IsEmulated;

            private static ASCIIEncoding _ASCII = new ASCIIEncoding();

            public LairdResponse(string rawResponse)
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
                    return String.Format("Err: {0}  Laird Controller Status: {1}  Data: {2}", ErrorCode, Data, RawResponse);
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

        private class LairdNotInitializedException : Exception
        {
            public LairdNotInitializedException(string name)
                : base(String.Format("Laird Controller '{0}' is not initialized!", name))
            {
            }
        }
    }
}
