using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MazeChallenge
{
    internal class Game
    {
        public string Operation { get; set; }
        public Game()
        {
            Operation = "Start";
        }
    }
}
