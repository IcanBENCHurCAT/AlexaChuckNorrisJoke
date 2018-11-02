using Alexa.NET.Request;
using Xunit;
using Amazon.Lambda.TestUtilities;
using System;
using Alexa.NET.Response;

namespace AlexaChuckNorrisJoke.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestBasicJokeRequest()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new SkillRequest();
            SkillResponse response = null;
            try
            {
                response = function.RandomJoke(request, context);
            }
            catch(Exception e)
            {
                Assert.Null(e);
            }

            Assert.NotNull(response.Response.OutputSpeech.ToString());
        }
    }
}
