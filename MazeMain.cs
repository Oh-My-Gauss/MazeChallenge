//Sesión 1. Inicio 22:30 del 13/05/22 Fin 0:45 del 14/05/22 (2horas 15).
//Sesión 2. Inicio 18:30 del 16/05/22 Fin 19:45 del 16/05/22 (1hora).
//Sesión 3. Inicio 17:45 del 19/05/22 Fin 18:15 del 19/05/22 (30 mins)
//sesión 4. Inicio 11:00 del 21/05/22 Fin 11:52 del 21/05/22 (50 mins)
//Sesión 5. Inicio 12:15 del 22/05/22 Fin 15:30 del 22/05/22 (2horas 45 mins)
//Sesión 6. Inicio 16:00 del 22/05/22 Fin 17:45 del 22/05/22 (1 hora 45 mins)
//Sesion 7. Inicio 20:00 del 23/05/22 Fin 22:00 del 23/05/22 (2 horas)
//Tiempo total 11 horas 5 mins. 

namespace MazeChallenge;
public class MazeMain
{
    static void Main(string[] args)
    {
        Maze newMaze = new Maze(25, 25);
        //if (newMaze.Play(newMaze))
        //{
        //    Console.WriteLine("The maze was solved");
        //}
        Console.WriteLine((newMaze.Play(newMaze)) ? "The maze was solved" : "The maze was imposible to solve");
    }
}



