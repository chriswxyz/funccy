using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Funccy
{
    public class OneOfConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        public override bool CanConvert(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(OneOf<,>);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader).ToString();

            JsonSerializationException exA = null;
            JsonSerializationException exB = null;

            try
            {
                var aType = objectType.GenericTypeArguments[0];
                var a = JsonConvert.DeserializeObject(token, aType, settings);
                var constructor = objectType.GetConstructor(new[] { aType });
                return constructor.Invoke(new[] { a });
            }
            catch (JsonSerializationException e)
            {
                exA = e;
            }

            try
            {
                var bType = objectType.GenericTypeArguments[1];
                var b = JsonConvert.DeserializeObject(token, bType, settings);
                var constructor = objectType.GetConstructor(new[] { bType });
                return constructor.Invoke(new[] { b });
            }
            catch (JsonSerializationException e)
            {
                exB = e;
            }

            throw new AggregateException(exA, exB);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dyn = (dynamic)value;
            var token = Unwrap(dyn);
            token.WriteTo(writer);
        }

        private static JToken Unwrap<TA, TB>(OneOf<TA, TB> oneOf)
        {
            // TODO: Handle primitives
            return oneOf.Extract(
                a => JToken.FromObject(a),
                b => JToken.FromObject(b)
            );
        }
    }
}
