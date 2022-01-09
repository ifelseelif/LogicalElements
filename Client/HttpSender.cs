using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = System.Xml.Formatting;

namespace Client
{
    public class HttpSender
    {
        private const string Host = "https://localhost:5001";
        private const string ControllerPath = "logicalelements";
        private const string AuthControllerPath = "auth";
        private const string ConnectionPath = "/connection";
        private const string PrintPath = "/result";
        private const string IOPath = "/io";
        private const string Elements = "/elements";
        private string token = "";
        private HttpClient _httpClient;

        private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public HttpSender()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            _httpClient = new HttpClient(clientHandler);
            _httpClient.BaseAddress = new Uri(Host);
        }

        public async Task Send(Message message)
        {
            if (string.IsNullOrEmpty(token) && message.CommandType != CommandType.Login &&
                message.CommandType != CommandType.Register)
            {
                Console.WriteLine("Need login");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            string path = "";
            HttpMethod httpMethod = null;
            var queryString = new Dictionary<string, string>();
            var controllerPath = ControllerPath;

            switch (message.CommandType)
            {
                case CommandType.Connection:
                    path = ConnectionPath;
                    httpMethod = HttpMethod.Post;
                    queryString.Add("idOfInput", message.InputId.ToString());
                    queryString.Add("idOfOutput", message.OutputId.ToString());
                    break;
                case CommandType.Print:
                    path = PrintPath;
                    httpMethod = HttpMethod.Get;
                    break;
                case CommandType.AddElem:
                    httpMethod = HttpMethod.Post;
                    path = Elements;
                    queryString.Add("elemType", message.ElemType.ToString());
                    break;
                case CommandType.Show:
                    httpMethod = HttpMethod.Get;
                    path = "/" + message.Id;
                    break;
                case CommandType.AddIO:
                    httpMethod = HttpMethod.Post;
                    path = IOPath;
                    queryString.Add("name", message.Name);
                    queryString.Add("isInput", message.IsInput.ToString());
                    break;
                case CommandType.Set:
                    httpMethod = HttpMethod.Put;
                    path = Elements;
                    queryString.Add("name", message.Name);
                    queryString.Add("value", message.Value.ToString());
                    break;
                case CommandType.Login:
                    controllerPath = AuthControllerPath;
                    path = "/login";
                    httpMethod = HttpMethod.Post;
                    queryString.Add("login", message.Name);
                    queryString.Add("password", message.Password);
                    break;
                case CommandType.Register:
                    controllerPath = AuthControllerPath;
                    path = "/register";
                    httpMethod = HttpMethod.Post;
                    queryString.Add("login", message.Name);
                    queryString.Add("password", message.Password);
                    break;
                default:
                    Console.WriteLine("Something went wrong");
                    break;
            }

            var requestUri = QueryHelpers.AddQueryString(controllerPath + path, queryString);
            using var request = new HttpRequestMessage(httpMethod, requestUri);
            var result = _httpClient.Send(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseMessage = await result.Content.ReadAsStringAsync();
                if (CommandType.Login == message.CommandType)
                {
                    token = JsonConvert.DeserializeObject<TokenResponse>(responseMessage, _jsonSettings).Token;
                    return;
                }

                Console.WriteLine(responseMessage);
            }
        }
    }
}