using Newtonsoft.Json;
using System.IO;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Helpers
{
    public static class JsonHelper
    {
        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                return default(T);
            }

            using (var _stream = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(_stream))
                {
                    var js = new JsonSerializer();
                    var result = js.Deserialize<T>(jsonReader);
                    return result;
                }
            }
        }
    }
}