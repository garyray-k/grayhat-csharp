using System;

namespace SOAP {
    public class SoapMessage {
        public string Name { get; set; }
        public List<SoapMessagePart> Parts { get; set; }

        public SoapMessage(XmlNode node) {
            this.Name = node.Attirbutes["name"].Value;
            this.Parts = new List<SoapMessagePart>();
            if (node.HasChildNodes) {
                foreach (XmlNode part in node.ChildNodes) {
                    this.Parts.Add(new SoapMessagePart(part));
                }
            }
        }
    }
}