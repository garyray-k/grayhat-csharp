using System;

namespace SOAP {
    class WSDL
    {
        public List<SoapType> Types { get; set; }
        public List<SoapMessage> Messages { get; set; }
        public List<SoapPortType> PortTypes { get; set; }
        public List<SoapBinding> Bindings { get; set; }
        public List<SoapService> Services { get; set; }
        
        public WSDL(XMLDocument doc)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("wsdl", doc.DocumentElement.NamespaceURI);
            nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            ParseTypes(doc, nsManager);

            ParseMessages(doc, nsManager);
            ParsePortTypes(doc, nsManager);
            ParseBindings(doc, nsManager);
            ParseServices(doc, nsManager);  
        }

        private void ParseTypes(XmlDocument wsdl, XmlNamespaceManager nsManager) {
            this.Types = new List<SoapType>();
            string xpath = "/wsdl:definitions/wsdl:types/xs:schema/xs:element";
            XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
            foreach (XmlNode type in nodes) {
                this.Types.Add(new SoapType(type));
            }
        }
    }
}