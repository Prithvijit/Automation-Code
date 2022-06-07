using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Linq;

//using NLog;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SerialPortLib
{
    /// <summary>
    /// Serial port I/O
    /// </summary>
    public class SerialPortInput
    {

        #region Private Fields

        //internal static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _sendException = false;

        private SerialPort _serialPort;
        private string _portName = "";
        private int _baudRate = 115200;
        private StopBits _stopBits = StopBits.One;
        private Parity _parity = Parity.None;
        private string _newLine = "";

        // Read/Write error state variable
        private bool gotReadWriteError = true;

        // Serial port reader task
        private Thread reader;
        // Serial port connection watcher
        private Thread connectionWatcher;

        private object accessLock = new object();
        private bool disconnectRequested = false;

        #endregion

        #region Public Events

        /// <summary>
        /// Connected state changed event.
        /// </summary>
        public delegate void ConnectionStatusChangedEventHandler(object sender, ConnectionStatusChangedEventArgs args);
        /// <summary>
        /// Occurs when connected state changed.
        /// </summary>
        public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

        /// <summary>
        /// Message received event.
        /// </summary>
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs args);
        /// <summary>
        /// Occurs when message received.
        /// </summary>
        public event MessageReceivedEventHandler MessageReceived;

        #endregion

        #region Public Members

        /// <summary>
        /// Connect to the serial port.
        /// </summary>
        public bool Connect()
        {
            if (disconnectRequested)
                return false;
            lock (accessLock)
            {
                Disconnect();
                Open();
                connectionWatcher = new Thread(ConnectionWatcherTask);
                connectionWatcher.Start();
            }
            return IsConnected;
        }

        /// <summary>
        /// Disconnect the serial port.
        /// </summary>
        public void Disconnect()
        {
            if (disconnectRequested)
                return;
            disconnectRequested = true;
            Close();
            lock (accessLock)
            {
                if (connectionWatcher != null)
                {
                    if (!connectionWatcher.Join(5000))
                        connectionWatcher.Abort();
                    connectionWatcher = null;
                }
                disconnectRequested = false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the serial port is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool IsConnected
        {
            get
            {
                if (_sendException)
                    return false; //rlm

                return _serialPort != null && !gotReadWriteError && !disconnectRequested;
            }
        }

        /// <summary>
        /// Sets the serial port options.
        /// </summary>
        /// <param name="portname">Portname.</param>
        /// <param name="baudrate">Baudrate.</param>
        /// <param name="stopbits">Stopbits.</param>
        /// <param name="parity">Parity.</param>
        public void SetPort(string portname, int baudrate = 115200, StopBits stopbits = StopBits.One, Parity parity = Parity.None)
        {
            if (_portName != portname)
            {
                // set to error so that the connection watcher will reconnect
                // using the new port
                gotReadWriteError = true;
            }
            _portName = portname;
            _baudRate = baudrate;
            _stopBits = stopbits;
            _parity = parity;
        }

        public void SetNewLine(string myNewLine)
        {
            _newLine = myNewLine;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns><c>true</c>, if message was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendMessage(byte[] message)
        {
            bool success = false;

            byte[] nl = Encoding.ASCII.GetBytes(_newLine);
            byte[] fullMessage = message.Concat(nl).ToArray();

            if (IsConnected)
            {
                try
                {
                    _serialPort.Write(fullMessage, 0, fullMessage.Length);
                    success = true;
                    //logger.Debug(BitConverter.ToString(fullMessage));
                }
                catch (Exception e)
                {
                    //logger.Error(e);
                    success = false;
                    _sendException = true;
                }
            }
            return success;
        }

        public bool SendMessage(string message)
        {
            byte[] mssgArray = Encoding.ASCII.GetBytes(message);
            return SendMessage(mssgArray);
        }

        public void ClearBuffer()
        {
            if (_serialPort.IsOpen)
                _serialPort.DiscardInBuffer();
        }

        #endregion

        #region Private members

        #region Serial Port handling

        private bool Open()
        {
            bool success = false;
            lock (accessLock)
            {
                Close();
                try
                {
                    bool tryOpen = true;
                    if (Environment.OSVersion.Platform.ToString().StartsWith("Win") == false)
                    {
                        tryOpen = (tryOpen && System.IO.File.Exists(_portName));
                    }
                    if (tryOpen)
                    {
                        _serialPort = new SerialPort();
                        _serialPort.ErrorReceived += HandleErrorReceived;
                        _serialPort.PortName = _portName;
                        _serialPort.BaudRate = _baudRate;
                        _serialPort.StopBits = _stopBits;
                        _serialPort.Parity = _parity;

                        // We are not using serialPort.DataReceived event for receiving data since this is not working under Linux/Mono.
                        // We use the readerTask instead (see below).
                        _serialPort.Open();
                        //System.Threading.Thread.Sleep(100);
                        _serialPort.DiscardInBuffer();
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    //logger.Error(e);
                    Close();
                }
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    gotReadWriteError = false;
                    // Start the Reader task
                    reader = new Thread(ReaderTask);
                    reader.Start();
                    OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(true));
                }
            }
            return success;
        }

        private void Close()
        {
            lock (accessLock)
            {
                // Stop the Reader task
                if (reader != null)
                {
                    if (!reader.Join(5000))
                        reader.Abort();
                    reader = null;
                }
                if (_serialPort != null)
                {
                    _serialPort.ErrorReceived -= HandleErrorReceived;
                    if (_serialPort.IsOpen)
                    {
                        try //rlm
                        {
                            _serialPort.Close();
                        }
                        catch (Exception)
                        {
                        }
                        OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(false));
                    }
                    _serialPort.Dispose();
                    _serialPort = null;
                }
                gotReadWriteError = true;
            }
        }

        private void HandleErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //logger.Error(e.EventType);
        }

        #endregion

        #region Background Tasks

        private void ReaderTask()
        {
            while (IsConnected)
            {
                int msglen = 0;
                //
//                try
                {
                    msglen = _serialPort.BytesToRead;
                    if (msglen > 0)
                    {
                        byte[] message = new byte[msglen];
                        //
                        int readbytes = 0;
                        while (_serialPort.Read(message, readbytes, msglen - readbytes) <= 0)
                            ; // noop
                        if (MessageReceived != null)
                        {
                            OnMessageReceived(new MessageReceivedEventArgs(message));
                        }
                    }
                    else
                    {
                        Thread.Sleep(1); //rlm was 100
                    }
                }
//                catch (Exception e)
//                {
//                    //logger.Error(e);
//                    gotReadWriteError = true;
//                    Thread.Sleep(1000);
//                }
            }
        }

        private void ConnectionWatcherTask()
        {
            // This task takes care of automatically reconnecting the interface
            // when the connection drops or if an I/O error occurs
            while (!disconnectRequested)
            {
                if (gotReadWriteError)
                {
                    try
                    {
                        Close();
                        // wait 1 sec before reconnecting
                        Thread.Sleep(1000);
                        if (!disconnectRequested)
                        {
                            try
                            {
                                Open();
                            }
                            catch (Exception e)
                            {
                                //logger.Error(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //logger.Error(e);
                    }
                }
                if (!disconnectRequested)
                    Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Compile an array of COM port names associated with given VID and PID
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        private List<string> ComPortNames(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }
        public string GetFirstPort(String VID, String PID)
        {
            string myComPort = null;
            List<string> myComPorts = ComPortNames(VID, PID);
            if (myComPorts.Count > 0)
                myComPort = myComPorts[0];
            return myComPort;
        }

        #endregion

        #region Events Raising

        /// <summary>
        /// Raises the connected state changed event.
        /// </summary>
        /// <param name="args">Arguments.</param>
        protected virtual void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs args)
        {
            //logger.Debug(args.Connected);
            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged(this, args);
        }

        /// <summary>
        /// Raises the message received event.
        /// </summary>
        /// <param name="args">Arguments.</param>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs args)
        {
            //logger.Debug(BitConverter.ToString(args.Data));
            if (MessageReceived != null)
                MessageReceived(this, args);
        }

        #endregion

        #endregion

    }

}
