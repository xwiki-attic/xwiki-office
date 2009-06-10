using System;
using CookComputing.XmlRpc;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains summary data regarding a space.
    /// </summary>
    public class Space : SpaceSummary
    {
        /// <summary>
        /// The description of the space.
        /// </summary>
        public String description;        

        /// <summary>
        /// Specifies the homepage of the space.
        /// </summary>
        public String homePage;
    }
}