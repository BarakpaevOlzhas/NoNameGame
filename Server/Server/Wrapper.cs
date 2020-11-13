using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server
{
    public class Wrapper
    {
        public Player Player { set; get; }
        protected internal NetworkStream Stream { get; private set; }
        private TcpClient client;
        private Server server;

        public Wrapper(TcpClient _tcpClient, Server _server)
        {
            server = _server;
            client = _tcpClient;            
        }

        public void IAmListeningToYou()
        {
            try
            {
                Stream = client.GetStream();
                
                while (client.Connected)
                {
                    string message = GetMessage();
                    string command = server.SearchCommands(message, this); 
                    Feedback(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (client != null)
                    client.Close();
            }
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }

        private void Feedback(string _command)
        {
            if (_command != null)
            {
                //code
            }
            _command = "";
        }

        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }
    }
}
