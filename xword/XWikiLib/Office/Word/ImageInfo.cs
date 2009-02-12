using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XWiki.Office.Word
{
    class ImageInfo
    {
        public static String XWORD_IMG_ATTRIBUTE = "xword_img_id";
        public String altText = "";
        public String imgWebSrc = "";
        public String imgLocalSrc = "";
        public String filePath = "";
        public String fileURI = "";
        public Decimal fileSize = 0;
        public DateTime fileCreationDate = DateTime.MinValue;
    }
}
