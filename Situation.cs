namespace MazeChallenge
{
    public class Situation
    {
        public int CurrentPositionX { get; set; }
        public int CurrentPositionY { get; set; }
        public bool NorthBlocked { get; set; }
        public bool SouthBlocked { get; set; }
        public bool EastBlocked { get; set; }
        public bool WestBlocked { get; set; }

        public Situation(int currentPositionX, int currentPositionY, bool northBlocked, bool southBlocked, bool eastBlocked, bool westBlocked)
        {
            CurrentPositionX = currentPositionX;
            CurrentPositionY = currentPositionY;

            NorthBlocked = northBlocked;
            SouthBlocked = southBlocked;
            EastBlocked = eastBlocked;
            WestBlocked = westBlocked;
        }
    }
}
