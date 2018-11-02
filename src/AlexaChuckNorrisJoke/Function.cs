using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaChuckNorrisJoke
{
    public class Function
    {
        private static HttpClient _client = new HttpClient();
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public SkillResponse RandomJoke(SkillRequest input, ILambdaContext context)
        {
            try
            {
                var requestType = input.GetRequestType();

                if (requestType == typeof(IntentRequest))
                {
                    var intentRequest = input.Request as IntentRequest;
                    if(intentRequest.Intent.Name == "AMAZON.HelpIntent")
                    {
                        return ResponseBuilder.AskWithCard("Try asking for a joke. Would you like to hear a joke?", "Chuck Norris Jokes Help", "Would you like to hear a joke?", new Reprompt("Would you like to hear a joke?"));
                    }
                    //// check the name to determine what you should do
                    //if (intentRequest.Intent.Name.Equals("RandomJoke"))
                    //{
                    //    // get the slots
                    //    var firstValue = intentRequest.Intent.Slots["topic"].Value;
                    //}
                    var jokeResponse = FetchJokeFromAPI().GetAwaiter().GetResult();
                    string joke = jokeResponse.value.joke;
                    return ResponseBuilder.TellWithCard(joke, "Chuck Norris Joke", joke);
                }
                else if (requestType == typeof(LaunchRequest))
                {
                    var response = ResponseBuilder.Ask("What can I do for you?", new Reprompt("Would you like to hear a joke?"));
                    response.Response.ShouldEndSession = false;
                    return response;
                }
                else if (requestType == typeof(AudioPlayerRequest))
                {
                    var audioRequest = input.Request as AudioPlayerRequest;

                    // these are events sent when the audio state has changed on the device
                    // determine what exactly happened
                    if (audioRequest.AudioRequestType == AudioRequestType.PlaybackNearlyFinished)
                    {
                        // queue up another audio file
                    }
                }
            }
            catch (ServiceUnavailableException sue)
            {
                Console.Write("Joke Service Unavailable: ");
                Console.WriteLine(sue);
                return ResponseBuilder.Tell("The joke service is currently unavailable.");
            }
            catch(Exception e)
            {
                Console.Write("Exception caught: ");
                Console.WriteLine(e);
            }

            string defaultJoke = "Would you like to hear a joke?";
            return ResponseBuilder.Ask(defaultJoke, new Reprompt(defaultJoke));

        }

        public async Task<Joke> FetchJokeFromAPI()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "http://api.icndb.com/jokes/random");

            var response = await _client.SendAsync(req);
            if(!response.IsSuccessStatusCode)
            {
                throw new ServiceUnavailableException(response);
            }
            var content = await response.Content.ReadAsStringAsync();
            Joke joke = JsonConvert.DeserializeObject<Joke>(content);

            return joke;
        }
    }
}
