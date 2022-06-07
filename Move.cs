namespace MazeChallenge
{
    public class Movement
    {
        public string Operation { get; set; }
        public Movement(string nextMove)
        {
            Operation = "Go" + nextMove;
        }
    }
}
