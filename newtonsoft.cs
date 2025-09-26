using Newtonsoft.Json;

namespace InsecureDeserialization
{
    public class InsecureNewtonsoftDeserialization
    {
        private object someJson;
        private object traceWriter;

        public object InvariantCulture { get; private set; }

        public void NewtonsoftDeserialization(string json)
        {
            try
            {
                JsonConvert.DeserializeObject<object>(json, new JsonSerializerSettings
                {
                    //{fact rule=untrusted-deserialization@v1.0 defects=1}
                    // ruleid: insecure-newtonsoft-deserialization
                    TypeNameHandling = TypeNameHandling.All
                });
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }//{/fact}

        public void ConverterOverrideSettings(){
            JsonConvert.DefaultSettings = () => 

                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                //ruleid: insecure-newtonsoft-deserialization
                new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.Auto};
            Bar newBar = JsonConvert.DeserializeObject<Bar>(someJson);
        }
        //{/fact}

        public void ConverterOverrideSettingsStaggeredInitialize(){
            var settings = new JsonSerializerSettings();

            //{fact rule=untrusted-deserialization@v1.0 defects=1}
            //ruleid: insecure-newtonsoft-deserialization
            settings.TypeNameHandling = TypeNameHandling.Auto;
            Bar newBar = JsonConvert.DeserializeObject<Bar>(someJson,settings);
        }
        //{/fact}

        public void ConverterOverrideSettingsMultipleSettingArgs(){
            JsonConvert.DefaultSettings = () => 
                new JsonSerializerSettings{
                    Culture = InvariantCulture,

                    //{fact rule=untrusted-deserialization@v1.0 defects=1}
                    //ruleid: insecure-newtonsoft-deserialization
                    TypeNameHandling = TypeNameHandling.Auto,
                    TraceWriter = traceWriter
                    };
            Bar newBar = JsonConvert.DeserializeObject<Bar>(someJson);
        }
        //{/fact}

      public void SafeDeserialize(){
        Bar newBar = JsonConvert.DeserializeObject<Bar>(someJson, new JsonSerializerSettings
        {

            //{fact rule=untrusted-deserialization@v1.0 defects=0}
            //ok: insecure-newtonsoft-deserialization
            TypeNameHandling = TypeNameHandling.None
        });
      }
      //{/fact}

      public void SafeDefaults(){

        //{fact rule=untrusted-deserialization@v1.0 defects=0}
        //ok: insecure-newtonsoft-deserialization
        Bar newBar = JsonConvert.DeserializeObject<Bar>(someJson);
      }
    }

    internal class Bar
    {
    }

    internal class TypeNameHandling
    {
        public static object All { get; internal set; }
        public static object Auto { get; internal set; }
        public static object None { get; internal set; }
    }

    internal class JsonSerializerSettings
    {
        public object TypeNameHandling { get; set; }
        public object Culture { get; internal set; }
        public object TraceWriter { get; internal set; }
    }

    internal class JsonConvert
    {
        public static Func<JsonSerializerSettings> DefaultSettings { get; internal set; }

        internal static void DeserializeObject<T>(string json, JsonSerializerSettings jsonSerializerSettings)
        {
            throw new NotImplementedException();
        }

        internal static T DeserializeObject<T>(object someJson, JsonSerializerSettings settings)
        {
            throw new NotImplementedException();
        }

        internal static T DeserializeObject<T>(object someJson)
        {
            throw new NotImplementedException();
        }
    }
}
        //{/fact}
