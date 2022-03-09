using System;
using Newtonsoft.Json;

namespace AspNetCoreAngularApp.Tests.IntegrationTests.Helpers
{
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, 
                                         decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, 
                                       decimal value, JsonSerializer serializer)
        {
            // Customise how you want the decimal value to be output in here
            // for example, you may want to consider culture
            writer.WriteRawValue(value.ToString());
        }
    }
}