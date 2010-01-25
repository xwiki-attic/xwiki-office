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
using XWiki.Html;

namespace XWiki.Office.Word
{
    public class AbstractConverter
    {
        protected string DOCTYPE = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" + Environment.NewLine +
                                        "\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" + Environment.NewLine;

        protected const string HTML_OPENING_TAG = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\"" +
                                        " xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
                                        " xmlns:w=\"urn:schemas-microsoft-com:office:word\"" +
                                        " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\"" +
                                        " xmlns=\"http://www.w3.org/1999/xhtml\">";
        protected ConversionManager manager;
        protected HtmlUtil htmlUtil = new HtmlUtil();

        #region properties
        public String LocalFolder
        {
            get { return manager.States.LocalFolder; }
            set { manager.States.LocalFolder = value; }
        }

        public String ServerURL
        {
            get { return manager.States.ServerURL; }
            set { manager.States.ServerURL = value; }
        }

        public String DocumentName
        {
            get { return manager.States.PageFullName; }
            set { manager.States.PageFullName = value; }
        }

        public String LocalFilename
        {
            get { return manager.States.LocalFileName; }
            set { manager.States.LocalFileName = value; }
        }
        #endregion
    }
}
