using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

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
        private bool isLogin;
        private HttpClient _httpClient;

        public HttpSender()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            _httpClient = new HttpClient(clientHandler);
            _httpClient.BaseAddress = new Uri(Host);
        }

        public async Task Send(Message message)
        {
            if (!isLogin && message.CommandType != CommandType.Login)
            {
                Console.WriteLine("Need login");
                return;
            }

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
                    httpMethod = HttpMethod.Post;
                    queryString.Add("login", message.Name);
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
                isLogin = true;
                var responseMessage = await result.Content.ReadAsStringAsync();
                Console.WriteLine(responseMessage);
            }
        }
    }
}