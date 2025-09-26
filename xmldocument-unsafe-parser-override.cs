using System.Xml;

public class Foonew{
    public void LoadBad(string input)
    {
        // string fileName = @"C:\Users\user\Documents\test.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.XmlResolver = new XmlUrlResolver();
        //{fact rule=xml-external-entity@v1.0 defects=1}
        // ruleid: xmldocument-unsafe-parser-override
        xmlDoc.Load(input);
        Console.WriteLine(xmlDoc.InnerText);

        Console.ReadLine();
    }
    //{/fact}

    public static void StaticLoadBad(string input)
    {
        // string fileName = @"C:\Users\user\Documents\test.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.XmlResolver = new XmlUrlResolver();

        //{fact rule=xml-external-entity@v1.0 defects=1}
        // ruleid: xmldocument-unsafe-parser-override
        xmlDoc.Load(input);
        Console.WriteLine(xmlDoc.InnerText);

        Console.ReadLine();
    }
    //{/fact}
    
    public void LoadGood(string input)
    {
        XmlDocument xmlDoc = new XmlDocument();

        //{fact rule=xml-external-entity@v1.0 defects=0}
        // ok: xmldocument-unsafe-parser-override
        xmlDoc.Load(input);
        Console.WriteLine(xmlDoc.InnerText);

        Console.ReadLine();
    }
}        //{/fact}
