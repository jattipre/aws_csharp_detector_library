namespace InsecureDeserialization
{
    public class InsecureLosFormatterDeserialization
    {
        public void LosFormatterDeserialization(string json)
        {
            try
            {
                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                // ruleid: insecure-losformatter-deserialization
                LosFormatter losFormatter = new LosFormatter();
                object obj = losFormatter.Deserialize(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class LosFormatter
    {
        internal object Deserialize(string json)
        {
            throw new NotImplementedException();
        }
    }
}
                //{/fact}
