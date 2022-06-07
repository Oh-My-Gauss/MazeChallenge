using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MazeChallenge
{
    //Hacer 3 methodos de API diferentes, uno para cada caso (Get, bla bla) Get bla bla y POST blabla. Hacer también una clase llamada Helper con los serializadores, y esta llamada APICalls.
    public class APIs 
    {
        //Tengo mis dudas sobre los static methods.. luego mockearlos (ya verás lo que es) son un pisto.. Pero no estoy 100% seguro si debería serlo o no. Me gustaría hablar con rico sobre esto
        public static string APICall(string? callToMake, string uriToCall, string mazeUid, string gameUid, string httpMethod, string bodyJSON)
        {
            StringBuilder uri = new StringBuilder(uriToCall);
            //Call to make sólo puede ser eso no? Si callToMake te viene como "aslkñfjañsfj" o null o 15, qué harías?

            //crear 3 metodos en vez de uno solo, GetMazeID, GetGameID, GetMovementID
            if (callToMake is not null)
            {
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
                //try catch
                var response = httpClient.SendAsync(request).Result;
                //Lo mismo que en line 28 pero aquí sí que no sabemos qué nos devuelve response.

                //Habría que throwear excepciones en función del status code que recibamos

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Error with the API response, check the API");
                    return string.Empty;
                }
                string resultsAPI = response.Content.ReadAsStringAsync().Result;
                return resultsAPI;


            }
            return string.Empty;
        }

        public static object DeserializeJSON(string callToMake, string resultsAPI)
        {
            //Mismo que line 13
            if (callToMake == "NewMaze")
            {
                try
                {
                    dynamic dynamicData = JsonConvert.DeserializeObject<Maze>(resultsAPI);
                    string mazeUid = dynamicData.mazeUid;
                    return mazeUid;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("The result from API was unable to be deserialized to a mazeUid, check the API request mehtod", ex);
                    Console.WriteLine(ex.StackTrace);
                    throw new ArgumentException("The result from API was unable to be deserialized to a mazeUid, check the API request mehtod", ex);

                }
            }
            else if (callToMake == "NewGame")
            {
                //Same que line 41
                dynamic dynamicData = JsonConvert.DeserializeObject(resultsAPI);
                string gameUid = dynamicData.gameUid;
                return gameUid;
            }
            else if (callToMake == "LookAround" || callToMake == "MoveTo")
            {

                //Same que line 41
                //cambiar la clase Situation para que sea replicar tal cual el json que me devuelve.
                dynamic dynamicData = JsonConvert.DeserializeObject(resultsAPI);
                //Situation situation = JsonConvert.DeserializeObject<Situation>(resultsAPI);

                int currentPositionX = dynamicData.game.currentPositionX;
                int currentPositionY = dynamicData.game.currentPositionY;

                bool northBlocked = dynamicData.mazeBlockView.northBlocked;
                bool southBlocked = dynamicData.mazeBlockView.southBlocked;
                bool eastBlocked = dynamicData.mazeBlockView.eastBlocked;
                bool westBlocked = dynamicData.mazeBlockView.westBlocked;

                Situation situation = new Situation(currentPositionX, currentPositionY, northBlocked, southBlocked, eastBlocked, westBlocked);
                return situation;
            }
            Console.WriteLine("Your call was not correct");
            return string.Empty;
        }
    }
}
