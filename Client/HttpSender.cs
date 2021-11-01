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
        private const string ConnectionPath = "/connection";
        private const string PrintPath = "/result";
        private const string IOPath = "/io";
        private const string Elements = "/elements";

        public async Task Send(Message message)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            using var httpClient = new HttpClient(clientHandler);

            httpClient.BaseAddress = new Uri(Host);
            string path = "";
            HttpMethod httpMethod = null;
            var queryString = new Dictionary<string, string>();

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
                default:
                    Console.WriteLine("Something went wrong");
                    break;
            }


            var requestUri = QueryHelpers.AddQueryString(ControllerPath + path, queryString);
            var request = new HttpRequestMessage(httpMethod, requestUri);
            var result = httpClient.Send(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseMessage = await result.Content.ReadAsStringAsync();
                Console.WriteLine(responseMessage);
            }
        }
    }
}