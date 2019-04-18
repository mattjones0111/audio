namespace Adapter.Azure
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public static class DocumentSerialization
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                JsonSerializerSettings result = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                result.Converters.Add(new StringEnumConverter());

                return result;
            }
        }
    }
}
