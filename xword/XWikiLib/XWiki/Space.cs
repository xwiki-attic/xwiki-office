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
using System.Xml.Serialization;

namespace XWiki
{
    /// <summary>
    /// Describes a space in a XWiki server.
    /// </summary>
    [Serializable]
    public class Space
    {
        /// <summary>
        /// The name of the space.
        /// </summary>
        [XmlAttribute]
        public String name;

        /// <summary>
        /// Specifies if the space should be visible to the user.
        /// </summary>
        [XmlAttribute]
        public bool hidden;

        /// <summary>
        /// Specifies if the space is published on wiki, or if it is a new local one.
        /// Default is TRUE, since the majority of spaces are from the server.
        /// FALSE only for new added spaces.
        /// </summary>
        [XmlAttribute]
        public bool published=true;

        /// <summary>
        /// The list of documents in the space.
        /// </summary>
        public List<XWikiDocument> documents;
        
        /// <summary>
        /// Default constructor. Creates a new instance of the Space class.
        /// </summary>
        public Space()
        {
            name = "";
            hidden = false;
            documents = new List<XWikiDocument>();
        }

        /// <summary>
        /// Gets the unpublished XWiki documents in the Space instance.
        /// </summary>
        /// <returns>A list of unpublished XWiki documents.</returns>
        public List<XWikiDocument> GetUnpublishedDocuments()
        {
            //The list of unpublished documents to be returned
            List<XWikiDocument> docs = new List<XWikiDocument>();
            foreach (XWikiDocument doc in documents)
            {
                if (!doc.published)
                {
                    docs.Add(doc);
                }
            }
            return docs;
        }

        /// <summary>
        /// Adds a collection of XWiki documents to the space instance.
        /// </summary>
        /// <param name="_documents">A collection containing XWiki documents.</param>
        public void AddDocuments(IEnumerable<XWikiDocument> _documents)
        {
            documents.AddRange(_documents);
        }

        /// <summary>
        /// Adds a collection of XWiki documents to the space instance.
        /// </summary>
        /// <param name="_documents">A collection containing XWiki documents names.</param>
        public void AddDocuments(IEnumerable<String> _documents)
        {
            foreach (String documentName in _documents)
            {
                XWikiDocument doc = new XWikiDocument();
                doc.name = documentName;
                doc.space = this.name;
                doc.published = true;
                documents.Add(doc);
            }
        }
    }
}
