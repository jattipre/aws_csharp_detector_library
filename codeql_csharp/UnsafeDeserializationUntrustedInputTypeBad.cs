using System.Runtime.Serialization.Json;
using System.IO;
using System;

class BadDataContractJsonSerializer
{
    public static object Deserialize(string type, Stream s)
    {
        //{fact rule=object-input-stream-insecure-deserialization@v1.0 defects=1}
        // BAD: stream and type are potentially untrusted
        var ds = new DataContractJsonSerializer(Type.GetType(type));
        return ds.ReadObject(s);
        //{/fact}
    }
}