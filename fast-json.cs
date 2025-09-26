namespace InsecureDeserialization
{
    public class InsecureFastJSONDeserialization
    {
        public void FastJSONDeserialization(string json)
        {
            try
            {
                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                // ruleid: insecure-fastjson-deserialization
                var obj = JSON.ToObject(json, new JSONParameters { BadListTypeChecking = false });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class JSON
    {
        internal static object ToObject(string json, JSONParameters jSONParameters)
        {
            throw new NotImplementedException();
        }
    }

    internal class JSONParameters
    {
        public bool BadListTypeChecking { get; set; }
    }
}
                //{/fact}
