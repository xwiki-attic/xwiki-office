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
using XWiki.Office.Word;
using XWiki.Clients;
using XOffice;

namespace ContentFiltering.Test.Util
{
    public class ConversionManagerTestUtil
    {
        private static string serverURL = "http://127.0.0.1:8080";
        private static string localFolder = "localFolder";
        private static string docFullName = "docFullName";
        private static string localFileName = "localFileName";

        /// <summary>
        /// Creates a dummy <code>ConverionManeger</code> to be used by unit tests.
        /// </summary>
        /// <returns>An instance of a dummy <code>ConversionManager</code></returns>
        public static ConversionManager DummyConversionManager()
        {
            IXWikiClient client = null;
            client = XWikiClientTestUtil.CreateMockInstance();
            XOfficeCommonSettings settings = new XOfficeCommonSettings();
            return new ConversionManager(settings, serverURL, localFolder, docFullName, localFileName, client);
        }
    }
}
