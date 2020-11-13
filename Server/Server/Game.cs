using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Game
    {
        public int MaxPlayers { set; get; } = 6;
        public List<Player> TeamA { set; get; }
        public List<Player> TeamB { set; get; }
        public string Stage { set; get; }
    }
}
