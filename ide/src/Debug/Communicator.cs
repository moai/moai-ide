using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading;

namespace MOAI.Debug
{
    public class MessageEventArgs : EventArgs
    {
        public Message Message { get; set; }

        public MessageEventArgs(Message m)
        {
            this.Message = m;
        }
    }

    public class Communicator
    {
        private TcpListener m_Listener = null;
        private Thread m_Thread = null;
        public event EventHandler<MessageEventArgs> MessageArrived;

        /// <summary>
        /// Creates a new debug communicator listening on the specified port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        public Communicator(int port)
        {
            this.m_Listener = new TcpListener(port);
            this.m_Listener.Start();

            this.m_Thread = new Thread(ListenThread);
            this.m_Thread.Start();
        }

        public void Close()
        {
            this.m_Thread.Abort();
            this.m_Listener.Stop();
        }

        private void ListenThread()
        {
            byte[] bytes = new byte[256];
            string data = null;

            MOAI.Debug.Messages.ExcpInternalMessage eim = new MOAI.Debug.Messages.ExcpInternalMessage();
            eim.ExceptionMessage = "The variable 'viewport' is nil.";
            eim.FileName = "Main.lua";
            eim.LineNumber = 10;
            eim.FunctionName = "@main";
            if (this.MessageArrived != null)
                this.MessageArrived(this, new MessageEventArgs(eim));

            while (true)
            {
                // Accept a connection from the engine debugger.
                TcpClient client = this.m_Listener.AcceptTcpClient();
                data = "";

                // Get a stream object for reading and writing.
                NetworkStream stream = client.GetStream();

                // Read the data from the stream as it arrives.
                int i = 0;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Append the data that was read.
                    data += Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                    // Get the index of the terminator.
                    int index = data.IndexOf("\0\0");
                    if (index == -1)
                    {
                        // There's no data terminator yet.
                        continue;
                    }

                    // We have at least a message in here; continually
                    // pull messages out until we are left with no
                    // more terminators.
                    while (data.IndexOf("\0\0") != -1)
                    {
                        // Get the message and remove it from the buffer.
                        string msgraw = data.Substring(0, data.IndexOf("\0\0"));
                        data = data.Substring(data.IndexOf("\0\0") + 1);

                        // Convert the message into a message object, from which
                        // we can work out the actual type.
                        Message msgbase = JsonConvert.DeserializeObject<Message>(msgraw);
                        if (msgbase == null)
                        {
                            // It might be a case of trailing \0\0 sequences.
                            continue;
                        }

                        // Dynamically work out what class it really is using
                        // reflection.
                        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
                        Type real = null;
                        foreach (Type t in types)
                            if (t.BaseType.FullName.EndsWith("MOAI.Debug.Message"))
                                if ((string)t.GetProperty("StaticID").GetValue(null, null) == msgbase.ID)
                                {
                                    real = t;
                                    break;
                                }

                        // Check to ensure we actually found a class.
                        if (real != null)
                        {
                            // Probably should throw an error here as the engine is
                            // sending a debugging message we can't handle...
                            Message msgreal = JsonConvert.DeserializeObject(msgraw, real) as Message;

                            // Fire the relevant event.
                            if (this.MessageArrived != null)
                                this.MessageArrived(this, new MessageEventArgs(msgreal));
                        }
                    }
                }
            }
        }
    }
}
