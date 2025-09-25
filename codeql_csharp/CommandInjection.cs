using System;
using System.Web;
using System.Diagnostics;

public class CommandInjectionHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext ctx)
    {
        //{fact rule=os-command-injection@v1.0 defects=1}
        string param = (string)ctx.Request;
        Process.Start("process.exe", "/c " + param);
         //{/fact}
    }
}
