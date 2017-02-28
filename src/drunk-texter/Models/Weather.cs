using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;
using System.Threading.Tasks;

namespace drunk_texter.Models
{
    public static class Weather
    {
        // Get astronomy and weather conditions for city in state.
        // Takes 2 strings, returns WeatherObservation Object
        public static WeatherObservation GetForecast(string city, string state)
        {
            RestClient client = new RestClient("http://api.wunderground.com/api/");
            RestRequest request = new RestRequest(EnvironmentVariables.WeatherAPIKey + "/astronomy/conditions/q/" + state + "/" + city + ".json", Method.GET);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            WeatherObservation jsonResponse = JsonConvert.DeserializeObject<WeatherObservation>(response.Content);
            return jsonResponse;
        }

        // Helper method for GetForecast
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
