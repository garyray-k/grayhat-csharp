using System;

public class SoapPortType {
    public string Name { get; set; }
    public List<SoapOperations> Operations { get; set; }
    public SoapPortType(XmlNode node) {
        this.Name = node.Attributes["name"].Value;
        this.Operations = new List<SoapOperations>();
        foreach (XmlNode op in node.ChildNodes) {
            this.Operations.Add(new SoapOperation(op));
        }
    }
}