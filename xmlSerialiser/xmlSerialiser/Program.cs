using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace xmlSerialiser
{
    [XmlRoot("configuration")]
    class Program
    {
        /* 
         * 0 => path
         * odds are keya
         * evens are value
         */
        static void Main(string[] args)
        {
            //args = new string[3] { "./text.xml", "fs.default.name", "hdfs://namenode:9000/" };

            if (!File.Exists(args[0]))
            {
                File.Create(args[0]);
            }

            XmlDocument doc = new XmlDocument();
            var sr = new StreamReader(args[0]);
            doc.Load(sr);
            sr.Close();
            for (int i = 1; i < args.Length; i+=2)
            {
                var dox = doc.SelectSingleNode("//configuration/property/name[. = '"+ args[i]+"']");
                if (dox != null) {
                    dox.ParentNode.ChildNodes[1].InnerText = args[i + 1];
                    continue;
                }

                XmlNode node = doc.CreateNode(XmlNodeType.Element, "property", "");
                XmlNode n = doc.CreateNode(XmlNodeType.Element, "name", "");
                XmlNode v = doc.CreateNode(XmlNodeType.Element, "value", "");
                n.InnerText = args[i];
                v.InnerText = args[i+1];
                node.AppendChild(n);
                node.AppendChild(v);
                doc.SelectSingleNode("//configuration").AppendChild(node);
            }
            var sw = new StreamWriter(args[0]);
            doc.Save(sw);
            sw.Flush();
            sw.Close();
        }
    }
}
