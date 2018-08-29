using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SimpleGame.Helpers
{
    public class ShipConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Ship));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["ShipLength"].Value<byte>() == 2)
                return jo.ToObject<TinyShip>(serializer);
            if (jo["ShipLength"].Value<byte>() == 3)
                return jo.ToObject<SmallShip>(serializer);
            if (jo["ShipLength"].Value<byte>() == 4)
                return jo.ToObject<MediumShip>(serializer);
            if (jo["ShipLength"].Value<byte>() == 6)
                return jo.ToObject<LargeShip>(serializer);

            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
