using System.Text;

namespace InsecureDeserialization
{
    public class InsecureSoapFormatterDeserialization
    {
        public void SoapFormatterDeserialization(string json)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                // ruleid: insecure-soapformatter-deserialization
                SoapFormatter soapFormatter = new SoapFormatter();
                object obj = soapFormatter.Deserialize(ms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class SoapFormatter
    {
        internal object Deserialize(MemoryStream ms)
        {
            throw new NotImplementedException();
        }
    }
}
                //{/fact}
