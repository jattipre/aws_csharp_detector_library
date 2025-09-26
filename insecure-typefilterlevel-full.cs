using System.Collections;
using System.Runtime.Serialization.Formatters;

namespace InsecureDeserialization
{
    public class InsecureTypeFilterLevel
    {
        private object formatterProps;

        public void SetTFL(string json)
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider(formatterProps, null);

            //{fact rule=untrusted-deserialization@v1.0 defects=1}
            // ruleid: insecure-typefilterlevel-full
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            //{/fact}

            //{fact rule=untrusted-deserialization@v1.0 defects=1}
            // ruleid: insecure-typefilterlevel-full
            var provider = new BinaryServerFormatterSinkProvider(formatterProps, null);
			//{ "TypeFilterLevel" = TypeFilterLevel.Full };

            var dict = new Hashtable();
            dict["typeFilterLevel"] = "Full";
            //{/fact}

            //{fact rule=untrusted-deserialization@v1.0 defects=1}
            // ruleid: insecure-typefilterlevel-full
            BinaryServerFormatterSinkProvider serverProvider2 = new BinaryServerFormatterSinkProvider(dict, null);
        }
    }

    internal class BinaryServerFormatterSinkProvider
    {
        private object formatterProps;
        private object value;

        public BinaryServerFormatterSinkProvider(object formatterProps, object value)
        {
            this.formatterProps = formatterProps;
            this.value = value;
        }

        public TypeFilterLevel TypeFilterLevel { get; internal set; }
    }
}
            //{/fact}
