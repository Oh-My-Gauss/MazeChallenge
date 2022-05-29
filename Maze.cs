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

        //Linea 15 y 16. En general esto se suele sacar a un archivo de configuración (una clase llamada Config). O se usan como variables de entorno (key vault)
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
            //Esto es un casteo a String? Si así es (no sé cómo va bien en .NET) pero los casteos hay que asegurarse que se pueden hacer bien.
            //Para ello try/catch + throw exceptions (Una muy común es NumberFormatException, míratela)
            string mazeUid = (string)APIs.DeserializeJSON(createNewMaze, APIs.APICall(createNewMaze, uriToCallMaze, string.Empty, string.Empty, httpMethodPOST, mazeBodyJSON));

            Game newGame = new Game();
            string gameBodyJSON = APIs.SerializeJSON(createNewGame, newGame);
            //same que 36, si aplica
            string gameUid = (string)APIs.DeserializeJSON(createNewGame, APIs.APICall(createNewGame, uriToCall, mazeUid, string.Empty, httpMethodPOST, gameBodyJSON));

            string lookingAroundStringJSON = APIs.APICall(lookAround, uriToCall, mazeUid, gameUid, httpMethodGET, string.Empty);
            //same que 36, si aplica
            Situation situation = (Situation)APIs.DeserializeJSON(lookAround, lookingAroundStringJSON);

            //Once we have the initial info, we start the algorithm to solve the maze.


            bool[,] wasHere = new bool[newMaze.Width, newMaze.Height];

            for (int i = 0; i < newMaze.Width; i++)
            {
                for (int j = 0; j < newMaze.Height; j++)
                    wasHere[i, j] = false;
            }
            Stack<Situation> lastVisited = new Stack<Situation>();

            bool SolveMaze(Situation situation)
            {
                lastVisited.Push(situation);
                //Esto puede dar IndexOutOfRange en algún caso? Lo has valorao?
                while (situation.CurrentPositionX < newMaze.Width - 1 & situation.CurrentPositionY < newMaze.Height - 1)
                {
                    //La ifesta esta.. Igual no hay otra manera y no es el caso, pero para que lo tengas en mente y valores el hacer cosas como:
                    //if (situation.CurrentPositionX >= (lo contrario) newMaze.Width - 1)
                    //{ pej return; }
                    //Esto es para evitar tanto anidamiento. Si ya lo sabías, ignórame y dime imbécil
                    //Valora tb sacarlo a un función (igual te viene bien para practicar) pq hay mucho código repetido
                    if (situation.CurrentPositionX < newMaze.Width - 1)
                    {
                        if (!wasHere[situation.CurrentPositionX + 1, situation.CurrentPositionY])
                        {
                            if (!situation.EastBlocked)
                            {
                                situation = MoveTo(mazeUid, gameUid, east);
                                lastVisited.Push(situation);
                                wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                                continue;
                            }
                        }
                    }
                    if (situation.CurrentPositionY < newMaze.Height - 1)
                    {
                        if (!wasHere[situation.CurrentPositionX, situation.CurrentPositionY + 1])
                        {
                            if (!situation.SouthBlocked)
                            {
                                situation = MoveTo(mazeUid, gameUid, south);
                                lastVisited.Push(situation);
                                wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                                continue;
                            }
                        }
                    }
                    if (situation.CurrentPositionX > 0)
                    {
                        if (!wasHere[situation.CurrentPositionX - 1, situation.CurrentPositionY])
                        {
                            if (!situation.WestBlocked)
                            {
                                situation = MoveTo(mazeUid, gameUid, west);
                                lastVisited.Push(situation);
                                wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                                continue;
                            }
                        }
                    }
                    if (situation.CurrentPositionY > 0)
                    {
                        if (!wasHere[situation.CurrentPositionX, situation.CurrentPositionY - 1])
                        {
                            if (!situation.NorthBlocked)
                            {
                                situation = MoveTo(mazeUid, gameUid, north);
                                lastVisited.Push(situation);
                                wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                                continue;
                            }
                        }
                    }

                    Situation currentSituation;
                    Situation wantToGoSituation;

                    //Para deshacer el camino, comparamos donde estamos y a donde queremos ir y volvemos hacia atrás en la pila.
                    currentSituation = lastVisited.Peek();
                    //No handleas las excepciones que puede throwear el .Pop(). Same for peek()
                    lastVisited.Pop();
                    wantToGoSituation = lastVisited.Peek();

                    if (wantToGoSituation.CurrentPositionX - currentSituation.CurrentPositionX == 0)
                    {
                        //Te vale tanto < como <= para entrar en el else?
                        if (currentSituation.CurrentPositionY - wantToGoSituation.CurrentPositionY > 0)
                        {
                            situation = MoveTo(mazeUid, gameUid, north);
                        }
                        else situation = MoveTo(mazeUid, gameUid, south);
                    }
                    if (wantToGoSituation.CurrentPositionY - currentSituation.CurrentPositionY == 0)
                    {
                        //Same que line 135
                        if (currentSituation.CurrentPositionX - wantToGoSituation.CurrentPositionX > 0)
                        {
                            situation = MoveTo(mazeUid, gameUid, west);
                        }
                        else situation = MoveTo(mazeUid, gameUid, east);
                    }
                }
                return true;
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

