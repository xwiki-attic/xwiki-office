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

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains the attributes of a page.
    /// </summary>
    public struct Attachment
    {
        /// <summary>
        /// Comment on file attachment.
        /// </summary>
        public String comment;

        /// <summary>
        /// Filesize in bytes.
        /// </summary>
        public String fileSize;

        /// <summary>
        /// The download url for the attachment.
        /// </summary>
        public String url;

        /// <summary>
        /// The title of the attachment.
        /// </summary>
        public String title;

        /// <summary>
        /// The name of the file.
        /// </summary>
        public String fileName;

        /// <summary>
        /// The date when the file was created.
        /// </summary>
        public DateTime created;

        /// <summary>
        /// The mime-type of the attachment.
        /// </summary>
        public String contentType;

        /// <summary>
        /// The id of the page: '[wiki:]Space.page[?param1=value1..]'
        /// </summary>
        public String pageId;

        /// <summary>
        /// The user that attached the file.
        /// </summary>
        public String creator;

        /// <summary>
        /// Constructor, initializes the fields with the default values.
        /// Creates a new Attacment instance.
        /// <param name="_pageId">The name of the document containing the attachment.</param>
        /// </summary>
        
        public Attachment(String _pageId)
        {
            pageId = _pageId;            
            comment = "";
            fileSize = "0";
            url = "";
            title = "";
            fileName = "";
            created = DateTime.Now;
            contentType = "";
            creator = "";
        }
    }
}