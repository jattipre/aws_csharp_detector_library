using System.Text;
using System.Web;


public class PrivateInformationHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext ctx)
    {
        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        string address = (string)ctx.Request;
        logger.Info("User has address: " + address);
        //{/fact}
    }
}

internal class logger
{
    internal static void Info(string v)
    {
        throw new NotImplementedException();
    }
}