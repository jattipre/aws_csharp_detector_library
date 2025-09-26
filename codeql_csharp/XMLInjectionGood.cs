using System;
using System.Security;
using System.Web;
using System.Xml;

public class XMLInjectionGood: IHttpHandler {
  public void ProcessRequest(HttpContext ctx) {
    string employeeName = (string)ctx.Request;

    using (XmlWriter writer = XmlWriter.Create("employees.xml"))
    {
        writer.WriteStartDocument();

        //{fact rule=xml-injection@v1.0 defects=0}
        // GOOD: Escape user input before inserting into string
        writer.WriteRaw("<employee><name>" + SecurityElement.Escape(employeeName) + "</name></employee>");
        //{/fact}

        //{fact rule=xml-injection@v1.0 defects=0}
        // GOOD: Use standard API, which automatically encodes values
        writer.WriteStartElement("Employee");
        writer.WriteElementString("Name", employeeName);
        writer.WriteEndElement();

        writer.WriteEndElement();
        writer.WriteEndDocument();
        //{/fact}
    }
  }
}