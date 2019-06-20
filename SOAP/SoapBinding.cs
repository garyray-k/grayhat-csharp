using System;

public class SoapBinding {
    public string Name { get; set; }
    public List<SoapBindingOperation> Operations { get; set; }
    public bool IsHTTP { get; set; }
    public string Verb { get; set; }
    public string Type { get; set; }

    public SoapBinding(XmlNode node) {
        this.Name = node.Attributes["name"].Value;
        this.Type = node.Attributes["type"].Value;
        this.IsHTTP = false;
        this.Operations = new List<SoapBindingOperation>();

        foreach (XmlNode operation in node.ChildNodes) {
            if (operation.Name.EndsWith("operation")) {
                this.Operations.Add(new SoapBindingOperation(operation));
            } else if (operation.Name == "http:binding") {
                this.Verb = operation.Attributes["verb"].Value;
                this.IsHTTP = true;
            }
        }
    }
}