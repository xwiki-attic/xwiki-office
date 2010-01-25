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
using System.Linq;
using System.Text;
using System.IO;

namespace XWiki.Office.Word
{
    /// <summary>
    /// Contains the states of a image in both web an local file system environments.
    /// </summary>
    class ImageInfo
    {
        /// <summary>
        /// Image attribute used by XWord
        /// </summary>
        public static String XWORD_IMG_ATTRIBUTE = "xword_img_id";
        /// <summary>
        /// Alternative text used by the image
        /// </summary>
        public String altText = "";
        /// <summary>
        /// The value of the src attribute while on a website.
        /// </summary>
        public String imgWebSrc = "";
        /// <summary>
        /// The value of the src attribute while on file system.
        /// </summary>
        public String imgLocalSrc = "";
        /// <summary>
        /// The full path to the local file.
        /// </summary>
        public String filePath = "";
        /// <summary>
        /// The URI representing the local file.
        /// </summary>
        public String fileURI = "";
        /// <summary>
        /// The size of the local file.
        /// </summary>
        public Decimal fileSize = 0;
        /// <summary>
        /// The creation date of the local file.
        /// </summary>
        public DateTime fileCreationDate = DateTime.MinValue;
    }
}
