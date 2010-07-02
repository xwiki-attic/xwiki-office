#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Net;
using CookComputing.XmlRpc;
using XWiki.XmlRpc;
using Rest = XWiki.Rest;

namespace XWiki.Clients
{
    /// <summary>
    /// Extends the behaviour of the XmlRpc client.
    /// </summary>
    public partial class XWikiXMLRPCClient : IXWikiClient
    {
        public String UpdateObject(string pageFullName, string className, int objectIndex, System.Collections.Specialized.NameValueCollection fieldsValues)
        {
            WebClient webClient = new WebClient();
            //set request headers
            byte[] authBytes = Encoding.UTF8.GetBytes((this.username + ":" + this.password).ToCharArray());
            webClient.Headers.Add("Authorization: Basic " + Convert.ToBase64String(authBytes));
            webClient.Headers.Add("Content-Type: application/xml");
            webClient.Headers.Add("Accept", "application/xml");

            String[] wikiPageName = pageFullName.Split('.');
            if (wikiPageName.Length > 2)
            {
                throw new Exception("Invalid page name");
            }
            String spaceName = wikiPageName[0];
            String pageName = wikiPageName[1];

            String path = "/wikis/xwiki/spaces/" + spaceName + "/pages/" + pageName + "/objects/" + className + "/" + objectIndex.ToString();
            String address = this.ServerURL + "/xwiki/rest" + path;

            //Set the fileld valies for the object
            Rest.Model.Object obj = new XWiki.Rest.Model.Object();
            obj.className = className;
            obj.number = objectIndex;
            obj.space = spaceName;
            obj.pageName = pageName;
            obj.pageId = pageFullName;
            obj.wiki = "xwiki";
            obj.property = new Rest.Model.Property[fieldsValues.Count];
            int i = 0;
            foreach (String key in fieldsValues)
            {
                Rest.Model.Property property = new XWiki.Rest.Model.Property();
                property.name = key;
                property.value = fieldsValues[key];
                obj.property[i] = property;
                i++;
            }

            //Serialize the object
            String body = "";

            XmlSerializer serializer = new XmlSerializer(typeof(Rest.Model.Object));
            // create a MemoryStream here, we are just working
            // exclusively in memory
            System.IO.Stream stream = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter xtWriter = new System.Xml.XmlTextWriter(stream, Encoding.UTF8);
            serializer.Serialize(xtWriter, obj);
            xtWriter.Flush();
            // go back to the beginning of the Stream to read its contents
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            // read back the contents of the stream and supply the encoding
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            body = reader.ReadToEnd();
            String response = "";
            try
            {
                response = webClient.UploadString(address, "PUT", body);
            }
            catch (Exception e)
            {
                Log.Warning("The styles for page " + pageFullName + " were not updated");
            }
            return response;
        }
    }
}