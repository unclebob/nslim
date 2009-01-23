// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using System.IO;
using System.Xml;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.application {
    public class Configuration {

        private readonly Dictionary<string, object> items = new Dictionary<string, object>();

        public void LoadXml(string configXml) {
            var document = new XmlDocument();
            document.LoadXml(configXml);
            if (document.DocumentElement == null) return;
            foreach (XmlNode typeNode in document.DocumentElement.ChildNodes) {
                foreach (XmlNode methodNode in typeNode.ChildNodes) {
                    LoadNode(typeNode.Name, methodNode);
                }
            }
        }

        public void LoadFile(string filePath) {
            var reader = new StreamReader(filePath);
            LoadXml(reader.ReadToEnd());
            reader.Close();
        }

        private void LoadNode(string typeName, XmlNode methodNode) {
            new Processor().Invoke(GetItem(typeName), methodNode.Name, NodeParameters(methodNode));
        }

        private static Tree<object> NodeParameters(XmlNode node) {
            var result = new TreeList<object>()
                .AddBranch(node.InnerText);
            foreach (XmlAttribute attribute in node.Attributes) {
                result.AddBranch(attribute.Value);
            }
            return result;
        }

        public T GetItem<T>() where T: new() {
            string typeName = typeof (T).FullName;
            if (!items.ContainsKey(typeName)) {
                items[typeName] = new T();
            }
            return (T)items[typeName];
        }

        public object GetItem(string typeName) {
            if (!items.ContainsKey(typeName)) {
                items[typeName] = new Processor().Create(typeName);
            }
            return items[typeName];
        }

        public void SetItem(string typeName, object value) { items[typeName] = value; }
    }
}