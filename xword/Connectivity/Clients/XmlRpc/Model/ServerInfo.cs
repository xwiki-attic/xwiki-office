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
using CookComputing.XmlRpc;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains information about the server.
    /// </summary>
    public struct ServerInfo
    {
        /// <summary>
        /// The base url for the XWiki server.
        /// </summary>
        public string baseUrl;

        /// <summary>
        /// The patch level.
        /// </summary>
        public string patchLevel;

        /// <summary>
        /// The maven version + svn revision number.
        /// </summary>
        public string buildId;

        /// <summary>
        /// The major version of the server.
        /// </summary>
        public string majorVersion;

        /// <summary>
        /// The minor version of the server.
        /// </summary>
        public string minorVersion;

        /// <summary>
        /// The default syntax of the wiki(since XWiki 2.1)
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string DefaultSyntax;

        /// <summary>
        /// Gets the configured syntaxes of the wiki(since XWiki 2.1)
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ConfiguredSyntaxes;
    }
}