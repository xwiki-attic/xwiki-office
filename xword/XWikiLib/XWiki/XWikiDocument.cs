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
    /// Describes a document on the XWiki server.
    /// </summary>
    [Serializable]
    public class XWikiDocument
    {
        /// <summary>
        /// The name of the document.
        /// </summary>
        [XmlAttribute]
        public String name;
        
        /// <summary>
        /// The space name of the document.
        /// </summary>
        [XmlAttribute]
        public String space;

        /// <summary>
        /// Specifies if the document is published on wiki or if it's a local one.
        /// Default is TRUE, since the majority of pages are from the server.
        /// FALSE only for new added (and not published) pages.
        /// </summary>
        public bool published=true;

        /// <summary>
        /// The rendered content of the document.
        /// </summary>
        [NonSerialized]
        protected StringBuilder content;

        /// <summary>
        /// Default constructor, creates a new instance of the XWikiDocument class.
        /// </summary>
        public XWikiDocument()
        {
            name = "";
            space = "";
            content = new StringBuilder();
        }
    }
}
