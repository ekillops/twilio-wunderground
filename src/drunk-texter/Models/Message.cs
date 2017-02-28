using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drunk_texter.Models
{
    public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public Message()
        {

        }
        public static List<Message> GetMessages()
        {
            RestClient client = new RestClient("https://api.twilio.com/2010-04-01");
            RestRequest request = new RestRequest("Accounts/" + EnvironmentVariables.TwilioAccountSid + "/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator(EnvironmentVariables.TwilioAccountSid, EnvironmentVariables.TwilioAuthToken);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Message> messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            return messageList;
        }

        public void Send()
        {
            RestClient client = new RestClient("https://api.twilio.com/2010-04-01");
            RestRequest request = new RestRequest("Accounts/" + EnvironmentVariables.TwilioAccountSid + "/Messages.json", Method.POST);
            request.AddParameter("To", To)
                .AddParameter("From", From)
                .AddParameter("Body", Body);

            client.Authenticator = new HttpBasicAuthenticator(EnvironmentVariables.TwilioAccountSid, EnvironmentVariables.TwilioAuthToken);

            client.ExecuteAsync(request, response => {
                // Do stuff here
            });
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
