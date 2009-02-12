using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using XWiki;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Tools = Microsoft.Office.Tools;
using System.Threading;
using TidyNet;
using XWriter.XWiki;

namespace XWriter
{
    public partial class UserControl1 : UserControl
    {
        XWikiClient client = new XWikiClient("http://localhost:8080","Admin","admin");
        //XWikiClient client = new XWikiClient(Globals.XWikiAddIn.serverURL, Globals.XWikiAddIn.username, Globals.XWikiAddIn.password);
        NetworkCredential nc = new NetworkCredential("Admin", "admin");
        XmlSerializer serializer = new XmlSerializer(typeof(WikiStructure));
        WikiStructure wiki;
        XWikiAddIn addin;
        private object missing = Type.Missing;
        String startDocument = "<html>\n<head>\n</head>\n<body>";
        String endDocument = "/n</body>\n</html>";
        // Use this property to set and get the document's text.
        public String DocumentContent
        {
            get
            {
                Word.Range range = addin.Application.ActiveDocument.Range(ref missing, ref missing);
                range.TextRetrievalMode.IncludeHiddenText = true;
                return range.Text; 
            }
            set
            { 
                Word.Range range = addin.Application.ActiveDocument.Range(ref missing, ref missing);
                range.Text = value;
            }
        }

        public List<String> Cookies
        {
            get { return XWikiAddIn.cookies; }
            set { XWikiAddIn.cookies = value; }
        }
        
                
        public UserControl1(XWikiAddIn addin)
        {
            InitializeComponent();
            this.addin = addin;
            client.Credentials = nc;
            try
            {
                String URL = client.ServerURL + XWikiURLs.WikiStructureURL;
                Stream data = client.OpenRead(URL);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                parseWikiStructure(s);
                buidTree();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XWriter",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            
        }

        private void parseWikiStructure(String s)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] buffer = encoding.GetBytes(s);
            MemoryStream memoryStream = new MemoryStream(buffer, false);
            XmlSerializer serializer = new XmlSerializer(typeof(WikiStructure));
            wiki = (WikiStructure)serializer.Deserialize(memoryStream);
            memoryStream.Close();
        }

        private void buidTree()
        {
            foreach (Space space in wiki.spaces)
            {
                TreeNode node=treeView1.Nodes.Add(space.name);
                foreach (XWikiDocument doc in space.documents)
                {
                    node.Nodes.Add(doc.space + "." + doc.name, doc.name);                    
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent != null)
                {
                    TreeNode node = treeView1.SelectedNode;
                    getPage(node.Name);
                }
            }
        }

        private void getPage(String pageName)
        {
            //Read from server
            String uri = client.ServerURL + XWikiURLs.GetPageURL;
            uri += "?pagename=" + pageName + "&xpage=plain";
            Stream data = client.OpenRead(uri);
            StreamReader reader = new StreamReader(data);
            StringBuilder pageContent = new StringBuilder(reader.ReadToEnd());
            String localFileName = pageName.Replace(".", "-");
            localFileName = Application.StartupPath + localFileName + ".html";
            XWikiAddIn.currentLocalFilePath = localFileName;
            data.Close();
            reader.Close();

            //Process the content
            pageContent.Insert(0, startDocument);
            pageContent.Append(endDocument);
            //Save the file
            FileStream stream = new FileStream(localFileName, FileMode.Create);
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(pageContent.ToString());
            stream.Write(buffer,0,buffer.Length);
            stream.Close();
            XWikiAddIn.currentPageFullName = pageName;

            //Open the file with Word
            openHTMLDocument(localFileName);                   
        }

        private void savePage(String pageName,ref String pageContent)
        {
            String targetURL = client.ServerURL + XWikiURLs.SavePageURL;
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("pagename",pageName);
            parameters.Add("pagecontent",pageContent);
            parameters.Add("xpage","plain");
            byte[] response = client.UploadValues(targetURL, "POST", parameters);
            String responseText = Encoding.UTF8.GetString(response);
            MessageBox.Show(responseText);
          
        }

        private void openHTMLDocument(String path)
        {
            object format = Word.WdOpenFormat.wdOpenFormatWebPages;
            object filePath = path;
            Word.Document newDoc = addin.Application.Documents.Open(ref filePath,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref format,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref missing);
            addin.Application.ActiveDocument.WebOptions.AllowPNG = true;            
        }

        private void SaveToXwiki(object sender, EventArgs e)
        {
            addin.Application.ActiveDocument.Save();
            /*
             * Object _false = false;
            Object _true = true;
            Object format = Word.WdSaveFormat.wdFormatFilteredHTML;
            Object path = addin.Application.ActiveDocument.Path;
            addin.Application.ActiveDocument.SaveAs(ref path,ref format,ref missing,
                                                    ref missing,ref _false,ref _false,ref missing,ref _false, ref _false, ref _false, ref missing,ref missing,
                                                    ref _false,ref missing,ref missing, ref missing);
            */                                        
            addin.Application.ActiveDocument.Close(ref missing,ref missing,ref missing);
            StreamReader sr = new StreamReader(XWikiAddIn.currentLocalFilePath);
            String fileContent = sr.ReadToEnd();
            sr.Close();
            String cleanHTML = "";
            cleanContent(ref fileContent, ref cleanHTML);
            openHTMLDocument(XWikiAddIn.currentLocalFilePath);
            savePage(XWikiAddIn.currentPageFullName,ref cleanHTML);           
        }

        private void cleanContent(ref String initialContent,ref String cleanContent)
        {
            Tidy tidy = new Tidy();
            /*
            tidy.Options.DocType = DocType.Strict;
            tidy.Options.DropFontTags = true;
            tidy.Options.LogicalEmphasis = true;            
            tidy.Options.XmlOut = true;            
            dy.Options.TidyMark = false;*/
            tidy.Options.Word2000 = true;
            //tidy.Options.MakeClean = true;
            tidy.Options.Xhtml = true;

            TidyMessageCollection tmc = new TidyMessageCollection();
            MemoryStream input = new MemoryStream();
            MemoryStream output = new MemoryStream();

            byte[] byteArray = Encoding.UTF8.GetBytes(initialContent);
            input.Write(byteArray, 0, byteArray.Length);
            input.Position = 0;
            tidy.Parse(input, output, tmc);

            cleanContent = Encoding.UTF8.GetString(output.ToArray());

            //Delete header & footer
            int startIndex, endIndex;
            startIndex = cleanContent.IndexOf("<body");
            endIndex = cleanContent.IndexOf(">", startIndex);
            cleanContent = cleanContent.Remove(0, endIndex + 1);
            startIndex = cleanContent.IndexOf("</body");
            if(startIndex >= 0)  
                cleanContent = cleanContent.Remove(startIndex);
        }
    }
}