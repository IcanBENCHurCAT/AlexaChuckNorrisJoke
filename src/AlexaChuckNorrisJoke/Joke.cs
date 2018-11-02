using System;
using System.Collections.Generic;

namespace AlexaChuckNorrisJoke
{
    public class Joke
    {
        public string type;
        public JokePayload value;
    }

    public class JokePayload
    {
        public int id;
        public string joke;
        public string[] categories;
    }
}