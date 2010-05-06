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
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using XWiki;

namespace XOffice
{
    /// <summary>
    /// Offers functionality for storing the settings to Isolated Storage.
    /// </summary>
    public class XOfficeCommonSettingsHandler
    {
        //Isolated storage file name;
        static string filename = "XWordSettings";

        /// <summary>
        /// Writes the settings to Isolated Storage.
        /// </summary>
        /// <param name="settings">The settings to be saved.</param>
        /// <returns>True if the operation succeded. False if the operation failed.</returns>
        public static bool WriteRepositorySettings(XOfficeCommonSettings settings)
        {
            IsolatedStorageFile isFile=null;
            IsolatedStorageFileStream stream = null;
            BinaryFormatter formatter = null;

            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                stream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile);
                formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
            }
            catch (IOException ioException)
            {
                Log.Exception(ioException);
            }
            finally
            {
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
            return true;
        }

        /// <summary>
        /// Gets the settings from Isolated Storage.
        /// </summary>
        /// <returns>A instance containing the settings.</returns>
        public static XOfficeCommonSettings GetSettings()
        {
            XOfficeCommonSettings settings=new XOfficeCommonSettings();
            IsolatedStorageFile isFile=null;
            IsolatedStorageFileStream stream=null;
            BinaryFormatter formatter;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                stream = new IsolatedStorageFileStream(filename, FileMode.Open, isFile);
                formatter = new BinaryFormatter();
                settings = (XOfficeCommonSettings)formatter.Deserialize(stream);
                NullsToDefaults(settings);
            }
            catch (InvalidCastException ce)
            {
                Log.ExceptionSummary(ce);
                settings = new XOfficeCommonSettings();
            }
            catch (Exception ex)
            {
                Log.ExceptionSummary(ex);
                settings = new XOfficeCommonSettings();
            }
            finally
            {
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
            return settings;
        }

        /// <summary>
        /// Specifies if the application has the settings saved in Isolated Storage.
        /// </summary>
        /// <returns>
        /// True is the repository configuration file exists in Isolated Storage.
        /// False if the file does not exist.
        /// </returns>
        public static bool HasSettings()
        {
            IsolatedStorageFile isFile=null;
            bool hasSettings=false;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                hasSettings = (isFile.GetFileNames(filename).Length > 0);
            }
            catch (IOException ioException)
            {
                Log.Exception(ioException);
            }
            finally
            {
                if (isFile != null)
                {
                    isFile.Dispose();
                    isFile.Close();
                }
            }

            return hasSettings;
        }

        private static void NullsToDefaults(XOfficeCommonSettings settings)
        {
            if (settings.DownloadedAttachmentsRepository == null)
            {
                settings.DownloadedAttachmentsRepository = Path.GetTempPath();
            }
            if (settings.PagesRepository == null)
            {
                settings.PagesRepository = Path.GetTempPath();
            }
            if (settings.PrefetchSettings == null)
            {
                settings.PrefetchSettings = new XWiki.Prefetching.PrefetchSettings();
            }
            if (settings.MetaDataFolderSuffix == null)
            {
                settings.MetaDataFolderSuffix = "";
            }
        }
    }
}
