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