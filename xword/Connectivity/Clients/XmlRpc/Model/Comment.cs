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
    /// Contains the attributes of a comment.
    /// </summary>
    public struct Comment
    {
        /// <summary>
        /// The content of the comment.
        /// </summary>
        public string content;

        /// <summary>
        /// The id of the page contining the comment.
        /// </summary>
        public string pageId;

        /// <summary>
        /// The url of the comment.
        /// </summary>
        public string url;

        /// <summary>
        /// The user that posted the comment.
        /// </summary>
        public string creator;

        /// <summary>
        /// The date when the comment was created.
        /// </summary>
        public DateTime created;

        /// <summary>
        /// The id of the comment. This id identifies the comment in the entire wiki.
        /// Eg: "Main.WebHome?commentId=0"
        /// </summary>
        public string id;

        /// <summary>
        /// The title of the comment.
        /// </summary>
        public string title;

    }
}