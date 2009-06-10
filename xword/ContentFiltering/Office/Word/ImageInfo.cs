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
