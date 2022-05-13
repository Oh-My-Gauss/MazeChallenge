//Sesión 1. Inicio 22:30 del 13/05/22 Fin 0:40 del 14/05/22 (2horas 10).
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class MazeGame
{
    static void Main(string[] args)
    {
        
    }
    //API 1 CreateANewMaze
    static string CreateANewMaze()
    {
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(new HttpMethod("POST"), "https://mazerunnerapi.azurewebsites.net/api/Maze?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw==");

        request.Content = new StringContent("{\n    Width:25,\n    Height:25\n}");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        var response = httpClient.SendAsync(request).Result;
        string resultsAPI1 = response.Content.ReadAsStringAsync().Result;
        dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI1);
        string mazeUid = DynamicData.mazeUid;
        return mazeUid;
    }

    //API 2 
    static string CreateANewGame()
    {
        StringBuilder uri = new StringBuilder("https://mazerunnerapi.azurewebsites.net/api/Game/?code=bINetL5Vm7pVuoPm/SXIMi9Niv3D9DxpPQ8tDPbsyJ0J4KZfSQl/yA==");
        uri.Insert(49, $"{CreateANewMaze}");

        var httpClient2 = new HttpClient();
        var request2 = new HttpRequestMessage(new HttpMethod("POST"), uri.ToString());

        request2.Content = new StringContent("{\n    \"Operation\":\"Start\"\n}");
        request2.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        var response2 = httpClient2.SendAsync(request2).Result;
        string resultsAPI2 = response2.Content.ReadAsStringAsync().Result;
        dynamic DynamicData2 = JsonConvert.DeserializeObject(resultsAPI2);
        string gameUid = DynamicData2.gameUid;
        return gameUid;
    }

    //API 3 
    public class BlockedPlaces
    {
       public static string TakeALook()
        {
            StringBuilder uri = new StringBuilder("https://mazerunnerapi.azurewebsites.net/api/Game/?code=bINetL5Vm7pVuoPm/SXIMi9Niv3D9DxpPQ8tDPbsyJ0J4KZfSQl/yA==");
            uri.Insert(49, $"{CreateANewMaze}");
            uri.Insert(85, $"/{CreateANewGame}");

            var httpClient3 = new HttpClient();
            var request3 = new HttpRequestMessage(new HttpMethod("GET"), uri.ToString());
            var response3 = httpClient3.SendAsync(request3).Result;
            string resultsAPI3 = response3.Content.ReadAsStringAsync().Result;
            return resultsAPI3;
        }


        dynamic DynamicData3 = JsonConvert.DeserializeObject(resultsAPI3);
        string currentPositionX = DynamicData3.currentPositionX;
        string currentPositionY = DynamicData3.currentPositionY;
        bool northBlocked = DynamicData3.mazeBlockView.northBlocked;
        bool southBlocked = DynamicData3.mazeBlockView.southBlocked;
        bool westBlocked = DynamicData3.mazeBlockView.westBlocked;
        bool eastBlocked = DynamicData3.mazeBlockView.eastBlocked;

    }



    //MoveEast


    //var httpClient4 = new HttpClient();
    //    var request4 = new HttpRequestMessage(new HttpMethod("POST"), uri.ToString());
    //    var response4 = httpClient3.SendAsync(request3).Result;
    //    request4.Content = new StringContent("{\n    \"Operation\":\"GoEast\"\n}");
    //    request4.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
    //    string resultsAPI4 = response3.Content.ReadAsStringAsync().Result;
    //    dynamic DynamicData4 = JsonConvert.DeserializeObject(resultsAPI3);

    ////MoveSouth

    //var httpClient4 = new HttpClient();
    //var request4 = new HttpRequestMessage(new HttpMethod("POST"), uri.ToString());
    //var response4 = httpClient3.SendAsync(request3).Result;
    //request4.Content = new StringContent("{\n    \"Operation\":\"GoSouth\"\n}");
    //request4.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
    //    string resultsAPI4 = response3.Content.ReadAsStringAsync().Result;
    //dynamic DynamicData4 = JsonConvert.DeserializeObject(resultsAPI3);




    //crear properties y cosas mejores para acceder a la posición y a los bloqueos. Y hacer llamadas desde funciones.. (OOP curso, darle caña por ahí)


    //string currentPositionX = DynamicData4.currentPositionX;
    //string currentPositionY = DynamicData4.currentPositionY;
    //bool northBlocked = DynamicData3.mazeBlockView.northBlocked;
    //bool southBlocked = DynamicData3.mazeBlockView.southBlocked;
    //bool westBlocked = DynamicData3.mazeBlockView.westBlocked;
    //bool eastBlocked = DynamicData3.mazeBlockView.eastBlocked;





}



    //public static void CreateANewMazeAndStartAGame()
    //{

    //}




}