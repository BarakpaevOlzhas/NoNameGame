using System;
using System.Net.Sockets;

namespace Server
{
    public class Player
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public int WinAmount { set; get; }
        public int LossAmount { set; get; }
        GameСharacter Character { set; get; }
        public string Password { set; get; }
        public string Login { set; get; }
    }
}