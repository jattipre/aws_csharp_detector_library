using System;
using System.Runtime.Serialization;

[Serializable]
public class PersonBad : ISerializable
{
    public int Age;

    public PersonBad(int age)
    {
        if (age < 0)
            throw new ArgumentException(nameof(age));
        Age = age;
    }

    [OnDeserializing]
    //{fact rule=untrusted-deserialization@v1.0 defects=1}
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Age = info.GetInt32("age");  // BAD - write is unsafe
    }
    //{/fact}
}