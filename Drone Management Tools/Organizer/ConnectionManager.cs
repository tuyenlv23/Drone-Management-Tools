using System;
using System.IO.Ports;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.Organizer
{
    public class ConnectionManager
    {
        private object _locker = new object();

        public event EventHandler<string> EventWhenOpened;
        public event EventHandler<string> EventWhenClosed;
        public event EventHandler<byte[]> EventWhenDataReceived;

        private CommParm _commParm;
        public CommParm CommParm
        {
            get { return _commParm; }
            set { _commParm = value; }
        }

        private bool _open;
        public bool IsOpen
        {
            get => _open;
        }

        private string _message;
        public string Message
        {
            get => _message;
        }

        private SerialPort _serialPort;

        private System.Timers.Timer _timer_Monitor;

        public ConnectionManager() 
        {
            this._commParm = new CommParm();

            this._timer_Monitor = new System.Timers.Timer();
            this._timer_Monitor.Interval = 1000;
            this._timer_Monitor.Elapsed += Timer_Monitor_Elapsed;
        }

        private void StartTimerMonitor()
        {
            this._timer_Monitor?.Start();
        }

        private void StopTimerMonitor()
        {
            this._timer_Monitor?.Stop();
        }

        private void Timer_Monitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this._serialPort == null || !this._serialPort.IsOpen)
            {
                StopTimerMonitor();
                OnClosed(CommStates.Closed.ToString());                
            }    
        }

        public void OnOpened()
        {
            this._open = true;
            this._message = $"Serial port [{this._commParm.ComPort}] {CommStates.Opened}";
            EventWhenOpened.Invoke(this, this._message);
            LogUtils.AddLog(this._message, LogTypes.Info);
        }

        public void OnClosed(string state)
        {
            this._open = false;
            this._message = $"Serial port [{this._commParm.ComPort}] {state}";
            EventWhenClosed.Invoke(this, this._message);
            LogUtils.AddLog(this._message, LogTypes.Info);
        }

        private bool ValidateSerial()
        {
            try
            {
                if (this._serialPort == null)
                {
                    this._serialPort = new SerialPort(this._commParm.ComPort, this._commParm.Baudrate);
                    this._serialPort.DataReceived += SerialPort_DataReceived;
                    this._serialPort.ReadTimeout = 500;
                    this._serialPort.WriteTimeout = 500;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ConnectSerial()
        {
            lock (this._locker)
            {
                try
                {        
                    if (ValidateSerial())
                    {
                        this._serialPort.PortName = this._commParm.ComPort;
                        this._serialPort.BaudRate = this._commParm.Baudrate;
                        if (this._serialPort != null && !this._serialPort.IsOpen)
                        {
                            this._serialPort.Open();
                            OnOpened();
                            StartTimerMonitor();
                        }
                    }    
                }
                catch
                {
                    OnClosed($"open error");
                }
            }    
        }

        public void DisconnectSerial()
        {
            lock (this._locker)
            {
                try
                {                    
                    if (this._serialPort != null && this._serialPort.IsOpen)
                    {
                        this._serialPort.Close();
                        this._serialPort.Dispose();
                        this._serialPort.DataReceived -= SerialPort_DataReceived;
                        this._serialPort = null;                        
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.AddLog(ex.Message, LogTypes.Error);
                }
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //var _dataStr = this._serialPort.ReadExisting();
                //var _encodeUtils = new EncodeUtils();
                //var _datas = _encodeUtils.ConvertStringToHex(_dataStr);

                int _length = this._serialPort.BytesToRead;
                byte[] _datas = new byte[_length];
                this._serialPort.Read(_datas, 0, _length);
#if DEBUG
                LogUtils.AddLog($"SerialPort Data Received: {_datas.Length} bytes", LogTypes.Info);
#endif                                
                EventWhenDataReceived.Invoke(this, _datas);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public void WriteData(string data)
        {
            if (this.IsOpen)
                this._serialPort.WriteLine(data);
        }

        public void WriteData(byte[] bytes)
        {
#if DEBUG
            LogUtils.AddLog($"Data sent: {BitConverter.ToString(bytes)}", LogTypes.Info);
#endif
            if (this.IsOpen)
                this._serialPort.Write(bytes, 0, bytes.Length);
        }
    }
}
