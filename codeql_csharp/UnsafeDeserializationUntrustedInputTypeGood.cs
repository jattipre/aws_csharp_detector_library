using System.Runtime.Serialization.Json;
using System.IO;
using System;

class Poco
{
    public int Count;

    public string Comment;
}

class GoodDataContractJsonSerializer
{
    public static Poco Deserialize(Stream s)
    {
        //{fact rule=object-input-stream-insecure-deserialization@v1.0 defects=0}
        // GOOD: while stream is potentially untrusted, the instantiated type is hardcoded
        var ds = new DataContractJsonSerializer(typeof(Poco));
        return (Poco)ds.ReadObject(s);
        //{/fact}
    }
}