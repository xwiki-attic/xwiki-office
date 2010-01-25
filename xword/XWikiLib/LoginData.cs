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
using System.IO.IsolatedStorage;
using System.IO;

namespace XWiki
{
    /// <summary>
    /// Provindes functionality for storing authentication data. 
    /// </summary>
    public class LoginData
    {
        public const string XWORD_LOGIN_DATA_FILENAME = "XWordLoginData";

        private string filename;

        /// <summary>
        /// Creates an instance of the <code>LoginData</code>.
        /// </summary>
        /// <param name="filename">
        /// Name of the file in which the authentication data will be stored.
        /// Default is <code>LoginData.XWORD_LOGIN_DATA_FILENAME</code>;
        /// </param>
        public LoginData(string filename)
        {
            if (filename != null)
            {
                this.filename = filename;
            }
            else
            {
                this.filename = XWORD_LOGIN_DATA_FILENAME;
            }
        }

        /// <summary>
        /// Writes the user credentials to the disk for automatic login
        /// </summary>
        /// <param name="credentials">Array of strings containing the credentials</param>
        /// <returns>True if the operation is uscessfull. False otherwise.</returns>
        public bool WriteCredentials(String[] credentials)
        {
            IsolatedStorageFile isFile = null;
            IsolatedStorageFileStream stream = null;
            StreamWriter writer = null;

            bool successfulWrite = false;

            try
            {

                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                stream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile);
                writer = new StreamWriter(stream);
                foreach (String s in credentials)
                {
                    writer.WriteLine(s);
                }
                successfulWrite = true;
            }
            catch (IOException ioException)
            {
                Log.Exception(ioException);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (isFile != null)
                {
                    isFile.Dispose();
                    isFile.Close();
                }
            }

            return successfulWrite;
        }
        /// <summary>
        /// Gets the last used credentials.
        /// </summary>
        /// <returns>Array of strings containing the last used credentials</returns>
        public String[] GetCredentials()
        {
            String[] credentials = new String[3];
            IsolatedStorageFile isFile = null;
            IsolatedStorageFileStream stream = null;
            StreamReader reader = null;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                stream = new IsolatedStorageFileStream(filename, FileMode.Open, isFile);
                reader = new StreamReader(stream);
                int i = 0;
                while (!reader.EndOfStream)
                {
                    String s = reader.ReadLine();
                    credentials[i] = s;
                    i++;
                }
            }
            catch (Exception ex)
            {
                Log.ExceptionSummary(ex);
                credentials = null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (isFile != null)
                {
                    isFile.Dispose();
                    isFile.Close();
                }

            }
            return credentials;
        }

        /// <summary>
        /// Erases the last saved credentials form isolated storage.
        /// </summary>
        public void ClearCredentials()
        {
            IsolatedStorageFile isFile = null;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                isFile.DeleteFile(filename);
            }
            catch (IsolatedStorageException ex)
            {
                Log.ExceptionSummary(ex);
            }
            finally
            {
                if (isFile != null)
                {
                    isFile.Dispose();
                    isFile.Close();
                }
            }
        }

        /// <summary>
        /// Specifies if the last user saved his credentials for autologin.
        /// </summary>
        /// <returns></returns>
        public bool CanAutoLogin()
        {
            IsolatedStorageFile isFile = null;
            bool canAutoLogin = false;

            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                canAutoLogin = (isFile.GetFileNames(filename).Length > 0);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
            finally
            {
                if (isFile != null)
                {
                    isFile.Dispose();
                    isFile.Close();
                }
            }

            return canAutoLogin;
        }


    }
}