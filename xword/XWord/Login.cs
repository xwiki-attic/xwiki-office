using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security = System.Security;
using System.IO;
using System.IO.IsolatedStorage;

namespace XWriter
{
    /// <summary>
    /// Provindes functionality for storing authentication data. 
    /// </summary>
    public class LoginData
    {
        private string filename = "XWordLoginData";

        /// <summary>
        /// Writes the user credentials to the disk for automatic login
        /// </summary>
        /// <param name="credentials">Array of strings containing the credentials</param>
        /// <returns>True if the operation is uscessfull. False otherwise.</returns>
        public bool WriteCredentials(String[] credentials)
        {
            IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile);
            StreamWriter writer = new StreamWriter(stream);
            foreach (String s in credentials)
            {
                writer.WriteLine(s);
            }
            writer.Close();
            stream.Close();
            return true;
        }
        /// <summary>
        /// Gets the last used credentials.
        /// </summary>
        /// <returns>Array of strings containing the last used credentials</returns>
        public String[] GetCredentials()
        {
            try
            {
                String[] credentials = new String[3];
                IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Open, isFile);
                StreamReader reader = new StreamReader(stream);
                int i=0;
                while (!reader.EndOfStream)
                {
                    String s = reader.ReadLine();
                    credentials[i] = s;
                    i++;
                }
                return credentials;
            }
            catch(Exception ex)
            {
                Log.ExceptionSummary(ex);
                return null;
            }
        }

        /// <summary>
        /// Erases the last saved credentials form isolated storage.
        /// </summary>
        public void ClearCredentials()
        {
            try
            {
                IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                isFile.DeleteFile(filename);
            }
            catch (IsolatedStorageException ex)
            {
                Log.ExceptionSummary(ex);
            }
        }

        /// <summary>
        /// Specifies if the last user saved his credentials for autologin.
        /// </summary>
        /// <returns></returns>
        public bool CanAutoLogin()
        {
            IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
            return (isFile.GetFileNames(filename).Length > 0);
        }
    }
}
