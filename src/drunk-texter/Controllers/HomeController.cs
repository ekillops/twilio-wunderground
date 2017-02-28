using drunk_texter.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace drunk_texter.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetMessages()
        {
            var allMessages = Message.GetMessages();
            return View(allMessages);
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Message newMessage)
        {
            newMessage.Send();
            return RedirectToAction("Index");
        }
        public IActionResult GetForecast()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetForecast(string city, string state)
        {
            var weatherObs = Weather.GetForecast(city, state);
            Debug.WriteLine(weatherObs);
            var location = weatherObs.current_observation.display_location.full;
            var weather = weatherObs.current_observation.weather;
            //string weather = (string)weatherJson.SelectToken("current_observation.weather");
            //string location = (string)weatherJson.SelectToken("current_observation.display_location.full");
            string body = "The forecast in " + location + " is " + weather + ".";
            Message newMessage = new Message() { Body = body, To = "+15037346176", From = "+19718036206" };
            newMessage.Send();
            return View("ForecastResults", weatherObs);
        }
    }
}
