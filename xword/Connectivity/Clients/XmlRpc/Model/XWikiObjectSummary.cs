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

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains summary information about a XWiki object.
    /// </summary>
    public class XWikiObjectSummary
    {
        /// <summary>
        /// The id of the page contining the object.
        /// </summary>
        public string pageId;

        /// <summary>
        /// The id of the object.
        /// </summary>
        public int id;

        /// <summary>
        /// The version of the page.
        /// </summary>
        public int pageVersion;

        /// <summary>
        /// The name of the class.
        /// </summary>
        public string className;

        /// <summary>
        /// A guid identifing the XWiki object.
        /// </summary>
        public string guid;

        /// <summary>
        /// The minor version of the page.
        /// </summary>
        public int pageMinorVersion;

        /// <summary>
        /// The pretty name of the object, containing the indexed access name. Eg: XWiki.JavaScriptExtension[0]
        /// </summary>
        public string prettyName;
    }
}
