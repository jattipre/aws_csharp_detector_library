class UnsafeDeserializationBad
{
    public static object Deserialize(string s)
    {
        JavaScriptSerializer sr = new(new SimpleTypeResolver());
        //{fact rule=untrusted-deserialization@v1.0 defects=1}
        // BAD
        return sr.DeserializeObject(s);
        //{/fact}
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

    public JavaScriptSerializer()
    {
    }

    public JavaScriptSerializer(SimpleTypeResolver simpleTypeResolver)
    {
        this.simpleTypeResolver = simpleTypeResolver;
    }

    internal object DeserializeObject(string s)
    {
        throw new NotImplementedException();
    }
}