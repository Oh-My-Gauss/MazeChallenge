using Newtonsoft.Json;

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
        /*****Respuesta:*****/
        //Tiene sentido. No lo he arreglado por falta de tiempo y ganas, pero me parece muy interesante tenerlo en cuenta.
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

        //Hacer una clase llamada MazeForCreation y otra llamada MazeResult
        public bool Play(Maze newMaze)
        {
            string mazeBodyJSON = JsonConvert.SerializeObject(newMaze);
            //Esto es un casteo a String? Si así es (no sé cómo va bien en .NET) pero los casteos hay que asegurarse que se pueden hacer bien.
            //Para ello try/catch + throw exceptions (Una muy común es NumberFormatException, míratela)
            /*****Respuesta:*****/
            //He estado leyendo respecto a castear. En .net está el as string al final, que es un casteo condicional.
            //Te castea si puede y si no te devuelve null. De ahí se deduce lo que va detraás del código y creo que es más limpio que un try catch. 
            //Lo aplico a todas las lineas que había casteos.


            string mazeResponse = APIs.APICall(createNewMaze, uriToCallMaze, string.Empty, string.Empty, httpMethodPOST, mazeBodyJSON);
            dynamic mazeDynamic = JsonConvert.DeserializeObject(mazeResponse);
            string? mazeUid = mazeDynamic.mazeUid;
            if (mazeUid is null)
            {
                Console.WriteLine("Error al crear el Maze (mazeUid no obtenido tras la request");
                return false;
            }
            Game newGame = new Game();
            string gameBodyJSON = JsonConvert.SerializeObject(newGame);
            //same que 36, si aplica
            //resuelto más arriba 

            string? gameUid = APIs.DeserializeJSON(createNewGame, APIs.APICall(createNewGame, uriToCall, mazeUid, string.Empty, httpMethodPOST, gameBodyJSON)) as string;
            if (gameUid is null)
            {
                Console.WriteLine("Error al iniciar la sesión de juego (no ha vuelto gameUid)");
                return false;
            }

            string lookingAroundStringJSON = APIs.APICall(lookAround, uriToCall, mazeUid, gameUid, httpMethodGET, string.Empty);
            //same que 36, si aplica
            //resuelto más arriba

            Situation? situation = APIs.DeserializeJSON(lookAround, lookingAroundStringJSON) as Situation;
            if (situation is null)
            {
                Console.WriteLine("Error al devolver la primera situación del Maze (Sin respuesta tras el lookAround, revisar la request)");
                return false;
            }

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

                /*****Respuesta:*****/

                //A qué te refieres con "esto"? Si es a  newMaze.Width-1 la respuesta es no ya que el Width tendrá un tamañano 2x2 al menos (un maze de 0x0 o 1x1 no tendría sentido).
                while (situation.CurrentPositionX < newMaze.Width - 1 & situation.CurrentPositionY < newMaze.Height - 1)
                {
                    //La ifesta esta.. Igual no hay otra manera y no es el caso, pero para que lo tengas en mente y valores el hacer cosas como:
                    //if (situation.CurrentPositionX >= (lo contrario) newMaze.Width - 1)
                    //{ pej return; }
                    //Esto es para evitar tanto anidamiento. Si ya lo sabías, ignórame y dime imbécil
                    //Valora tb sacarlo a un función (igual te viene bien para practicar) pq hay mucho código repetido

                    /*****Respuesta:*****/
                    // He cambiado los 3 ifs anidados por dos uniendo dos de ellos con un &. El tercero no se puede unir ya que podríamos tener un indexOutOfBounds exception al hacer wasHere[de algo +1 o -1]
                    //Sigue funcionando y parece más limpio. Lo que me comentas de if (situation.CurrentPositionX >= (lo contrario) newMaze.Width - 1) no se podría aplicar por las condiciones del problema.
                    
                    //Se podría refactorizar con un switch, las 4 condiciones para moverse quedarían dentro, no sé si es más limpio o es lo mismo
                
                    
                    
                    if (situation.CurrentPositionX < newMaze.Width - 1 & !situation.EastBlocked)
                    {
                        if (!wasHere[situation.CurrentPositionX + 1, situation.CurrentPositionY])
                        {
                            situation = MoveTo(mazeUid, gameUid, east);
                            lastVisited.Push(situation);
                            wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                            continue;
                        }
                    }
                    if (situation.CurrentPositionY < newMaze.Height - 1 & !situation.SouthBlocked)
                    {
                        if (!wasHere[situation.CurrentPositionX, situation.CurrentPositionY + 1])
                        {
                            situation = MoveTo(mazeUid, gameUid, south);
                            lastVisited.Push(situation);
                            wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                            continue;
                        }
                    }
                    if (situation.CurrentPositionX > 0 & !situation.WestBlocked)
                    {
                        if (!wasHere[situation.CurrentPositionX - 1, situation.CurrentPositionY])
                        {
                            situation = MoveTo(mazeUid, gameUid, west);
                            lastVisited.Push(situation);
                            wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                            continue;
                        }
                    }
                    if (situation.CurrentPositionY > 0 & !situation.NorthBlocked)
                    {
                        if (!wasHere[situation.CurrentPositionX, situation.CurrentPositionY - 1])
                        {
                            situation = MoveTo(mazeUid, gameUid, north);
                            lastVisited.Push(situation);
                            wasHere[situation.CurrentPositionX, situation.CurrentPositionY] = true;
                            continue;
                        }
                    }
                    Situation currentSituation;
                    Situation wantToGoSituation;
                   //lastVisited.Clear();
                    if (lastVisited.Count>1)
                    {
                        currentSituation = lastVisited.Pop();
                        wantToGoSituation = lastVisited.Peek();
                    }
                   else
                    {                        
                        Console.WriteLine("Error al rellenar la pila, revisar el algoritmo");
                        throw new InvalidOperationException();
                        //return false;
                    }

                    //No handleas las excepciones que puede throwear el .Pop(). Same for peek()

                    /*****Respuesta:*****/

                    //He creado esas condiciones para comprobar antes de llamar a .Pop y .Peek que la pila tiene suficiente tamañano como para hacerlo.


                    if (wantToGoSituation.CurrentPositionX - currentSituation.CurrentPositionX == 0)
                    {
                        //Te vale tanto < como <= para entrar en el else?

                        /*****Respuesta:*****/
                        // Sí y no. No me vale el = pero no es posible que sea=  ya que o bien me he movido horizontal o bien vertical.
                        //Si sé que la posición en X es igual (estoy en un if que me lo confirma)
                        // implica que me he movido verticalmente, por tanto el = no es posible, por lo que en el else solo entra con el <.

                        if (currentSituation.CurrentPositionY - wantToGoSituation.CurrentPositionY > 0)
                        {
                            situation = MoveTo(mazeUid, gameUid, north);
                            continue;
                        }
                        situation = MoveTo(mazeUid, gameUid, south);
                        continue;
                    }
                    if (wantToGoSituation.CurrentPositionY - currentSituation.CurrentPositionY == 0)
                    {
                        //Same que line 135

                        /*****Respuesta:*****/

                        //Misma respuesta aplica
                        if (currentSituation.CurrentPositionX - wantToGoSituation.CurrentPositionX > 0)
                        {
                            situation = MoveTo(mazeUid, gameUid, west);
                            continue;
                        }
                        situation = MoveTo(mazeUid, gameUid, east);
                    }
                }
                return true;
            }
            return SolveMaze(situation);
        }

        private Situation MoveTo(string mazeUid, string gameUid, Movement nextMove)
        {
            Situation situation;
            string movementBodyJSON = JsonConvert.SerializeObject(nextMove);
            string newSituation = APIs.APICall(moveTo, uriToCall, mazeUid, gameUid, httpMethodPOST, movementBodyJSON);
            situation = (Situation)APIs.DeserializeJSON(moveTo, newSituation);
            return situation;
        }
    }
}

