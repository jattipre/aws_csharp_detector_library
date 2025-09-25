using System;
using System.Web;
using System.Reflection;

public class AssemblyPathInjectionHandler1 : IHttpHandler {
  public void ProcessRequest(HttpContext ctx) {
    string configType = (string)ctx.Request;

    if (configType.Equals("configType1") || configType.Equals("configType2")) {
      //{fact rule=assembly-path-injection@v1.0 defects=0}
      // GOOD: Loaded assembly is one of the two known safe options
      var safeAssembly = Assembly.LoadFile(@"C:\SafeLibraries\" + configType + ".dll");

      // Code execution is limited to one of two known and vetted assemblies
      MethodInfo m = safeAssembly.GetType("Config").GetMethod("GetCustomPath");
      Object customPath = m.Invoke(null, null);
      // ...
    }
    //{/fact}
  }
}

public interface IHttpHandler
{
}
