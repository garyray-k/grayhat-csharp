using System;

namespace SOAP {
    public class SoapType {
        public string Name { get; set; }
        public List<SoapTypeParameter> Parameters { get; set; }
        
        public SoapType(XmlNode type) {
            this.Name = type.Attributes["name"].Value;
            this.Parameters = new List<SoapTypeParameter>();
            if (type.HasChildNodes && type.FirstChild.HasChildNodes) {
                foreach (XmlNode node in type.FirstChild.ChildNodes) {
                    this.Parameters.Add(new SoapTypeParameter(node));
                }
            }
        }
    }
}