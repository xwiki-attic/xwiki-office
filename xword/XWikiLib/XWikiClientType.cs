using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki
{
    /// <summary>
    /// Specifies the available client types for connecting to a XWiki server.
    /// </summary>
    public enum XWikiClientType
    {
        /// <summary>
        /// The client calls velocity and groovy services via plain HTTP.
        /// </summary>
        HTTP_Client,

        /// <summary>
        /// The client communicates with the XWiki server using the XML-RPC API.
        /// </summary>
        XML_RPC,
    }
}
