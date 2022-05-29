using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MazeChallenge
{
    public class APIs
    {
        //Tengo mis dudas sobre los static methods.. luego mockearlos (ya verás lo que es) son un pisto.. Pero no estoy 100% seguro si debería serlo o no. Me gustaría hablar con rico sobre esto
        public static string APICall(string callToMake, string uriToCall, string mazeUid, string gameUid, string httpMethod, string bodyJSON)
        {
            StringBuilder uri = new StringBuilder(uriToCall);

            //Call to make sólo puede ser eso no? Si callToMake te viene como "aslkñfjañsfj" o null o 15, qué harías?
            if (callToMake == "LookAround" || callToMake == "MoveTo")
            {
                //Por qué 49 y 85? Qué pasa si dentro de 9 años es 58, 22? Igual es más fácil poner esto 
                //como variable de entorno/archivo de config, y cambiarlo sólo ahí. No tener que hacer buscarlo por el código e ir cambiándolo
                uri.Insert(49, mazeUid);
                uri.Insert(85, $"/{gameUid}");
            }
            else if (callToMake == "NewGame")
            {
                uri.Insert(49, mazeUid);
            }
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), uri.ToString());
            request.Content = new StringContent(bodyJSON);
            //No creo que request.Content sea null, pero si lo fuera, al hacer .Headers petaría
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = httpClient.SendAsync(request).Result;
            //Lo mismo que en line 28 pero aquí sí que no sabemos qué nos devuelve response.
            //Habría que throwear excepciones en función del status code que recibamos
            string resultsAPI = response.Content.ReadAsStringAsync().Result;
            return resultsAPI;
        }


        public static object DeserializeJSON(string callToMake, string resultsAPI)
        {
            //Mismo que line 13
            if (callToMake == "NewMaze")
            {
                //Qué pasa si nos falla la deserialización del Object? 
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);
                string mazeUid = DynamicData.mazeUid;
                return mazeUid;
            }
            else if (callToMake == "NewGame")
            {
                //Same que line 41
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);
                string gameUid = DynamicData.gameUid;
                return gameUid;
            }
            else if (callToMake == "LookAround" || callToMake == "MoveTo")
            {
                if (resultsAPI== " -You shall not pass- Gandalf said. (There is a wall in that direction")
                {
                    Console.WriteLine("He intentado saltar un muro");
                    return "";
                  
                }
                //Same que line 41
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);

                int currentPositionX = DynamicData.game.currentPositionX;
                int currentPositionY = DynamicData.game.currentPositionY;

                bool northBlocked = DynamicData.mazeBlockView.northBlocked;
                bool southBlocked = DynamicData.mazeBlockView.southBlocked;
                bool eastBlocked = DynamicData.mazeBlockView.eastBlocked;
                bool westBlocked = DynamicData.mazeBlockView.westBlocked;
                
                Situation situation = new Situation(currentPositionX, currentPositionY, northBlocked, southBlocked, eastBlocked, westBlocked);
                return situation;
            }
            Console.WriteLine("Your call was not correct");
            return string.Empty;
        }
        public static string SerializeJSON(string callToMake, object bodyToJSON)
        {
            if (callToMake == "NewMaze")
            {
                //Same que line 41
                string newMazeBodyToJSON = JsonConvert.SerializeObject(bodyToJSON);
                return newMazeBodyToJSON;
            }
            else if (callToMake == "NewGame")
            {
                //Same que line 41
                string newGameBodyToJSON = JsonConvert.SerializeObject(bodyToJSON);
                return newGameBodyToJSON;
            }
            else if (callToMake == "MoveTo")
            {
                //Same que line 41
                string newMoveBodyToJSON = JsonConvert.SerializeObject(bodyToJSON);
                return newMoveBodyToJSON;
            }
            {
                Console.WriteLine("Your call was not correct");
                return string.Empty;
            }
        }
    }
}
