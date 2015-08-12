using Pinger.Models;
using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Pinger
{
    class PingThread
    {
        private Thread _thread;
        private bool _end;        
        private Ping _pingSender;
        private int _timeout;
        private byte[] _buffer;
        private PingOptions _options;        

        public delegate void PingReplyReceivedEventHandler(object sender, PingReplyReceivedEventArgs e);
        public event PingReplyReceivedEventHandler PingReplyReceived;

        public delegate void PingStoppedEventHandler(object sender, PingStoppedEventArgs e);
        public event PingStoppedEventHandler PingStopped;

        public Connection Connection
        {
            get;
            private set;
        }

        public PingThread()
        {
            _end = false;
            _pingSender = new Ping();
            _options = new PingOptions() { DontFragment = true };
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            _buffer = Encoding.ASCII.GetBytes(data);
            _timeout = 3000;
        }

        public void StartPing(Connection connection)
        {
            Connection = connection;
            _end = false;
            _thread = new Thread(_run);
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void StopPing()
        {
            _end = true;
        }

        private void _run()
        {
            while (!_end)
            {
                try
                {
                    PingReply reply = _pingSender.Send(Connection.Host, _timeout, _buffer, _options);                    
                    if (PingReplyReceived != null)
                    {                        
                        PingReplyReceived(this, new PingReplyReceivedEventArgs(reply));
                    }
                }
                catch (PingException)
                {
                    StopPing();
                    OnPingStopped(ReasonStopPing.ExceptionStop);
                    return;
                }
                Thread.Sleep(1000);
            }
            OnPingStopped(ReasonStopPing.ManualStop);
        }

        private void OnPingStopped(ReasonStopPing reason)
        {
            if (PingStopped != null)
            {
                PingStopped(this, new PingStoppedEventArgs(reason));
            }
        }

        public bool IsExecuted { get {return !_end;} }
    }

    class PingReplyReceivedEventArgs : EventArgs
    {
        public PingReplyReceivedEventArgs(PingReply reply)
        {
            PingReply = reply;
        }
        public PingReply PingReply { get; private set; }
    }

    class PingStoppedEventArgs : EventArgs
    {
        public PingStoppedEventArgs(ReasonStopPing reason)
        {
            ReasonStopPing = reason;
        }
        public ReasonStopPing ReasonStopPing { get; private set; }
    }

    enum ReasonStopPing
    {
        ManualStop,
        ExceptionStop
    }
}
