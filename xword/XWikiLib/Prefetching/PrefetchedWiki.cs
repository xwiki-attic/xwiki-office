using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Prefetching
{
    /// <summary>
    /// Contains the prefetch data of a wiki
    /// </summary>
    [Serializable]
    public class PrefetchedWiki
    {
        List<String> documentNames;

        DateTime lastUpdate;        

        /// <summary>
        /// The list of prefetch page names of the wiki.
        /// </summary>
        public List<String> DocumentNames
        {
            get { return documentNames; }
            set { documentNames = value; }
        }

        /// <summary>
        /// The last 
        /// </summary>
        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }
    }
}
