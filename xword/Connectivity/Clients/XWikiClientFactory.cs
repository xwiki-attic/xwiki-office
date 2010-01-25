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

namespace XWiki.Clients
{
    /// <summary>
    /// Creates xwiki client instances.
    /// </summary>
    public class XWikiClientFactory
    {
        /// <summary>
        /// Creates a new instance of a xwiki client.
        /// </summary>
        /// <param name="clientType">The protocol used by the client.</param>
        /// <param name="serverURL">The base url of the server.</param>
        /// <param name="username">The username used to authenticate.</param>
        /// <param name="password">The password used to authenticate.</param>
        /// <returns>A new IXWikiClient instance.</returns>
        public static IXWikiClient CreateXWikiClient(XWikiClientType clientType, String serverURL, String username, String password)
        {
            switch (clientType)
            {
                case XWikiClientType.HTTP_Client :
                    return new XWikiHTTPClient(serverURL, username, password);                    
                case XWikiClientType.XML_RPC :
                    return new XWikiXMLRPCClient(serverURL, username, password);          
            }
            throw new ArgumentException("The client type is not recognized", "clientType");
        }
    }
}
