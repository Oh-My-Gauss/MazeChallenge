using Newtonsoft.Json;

namespace MazeChallenge
{
    public class MazeResult
    {
        [JsonProperty("mazeUid")]
        public string MazeUid { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]        
        public int Height { get; set; }

        public MazeResult(string mazeUid, int width, int height)
        {
            MazeUid = mazeUid;
            Width = width;
            Height = height;
        }
    }
}