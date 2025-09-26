class Good
{
    public static object Deserialize(string s)
    {
        //{fact rule=untrusted-deserialization@v1.0 defects=0}
        // GOOD
        JavaScriptSerializer sr = new();
        return sr.DeserializeObject(s);
        //{/fact}
    }
}