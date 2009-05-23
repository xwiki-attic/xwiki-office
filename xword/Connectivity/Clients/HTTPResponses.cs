using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Clients
{
    /// <summary>
    /// Contains response strings from the velocity pages that provide XWord functionality.
    /// </summary>
    public class HTTPResponses
    {
        /// <summary>
        /// Specifies that the saving process was completed successfully.
        /// </summary>
        public static String SAVE_OK = "RESPONSE 0 - SAVE-OK";
        /// <summary>
        /// Appears when a request was in a wrong format or had insufficinet parameters.
        /// </summary>
        public static String WRONG_REQUEST = "RESPONSE 100  - Error! Wrong request.";
        /// <summary>
        /// Appears when the the server fails to parse a page.
        /// </summary>
        public static String VELOCITY_PARSER_ERROR = "Error number 4001";
        /// <summary>
        /// Appears when the server has insufficeinet memmory for the current tasks.
        /// </summary>
        public static String INSUFFICIENT_MEMMORY = "java.lang.OutOfMemoryError";
        /// <summary>
        /// Returned if the page requires programming rights 
        /// but it was las saved by a user without programming rights
        /// </summary>
        public static String NO_PROGRAMMING_RIGHTS = "RESPONSE 101 - The page does not have programming rights";
        /// <summary>
        /// Returned if the current user does not have the right to edit the specified page.
        /// </summary>
        public static String NO_EDIT_RIGHTS = "RESPONSE 102 - You don't have edit right on this page.";
        /// <summary>
        /// Returned when the user/page does not have the right to parse and execute groovy pages.
        /// </summary>
        public static String NO_GROOVY_RIGHTS = "RESPONSE 103 - groovy_missingrights";
    }
}
