using System.Xml;
using System.IO;
using System;
class xmlreadersettings_unsafe_parser_override{


public void ParseBad(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    rs.DtdProcessing = DtdProcessing.Parse;

    //{fact rule=xml-external-entity@v1.0 defects=1}
    // ruleid:xmlreadersettings-unsafe-parser-override
    XmlReader myReader = XmlReader.Create(new StringReader(input),rs);
            
    while (myReader.Read())
    {
        Console.WriteLine(myReader.Value);
    }
    Console.ReadLine();
}
//{/fact}

public static void StaticParseBad(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    rs.DtdProcessing = DtdProcessing.Parse;


    //{fact rule=xml-external-entity@v1.0 defects=1}
    // ruleid:xmlreadersettings-unsafe-parser-override
    XmlReader myReader = XmlReader.Create(new StringReader(input),rs);
            
    while (myReader.Read())
    {
        Console.WriteLine(myReader.Value);
    }
    Console.ReadLine();
}
//{/fact}

public void ParseBad2(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    rs.DtdProcessing = DtdProcessing.Parse;

    //{fact rule=xml-external-entity@v1.0 defects=1}
    // ruleid:xmlreadersettings-unsafe-parser-override
    XmlReader myReader = XmlReader.Create(input,rs);
            
    while (myReader.Read())
    {
        Console.WriteLine(myReader.Value);
    }
    Console.ReadLine();
}
//{/fact}

public void ParseBad3(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    rs.DtdProcessing = DtdProcessing.Parse;

    using(var reader = new StringReader(input)){

        //{fact rule=xml-external-entity@v1.0 defects=1}
        // ruleid:xmlreadersettings-unsafe-parser-override
        XmlReader myReader = XmlReader.Create(reader,rs);
                
        while (myReader.Read())
        {
            Console.WriteLine(myReader.Value);
        }
        Console.ReadLine();
    }
}
//{/fact}

public void ParseGood(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    rs.DtdProcessing = DtdProcessing.Ignore;

    //{fact rule=xml-external-entity@v1.0 defects=0}
    // ok: xmlreadersettings-unsafe-parser-override
    XmlReader myReader = XmlReader.Create(new StringReader(input),rs);
            
    while (myReader.Read())
    {
        Console.WriteLine(myReader.Value);
    }
    Console.ReadLine();
}
//{/fact}

public void ParseGood2(string input){
    XmlReaderSettings rs = new XmlReaderSettings();
    // pre-override, not broken{/fact}
    // pre-override, not broken
    //{fact rule=xml-external-entity@v1.0 defects=0}
    // ok: xmlreadersettings-unsafe-parser-override
    using(var reader = new StringReader(input))//argument error
    {
        XmlReader myReader = XmlReader.Create(reader);
                
        while (myReader.Read())
        {
            Console.WriteLine(myReader.Value);
        }
        Console.ReadLine();
    }
    rs.DtdProcessing = DtdProcessing.Parse;
    //{/fact}

    // post-override, not providing reader settings, not broken{/fact}
    // post-override, not providing reader settings, not broken
    //{fact rule=xml-external-entity@v1.0 defects=0}
    // ok: xmlreadersettings-unsafe-parser-override
    using(var reader = new StringReader(input)){
        XmlReader myReader = XmlReader.Create(reader);
                
        while (myReader.Read())
        {
            Console.WriteLine(myReader.Value);
        }
        Console.ReadLine();
    }
    //{/fact}
}

public void ParseGood3(string input){
    XmlReaderSettings rs = new XmlReaderSettings();

    //{fact rule=xml-external-entity@v1.0 defects=0}
    // ok: xmlreadersettings-unsafe-parser-override
    var something = input;
    rs.DtdProcessing = DtdProcessing.Parse;

    var notInput = someSafeLoad();
    //{/fact}

    //{fact rule=xml-external-entity@v1.0 defects=0}
    // ok: xmlreadersettings-unsafe-parser-override
    XmlReader myReader = XmlReader.Create(new StringReader(notInput),rs);
            
    while (myReader.Read())
    {
        Console.WriteLine(myReader.Value);
    }
    Console.ReadLine();
}

string someSafeLoad()
{
    throw new NotImplementedException();
}

//{/fact}
}