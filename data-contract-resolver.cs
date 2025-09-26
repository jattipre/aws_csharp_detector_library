using System.Runtime.Serialization;
using System.Xml;

namespace DCR
{
    //{fact rule=untrusted-deserialization@v1.0 defects=1}
    // ruleid: data-contract-resolver
    class MyDCR : DataContractResolver
    {
        public void ResolveDataContract()
        {
            
        }

        public override Type? ResolveName(string typeName, string? typeNamespace, Type? declaredType, DataContractResolver knownTypeResolver)
        {
            throw new NotImplementedException();
        }

        public override bool TryResolveType(Type type, Type? declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString? typeName, out XmlDictionaryString? typeNamespace)
        {
            throw new NotImplementedException();
        }
    }
}
    //{/fact}
