using System.Xml;

namespace SomeNamespace
{
    public class Foo{
        public void ReaderBad(string userInput)
        {
            XmlTextReader myReader = new XmlTextReader(new StringReader(userInput));
						myReader.XmlResolver = new XmlUrlResolver();
						
            //{fact rule=xml-external-entity@v1.0 defects=1}
            // ruleid: xmltextreader-unsafe-defaults
            while (myReader.Read())
            {
                if (myReader.NodeType == XmlNodeType.Element)
                {
                    //{/fact}
                    //{fact rule=xml-external-entity@v1.0 defects=1}
                    // ruleid: xmltextreader-unsafe-defaults
                    Console.WriteLine(myReader.ReadElementContentAsString());
                }
            }
            Console.ReadLine();
        }
        //{/fact}

        public static void StaticReaderBad(string userInput)
        {
            XmlTextReader myReader = new XmlTextReader(new StringReader(userInput));
						myReader.XmlResolver = new XmlUrlResolver();

            //{fact rule=xml-external-entity@v1.0 defects=1}
            // ruleid: xmltextreader-unsafe-defaults
            while (myReader.Read())
            {
                if (myReader.NodeType == XmlNodeType.Element)
                {
                    //{/fact}
                    //{fact rule=xml-external-entity@v1.0 defects=1}
                    // ruleid: xmltextreader-unsafe-defaults
                    Console.WriteLine(myReader.ReadElementContentAsString());
                }
            }
            Console.ReadLine();
        }
        //{/fact}

        public void ReaderGood(string userInput)
        {
            XmlTextReader myReader = new XmlTextReader(new StringReader(userInput));
            myReader.DtdProcessing = DtdProcessing.Prohibit;

            //{fact rule=xml-external-entity@v1.0 defects=0}
            // ok: xmltextreader-unsafe-defaults
            while (myReader.Read())
            {
                if (myReader.NodeType == XmlNodeType.Element)
                {
                    //{/fact}
                    
                    //{fact rule=xml-external-entity@v1.0 defects=0}
                    // ok: xmltextreader-unsafe-defaults
                    Console.WriteLine(myReader.ReadElementContentAsString());
                }
                //{/fact}
            }
            Console.ReadLine();
        }
    }

    internal class XmlNodeType
    {
        public static object Element { get; internal set; }
    }

    internal class XmlTextReader
    {
        public XmlTextReader(StringReader stringReader)
        {
        }

        public object NodeType { get; internal set; }
        public object DtdProcessing { get; internal set; }

        internal bool Read()
        {
            throw new NotImplementedException();
        }

        internal bool ReadElementContentAsString()
        {
            throw new NotImplementedException();
        }
    }
 
}
