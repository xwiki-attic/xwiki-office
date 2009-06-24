using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace ContentFiltering.Test.Util
{
    public class XmlDocComparator
    {
        /// <summary>
        /// Compares two <code>XmlDocument</code>s.
        /// Returns TRUE if the xml dcouments have the same nodes, in the same position with the exact attributes.
        /// </summary>
        /// <returns>True if the xml dcouments have the same nodes, in the same position with the exact attributes.</returns>
        public static bool AreIdentical(XmlDocument xmlDoc1,XmlDocument xmlDoc2)
        {
            XmlNodeList nodeList1 = xmlDoc1.ChildNodes;
            XmlNodeList nodeList2 = xmlDoc2.ChildNodes;
            bool same = true;
            
            if (nodeList1.Count != nodeList2.Count)
            {
                return false;
            }

            IEnumerator enumerator1 = nodeList1.GetEnumerator();
            IEnumerator enumerator2 = nodeList2.GetEnumerator();
            while (enumerator1.MoveNext() && enumerator2.MoveNext() && same)
            {
                same = CompareNodes((XmlNode)enumerator1.Current, (XmlNode)enumerator2.Current);
            }
            return same;
        }

        private static bool CompareNodes(XmlNode node1, XmlNode node2)
        {
            //compare properties
            if (node1.Attributes == null||node2.Attributes==null)
            {
                bool nullAttributes = (node1.Attributes == null && node2.Attributes == null);
                if (!nullAttributes)
                {
                    return false;
                }
            }
            else
            {
                if (node1.Attributes.Count != node2.Attributes.Count)
                {
                    Console.WriteLine("Attributes count: " + node1.Attributes.Count + "!=" + node2.Attributes.Count);
                    Console.WriteLine(node1.ParentNode.InnerXml);
                    Console.WriteLine(node2.ParentNode.InnerXml);
                    return false;
                }
            }
            if (node1.ChildNodes.Count != node2.ChildNodes.Count)
            {
                Console.WriteLine("Child nodes count: " + node1.ChildNodes.Count + " !=" + node2.ChildNodes.Count);
                return false;
            }

            if (node1.Name != node2.Name)
            {
                Console.WriteLine("Nodes Name: " + node1.Name + "!=" + node2.Name);
                return false;
            }

            if (node1.NodeType != node2.NodeType)
            {
                Console.WriteLine("Nodes Type: " + node1.NodeType + "!=" + node2.NodeType);
                return false;
            }

            if ((""+node1.Value).Trim() != (""+node2.Value).Trim())
            {
                Console.WriteLine("Nodes value: " + node1.Value + "!=" + node2.Value);
                return false;
            }


            //compare attributes
            XmlAttribute attribute;
            if (node1.Attributes != null && node2.Attributes != null)
            {
                foreach (XmlAttribute attr in node1.Attributes)
                {
                    attribute = node2.Attributes[attr.Name];
                    if (attribute == null)
                    {
                        Console.WriteLine("Null attribute: " + attr.Name);
                        return false;
                    }
                    if (attribute.Value != attr.Value)
                    {
                        Console.WriteLine("Attribute values: " + attribute.Value + "!=" + attr.Value);
                        return false;
                    }
                }
            }
            //compare child nodes
            IEnumerator enumerator1 = node1.GetEnumerator();
            IEnumerator enumerator2 = node2.GetEnumerator();
            bool childrenOK = true;
            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                childrenOK = CompareNodes((XmlNode)enumerator1.Current, (XmlNode)enumerator2.Current);
                if (!childrenOK)
                {
                    return false;
                }
            }


            //same properties, same attributes, same child nodes
            return true;
        }
    }
}
