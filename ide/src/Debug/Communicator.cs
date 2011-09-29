using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading;
using System.IO;

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

    public class ReadState
    {
        /// <summary>
        /// The raw ASCII bytes associated with this read state.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// The raw ASCII string associated with this read state.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The network stream associated with this read state.
        /// </summary>
        public NetworkStream Stream { get; set; }

        /// <summary>
        /// The index of the null message terminator within the
        /// data string.
        /// </summary>
        public int NullIndex
        {
            get
            {
                return this.Data.IndexOf("\0\0");
            }
        }

        /// <summary>
        /// Creates a new read state.
        /// </summary>
        public ReadState(NetworkStream stream)
        {
            this.Stream = stream;
            this.Bytes = new byte[256];
            this.Data = "";
        }
    }

    public class Communicator
    {
        private TcpListener m_Listener = null;
        private Thread m_Thread = null;
        private Queue<Message> m_MessageQueue = new Queue<Message>();

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

        /// <summary>
        /// Sends a message to the game via the debugging interface.
        /// </summary>
        /// <param name="m">The message to send.</param>
        public void Send(Message m)
        {
            this.m_MessageQueue.Enqueue(m);
        }

        private void ListenThread()
        {
            while (true)
            {
                // Accept a connection from the engine debugger.
                TcpClient client = this.m_Listener.AcceptTcpClient();

                // Get a stream object for reading and writing.
                NetworkStream stream = client.GetStream();

                // Start an asynchronous request to start reading data from
                // the socket.
                ReadState state = new ReadState(stream);
                stream.BeginRead(state.Bytes, 0, state.Bytes.Length, this.stream_OnRead, state);

                // Now loop continously, checking for new messages in the queue
                // that we need to send off.
                while (true)
                {
                    // First check if we need to send any data back to
                    // the game (such as any messages we want to send).
                    if (this.m_MessageQueue.Count > 0)
                    {
                        Message msg = this.m_MessageQueue.Dequeue();
                        string wdata = JsonConvert.SerializeObject(msg);
                        List<byte> wbytes = new List<byte>(Encoding.ASCII.GetBytes(wdata));
                        wbytes.Add(0);
                        wbytes.Add(0);

                        stream.Write(wbytes.ToArray(), 0, wbytes.Count);
                    }

                    // Let the thread sleep and the continue the while loop.
                    Thread.Sleep(0);
                    continue;
                }
            }
        }

        public void stream_OnRead(IAsyncResult result)
        {
            // Convert the userdata back into a ReadState.
            ReadState state = result.AsyncState as ReadState;

            // Append the data that was read.
            state.Data += Encoding.ASCII.GetString(state.Bytes, 0, state.Bytes.Length);
            state.Bytes = new byte[256];

            // Check to make sure there is a null terminator.
            if (state.NullIndex == -1)
            {
                // There is no data terminator yet, so send
                // another read request.
                try
                {
                    state.Stream.EndRead(result);
                    state.Stream.BeginRead(state.Bytes, 0, state.Bytes.Length, this.stream_OnRead, state);
                }
                catch (IOException)
                {
                    // The network connection may have been forcibly closed when the game
                    // was, so ignore this exception and simply don't send another read request.
                }
                return;
            }

            // We have at least a message in here; continually
            // pull messages out until we are left with no
            // more terminators.
            while (state.NullIndex != -1)
            {
                // Get the message and remove it from the buffer.
                string msgraw = state.Data.Substring(0, state.Data.IndexOf("\0\0")).TrimStart(new char[] { '\0' });
                state.Data = state.Data.Substring(state.Data.IndexOf("\0\0") + 1);

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
                    if (t.BaseType != null && t.BaseType.FullName.EndsWith("MOAI.Debug.Message"))
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

            // We have finished processing all of the messages, so
            // send another read request.
            try
            {
                state.Stream.EndRead(result);
                state.Stream.BeginRead(state.Bytes, 0, state.Bytes.Length, this.stream_OnRead, state);
            }
            catch (IOException)
            {
                // The network connection may have been forcibly closed when the game
                // was, so ignore this exception and simply don't send another read request.
            }
        }
    }
}
