using System.Web.Script.Serialization;

namespace InsecureDeserialization
{
    public class InsecureJavascriptSerializerDeserialization
    {
        public void JavascriptSerializerDeserialization(string json)
        {
            try
            {
                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                // ruleid: insecure-javascriptserializer-deserialization
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                serializer.DeserializeObject(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class SimpleTypeResolver
    {
        public SimpleTypeResolver()
        {
        }
    }

    internal class JavaScriptSerializer
    {
        private SimpleTypeResolver simpleTypeResolver;

        public JavaScriptSerializer(SimpleTypeResolver simpleTypeResolver)
        {
            this.simpleTypeResolver = simpleTypeResolver;
        }

        internal void DeserializeObject(string json)
        {
            throw new NotImplementedException();
        }
    }
}
                //{/fact}
