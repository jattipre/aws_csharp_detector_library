using System.Runtime.Serialization;
using System.Text;

namespace InsecureDeserialization
{
    public class InsecureNetDataContractDeserialization
    {
        public void NetDataContractDeserialization(string json)
        {
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

                //{fact rule=untrusted-deserialization@v1.0 defects=1}
                // ruleid: insecure-netdatacontract-deserialization
                NetDataContractSerializer netDataContractSerializer = new NetDataContractSerializer();
                object obj = netDataContractSerializer.Deserialize(ms);
                Console.WriteLine(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class NetDataContractSerializer
    {
        internal object Deserialize(MemoryStream ms)
        {
            throw new NotImplementedException();
        }
    }
}
                //{/fact}
