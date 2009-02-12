using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TestXWikiLib
{
    public class WebClientMock : WebClient
    {
        public static byte[] responseOK = { 1, 2, 3 };
        /// <summary>
        /// Default constructor for WebClientMock
        /// </summary>
        public WebClientMock()
        {
            
        }

        public new byte[] UploadValues(string address,System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValues(Uri address, System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValues(Uri address, String method, System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValues(String address, String method, System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValuesAsync(Uri address, System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValuesAsync(Uri address, String method, System.Collections.Specialized.NameValueCollection data)
        {
            return responseOK;
        }

        public new byte[] UploadValuesAsync(Uri address, String method, System.Collections.Specialized.NameValueCollection data, object UserToken)
        {
            return responseOK;
        }

    }
}
