using System;
using System.Web;
using System.Reflection;

public class AssemblyPathInjectionHandler : HttpHandler {
  public void ProcessRequest(HttpContext ctx) {
    string assemblyPath = (string)ctx.Request;

    //{fact rule=assembly-path-injection@v1.0 defects=1}
    // BAD: Load assembly based on user input
    var badAssembly = Assembly.LoadFile(assemblyPath);

    // Method called on loaded assembly. If the user can control the loaded assembly, then this
    // could result in a remote code execution vulnerability
    MethodInfo m = badAssembly.GetType("Config").GetMethod("GetCustomPath");
    Object customPath = m.Invoke(null, null);
    // ...
  }
    //{/fact}
}

public class HttpContext
{
    public static object? Current { get; internal set; }
    public object? Request { get; internal set; }
    public object? Response { get; internal set; }
    public object? Session { get; internal set; }
}
