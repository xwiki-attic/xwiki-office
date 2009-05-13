using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using XWiki;

namespace XWord
{
    /// <summary>
    /// Offers functionality for storing the repository settings to Isolated Storage.
    /// </summary>
    public class Repositories
    {
        //Isolated storage file name;
        static string filename = "XWordRepositoriesSettings";

        /// <summary>
        /// Writes the repository settings to Isolated Storage.
        /// </summary>
        /// <param name="settings">The settings to be saved.</param>
        /// <returns>True if the operation succeded. False if the operation failed.</returns>
        public static bool WriteRepositorySettings(RepositorySettings settings)
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
        /// Gets the repository settings from Isolated Storage.
        /// </summary>
        /// <returns>A instance containing the repository settings.</returns>
        public static RepositorySettings GetRepositorySettings()
        {
            RepositorySettings settings=new RepositorySettings();
            IsolatedStorageFile isFile=null;
            IsolatedStorageFileStream stream=null;
            BinaryFormatter formatter;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                stream = new IsolatedStorageFileStream(filename, FileMode.Open, isFile);
                formatter = new BinaryFormatter();
                settings = (RepositorySettings)formatter.Deserialize(stream);
            }
            catch (InvalidCastException ce)
            {
                Log.ExceptionSummary(ce);
                settings = new RepositorySettings();
            }
            catch (Exception ex)
            {
                Log.ExceptionSummary(ex);
                settings = new RepositorySettings();
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
        /// Specifies if the application has the repository settings saved in Isolated Storage.
        /// </summary>
        /// <returns>
        /// True is the repository configuration file exists in Isolated Storage.
        /// False if the file does not exist.
        /// </returns>
        public static bool HasRepositorySettings()
        {
            IsolatedStorageFile isFile=null;
            bool hasRepositorySettings=false;
            try
            {
                isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                hasRepositorySettings = (isFile.GetFileNames(filename).Length > 0);
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

            return hasRepositorySettings;
        }
    }
}
