using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Funccy
{
    public class MaybeConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        public override bool CanConvert(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Maybe<>);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = JToken.Load(reader).ToString();

            var aType = objectType.GenericTypeArguments[0];
            var a = JsonConvert.DeserializeObject(jObj, aType, settings);

            if (a is null)
            {
                var constructor = objectType.GetConstructor(new Type[0]);
                return constructor.Invoke(new object[0]);
            }
            else
            {
                var constructor = objectType.GetConstructor(new[] { aType });
                return constructor.Invoke(new[] { a });
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dync = (dynamic)value;
            var jtoken = Unwrap(dync);
            jtoken.WriteTo(writer);
        }

        private static JToken Unwrap<TA>(Maybe<TA> maybe) => maybe
            .Map(x => JToken.FromObject(x))
            .Extract(JValue.CreateNull);
    }
}
