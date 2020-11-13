using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    struct SingUp
    {
        public string Name { set; get; }
        public string Password { set; get; }
        public string Login { set; get; }
    }

    struct SingIn
    {
        public string Password { set; get; }
        public string Login { set; get; }
    }

    public class Server
    {
        static TcpListener tcpListener;
        static string Ip = "192.168.1.65";
        List<Player> players = new List<Player>();
        List<Wrapper> wrappers = new List<Wrapper>();

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(Ip), 8005);
                tcpListener.Start();                

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();                    
                    Wrapper clientObject = new Wrapper(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.IAmListeningToYou));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void SendMessage(string _message, string _id)
        {
            byte[] data = Encoding.Unicode.GetBytes(_message);
            for (int i = 0; i < wrappers.Count; i++)
            {
                if (wrappers[i].Player.Id == _id)
                    wrappers[i].Stream.Write(data, 0, data.Length);
            }
        }

        protected internal void BroadcastMessage(string _message)
        {
            byte[] data = Encoding.Unicode.GetBytes(_message);
            for (int i = 0; i < wrappers.Count; i++)
            {
                wrappers[i].Stream.Write(data, 0, data.Length);
            }
        }

        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < wrappers.Count; i++)
            {
                wrappers[i].Close();
            }
            Environment.Exit(0);
        }

        public string SearchCommands(string _command, Wrapper _wrapper)
        {
            if (_command[0] == '-')
            {
                switch (_command[1])
                {
                    case 'r':                        
                        return Registration(_command);
                    case 'l':
                        return Login(_command, _wrapper);                        
                }
            }   
            return "";
        }

        public string Registration(string _command)
        {
            _command = _command.Trim(new char[] { '-', 'r' });
            var json = JsonConvert.DeserializeObject<SingUp>(_command);
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Password == json.Password && players[i].Login == json.Login)
                    return "-ru";
            }
            players.Add(new Player
            {
                Id = Guid.NewGuid().ToString(),
                Login = json.Login,
                Password = json.Password
            });
            return "-rs";
        }

        public string Login(string _command, Wrapper _wrapper)
        {
            _command = _command.Trim(new char[] { '-', 'l' });
            var json = JsonConvert.DeserializeObject<SingIn>(_command);
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Password == json.Password && players[i].Login == json.Login)
                {
                    _wrapper.Player = players[i];
                    return $"-ls{JsonConvert.SerializeObject(_wrapper.Player)}";
                }
            }
            return "-lu";
        }
    }
}
