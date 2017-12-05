using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ApiKey
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        public ApiKey(string token)
        {
            Token = token;
        }

        public ApiKey() { }
    }
}
