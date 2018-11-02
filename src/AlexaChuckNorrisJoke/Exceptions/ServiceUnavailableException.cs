using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace AlexaChuckNorrisJoke
{
    [Serializable]
    internal class ServiceUnavailableException : Exception
    {
        public HttpResponseMessage httpMessage;
        public ServiceUnavailableException(HttpResponseMessage message)
        {
            httpMessage = message;
        }

        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}