using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LikeKero.Infra
{
    public class XMLLib
    {

        public XMLLib()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public XmlDocument DummyXMLObj = new XmlDocument();
        public XmlNamespaceManager NSManager = null;

        // Get the value of a node
        public string fGetValue(XmlNode aNode, string nodeName)
        {
            try
            {
                XmlNode aValueNode = null;
                //aValueNode = aNode.SelectSingleNode(nodeName);
                fCreateFirstContext(aNode, nodeName, ref aValueNode);
                if (aValueNode != null)
                    return aValueNode.InnerText;
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Select a Single Node 


        public bool fAddDataToNode(ref XmlNode aNode, string cDataValue)
        {
            try
            {
                aNode.InnerText = cDataValue;
                if (aNode != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool fSetValue(ref XmlNode aNode, string nodeName, string retVal)
        {
            try
            {
                XmlNode aValueNode;
                aValueNode = aNode.SelectSingleNode(nodeName);
                if (aValueNode == null)
                {
                    return false;
                }
                else
                {
                    return fAddDataToNode(ref aValueNode, retVal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fSetValueWithCDATA(ref XmlDocument xDoc, ref XmlNode RootNode, string nodeName, string retVal)
        {
            try
            {
                XmlNode aValueNode;
                XmlCDataSection cdata;
                cdata = xDoc.CreateCDataSection(retVal);
                aValueNode = RootNode.SelectSingleNode(nodeName);
                aValueNode.InnerText = "";
                aValueNode.AppendChild(cdata);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool aCreateNode(string aNodeName, ref XmlNode aNode)
        {
            try
            {
                aNode = DummyXMLObj.CreateNode(XmlNodeType.Element, aNodeName, "");
                if (aNode != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fAppendNode(ref XmlNode WhereToAppend, ref XmlNode WhatToAppend)
        {
            try
            {
                WhereToAppend.AppendChild(WhatToAppend);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper() == "THE NODE TO BE INSERTED IS FROM A DIFFERENT DOCUMENT CONTEXT.")
                {
                    try
                    {
                        WhereToAppend.AppendChild(WhereToAppend.OwnerDocument.ImportNode(WhatToAppend, true));
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        public void fCreateAndAppendNode(string anodeName, ref XmlNode aNode, ref XmlNode WhereToAppend)
        {
            try
            {
                aCreateNode(anodeName, ref aNode);
                fAppendNode(ref WhereToAppend, ref aNode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fSetAttribute(ref XmlNode aNode, string attributeName, string attributeValue)
        {
            try
            {
                XmlAttribute tempNode;
                tempNode = DummyXMLObj.CreateAttribute(attributeName);
                if (tempNode != null)
                {
                    tempNode.InnerText = attributeValue;
                    aNode.Attributes.SetNamedItem(tempNode);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fCreateXMLObj(ref XmlDocument xDoc)
        {
            try
            {
                xDoc = new XmlDocument();
                if (xDoc != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fOpenFreeXMLDoc(ref XmlDocument xDoc, string XMLFileName)
        {
            if (xDoc == null)
                fCreateXMLObj(ref xDoc);

            try
            {
                xDoc.Load(XMLFileName);
                if (xDoc != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool fOpenFreeXMLDoc(ref XmlDocument xDoc, string XMLFileName, bool blnIsOnlyString)
        {
            if (xDoc == null)
                fCreateXMLObj(ref xDoc);

            try
            {
                xDoc.LoadXml(XMLFileName);
                if (xDoc != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public void fAppendNodeList(ref XmlNode lDestNode, ref XmlNode lSrcNode, string lXPath)
        {
            try
            {
                XmlNodeList lstNodes = null;
                XmlNode tempNode;
                fCreateContext(lSrcNode, lXPath, ref lstNodes);
                foreach (XmlNode loopNode in lstNodes)
                {
                    tempNode = loopNode.CloneNode(true);
                    fAppendNode(ref lDestNode, ref tempNode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fRemoveNode(ref XmlDocument xDoc, string lXpath)
        {
            try
            {
                XmlNode Node = null;
                if (fCreateFirstContext(xDoc, lXpath, ref Node))
                {
                    Node.ParentNode.RemoveChild(Node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StringBuilder fRemoveCdata(string lString)
        {
            StringBuilder oString = new StringBuilder(lString);
            oString.Replace("<![CDATA[", "");
            oString.Replace("]]>", "");
            return oString;
        }

        public void fAppendNodelist(ref XmlNode lDestNode, ref XmlNode lSrcNode, string lXPath)
        {
            try
            {
                XmlNodeList lstNodes = null;
                if (fCreateContext(lSrcNode, lXPath, ref lstNodes) == true)
                {
                    foreach (XmlNode loopNode in lstNodes)
                    {
                        lDestNode.AppendChild(loopNode);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FGetXML(XmlDocument xDoc, string lXPath)
        {
            try
            {
                XmlNode tempnode = null;
                bool retval = fCreateFirstContext(xDoc, lXPath, ref tempnode);
                if (retval == true)
                {
                    return tempnode.OuterXml;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int fGetLength(XmlNode lNode, string lXPath)
        {
            XmlNodeList lstNodes = null;
            int nLength = 0;
            if (fCreateContext(lNode, lXPath, ref lstNodes) == true)
            {
                nLength = lstNodes.Count;
            }
            return nLength;
        }

        public string fGetXMlFromDataSet(DataSet DS)
        {
            string sXML;
            sXML = DS.GetXml();
            return sXML;
        }

        public string fGetXMlSchemaFromDataSet(DataSet DS)
        {
            string sXML;
            sXML = DS.GetXmlSchema();
            return sXML;
        }

        public void fAppendNodeAndText(ref XmlNode lDataXMLObj, string lContext, string lNodeName, string lNodeValue)
        {

            XmlNode lContextNode = null;
            XmlNode lNewNode = null;

            if (fCreateFirstContext(lDataXMLObj, lContext, ref lContextNode) == true)
            {
                aCreateNode(lNodeName, ref lNewNode);
                fAddDataToNode(ref lNewNode, lNodeValue);
                fAppendNode(ref lContextNode, ref lNewNode);
            }
        }

        public void fCreateNodeFromString(string xmlString, ref XmlNode aNode)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlString);
            aNode = xDoc.DocumentElement;
        }


        // Select a Single Node 
        public bool fCreateFirstContext(XmlNode aContextNode, string context, ref XmlNode aNod)
        {
            try
            {
                if (NSManager == null)
                {
                    aNod = aContextNode.SelectSingleNode(context);
                }
                else
                {
                    aNod = aContextNode.SelectSingleNode(context, NSManager);
                }
                if (aNod != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Create a Node List
        public bool fCreateContext(XmlNode aContextNode, string context, ref XmlNodeList aNod)
        {
            try
            {
                if (NSManager == null)
                {
                    aNod = aContextNode.SelectNodes(context);
                }
                else
                {
                    aNod = aContextNode.SelectNodes(context, NSManager);
                }
                if (aNod != null)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public XmlDocument StripDocumentNamespace(XmlDocument oldDom)
        {
            // some config files have a default namespace
            // we are going to get rid of that to simplify our xpath expressions
            if (oldDom.DocumentElement.NamespaceURI.Length > 0)
            {
                oldDom.DocumentElement.SetAttribute("xmlns", "");
                // must serialize and reload the DOM
                // before this will actually take effect
                XmlDocument newDom = new XmlDocument();
                newDom.LoadXml(oldDom.OuterXml);
                return newDom;
            }
            else return oldDom;
        }


        public string fDirectGetValue(XmlNode aNode, string nodeName)
        {
            try
            {
                XmlNode aValueNode = null;
                aValueNode = aNode.SelectSingleNode(nodeName);
                //fCreateFirstContext(aNode, nodeName, ref aValueNode);
                if (aValueNode != null)
                    return aValueNode.Value;
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
