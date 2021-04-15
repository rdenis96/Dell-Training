using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace RabbitMQPlayground.Domain.Converters
{
    public class ObjectIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                value = ObjectId.Empty;
            }

            if (value.GetType().IsArray)
            {
                writer.WriteStartArray();
                foreach (var item in (Array)value)
                {
                    serializer.Serialize(writer, item);
                }
                writer.WriteEndArray();
            }
            else
                serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.ToObject<string>();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ObjectId));
        }
    }
}