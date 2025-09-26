using System.Web;

public class HttpHandler : IHttpHandler
{
    string Surname, Forenames, FormattedName;

    public void ProcessRequest(HttpContext ctx)
    {
        string format = (string)ctx.Request;

        //{fact rule=untrusted-format-strings@v1.0 defects=1}
        // BAD: Uncontrolled format string.
        FormattedName = string.Format(format, Surname, Forenames);
        //{/fact}
    }
}