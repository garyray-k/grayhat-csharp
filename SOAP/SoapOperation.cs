using System;

public class SoapOperation {
    public string Name { get; set; }
    public string Input { get; set; }
    public string Output { get; set; }

    public SoapOperation(XmlNode operation) {
        this.Name = operation.Attributes["name"].Value;
        foreach (XmlNode message in operation.ChildNodes) {
            if (message.Name.EndsWith("input")) {
                this.Input = message.Attributes["message"].Value;
            } else if (message.Name.EndsWith("output")) {
                this.Output = message.Attributes["message"].Value;
            }
        }
    }
}