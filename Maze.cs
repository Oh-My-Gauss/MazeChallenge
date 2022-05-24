namespace MazeChallenge
{
    internal class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Maze(int width, int height)
        {
            Height = height;
            Width = width;
        }

        const string uriToCall = "https://mazerunnerapi.azurewebsites.net/api/Game/?code=bINetL5Vm7pVuoPm/SXIMi9Niv3D9DxpPQ8tDPbsyJ0J4KZfSQl/yA==";
        const string uriToCallMaze = "https://mazerunnerapi.azurewebsites.net/api/Maze?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw==";

        const string httpMethodPOST = "POST";
        const string httpMethodGET = "GET";

        const string createNewMaze = "NewMaze";
        const string createNewGame = "NewGame";
        const string lookAround = "LookAround";
        const string moveTo = "MoveTo";

        Movement north = new Movement("North");
        Movement south = new Movement("South");
        Movement east = new Movement("East");
        Movement west = new Movement("West");

        public bool Play(Maze newMaze)

        {
            // From the maze we just created, we initiate a new Game session and its objects. We also make the API calls to collect both mazeUid and gameUid that we will need for the hole session.
            string mazeBodyJSON = APIs.SerializeJSON(createNewMaze, newMaze);
            string mazeUid = (string)APIs.DeserializeJSON(createNewMaze, APIs.APICall(createNewMaze, uriToCallMaze, string.Empty, string.Empty, httpMethodPOST, mazeBodyJSON));

            Game newGame = new Game();
            string gameBodyJSON = APIs.SerializeJSON(createNewGame, newGame);
            string gameUid = (string)APIs.DeserializeJSON(createNewGame, APIs.APICall(createNewGame, uriToCall, mazeUid, string.Empty, httpMethodPOST, gameBodyJSON));

            string lookingAroundStringJSON = APIs.APICall(lookAround, uriToCall, mazeUid, gameUid, httpMethodGET, string.Empty);
            Situation situation = (Situation)APIs.DeserializeJSON(lookAround, lookingAroundStringJSON);

            //Once we have the initial info, we start the algorithm to solve the maze.


            bool[,] wasHere = new bool[newMaze.Width, newMaze.Height];

            for (int i = 0; i < newMaze.Width; i++)
            {
                for (int j = 0; j < newMaze.Height; j++)
                    wasHere[i, j] = false;
            }

            bool SolveMaze(Situation situation)
            {
                if (situation.CurrentPositionX == newMaze.Width - 1 & situation.CurrentPositionY == newMaze.Height - 1)
                {
                    return true;
                }
                if (wasHere[situation.CurrentPositionX, situation.CurrentPositionY]) 
                { 
                    return false; 
                }
                wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;

                if (!situation.EastBlocked & situation.CurrentPositionX < newMaze.Width - 1)
                {
                    if (SolveMaze(MoveTo(mazeUid, gameUid, east)))
                    {
                        return true;
                    }
                }
                if (!situation.SouthBlocked & situation.CurrentPositionX < newMaze.Height - 1)
                {
                    if (SolveMaze(MoveTo(mazeUid, gameUid, south)))
                    {
                        return true;
                    }
                }
                if (!situation.WestBlocked & situation.CurrentPositionX > 0)
                {
                    if (SolveMaze(MoveTo(mazeUid, gameUid, west)))
                    {
                        return true;
                    }
                }
                if (!situation.NorthBlocked & situation.CurrentPositionY > 0 )
                {
                    if (SolveMaze(MoveTo(mazeUid, gameUid, north)))
                    {
                        return true;
                    }                    
                }
                return false;
            }
            return SolveMaze(situation);
        }

        private Situation MoveTo(string mazeUid, string gameUid, Movement nextMove)
        {
            Situation situation;
            string movementBodyJSON = APIs.SerializeJSON(moveTo, nextMove);
            string newSituation = APIs.APICall(moveTo, uriToCall, mazeUid, gameUid, httpMethodPOST, movementBodyJSON);
            situation = (Situation)APIs.DeserializeJSON(moveTo, newSituation);
            return situation;
        }
    }
}
