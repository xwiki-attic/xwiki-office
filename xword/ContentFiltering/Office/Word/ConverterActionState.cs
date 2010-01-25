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

namespace XWiki.Office.Word
{
    /// <summary>
    /// The states of the conversion.
    /// </summary>
    public enum ConverterActionState
    {
        /// <summary>
        /// The page is retrieved from server.
        /// </summary>
        Downloading,
        /// <summary>
        /// The page is uploaded/saved on the server.
        /// </summary>
        Uploading,
        /// <summary>
        /// The user is editing the opened page.
        /// </summary>
        EditingPage,
        /// <summary>
        /// The page is stored for offline use.
        /// <remarks>
        /// Not yet implemented.
        /// </remarks>
        /// </summary>
        StoringForOffileUsage,
        /// <summary>
        /// Exporting a new document to wiki.
        /// </summary>
        Exporting
    }
}
