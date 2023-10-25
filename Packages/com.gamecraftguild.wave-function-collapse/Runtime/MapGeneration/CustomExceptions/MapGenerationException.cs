using System;
using System.Runtime.Serialization;

namespace GameCraftGuild.WaveFunctionCollapse {

    [Serializable]
    public class MapGenerationException : Exception {

        public MapGenerationException() { }

        public MapGenerationException(string message) : base(message) { }

        public MapGenerationException(string message, Exception innerException) : base(message, innerException) { }

        protected MapGenerationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

}
