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
