using System;

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
    }
}