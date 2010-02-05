using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Prefetching
{
    /// <summary>
    /// Settings for prefetch behaviour
    /// </summary>
    [Serializable]
    public class PrefetchSettings
    {
        //set default polling interval to 10 seconds
        private const double DEFAULT_POLLING_INTERVAL = 10;
        //set default polling result set to 100 items
        private const int DEFAULT_RESULTSET_SIZE = 100;

        bool prefetchEnabled;
       
        //Dictionarry containing the urls and the local stirage path for the visited wikis.
        [NonSerialized]       
        private Dictionary<String, String> wikis;

        double pollingInterval;
        int resultSetSize;        

        public Dictionary<String, String> Wikis
        {
            get { return wikis; }
            set { wikis = value; }
        }

        public double PollingInterval
        {
            get { return pollingInterval; }
            set { pollingInterval = value; }
        }

        public int ResultSetSize
        {
            get { return resultSetSize; }
            set { resultSetSize = value; }
        }

        public bool PrefetchEnabled
        {
            get { return prefetchEnabled; }
            set { prefetchEnabled = value; }
        }

        /// <summary>
        /// Creates an instance containing the default settings.
        /// </summary>
        public PrefetchSettings()
        {
            prefetchEnabled = true;
            pollingInterval = DEFAULT_POLLING_INTERVAL;
            resultSetSize = DEFAULT_RESULTSET_SIZE;
            wikis = new Dictionary<string, string>();
        }
    }
}
