using System.Xml;
using UnityEngine;

public class XmlLoader
{
    public string GetNodeValue(XmlNode nodePath)
    {
        string value = nodePath.Value;
        return value;
    }

    public string GetNodeText(XmlNode nodePath)
    {
        string text = nodePath.InnerText;
        return text;
    }

    public XmlNodeList LoadXmlData(string path)
    {
        TextAsset xmlData = (TextAsset) Resources.Load(path);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlData.text);

        return xmlDocument.ChildNodes;
    }
}
