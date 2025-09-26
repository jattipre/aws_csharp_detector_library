using System;
using System.Security;
using System.Web;
using System.Xml;

public class XMLInjectionHandler : IHttpHandler {
  public void ProcessRequest(HttpContext ctx) {
    string employeeName = (string)ctx.Request;

    using (XmlWriter writer = XmlWriter.Create("employees.xml"))
    {
        writer.WriteStartDocument();
        //{fact rule=xml-injection@v1.0 defects=1}
        // BAD: Insert user input directly into XML
        writer.WriteRaw("<employee><name>" + employeeName + "</name></employee>");

        writer.WriteEndElement();
        writer.WriteEndDocument();

        //{/fact}
    }
  }
}