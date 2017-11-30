using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ApiToken
    {
        [JsonProperty("login")]
        public static Login Login { get; set; }
    }

    public class Login
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
