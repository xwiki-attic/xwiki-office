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
using System.IO;
using System.Linq;
using System.Text;
using XWiki;

namespace XWord
{
    /// <summary>
    /// Structure used for writing and retrieving settings data from Isolated Storage.
    /// </summary>
    [Serializable]
    public class XOfficeCommonSettings
    {
        private string pagesRepository;        
        private string downloadedAttachmentsRepository;
        private XWikiClientType clientType;

        /// <summary>
        /// Creates a new instance of the class.
        /// <remarks>Used for serialization.</remarks>
        /// </summary>
        public XOfficeCommonSettings()
        {
            pagesRepository = Path.GetTempPath();
            downloadedAttachmentsRepository = Path.GetTempPath();
            clientType = XWikiClientType.XML_RPC;
        }

        /// <summary>
        /// Gets or sets the value for the pages repositoy.
        /// </summary>
        public string PagesRepository
        {
            get { return pagesRepository; }
            set { pagesRepository = value; }
        }

        /// <summary>
        /// Gets ot sets the value of the path where the downloaded attachments are stored.
        /// </summary>
        public string DownloadedAttachmentsRepository
        {
            get { return downloadedAttachmentsRepository; }
            set { downloadedAttachmentsRepository = value; }
        }

        /// <summary>
        /// Gets or sets the value for the connetivity type setting.
        /// </summary>
        public XWikiClientType ClientType
        {
            get { return clientType; }
            set { clientType = value; }
        }
    }
}
