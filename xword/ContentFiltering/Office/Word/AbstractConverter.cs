using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Html;

namespace XWiki.Office.Word
{
    class AbstractConverter
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
