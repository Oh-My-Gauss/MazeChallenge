using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MazeChallenge
{
    public class APIs
    {
        public static string APICall(string callToMake, string uriToCall, string mazeUid, string gameUid, string httpMethod, string bodyJSON)
        {
            StringBuilder uri = new StringBuilder(uriToCall);

            if (callToMake == "LookAround" || callToMake == "MoveTo")
            {
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
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = httpClient.SendAsync(request).Result;
            string resultsAPI = response.Content.ReadAsStringAsync().Result;
            return resultsAPI;
        }
        public static object DeserializeJSON(string callToMake, string resultsAPI)
        {
            if (callToMake == "NewMaze")
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);
                string mazeUid = DynamicData.mazeUid;
                return mazeUid;
            }
            else if (callToMake == "NewGame")
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);
                string gameUid = DynamicData.gameUid;
                return gameUid;
            }
            else if (callToMake == "LookAround" || callToMake == "MoveTo")
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(resultsAPI);

                int currentPositionX = DynamicData.game.currentPositionX;
                int currentPositionY = DynamicData.game.currentPositionY;

                bool northBlocked = DynamicData.mazeBlockView.northBlocked;
                bool southBlocked = DynamicData.mazeBlockView.southBlocked;
                bool westBlocked = DynamicData.mazeBlockView.westBlocked;
                bool eastBlocked = DynamicData.mazeBlockView.eastBlocked;

                Situation situation = new Situation(currentPositionX, currentPositionY, northBlocked, southBlocked, westBlocked, eastBlocked);
                return situation;
            }
            Console.WriteLine("Your call was not correct");
            return string.Empty;
        }
        public static string SerializeJSON(string callToMake, object bodyToJSON)
        {
            if (callToMake == "NewMaze")
            {
                string newMazeBodyToJSON = JsonConvert.SerializeObject(bodyToJSON);
                return newMazeBodyToJSON;
            }
            else if (callToMake == "NewGame")
            {
                string newGameBodyToJSON = JsonConvert.SerializeObject(bodyToJSON);
                return newGameBodyToJSON;
            }
            else if (callToMake == "MoveTo")
            {
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
